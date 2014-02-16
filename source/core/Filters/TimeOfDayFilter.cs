//=============================================================================
// Copyright © Jason Heddings, All Rights Reserved
// $Id: TimeOfDayFilter.cs 84 2013-11-11 16:35:56Z jheddings $
//=============================================================================
using System;
using Flynn.Utilities;

// filters based on a valid time window during any given day

// if the minimum is larger than the maximum, the range is assumed to cross midnight

namespace Flynn.Core.Filters {
    public sealed class TimeOfDayFilter : FilterBase {

		private static readonly Logger _logger = Logger.Get(typeof(TimeOfDayFilter));

        ///////////////////////////////////////////////////////////////////////
        private TimeSpan _start = new TimeSpan(0, 0, 0, 0, 0);
        public TimeSpan Start {
            get { return _start; }

            set {
                if ((value.TotalHours < 0) || (value.TotalHours >= 24)) {
                    throw new ArgumentOutOfRangeException("value", "value must be within one day");
                }

                _start = value;
            }
        }

        ///////////////////////////////////////////////////////////////////////
        private TimeSpan _stop = new TimeSpan(0, 23, 59, 59, 999);
        public TimeSpan Stop {
            get { return _stop; }

            set {
                if ((value.TotalHours < 0) || (value.TotalHours >= 24)) {
                    throw new ArgumentOutOfRangeException("value", "value must be within one day");
                }

                _stop = value;
            }
        }

        ///////////////////////////////////////////////////////////////////////
        public TimeOfDayFilter() {
        }

        ///////////////////////////////////////////////////////////////////////
        public TimeOfDayFilter(TimeSpan start, TimeSpan stop) {
            _start = start;
            _stop = stop;
        }

        ///////////////////////////////////////////////////////////////////////
        public bool Accept(TimeSpan when) {
            _logger.Debug("accept: {1:g} - {2:g} :: {3:g}", _start, _stop, when);

            if (_start > _stop) {
                return ((when >= _start) || (when <= _stop));
            }

            return ((when >= _start) && (when <= _stop));
        }

        ///////////////////////////////////////////////////////////////////////
		public override bool Accept(DateTime when) {
            return Accept(when.TimeOfDay);
        }
    }
}
