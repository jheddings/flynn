//=============================================================================
// Copyright © Jason Heddings, All Rights Reserved
// $Id: ITrigger.cs 81 2013-11-06 04:44:58Z jheddings $
//=============================================================================
using System;

namespace Flynn.Core {
    public interface ITrigger {

        ///////////////////////////////////////////////////////////////////////
        event EventHandler Fire;

        ///////////////////////////////////////////////////////////////////////
        DateTime LastTime { get; }
    }
}
