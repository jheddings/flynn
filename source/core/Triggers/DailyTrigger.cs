//=============================================================================
// Copyright © Jason Heddings, All Rights Reserved
// $Id: DailyTrigger.cs 84 2013-11-11 16:35:56Z jheddings $
//=============================================================================
using System;
using System.Xml.Serialization;

// provides a trigger at the same time every day

namespace Flynn.Core.Triggers {

    [XmlRoot("DailyTrigger", Namespace = "Flynn.Core.Triggers")]
    public sealed class DailyTrigger : TimeTrigger {

        ///////////////////////////////////////////////////////////////////////
		public TimeSpan Time { get; set; }

        ///////////////////////////////////////////////////////////////////////
        public DailyTrigger(int hour, int minute) {
            Time = new TimeSpan(hour, minute, 0);
        }

        ///////////////////////////////////////////////////////////////////////
        public DailyTrigger(int hour, int minute, int second) {
            Time = new TimeSpan(hour, minute, second);
        }

        ///////////////////////////////////////////////////////////////////////
        public DailyTrigger(TimeSpan time) {
            Time = time;
        }

        ///////////////////////////////////////////////////////////////////////
        protected override DateTime CalcNextTime(DateTime timebase) {
            // start at the expected time today and move to tomorrow if needed

			var next = new DateTime(
                timebase.Year, timebase.Month, timebase.Day,
                Time.Hours, Time.Minutes, Time.Seconds
            );

            while (next <= timebase) {
                next = next.AddDays(1);
            }

            return next;
        }
    }
}
