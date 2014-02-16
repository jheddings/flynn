// =============================================================================
//  Copyright © Jason Heddings, All Rights Reserved
//  $Id: IDevice.cs 82 2013-11-06 22:04:47Z jheddings $
// =============================================================================
using System;

// TODO support DeviceStatus, i.e. ON, OFF, UNKNOWN

namespace Flynn.Core {
    public interface IDevice {

        ///////////////////////////////////////////////////////////////////////
        void TurnOn();

        ///////////////////////////////////////////////////////////////////////
        void TurnOff();
    }
}
