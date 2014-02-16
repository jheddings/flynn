// =============================================================================
//  Copyright © Jason Heddings, All Rights Reserved
//  $Id: FadeAction.cs 82 2013-11-06 22:04:47Z jheddings $
// =============================================================================
using System;

// TODO add support for different fade types, i.e. LINEAR, EXP_ACCEL, EXP_DECEL

namespace Flynn.Core.Actions {
    public sealed class FadeAction : ActionBase {

		public IDimmable Device { get; set; }

		public int Duration { get; set; }

        ///////////////////////////////////////////////////////////////////////
        private int _start = 0;
        public int FadeStart {
            get { return _start; }

            set {
                if ((value < 0) || (value > 100)) {
                    throw new ArgumentOutOfRangeException();
                }
                _start = value;
            }
        }
        
        ///////////////////////////////////////////////////////////////////////
        private int _stop = 100;
        public int FadeStop {
            get { return _stop; }

            set {
                if ((value < 0) || (value > 100)) {
                    throw new ArgumentOutOfRangeException();
                }
                _stop = value;
            }
        }
        
        ///////////////////////////////////////////////////////////////////////
        public FadeAction() {
        }

        ///////////////////////////////////////////////////////////////////////
        public FadeAction(int start, int stop) {
            _start = start;
            _stop = stop;
        }

        ///////////////////////////////////////////////////////////////////////
        public FadeAction(int start, int stop, int duration_sec) {
            _start = start;
            _stop = stop;
            Duration = duration_sec;
        }

        ///////////////////////////////////////////////////////////////////////
        protected override void PerformAction() {
            throw new NotImplementedException();
        }
    }
}
