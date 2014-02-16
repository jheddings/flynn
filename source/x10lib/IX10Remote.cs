// =============================================================================
//  Copyright © Jason Heddings, All Rights Reserved
//  $Id: IX10Remote.cs 81 2013-11-06 04:44:58Z jheddings $
// =============================================================================
using System;

namespace Flynn.X10 {
    public interface IX10Remote {
        void Send(X10Command cmd);
    }
}
