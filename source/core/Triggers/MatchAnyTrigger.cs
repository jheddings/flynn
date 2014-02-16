//=============================================================================
// Copyright © Jason Heddings, All Rights Reserved
// $Id: MatchAnyTrigger.cs 83 2013-11-06 23:34:18Z jheddings $
//=============================================================================
using System.Collections.Generic;

namespace Flynn.Core.Triggers {
    public sealed class MatchAnyTrigger : CompositeTrigger {

        ///////////////////////////////////////////////////////////////////////
        protected override bool IsReadyToFire(ITrigger current, List<ITrigger> triggers) {
            return true;
        }
    }
}
