// =============================================================================
//  Copyright © Jason Heddings, All Rights Reserved
//  $Id: InsteonCommand.cs 81 2013-11-06 04:44:58Z jheddings $
// =============================================================================
using System;

namespace Flynn.X10 {
    public sealed class InsteonCommand {

        ///////////////////////////////////////////////////////////////////////
        // http://www.madreporite.com/insteon/commands.htm
        public enum Command {
            ON = 0x11,  // device ON
            FAST_ON = 0x12,  // fast ON
            OFF = 0x13,  // device OFF
            FAST_OFF = 0x14,  // fast OFF
            BRI = 0x15,  // brighten one step
            DIM = 0x16,  // dim one step
            STAT = 0x19,  // get device status
        }

        ///////////////////////////////////////////////////////////////////////
        private String _address;
        public String Address {
            get { return _address; }

            set {
                if (value == null) {
                    throw new ArgumentNullException();
                } else  if (IsValidAddress(value)) {
                    _address = value;
                } else {
                    throw new ArgumentException("invalid address");
                }
            }
        }

        ///////////////////////////////////////////////////////////////////////
        private Command _action = Command.OFF;
        public Command Action {
            get { return _action; }
            set { _action = value; }
        }
        
        ///////////////////////////////////////////////////////////////////////
        private int _level = 0;
        public int Level {
            get { return _level; }
            set {
                if ((value < 0x00) || (value > 0xFF)) {
                    throw new ArgumentOutOfRangeException();
                }
                _level = value;
            }
        }

        ///////////////////////////////////////////////////////////////////////
        public InsteonCommand() {
        }

        ///////////////////////////////////////////////////////////////////////
        public InsteonCommand(String address) {
            _address = address;
        }
        
        ///////////////////////////////////////////////////////////////////////
        public InsteonCommand(String address, Command action) {
            _address = address;
            _action = action;
        }

        ///////////////////////////////////////////////////////////////////////
        public InsteonCommand(String address, Command action, int level) {
            _address = address;
            _action = action;
            _level = level;
        }

        ///////////////////////////////////////////////////////////////////////
        public override String ToString() {
            return String.Format("{0}/{1},0x{2:X2}", _address, _action, _level);
        }

        ///////////////////////////////////////////////////////////////////////
        public static bool IsValidAddress(String addr) {
            if (addr == null) { return false; }

            // TODO check address
            return true;
        }
    }
}
