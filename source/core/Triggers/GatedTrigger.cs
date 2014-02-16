//=============================================================================
// Copyright © Jason Heddings, All Rights Reserved
// $Id: GatedTrigger.cs 83 2013-11-06 23:34:18Z jheddings $
//=============================================================================
using System.Collections.Generic;

// XXX there may be some weirdness here since we are using the internals of the
// ITrigger class to determine the firing state... if triggers are being fired
// asyncronously, we may encounter unexpected behavior.  --jah

namespace Flynn.Core.Triggers {
    public sealed class GatedTrigger : CompositeTrigger {

        ///////////////////////////////////////////////////////////////////////
        protected override bool IsReadyToFire(ITrigger current, List<ITrigger> triggers) {

            // check that all sub-triggers have fired since we last fired

			foreach (var trigger in triggers) {
                if (current == trigger) {
                    continue;
                }

                if (trigger.LastTime <= LastTime) {
                    return false;
                }
            }

            return true;
        }
    }
}
