//=============================================================================
// Copyright © Jason Heddings, All Rights Reserved
// $Id: IFilter.cs 82 2013-11-06 22:04:47Z jheddings $
//=============================================================================
using System;

namespace Flynn.Core {
    public interface IFilter {

        ///////////////////////////////////////////////////////////////////////
        // indicates whether the filter should allow an action at the time
        // this method is called; certain filters may have additional Accept
        // methods that take additional arguments
        bool Accept();
    }
}
