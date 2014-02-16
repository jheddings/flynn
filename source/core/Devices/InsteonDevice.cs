// =============================================================================
//  Copyright © Jason Heddings, All Rights Reserved
//  $Id: InsteonDevice.cs 83 2013-11-06 23:34:18Z jheddings $
// =============================================================================
using System;
using System.Text;
using Flynn.X10;

namespace Flynn.Core.Devices {
    public class InsteonDevice : DeviceBase {

        ///////////////////////////////////////////////////////////////////////
        private IInsteonRemote _controller;
        public IInsteonRemote Controller {
			get {
				return (_controller ?? InsteonController.Default);
			}

            set { _controller = value; }
        }

        ///////////////////////////////////////////////////////////////////////
        private readonly String _address;
        public String Address {
            get { return _address; }
        }

        ///////////////////////////////////////////////////////////////////////
        public InsteonDevice(String address) {
            _address = address;
        }

        ///////////////////////////////////////////////////////////////////////
        public override void TurnOn() {
            SendDeviceCommand(InsteonCommand.Command.FAST_ON, 0xFF);
        }

        ///////////////////////////////////////////////////////////////////////
        public override void TurnOff() {
            SendDeviceCommand(InsteonCommand.Command.FAST_OFF, 0x00);
        }

        ///////////////////////////////////////////////////////////////////////
        public override String ToString() {
			var str = new StringBuilder("[InsteonDevice (");
            str.Append(_address).Append(')');
            
            if (Name != null) {
                str.Append(": ").Append(Name);
            }
            
            str.Append(']');
            return str.ToString();
        }

        ///////////////////////////////////////////////////////////////////////
        private void SendDeviceCommand(InsteonCommand.Command action, int level) {
			var cmd = new InsteonCommand {
                Address = Address,
                Action = action,
                Level = level
            };
            
            Controller.Send(cmd);
        }
    }
}
