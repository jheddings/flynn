//=============================================================================
// Copyright © Jason Heddings, All Rights Reserved
// $Id: SmartLinc.cs 88 2013-12-23 18:04:48Z jheddings $
//=============================================================================
using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using Flynn.Utilities;
using Flynn.X10.Properties;

// API information - http://www.leftovercode.info/smartlinc.html

// TODO add support for turning scenes/groups on and off

namespace Flynn.X10.Controllers {
    public sealed class SmartLinc : InsteonController, IX10Remote {

        private static readonly Logger _logger = Logger.Get(typeof(SmartLinc));

		private readonly AutoResetEvent _devlock = new AutoResetEvent(true);

        ///////////////////////////////////////////////////////////////////////
        private String _address;
        public String Address {
            get { return _address; }
        }

        ///////////////////////////////////////////////////////////////////////
        public SmartLinc(String addr) {
            _logger.Debug("new SmartLinc({0})", addr);

            _address = addr;
        }
        
        ///////////////////////////////////////////////////////////////////////
        protected override void SendCommand(InsteonCommand cmd) {
			var str = new StringBuilder("3?0262");

			String address = cmd.Address.Replace(".", "");
			str.Append(address).Append("0F");

			String code = String.Format("{0:X2}", (int) cmd.Action);
			String level = cmd.Level.ToString("X2");
			str.Append(code).Append(level).Append("=I=3");

			Transmit(str.ToString());
        }

        ///////////////////////////////////////////////////////////////////////
        public void Send(X10Command cmd) {
			var str = new StringBuilder("3?0263");

			char house = GetX10HouseCodeValue(cmd.House);
            str.Append(house);

            if (cmd.Device != 0) {
				char unit = GetX10DeviceCodeValue(cmd.Device);
                str.Append(unit).Append("00P10263").Append(house);
            }

			char code = GetX10CommandCodeValue(cmd.Action);
            str.Append(code).Append("80P1=I=3");

            Transmit(str.ToString());
        }

        ///////////////////////////////////////////////////////////////////////
        private void Transmit(String cmd) {
            try {
                _devlock.WaitOne();
                UnsafeTransmit(cmd);
            } catch (Exception e) {
                _logger.Warn(e);
            } finally {
                _devlock.Set();
            }
        }

        ///////////////////////////////////////////////////////////////////////
        private HttpStatusCode UnsafeTransmit(String cmd) {
            String url = String.Format("http://{0}/{1}", _address, cmd);

			var req = (HttpWebRequest) HttpWebRequest.Create(url);
			var resp = (HttpWebResponse) req.GetResponse();

            Stream stream = resp.GetResponseStream();
            StreamReader reader = new StreamReader(stream, Encoding.UTF8);
            String data = reader.ReadToEnd();

            HttpStatusCode status = resp.StatusCode;
            _logger.Debug("{2} [{0}] => {1}", status, data, url);
            
            reader.Close();
            resp.Close();

            Thread.Sleep(Settings.Default.SmartLinc_SendDelayMs);

            return status;
        }

		///////////////////////////////////////////////////////////////////////
		public static char GetX10HouseCodeValue(char house) {
			switch (house) {
				case 'A': return '6';
				case 'B': return 'E';
				case 'C': return '2';
				case 'D': return 'A';
				case 'E': return '1';
				case 'F': return '9';
				case 'G': return '5';
				case 'H': return 'D';
				case 'I': return '7';
				case 'J': return 'F';
				case 'K': return '3';
				case 'L': return 'B';
				case 'M': return '0';
				case 'N': return '8';
				case 'O': return '4';
				case 'P': return 'C';
			}
			throw new ArgumentException("Invalid House Code");
		}

		///////////////////////////////////////////////////////////////////////
		public static char GetX10DeviceCodeValue(int device) {
			switch (device) {
				case 1: return '6';
				case 2: return 'E';
				case 3: return '2';
				case 4: return 'A';
				case 5: return '1';
				case 6: return '9';
				case 7: return '5';
				case 8: return 'D';
				case 9: return '7';
				case 10: return 'F';
				case 11: return '3';
				case 12: return 'B';
				case 13: return '0';
				case 14: return '8';
				case 15: return '4';
				case 16: return 'C';
			}
			throw new ArgumentException("Invalid Device Code");
		}

		///////////////////////////////////////////////////////////////////////
		public static char GetX10CommandCodeValue(X10Command.Command cmd) {
			switch (cmd) {
				case X10Command.Command.ALF: return '6';
				case X10Command.Command.ALO: return '1';
				case X10Command.Command.AUF: return '0';
				case X10Command.Command.BRI: return '5';
				case X10Command.Command.DIM: return '4';
				case X10Command.Command.OFF: return '3';
				case X10Command.Command.ON: return '2';
			}
			throw new ArgumentException("Invalid Command Code");
		}
    }
}
