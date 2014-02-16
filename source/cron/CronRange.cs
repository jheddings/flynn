//=============================================================================
// Copyright © Jason Heddings, All Rights Reserved
// $Id: CronRange.cs 83 2013-11-06 23:34:18Z jheddings $
//=============================================================================
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

// TODO need to ensure Maximum >= Minimum

// although not very useful in cron expressions, negative values are supported

namespace Flynn.Cron {
    public sealed class CronRange {

        // matches a wildcard with an optional step
        public static readonly Regex WildcardRE =
            new Regex(@"\*(/(?<step>\d+))?");

        // matches a range with an optional step
        public static readonly Regex RangeRE =
            new Regex(@"(?<begin>-?\d+)-(?<end>-?\d+)(/(?<step>\d+))?");

        // matches a single, literal value with optional increment
        public static readonly Regex LiteralRE =
            new Regex(@"(?<value>-?\d+)(/(?<step>\d+))?");

		///////////////////////////////////////////////////////////////////////
		public long Minimum { get; set; }

		///////////////////////////////////////////////////////////////////////
		public long Maximum { get; set; }

        ///////////////////////////////////////////////////////////////////////
        private uint _step = 1;
        public uint Step {
            get { return _step; }

            set {
                if (value <= 0) {
                    throw new ArgumentException("step must be greater than zero");
                }

                _step = value;
            }
        }

        ///////////////////////////////////////////////////////////////////////
        public CronRange() {
        }

        ///////////////////////////////////////////////////////////////////////
        public CronRange(long value) : this(value, value, 1) {
        }

        ///////////////////////////////////////////////////////////////////////
        public CronRange(long min, long max) : this(min, max, 1) {
        }

        ///////////////////////////////////////////////////////////////////////
        public CronRange(long min, long max, uint step) : this() {
            Minimum = min;
            Maximum = max;
            _step = step;
        }

        ///////////////////////////////////////////////////////////////////////
        public bool Contains(int value) {
            return Contains((long) value);
        }

        ///////////////////////////////////////////////////////////////////////
        public bool Contains(uint value) {
            return Contains((long) value);
        }

        ///////////////////////////////////////////////////////////////////////
        public bool Contains(long value) {
            // check the range (inclusive)
            if (value < Minimum) { return false; }
            if (value > Maximum) { return false; }

            // ensure the value falls on an increment in the range
            if (((value - Minimum) % _step) != 0) {
                return false;
            }

            return true;
        }

        ///////////////////////////////////////////////////////////////////////
        public override String ToString() {
			var str = new StringBuilder();

            if ((Minimum == long.MinValue) && (Maximum == long.MaxValue)) {
                str.Append('*');

            } else if (Minimum != Maximum) {
                str.Append(Minimum);

                if (Maximum != long.MaxValue) {
                    str.Append('-').Append(Maximum);
                }
            } else {
                str.Append(Minimum);
            }

            if (_step > 1) {
                str.Append('/').Append(_step);
            }

            return str.ToString();
        }

        ///////////////////////////////////////////////////////////////////////
        public static CronRange Parse(String expr) {
			var set = new CronRange();

            Match match = null;

            if ((match = WildcardRE.Match(expr)).Success) {
                set.Minimum = long.MinValue;
                set.Maximum = long.MaxValue;

                if (match.Groups["step"].Success) {
                    set.Step = uint.Parse(match.Groups["step"].Value);
                }

            } else if ((match = RangeRE.Match(expr)).Success) {
                set.Minimum = long.Parse(match.Groups["begin"].Value);
                set.Maximum = long.Parse(match.Groups["end"].Value);

                if (match.Groups["step"].Success) {
                    set.Step = uint.Parse(match.Groups["step"].Value);
                }

            } else if ((match = LiteralRE.Match(expr)).Success) {
                set.Minimum = long.Parse(match.Groups["value"].Value);

                if (match.Groups["step"].Success) {
                    set.Maximum = long.MaxValue;
                    set.Step = uint.Parse(match.Groups["step"].Value);
                } else {
                    set.Maximum = set.Minimum;
                }
            }

            return set;
        }

        ///////////////////////////////////////////////////////////////////////
        private class Enumerator : IEnumerator<long> {

            private long _value;
            private CronRange _range;

            ///////////////////////////////////////////////////////////////////
            public long Current {
                get { return _value; }
            }

            ///////////////////////////////////////////////////////////////////
            Object IEnumerator.Current {
                get { return _value; }
            }

            ///////////////////////////////////////////////////////////////////
            public void Dispose() {
            }

            ///////////////////////////////////////////////////////////////////
            public Enumerator(CronRange range) {
                _range = range;

                Reset();
            }

            ///////////////////////////////////////////////////////////////////
            public bool MoveNext() {
                do {
                    _value++;

                    if (_value > _range.Maximum) {
                        return false;
                    }

                } while (! _range.Contains(_value));

                return true;
            }

            ///////////////////////////////////////////////////////////////////
            public void Reset() {
                _value = _range.Minimum;
            }
        }
    }
}
