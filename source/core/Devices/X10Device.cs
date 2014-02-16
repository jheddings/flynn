// =============================================================================
//  Copyright © Jason Heddings, All Rights Reserved
//  $Id: X10Device.cs 83 2013-11-06 23:34:18Z jheddings $
// =============================================================================
using System;
using System.Text;
using System.Xml.Serialization;
using Flynn.X10;

namespace Flynn.Core.Devices {

    [XmlRoot("X10Device", Namespace = "Flynn.Core.Devices")]
    public class X10Device : DeviceBase {

        ///////////////////////////////////////////////////////////////////////
        private IX10Remote _controller;
        public IX10Remote Controller {
            get {
				return (_controller ?? X10Controller.Default);
            }

            set { _controller = value; }
        }

        ///////////////////////////////////////////////////////////////////////
        private char _house;
        public char House {
            get { return _house; }

            set {
                char ch = char.ToUpper(value);
                if ((ch < 'A') || (ch > 'P')) {
					throw new ArgumentOutOfRangeException("value", ch, "invalid house code");
                }
                
                _house = value;
            }
        }

        ///////////////////////////////////////////////////////////////////////
        private int _unit;
        public int Unit {
            get { return _unit; }

            set {
                if ((value < 1) || (value > 16)) {
					throw new ArgumentOutOfRangeException("value", value, "invalid unit code");
                }
                
                _unit = value;
            }
        }
        
        ///////////////////////////////////////////////////////////////////////
        public X10Device(char house, int unit) {
            House = house;
            Unit = unit;
        }

        ///////////////////////////////////////////////////////////////////////
        public override void TurnOn() {
            SendDeviceCommand(X10Command.Command.ON);
        }

        ///////////////////////////////////////////////////////////////////////
        public override void TurnOff() {
            SendDeviceCommand(X10Command.Command.OFF);
        }

        ///////////////////////////////////////////////////////////////////////
        public override String ToString() {
			var str = new StringBuilder("[X10Device (");
            str.Append(_house).Append(_unit).Append(')');

            if (Name != null) {
                str.Append(": ").Append(Name);
            }

            str.Append(']');
            return str.ToString();
        }

        ///////////////////////////////////////////////////////////////////////
        private void SendDeviceCommand(X10Command.Command action) {
			var cmd = new X10Command {
                House = House,
                Device = Unit,
                Action = action
            };

            Controller.Send(cmd);
        }
    }
}
