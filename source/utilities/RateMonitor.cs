//=============================================================================
// Copyright © Jason Heddings, All Rights Reserved
// $Id: RateMonitor.cs 81 2013-11-06 04:44:58Z jheddings $
//=============================================================================
using System;

// collects information for actions at regular intervals

namespace Flynn.Utilities {
    public sealed class RateMonitor {

        ///////////////////////////////////////////////////////////////////////
        private bool _first = true;
        public bool FirstTick {
            get { return _first; }
        }

        ///////////////////////////////////////////////////////////////////////
        private DateTime _lastPulse = DateTime.MinValue;
        public DateTime LastPulse {
            get { return _lastPulse; }
        }

        ///////////////////////////////////////////////////////////////////////
        private int _rate = 0;
        public int Rate {
            get { return _rate; }
        }

        ///////////////////////////////////////////////////////////////////////
        private int _drift = 0;
        public int Drift {
            get { return _drift; }
        }

        ///////////////////////////////////////////////////////////////////////
        private int _skip = 0;
        public int Skip {
            get { return _skip; }
        }

        ///////////////////////////////////////////////////////////////////////
        public RateMonitor() {
        }

        ///////////////////////////////////////////////////////////////////////
        public void Pulse() {
            DateTime now = DateTime.Now;

            if (this.FirstTick) {
                FirstUpdate(now);
            } else {
                NormalUpdate(now);
            }

            _lastPulse = now;
        }

        ///////////////////////////////////////////////////////////////////////
        private void FirstUpdate(DateTime date) {
            _first = false;
        }

        ///////////////////////////////////////////////////////////////////////
        private void NormalUpdate(DateTime date) {
            // FIXME skips should be general, not second-specific
            _skip = date.Second - _lastPulse.Second;
            while (_skip < 0) { _skip += 60;  }

            // TODO update drift

            // TODO update rate
        }
    }
}
