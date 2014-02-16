//=============================================================================
// Copyright © Jason Heddings, All Rights Reserved
// $Id: CompositeTrigger.cs 84 2013-11-11 16:35:56Z jheddings $
//=============================================================================
using System;
using System.Collections.Generic;
using Flynn.Utilities;

namespace Flynn.Core.Triggers {
    public abstract class CompositeTrigger : TriggerBase, IDisposable {

        private static readonly Logger _logger = Logger.Get(typeof(CompositeTrigger));

		private readonly List<ITrigger> _triggers = new List<ITrigger>();

        ///////////////////////////////////////////////////////////////////////
        public void Add(ITrigger trigger) {
            lock (_triggers) {
				trigger.Fire += SubTrigger_OnFire;
                _triggers.Add(trigger);
            }
        }

        ///////////////////////////////////////////////////////////////////////
        public void Dispose() {
            lock (_triggers) {
				foreach (var trigger in _triggers) {
					trigger.Fire -= SubTrigger_OnFire;
                }
            }

			_triggers.Clear();
        }

        ///////////////////////////////////////////////////////////////////////
        protected abstract bool IsReadyToFire(ITrigger current, List<ITrigger> triggers);

        ///////////////////////////////////////////////////////////////////////
        private void SubTrigger_OnFire(Object sender, EventArgs e) {
			var trigger = sender as ITrigger;

            if (trigger == null) {
                _logger.Warn("unknown sender: {0}", sender.GetType());
                return;
            }

            _logger.Debug("sub-trigger fired");

            bool fire = false;

            lock (_triggers) {
                fire = IsReadyToFire(trigger, _triggers);
            }

            if (fire) {
                NotifyDelegates();
            }
        }
    }
}
