// =============================================================================
//  Copyright © Jason Heddings, All Rights Reserved
//  $Id: IInsteonRemote.cs 85 2013-11-11 16:38:38Z jheddings $
// =============================================================================
using System;

namespace Flynn.X10 {
    public interface IInsteonRemote {
        void Send(InsteonCommand cmd);
    }
}
