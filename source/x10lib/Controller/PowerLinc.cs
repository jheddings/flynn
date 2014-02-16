// =============================================================================
//  Copyright © Jason Heddings, All Rights Reserved
//  $Id: PowerLinc.cs 88 2013-12-23 18:04:48Z jheddings $
// =============================================================================
using System;
using System.IO.Ports;
using System.Text;
using Flynn.Utilities;

namespace Flynn.X10.Controllers {
	public class PowerLinc : InsteonController, IX10Remote {

		private static readonly Logger _logger = Logger.Get(typeof(PowerLinc));

		private readonly SerialPort _port;

		///////////////////////////////////////////////////////////////////////
		public PowerLinc(String port) {
			_logger.Debug("new PowerLinc({0})", port);

			_port = new SerialPort(port, 9600, Parity.None, 8, StopBits.None);
			_port.Handshake = Handshake.None;
        }

		///////////////////////////////////////////////////////////////////////
		protected override void SendCommand(InsteonCommand cmd) {
			_logger.Debug("Send({0}) => {2} [{1}]", cmd, null, _port.PortName);

			throw new NotImplementedException();
		}

		///////////////////////////////////////////////////////////////////////
		public void Send(X10Command cmd) {
			var str = new StringBuilder('c');  // 0x63

			char house = GetX10HouseCodeValue(cmd.House);
			str.Append(house);

			if (cmd.Device == 0) {
				char func = GetX10CommandCodeValue(cmd.Action);
				str.Append(func).Append('\0');
			} else {
				char unit = GetX10DeviceCodeValue(cmd.Device);
				char func = GetX10CommandCodeValue(cmd.Action);
				str.Append(unit).Append(func);
			}

			_logger.Debug("Send({0}) => {2} [{1}]", cmd, str, _port.PortName);

			Transmit(str.ToString());
		}

		///////////////////////////////////////////////////////////////////////
		private void Transmit(String payload, int repeat = 1) {
			lock (_port) {
				if (! _port.IsOpen) {
					_port.Open();
				}

				// wakeup PLM, wait for ACK
				// retry on NAK (conf num times)

				// send payload
				// send repeat : 0x41

				// read response (how many lines?)

				_port.Close();
			}
		}

		///////////////////////////////////////////////////////////////////////
		public static char GetX10HouseCodeValue(char house) {
			switch (house) {
				case 'A': return 'F';
				case 'B': return 'N';
				case 'C': return 'B';
				case 'D': return 'J';
				case 'E': return 'A';
				case 'F': return 'I';
				case 'G': return 'E';
				case 'H': return 'M';
				case 'I': return 'G';
				case 'J': return 'O';
				case 'K': return 'C';
				case 'L': return 'K';
				case 'M': return '@';
				case 'N': return 'H';
				case 'O': return 'D';
				case 'P': return 'L';
			}
			throw new ArgumentException("Invalid House Code");
		}

		///////////////////////////////////////////////////////////////////////
		public static char GetX10DeviceCodeValue(int device) {
			switch (device) {
				case 1:  return 'L';
				case 2:  return '\\';
				case 3:  return 'D';
				case 4:  return 'T';
				case 5:  return 'B';
				case 6:  return 'R';
				case 7:  return 'J';
				case 8:  return 'Z';
				case 9:  return 'N';
				case 10: return '^';
				case 11: return 'F';
				case 12: return 'V';
				case 13: return '@';
				case 14: return 'P';
				case 15: return 'H';
				case 16: return 'X';
			}
			throw new ArgumentException("Invalid Device Code");
		}

		///////////////////////////////////////////////////////////////////////
		public static char GetX10CommandCodeValue(X10Command.Command cmd) {
			switch (cmd) {
				case X10Command.Command.ALF: return 'M';
				case X10Command.Command.ALO: return 'C';
				case X10Command.Command.AUF: return 'A';
				case X10Command.Command.BRI: return 'K';
				case X10Command.Command.DIM: return 'I';
				case X10Command.Command.OFF: return 'G';
				case X10Command.Command.ON:  return 'E';
			}
			throw new ArgumentException("Invalid Command Code");
		}
    }
}