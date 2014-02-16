//=============================================================================
// Copyright © Jason Heddings, All Rights Reserved
// $Id: CronSet.cs 83 2013-11-06 23:34:18Z jheddings $
//=============================================================================
using System;
using System.Collections.Generic;
using System.Text;

// a CronSet may contain multiple ranges, each separated by a comma

namespace Flynn.Cron {
    public sealed class CronSet {

        private readonly List<CronRange> _ranges = new List<CronRange>();

        ///////////////////////////////////////////////////////////////////////
        public List<CronRange> Ranges {
            get { return _ranges; }
        }

        ///////////////////////////////////////////////////////////////////////
        public bool Contains(params int[] values) {
			// XXX 'twould be nice to improve the readability of this
			foreach (var range in _ranges) {
                foreach (int val in values) {
                    if (range.Contains(val)) {
                        return true;
                    }
                }
            }

            return false;
        }

        ///////////////////////////////////////////////////////////////////////
        public bool Contains(DayOfWeek day) {
			// we recognize Sunday as either day 0 or 7
            if (day == DayOfWeek.Sunday) {
                return Contains(0, 7);
            }

            return (Contains((int) day));
        }

        ///////////////////////////////////////////////////////////////////////
        public override String ToString() {
			var str = new StringBuilder();

            int len = _ranges.Count;
            for (int cur = 0; cur < len; cur++) {
				var range = _ranges[cur];
                str.Append(range.ToString());

                if (cur < (len - 1)) {
                    str.Append(',');
                }
            }

            return str.ToString();
        }

        ///////////////////////////////////////////////////////////////////////
        public static CronSet Parse(String expr) {
			var field = new CronSet();

			var parts = expr.Split(',');
			foreach (var part in parts) {
				var range = CronRange.Parse(part);

                if (range != null) {
                    field.Accept(range);
                }
            }

            return field;
        }

        ///////////////////////////////////////////////////////////////////////
        private void Accept(CronRange range) {
            _ranges.Add(range);
        }
    }
}
