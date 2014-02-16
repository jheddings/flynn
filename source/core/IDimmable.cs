// =============================================================================
//  Copyright © Jason Heddings, All Rights Reserved
//  $Id: IDimmable.cs 82 2013-11-06 22:04:47Z jheddings $
// =============================================================================
using System;

namespace Flynn.Core {
    public interface IDimmable {

        ///////////////////////////////////////////////////////////////////////
        // the current brightness from 0-100 (negative is considered "unknown")
        int Brightness { get; set; }
    }
}
