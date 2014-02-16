//=============================================================================
// Copyright © Jason Heddings, All Rights Reserved
// $Id: CM17A.cs 88 2013-12-23 18:04:48Z jheddings $
//=============================================================================
using System;
using System.IO.Ports;
using System.Threading;
using Flynn.Utilities;
using Flynn.X10.Properties;

// enables control of the CM17A (Firecracker) RF interface

// the SerialPort class is VERY slow under the Microsoft .NET runtime...  this
// causes all commands to take much longer than expected; to help mitigate the
// differences between implementations, the data rates are significantly slower
// than what the firecracker is actually capable of; of course this is all set
// in the application config file and can be customized per installation

namespace Flynn.X10.Controllers {
    public sealed class CM17A : X10Controller, IDisposable {

        private static readonly Logger _logger = Logger.Get(typeof(CM17A));

        // define the lengths in bits of the command
        private const ushort kCmdHeaderLen = 16;
        private const ushort kCmdDataLen = 16;
        private const ushort kCmdFooterLen = 8;
        private const ushort kCmdLength = kCmdHeaderLen + kCmdDataLen + kCmdFooterLen;
        
        // constant header & footer
        private const uint kCmdHeader = 0xd5aa;
        private const uint kCmdFooter = 0xad;

		private readonly SerialPort _port;
		private readonly Settings _conf;

        ///////////////////////////////////////////////////////////////////////
        public CM17A(String port) {
            _logger.Debug("new CM17A({0})", port);

            _port = new SerialPort(port);
            _port.Handshake = Handshake.None;
            _conf = Settings.Default;
        }

        ///////////////////////////////////////////////////////////////////////
        public void Dispose() {
            _logger.Debug("Disposing({0})", _port.PortName);

            if (_port.IsOpen) {
                ClosePort();
            }

            _port.Dispose();
        }

        ///////////////////////////////////////////////////////////////////////
        protected override void SendCommand(X10Command cmd) {
            uint payload = GetBits(cmd);

            _logger.Debug("Send({0}) => {2} [0x{1:X4}]", cmd, payload, _port.PortName);

            Transmit(payload, kCmdDataLen);
        }

        ///////////////////////////////////////////////////////////////////////
        private void Transmit(uint payload, ushort length) {
            lock (_port) {
                OpenPort();

                Wiggle(kCmdHeader, kCmdHeaderLen);
                Wiggle(payload, length);
                Wiggle(kCmdFooter, kCmdFooterLen);

                Thread.Sleep(_conf.CM17A_SendDelayMs);

                ClosePort();
            }
        }

        ///////////////////////////////////////////////////////////////////////////
        private void OpenPort() {
			if (! _port.IsOpen) {
				_port.Open();
			}

            // reset the firecracker
            _port.DtrEnable = false;
            _port.RtsEnable = false;
            Thread.Sleep(_conf.CM17A_ResetHoldMs);

            // power on the firecracker
            _port.DtrEnable = true;
            _port.RtsEnable = true;
            Thread.Sleep(_conf.CM17A_ResetHoldMs);
        }

        ///////////////////////////////////////////////////////////////////////
        private void ClosePort() {
            _port.DtrEnable = false;
            _port.RtsEnable = false;

            _port.Close();
        }

        ///////////////////////////////////////////////////////////////////////
        // Sends the given bits to the port using the firecracker protocol.
        private void Wiggle(uint bits, ushort len) {

            // start at the most significant bit; (len-1) positions remain
            uint mask = (uint) (1 << (len-1));

            while (mask > 0) {
                if ((bits & mask) == 0) {
                    _port.RtsEnable = false;
                    Thread.Sleep(_conf.CM17A_BitHoldMs);
                    _port.RtsEnable = true;

                } else {
                    _port.DtrEnable = false;
                    Thread.Sleep(_conf.CM17A_BitHoldMs);
                    _port.DtrEnable = true;
                }

                Thread.Sleep(_conf.CM17A_BitHoldMs);

                // next bit...
                mask >>= 1;
            }
        }

        ///////////////////////////////////////////////////////////////////
        private static ushort GetBits(X10Command cmd) {
            ushort houseBits = GetHouseBits(cmd.House);
            ushort deviceBits = GetDeviceBits(cmd.Device);
            ushort actionBits = GetActionBits(cmd.Action);

            return (ushort) (houseBits | deviceBits | actionBits);
        }

        ///////////////////////////////////////////////////////////////////////////
        private static ushort GetHouseBits(char house) {
            switch (house) {
                case 'A': return 0x6000;
                case 'B': return 0x7000;
                case 'C': return 0x4000;
                case 'D': return 0x5000;
                case 'E': return 0x8000;
                case 'F': return 0x9000;
                case 'G': return 0xA000;
                case 'H': return 0xB000;
                case 'I': return 0xE000;
                case 'J': return 0xF000;
                case 'K': return 0xC000;
                case 'L': return 0xD000;
                case 'M': return 0x0000;
                case 'N': return 0x1000;
                case 'O': return 0x2000;
                case 'P': return 0x3000;
            }

            throw new ArgumentException("Invalid House Code");
        }

        ///////////////////////////////////////////////////////////////////////
        private static ushort GetDeviceBits(int device) {
            switch (device) {
                case 0: return 0x0000;
                case 1: return 0x0000;
                case 2: return 0x0010;
                case 3: return 0x0008;
                case 4: return 0x0018;
                case 5: return 0x0040;
                case 6: return 0x0050;
                case 7: return 0x0048;
                case 8: return 0x0058;
                case 9: return 0x0400;
                case 10: return 0x0410;
                case 11: return 0x0408;
                case 12: return 0x0418;
                case 13: return 0x0440;
                case 14: return 0x0450;
                case 15: return 0x0448;
                case 16: return 0x0458;
            }

            throw new ArgumentException("Invalid Device Code");
        }

        ///////////////////////////////////////////////////////////////////////
        private static ushort GetActionBits(X10Command.Command action) {
            switch (action) {
                case X10Command.Command.ON: return 0x0000;
                case X10Command.Command.OFF: return 0x0020;
                case X10Command.Command.DIM: return 0x0098;
                case X10Command.Command.BRI: return 0x0088;
                case X10Command.Command.ALO: return 0x0094;
                case X10Command.Command.ALF: return 0x0084;
                case X10Command.Command.AUF: return 0x0080;
                case X10Command.Command.AUO: return 0x0091;
            }

            throw new ArgumentException("Invalid Action Code");
        }
    }
}