// =============================================================================
//  Copyright © Jason Heddings, All Rights Reserved
//  $Id: TurnDeviceOn.cs 82 2013-11-06 22:04:47Z jheddings $
// =============================================================================
using Flynn.Utilities;

namespace Flynn.Core.Actions {
    public sealed class TurnDeviceOn : DeviceAction {

        private static readonly Logger _logger = Logger.Get(typeof(TurnDeviceOn));
        
        ///////////////////////////////////////////////////////////////////////
        public TurnDeviceOn() {
        }
        
        ///////////////////////////////////////////////////////////////////////
        public TurnDeviceOn(IDevice device) : base(device) {
        }

        ///////////////////////////////////////////////////////////////////////
        protected override void PerformAction() {
            IDevice device = Device;
            if (device == null) { return; }

            _logger.Info("device on: {0}", device);

            device.TurnOn();
        }
    }
}
