//=============================================================================
// Copyright © Jason Heddings, All Rights Reserved
// $Id: CalendarFilter.cs 84 2013-11-11 16:35:56Z jheddings $
//=============================================================================
using System;
using System.Collections.Generic;
using System.Text;
using Flynn.Utilities;

// provides a typed interface for a cron-like filter
namespace Flynn.Core.Filters {
	public class CalendarFilter : FilterBase {

		private static readonly Logger _logger = Logger.Get(typeof(CalendarFilter));

		///////////////////////////////////////////////////////////////////////
		private readonly List<int> _seconds = new List<int>();

		public List<int> Seconds {
			get { return _seconds; }
		}

		///////////////////////////////////////////////////////////////////////
		private readonly List<int> _minutes = new List<int>();

		public List<int> Minutes {
			get { return _minutes; }
		}

		///////////////////////////////////////////////////////////////////////
		private readonly List<int> _hours = new List<int>();

		public List<int> Hours {
			get { return _hours; }
		}

		///////////////////////////////////////////////////////////////////////
		private readonly List<int> _daysOfMonth = new List<int>();

		public List<int> DaysOfTheMonth {
			get { return _daysOfMonth; }
		}

		///////////////////////////////////////////////////////////////////////
		private readonly List<DayOfWeek> _daysOfWeek = new List<DayOfWeek>();

		public List<DayOfWeek> DaysOfTheWeek {
			get { return _daysOfWeek; }
		}

		///////////////////////////////////////////////////////////////////////
		private readonly List<int> _months = new List<int>();

		public List<int> Months {
			get { return _months; }
		}

		///////////////////////////////////////////////////////////////////////
		private readonly List<int> _years = new List<int>();

		public List<int> Years {
			get { return _years; }
		}

		///////////////////////////////////////////////////////////////////////
		public override bool Accept(DateTime when) {
			_logger.Debug("accept: {0} :: {1} ?", this, when);

			if ((_seconds.Count > 0) && (!_seconds.Contains(when.Second))) {
				return false;
			}

			if ((_minutes.Count > 0) && (!_minutes.Contains(when.Minute))) {
				return false;
			}

			if ((_hours.Count > 0) && (!_hours.Contains(when.Hour))) {
				return false;
			}

			if ((_daysOfMonth.Count > 0) && (!_daysOfMonth.Contains(when.Day))) {
				return false;
			}

			if ((_daysOfWeek.Count > 0) && (!_daysOfWeek.Contains(when.DayOfWeek))) {
				return false;
			}

			if ((_months.Count > 0) && (!_months.Contains(when.Month))) {
				return false;
			}

			if ((_years.Count > 0) && (!_years.Contains(when.Year))) {
				return false;
			}

			return true;
		}

		///////////////////////////////////////////////////////////////////////
		public override string ToString() {
			var str = new StringBuilder("{");

			str.Append(ToString(_seconds)).Append(' ');
			str.Append(ToString(_minutes)).Append(' ');
			str.Append(ToString(_hours)).Append(' ');
			str.Append(ToString(_daysOfMonth)).Append(' ');
			str.Append(ToString(_daysOfWeek)).Append(' ');
			str.Append(ToString(_months)).Append(' ');
			str.Append(ToString(_years));

			str.Append('}');
			return str.ToString();
		}

		///////////////////////////////////////////////////////////////////////
		private static String ToString<T>(List<T> list) {
			var str = new StringBuilder();

			if (list.Count > 0) {
				str.Append(list[0]);
				for (int idx = 1; idx < list.Count; idx++) {
					str.Append(',').Append(list[idx]);
				}
			} else {
				str.Append('*');
			}

			return str.ToString();
		}
	}
}
