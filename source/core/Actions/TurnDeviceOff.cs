// =============================================================================
//  Copyright © Jason Heddings, All Rights Reserved
//  $Id: TurnDeviceOff.cs 82 2013-11-06 22:04:47Z jheddings $
// =============================================================================
using Flynn.Utilities;

namespace Flynn.Core.Actions {
    public sealed class TurnDeviceOff : DeviceAction {

        private static readonly Logger _logger = Logger.Get(typeof(TurnDeviceOff));

        ///////////////////////////////////////////////////////////////////////
        public TurnDeviceOff() {
        }

        ///////////////////////////////////////////////////////////////////////
        public TurnDeviceOff(IDevice device) : base(device) {
        }
        
        ///////////////////////////////////////////////////////////////////////
        protected override void PerformAction() {
            IDevice device = Device;
            if (device == null) { return; }

            _logger.Info("device off: {0}", device);
            
            device.TurnOff();
        }
    }
}
