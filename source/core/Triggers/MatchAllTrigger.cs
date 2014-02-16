//=============================================================================
// Copyright © Jason Heddings, All Rights Reserved
// $Id: MatchAllTrigger.cs 83 2013-11-06 23:34:18Z jheddings $
//=============================================================================
using System;
using System.Collections.Generic;
using Flynn.Core.Properties;

// because we depend on the accurate reporting of time to determine that two
// triggers fire "at once", there may be missed events due to timing issues
// in general, a better solution would be to use filters to exclude events

namespace Flynn.Core.Triggers {
    public sealed class MatchAllTrigger : CompositeTrigger {

		private static readonly int kThreshold = Settings.Default.TriggerSyncThreshold_Ms;

        ///////////////////////////////////////////////////////////////////////
        protected override bool IsReadyToFire(ITrigger current, List<ITrigger> triggers) {
			var now = DateTime.Now;

            // make sure all sub-triggers have fired within the threshold period

			foreach (var trigger in triggers) {
                if (current == trigger) {
                    continue;
                }

                var diff = now - trigger.LastTime;

				if (diff.TotalMilliseconds > kThreshold) {
                    return false;
                }
            }

            return true;
        }
    }
}
