//=============================================================================
// Copyright © Jason Heddings, All Rights Reserved
// $Id: TriggerBase.cs 83 2013-11-06 23:34:18Z jheddings $
//=============================================================================
using System;
using System.Threading;
using Flynn.Core.Properties;
using Flynn.Utilities;

// TODO it would be really hepful to add a name to logger statements...

namespace Flynn.Core.Triggers {
    public abstract class TriggerBase : ITrigger {

        private static readonly Logger _logger = Logger.Get(typeof(TriggerBase));

        public event EventHandler PreFire;
        public event EventHandler Fire;
        public event EventHandler PostFire;

		///////////////////////////////////////////////////////////////////////
		public String Name { get; set; }
        
        ///////////////////////////////////////////////////////////////////////
        private DateTime _lastTime = DateTime.MinValue;
        public DateTime LastTime {
            get { return _lastTime; }
        }

        ///////////////////////////////////////////////////////////////////////
        private int _count = 0;
        public int FiredCount {
            get { return _count; }
        }

        ///////////////////////////////////////////////////////////////////////
        public override String ToString() {
            return String.Format("[Trigger: {0}]", Name);
        }

        ///////////////////////////////////////////////////////////////////////
        protected void NotifyDelegates() {
            _logger.Info("trigger fired");

            if (Settings.Default.TriggerAsyncFire) {
                NotifyAsync();
            } else {
                NotifySync();
            }

            _count++;
            _lastTime = DateTime.Now;
        }

        ///////////////////////////////////////////////////////////////////////
        private void NotifySync() {
            if (PreFire != null) {
                PreFire(this, EventArgs.Empty);
            }

            if (Fire != null) {
                Fire(this, EventArgs.Empty);
            }

            if (PostFire != null) {
                PostFire(this, EventArgs.Empty);
            }
        }

        ///////////////////////////////////////////////////////////////////////
        private void NotifyAsync() {
            ThreadPool.QueueUserWorkItem(
                delegate { NotifySync(); }
            );
        }
    }
}
