//=============================================================================
// Copyright © Jason Heddings, All Rights Reserved
// $Id: MonitoredTrigger.cs 84 2013-11-11 16:35:56Z jheddings $
//=============================================================================
using System;
using System.Threading;
using Flynn.Core.Properties;
using Flynn.Utilities;

// a MonitoredTrigger is polled once per second; subclasses must implement
// the Expired property, which informst he trigger that it is time to fire

// TODO allow subclasses to determine poll frequency

namespace Flynn.Core.Triggers {
    public abstract class MonitoredTrigger : TriggerBase, IDisposable {

        private static readonly Logger _logger = Logger.Get(typeof(MonitoredTrigger));

		private readonly AutoResetEvent _monitor = new AutoResetEvent(true);

        ///////////////////////////////////////////////////////////////////////
        protected abstract bool Expired { get; }

        ///////////////////////////////////////////////////////////////////////
        protected MonitoredTrigger() {
			WallClock.Instance.SecondTick += OnSecondTick;
        }

        ///////////////////////////////////////////////////////////////////////
        public void Dispose() {
			WallClock.Instance.SecondTick -= OnSecondTick;
        }

        ///////////////////////////////////////////////////////////////////////
        private void OnSecondTick(Object sender, EventArgs args) {

            // subclasses may take an extended period of time in Expired.get
            // and delegates make take a while to complete the notification, so
            // we will give the lock a few msec to prefent our handler from
            // hanging up the WallClock

            if (_monitor.WaitOne(Settings.Default.TriggerBusyWait_Ms)) {
                UnsafeTick();
                _monitor.Set();
            } else {
                _logger.Debug("trigger busy");
            }
        }

        ///////////////////////////////////////////////////////////////////////
        private void UnsafeTick() {
            if (this.Expired) {
                _logger.Debug("trigger expired");

                try {
                    NotifyDelegates();
                } catch (Exception e) {
                    _logger.Error(e);
                }
            }
        }
    }
}
