//=============================================================================
// Copyright © Jason Heddings, All Rights Reserved
// $Id: X10Command.cs 83 2013-11-06 23:34:18Z jheddings $
//=============================================================================
using System;
using System.Text;
using System.Text.RegularExpressions;

namespace Flynn.X10 {
    public sealed class X10Command {

        public static readonly Regex kCommandRE =
            new Regex(@"^(?<house>[A-P])(((?<device>1[0-6]|[1-9])_(?<devcmd>ON|OFF))|_(?<allcmd>ALO|ALF|AUF|BRI|DIM))$");

        ///////////////////////////////////////////////////////////////////////
        public enum Command {
            ON,   // device ON
            OFF,  // device OFF
            ALO,  // all lamps ON
            ALF,  // all lamps OFF
            AUF,  // all units OFF
            AUO,  // all units ON
            DIM,  // last device DIM
            BRI   // last device BRIGHT
        }

        ///////////////////////////////////////////////////////////////////////
        private char _house = 'A';
        private int _device = 0;
        private Command _action = Command.AUF;

        ///////////////////////////////////////////////////////////////////////
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
        public int Device {
            get { return _device; }

            set {
                if ((value < 0) || (value > 16)) {
                    throw new ArgumentOutOfRangeException("value", value, "invalid device code");
                }

                _device = value;
            }
        }

        ///////////////////////////////////////////////////////////////////////
        public Command Action {
            get { return _action; }
            set { _action = value; }
        }

        ///////////////////////////////////////////////////////////////////////
        public X10Command() {
        }
        
        ///////////////////////////////////////////////////////////////////////
        public X10Command(char house, int device) {
            House = house;
            Device = device;
        }
        
        ///////////////////////////////////////////////////////////////////////
        public X10Command(char house, int device, Command action) {
            House = house;
            Device = device;
            Action = action;
        }

        ///////////////////////////////////////////////////////////////////////
        public override String ToString() {
			var str = new StringBuilder();
            str.Append(_house);

            if (_device != 0) {
                str.Append(_device);
            }

            str.Append('_').Append(_action.ToString());

            return str.ToString();
        }

        ///////////////////////////////////////////////////////////////////////
        public static X10Command Parse(String x10) {
			var cmd = new X10Command();
			var match = kCommandRE.Match(x10);

            if (match.Success) {
                cmd.House = match.Groups["house"].Value[0];

                if (match.Groups["device"].Success) {
                    cmd.Device = byte.Parse(match.Groups["device"].Value);
                }

                String action = null;
                if (match.Groups["devcmd"].Success) {
                    action = match.Groups["devcmd"].Value;
                } else if (match.Groups["allcmd"].Success) {
                    action = match.Groups["allcmd"].Value;
                }

                cmd.Action = (Command) Enum.Parse(typeof(Command), action);
            }

            return cmd;
        }
    }
}
