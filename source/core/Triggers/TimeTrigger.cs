//=============================================================================
// Copyright © Jason Heddings, All Rights Reserved
// $Id: TimeTrigger.cs 83 2013-11-06 23:34:18Z jheddings $
//=============================================================================
using System;
using Flynn.Utilities;

// represents a trigger that will fire at a specific time; subclasses must
// implement CalcNextTime, which will be used by this class to determine when
// to notify delegates; this method is only called as-needed (using DirtyBit)

namespace Flynn.Core.Triggers {
    public abstract class TimeTrigger : MonitoredTrigger {

        private static readonly Logger _logger = Logger.Get(typeof(TimeTrigger));

        private readonly Random _rand = new Random();
        private readonly DirtyBit _dirtybit = new DirtyBit(true);
        
        // keeps track of the original time returned from the trigger; needed
        // to properly track "next time" when negative offsets are in use
        private DateTime _timebase = DateTime.Now;

        ///////////////////////////////////////////////////////////////////////
        private DateTime _next = DateTime.MaxValue;
        public DateTime NextTime {
            get {
                UpdateNext();
                return _next;
            }
        }

        ///////////////////////////////////////////////////////////////////////
        private bool _randomize = false;
        public bool Randomize {
            get { return _randomize; }

            set {
                if (_randomize != value) {
                    _randomize = value;
                    _dirtybit.Set();
                }
            }
        }

        ///////////////////////////////////////////////////////////////////////
        private int _offset = 0;
        public int Offset {
            get { return _offset; }

            set {
                if (_offset != value) {
                    _offset = value;
                    _dirtybit.Set();
                }
            }
        }

        ///////////////////////////////////////////////////////////////////////
        protected sealed override bool Expired {
            get { return (NextTime <= DateTime.Now); }
        }

        ///////////////////////////////////////////////////////////////////////
        protected TimeTrigger() {
            // after firing, _next needs to be recalculated
            PostFire += delegate { _dirtybit.Set(); };
        }

        ///////////////////////////////////////////////////////////////////////
		protected TimeTrigger(int offset) : this() {
            _offset = offset;
        }

        ///////////////////////////////////////////////////////////////////////
        protected abstract DateTime CalcNextTime(DateTime timebase);

        ///////////////////////////////////////////////////////////////////////
        private void UpdateNext() {
            _dirtybit.Update(UnsafeUpdate);
        }

        ///////////////////////////////////////////////////////////////////////
        private void UnsafeUpdate() {
			var calc = CalcNextTime(_timebase);
            
            if (calc < DateTime.MaxValue) {
                _next = ApplyOffset(calc);
                _timebase = MathFu.Max(_next, calc);
            } else {
                _next = DateTime.MaxValue;
                _timebase = DateTime.Now;
            }
            
            _logger.Debug("next: {0}", _next);
        }
        
        ///////////////////////////////////////////////////////////////////////
        private DateTime ApplyOffset(DateTime dt) {
            int offset = _offset;

            if (_randomize) {
                if (offset < 0) {
                    offset = _rand.Next(offset, 0);
                } else if (offset > 0) {
                    offset = _rand.Next(0, offset);
                } else {
                    // XXX should we be able to randomize w/out an offset?
                    // maybe a config setting specifies the range (e.g. +/- 5)
                }
            }

            // XXX note that offsets are only supported in minutes...

            return dt.AddMinutes(offset);
        }
    }
}
