// =============================================================================
//  Copyright © Jason Heddings, All Rights Reserved
//  $Id: DeviceBase.cs 83 2013-11-06 23:34:18Z jheddings $
// =============================================================================
using System;

namespace Flynn.Core.Devices {
    public abstract class DeviceBase : IDevice {

		public String Name { get; set; }

        ///////////////////////////////////////////////////////////////////////
        public abstract void TurnOn();
        public abstract void TurnOff();

        ///////////////////////////////////////////////////////////////////////
        public override String ToString() {
            return String.Format("[Device: {0}]", Name);
        }
    }
}
