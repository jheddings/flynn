//=============================================================================
// Copyright © Jason Heddings, All Rights Reserved
// $Id: ManualTrigger.cs 83 2013-11-06 23:34:18Z jheddings $
//=============================================================================
using System;

// defines a trigger that will only fire when invoked manually

namespace Flynn.Core.Triggers {
    public sealed class ManualTrigger : TriggerBase {

        ///////////////////////////////////////////////////////////////////////
        public void FireOne() {
            NotifyDelegates();
        }

        // TODO may be useful for testing
        // public void FireAtWill(int min, int max);  // begin firing at random intervals
        // public void FireRepeat(int invervalMs);  // begin firing at speficied interval
        // public void CeaseFire();  // stop any firing threads
    }
}
