//=============================================================================
// Copyright © Jason Heddings, All Rights Reserved
// $Id: CronExpr.cs 83 2013-11-06 23:34:18Z jheddings $
//=============================================================================
using System;
using System.Text;
using Flynn.Cron.Properties;
using Flynn.Utilities;

// provides a simple, lightweight representation of cron expressions
// http://en.wikipedia.org/wiki/CRON_expression

// A B C D E F G
// | | | | | | |
// | | | | | | +--- year (4 digit)
// | | | | | +----- day of week (0 - 6)
// | | | | +------- month (1 - 12)
// | | | +--------- day of month (1 - 31)
// | | +----------- hour (0 - 23)
// | +------------- minute (0 - 59)
// +--------------- second (0 - 59)

// there are a few exceptions to the expressions defined above (and why):
// - Sunday can be either 0 or 7 for "day of week" (legacy)
// - the 'L' or 'W' or '#' operators are not supported - maybe someday
// - text values are not supported (i.e. MON-FRI or JAN-DEC) - maybe someday
// - the '?' value is not supported - use '*' for unrestricted values
// - if there are less than 6 fields, "Seconds" is assumed to be '0' (legacy)
// - any missing fields are assumed to be wildcards (convenience)
// - this implies that a default construction will fire every second (weird)

namespace Flynn.Cron {
    public sealed class CronExpr {

		public CronSet Seconds { get; set; }
		public CronSet Minutes { get; set; }
		public CronSet Hours { get; set; }
		public CronSet DaysOfMonth { get; set; }
		public CronSet DaysOfWeek { get; set; }
		public CronSet Months { get; set; }
		public CronSet Years { get; set; }

        ///////////////////////////////////////////////////////////////////////
        public DateTime Next {
            get { return CalcNextTime(DateTime.Now); }
        }

        ///////////////////////////////////////////////////////////////////////
        public CronExpr() {
        }

        ///////////////////////////////////////////////////////////////////////
        public CronExpr(String expr) {
            String[] parts = expr.Split(new char[] { ' ', '\t', '\r', '\n' },
                                        StringSplitOptions.RemoveEmptyEntries);

            AssignFields(parts);
        }

        ///////////////////////////////////////////////////////////////////////
        public DateTime CalculateNext(DateTime dt) {
            return CalcNextTime(dt);
        }

        ///////////////////////////////////////////////////////////////////////
        public bool Matches(DateTime dt) {
            return Matches(Seconds, dt.Second)
                && Matches(Minutes, dt.Minute)
                && Matches(Hours, dt.Hour)
                && Matches(DaysOfMonth, dt.Day)
                && Matches(DaysOfWeek, dt.DayOfWeek)
                && Matches(Months, dt.Month)
                && Matches(Years, dt.Year);
        }

        ///////////////////////////////////////////////////////////////////////
        public override String ToString() {
			var str = new StringBuilder();

            str.Append(ToString(Seconds)).Append(' ');
            str.Append(ToString(Minutes)).Append(' ');
            str.Append(ToString(Hours)).Append(' ');
            str.Append(ToString(DaysOfMonth)).Append(' ');
            str.Append(ToString(Months)).Append(' ');
            str.Append(ToString(DaysOfWeek)).Append(' ');
            str.Append(ToString(Years));

            return str.ToString();
        }

        ///////////////////////////////////////////////////////////////////////
        private String ToString(CronSet set) {
            return (set == null) ? "*" : set.ToString();
        }

        ///////////////////////////////////////////////////////////////////////
        private void AssignFields(String[] parts) {

            // keep track of where we are
            int part = 0;

            // missing seconds are assumed to be 0
            if (parts.Length < 6) {
                Seconds = CronSet.Parse("0");
                part++;
            }

            for (int idx = 0; idx < parts.Length; idx++) {
                AssignField(part++, parts[idx]);
            }
        }

        ///////////////////////////////////////////////////////////////////////
        private void AssignField(int field, String part) {
			var set = CronSet.Parse(part);

            switch (field) {
                case 0:
                    Seconds = set;
                    break;

                case 1:
                    Minutes = set;
                    break;

                case 2:
                    Hours = set;
                    break;

                case 3:
                    DaysOfMonth = set;
                    break;

                case 4:
                    Months = set;
                    break;

                case 5:
                    DaysOfWeek = set;
                    break;

                case 6:
                    Years = set;
                    break;

                default:
                    throw new ArgumentOutOfRangeException("field");
            }
        }

        ///////////////////////////////////////////////////////////////////////
        // concept taken from http://tinyurl.com/canztx#322058 (loopy version)
        internal DateTime CalcNextTime(DateTime start) {

            // we are looking for the "next" valid time, so the first thing we
            // need to do is move one second beyond the time we were provided
			var next = start.NextSecond();

            // start searching for the next valid time in this expression;
            // anytime a field changes, restart the loop with the new time

            for (int loop = 0; loop < Settings.Default.RetryCount; loop++) {

                // XXX should we break sooner?
                if (next >= DateTime.MaxValue) {
                    break;
                }

                if (! Matches(Years, next.Year)) {
                    next = next.NextYear();
                    continue;
                }

                if (! Matches(Months, next.Month)) {
                    next = next.NextMonth();
                    continue;
                }

                if (! Matches(DaysOfMonth, next.Day)) {
                    next = next.NextDay();
                    continue;
                }

                if (! Matches(DaysOfWeek, next.DayOfWeek)) {
                    next = next.NextDay();
                    continue;
                }

                if (! Matches(Hours, next.Hour)) {
                    next = next.NextHour();
                    continue;
                }

                if (! Matches(Minutes, next.Minute)) {
                    next = next.NextMinute();
                    continue;
                }

                if (! Matches(Seconds, next.Second)) {
                    next = next.NextSecond();
                    continue;
                }

                // we have the next DateTime that matches
                // our cron expression

                return next;
            }

            // we were unable to find a suitable DateTime that
            // satisfies the cron expression

            return DateTime.MaxValue;
        }

        ///////////////////////////////////////////////////////////////////////
        private static bool Matches(CronSet set, int value) {
			return (set == null) || set.Contains(value);
        }

        ///////////////////////////////////////////////////////////////////////
        private static bool Matches(CronSet set, DayOfWeek day) {
			return (set == null) || set.Contains(day);
        }
    }
}
