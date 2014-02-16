//=============================================================================
// Copyright © Jason Heddings, All Rights Reserved
// $Id: DelayedAction.cs 82 2013-11-06 22:04:47Z jheddings $
//=============================================================================
using System;
using System.Threading;
using Flynn.Utilities;

// note that a delayed action depends on the resolution of the system timers

// TODO support aborting a delayed action, i.e. another trigger overrides it

namespace Flynn.Core.Actions {
    public class DelayedAction : ActionBase, IDisposable {

        private static readonly Logger _logger = Logger.Get(typeof(DelayedAction));

        private readonly Timer _timer;

        ///////////////////////////////////////////////////////////////////////
        public int DelaySec { get; set; }
        public IAction Action { get; set; }

        ///////////////////////////////////////////////////////////////////////
        public DelayedAction() {
            _timer = new Timer(new TimerCallback(Callback));
        }

        ///////////////////////////////////////////////////////////////////////
        public void Dispose() {
            _timer.Dispose();
        }

        ///////////////////////////////////////////////////////////////////////
        protected override void PerformAction() {
            _timer.Change(DelaySec * 1000, Timeout.Infinite);
        }

        ///////////////////////////////////////////////////////////////////////
        private void Callback(Object state) {
			if (Action == null) {
				_logger.Warn("no action specified");
			}

			try {
                Action.Invoke();
            } catch (Exception e) {
                _logger.Error(e);
            }
        }
    }
}
