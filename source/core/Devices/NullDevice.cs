// =============================================================================
//  Copyright © Jason Heddings, All Rights Reserved
//  $Id: NullDevice.cs 83 2013-11-06 23:34:18Z jheddings $
// =============================================================================
using System;

namespace Flynn.Core.Devices {
    public class NullDevice : IDevice, IDimmable {

        ///////////////////////////////////////////////////////////////////////
        private int _brightness = -1;
        public int Brightness {
            get { return _brightness; }

            set {
                if ((value < 0) || (value > 100)) {
                    throw new ArgumentOutOfRangeException();
                }

                _brightness = value;
            }
        }

        ///////////////////////////////////////////////////////////////////////
        public void TurnOn() {
        }

        ///////////////////////////////////////////////////////////////////////
        public void TurnOff() {
        }
    }
}
