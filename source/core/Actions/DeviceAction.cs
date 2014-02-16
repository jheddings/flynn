// =============================================================================
//  Copyright © Jason Heddings, All Rights Reserved
//  $Id: DeviceAction.cs 82 2013-11-06 22:04:47Z jheddings $
// =============================================================================
using System;

namespace Flynn.Core.Actions {
    public abstract class DeviceAction : ActionBase {

		public IDevice Device { get; set; }

        ///////////////////////////////////////////////////////////////////////
		protected DeviceAction() {
        }

        ///////////////////////////////////////////////////////////////////////
		protected DeviceAction(IDevice device) {
            Device = device;
        }
    }
}
