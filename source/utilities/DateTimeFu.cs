//=============================================================================
// Copyright © Jason Heddings, All Rights Reserved
// $Id: DateTimeFu.cs 81 2013-11-06 04:44:58Z jheddings $
//=============================================================================
using System;

namespace Flynn.Utilities {
    public static class DateTimeFu {

        public static readonly DateTime Epoch =
            new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        ///////////////////////////////////////////////////////////////////////
        public static DateTime SetMillisecond(this DateTime dt, int msec) {
            if ((msec < 0) || (msec > 999)) {
                throw new ArgumentOutOfRangeException("msec");
            }

            int diff = msec - dt.Millisecond;
            return dt.AddMilliseconds(diff);
        }

        ///////////////////////////////////////////////////////////////////////
        public static DateTime SetSecond(this DateTime dt, int second) {
            if ((second < 0) || (second > 59)) {
                throw new ArgumentOutOfRangeException("second");
            }

            int diff = second - dt.Second;
            return dt.AddSeconds(diff);
        }

        ///////////////////////////////////////////////////////////////////////
        public static DateTime SetMinute(this DateTime dt, int minute) {
            if ((minute < 0) || (minute > 59)) {
                throw new ArgumentOutOfRangeException("minute");
            }

            int diff = minute - dt.Minute;
            return dt.AddMinutes(diff);
        }

        ///////////////////////////////////////////////////////////////////////
        public static DateTime SetHour(this DateTime dt, int hour) {
            if ((hour < 0) || (hour > 23)) {
                throw new ArgumentOutOfRangeException("hour");
            }

            int diff = hour - dt.Hour;
            return dt.AddHours(diff);
        }

        ///////////////////////////////////////////////////////////////////////
        public static DateTime SetDayOfMonth(this DateTime dt, int day) {
            // TODO valid day range depends on month & leap year

            if ((day < 1) || (day > 31)) {
                throw new ArgumentOutOfRangeException("day");
            }

            int diff = day - dt.Day;
            return dt.AddDays(diff);
        }

        ///////////////////////////////////////////////////////////////////////
        public static DateTime SetMonth(this DateTime dt, int month) {
            if ((month < 1) || (month > 12)) {
                throw new ArgumentOutOfRangeException("month");
            }

            int diff = month - dt.Month;
            return dt.AddMonths(diff);
        }

        ///////////////////////////////////////////////////////////////////////
        public static DateTime SetYear(this DateTime dt, int year) {
            if ((year < 0) || (year > 9999)) {
                throw new ArgumentOutOfRangeException("year");
            }

            int diff = year - dt.Year;
            return dt.AddYears(diff);
        }

        ///////////////////////////////////////////////////////////////////////
        public static DateTime SetTime(this DateTime dt, TimeSpan time) {
            TimeSpan diff = time - dt.TimeOfDay;
            return dt.Add(diff);
        }

        ///////////////////////////////////////////////////////////////////////
        public static DateTime NextYear(this DateTime date) {
            DateTime next = date.AddYears(1);
            next = next.SetMonth(1);
            next = next.SetDayOfMonth(1);
            next = next.SetHour(0);
            next = next.SetMinute(0);
            next = next.SetSecond(0);
            next = next.SetMillisecond(0);
            return next;
        }

        ///////////////////////////////////////////////////////////////////////
        public static DateTime NextMonth(this DateTime date) {
            DateTime next = date.AddMonths(1);
            next = next.SetDayOfMonth(1);
            next = next.SetHour(0);
            next = next.SetMinute(0);
            next = next.SetSecond(0);
            next = next.SetMillisecond(0);
            return next;
        }

        ///////////////////////////////////////////////////////////////////////
        public static DateTime NextDay(this DateTime date) {
            DateTime next = date.AddDays(1);
            next = next.SetHour(0);
            next = next.SetMinute(0);
            next = next.SetSecond(0);
            next = next.SetMillisecond(0);
            return next;
        }

        ///////////////////////////////////////////////////////////////////////
        public static DateTime NextHour(this DateTime date) {
            DateTime next = date.AddHours(1);
            next = next.SetMinute(0);
            next = next.SetSecond(0);
            next = next.SetMillisecond(0);
            return next;
        }

        ///////////////////////////////////////////////////////////////////////
        public static DateTime NextMinute(this DateTime date) {
            DateTime next = date.AddMinutes(1);
            next = next.SetSecond(0);
            next = next.SetMillisecond(0);
            return next;
        }

        ///////////////////////////////////////////////////////////////////////
        public static DateTime NextSecond(this DateTime date) {
            DateTime next = date.AddSeconds(1);
            next = next.SetMillisecond(0);
            return next;
        }

        ///////////////////////////////////////////////////////////////////////
        public static DateTime FromUnixTime(long time) {
            return Epoch.AddSeconds(time);
        }

        ///////////////////////////////////////////////////////////////////////
        public static long ToUnixTime(this DateTime date) {
            DateTime utc = date.ToUniversalTime();
            return Convert.ToInt64((utc - Epoch).TotalSeconds);
        }
    }
}
