//=============================================================================
// Copyright © Jason Heddings, All Rights Reserved
// $Id: CronExprTest.cs 81 2013-11-06 04:44:58Z jheddings $
//=============================================================================
using System;
using Flynn.Utilities;
using NUnit.Framework;

// TODO test for explicit date / time strings

namespace Flynn.Cron.UnitTest {

    [TestFixture]
    public class CronExprTest {

        public const int LoopCount = 1000;

        ///////////////////////////////////////////////////////////////////////
        [Test]
        public void WalkingEverySecondTest() {
            CronExpr cron = new CronExpr("* * * * * *");
            DateTime when = DateTime.Now.NextSecond();

            for (int loop = 0; loop < LoopCount; loop++) {
                DateTime calc = cron.CalcNextTime(when);

                when = when.AddSeconds(1);

                Assert.AreEqual(when, calc);
                Assert.IsTrue(cron.Matches(when));
            }
        }

        ///////////////////////////////////////////////////////////////////////
        [Test]
        public void WalkingEveryMinuteTest() {
            CronExpr cron = new CronExpr("*");
            DateTime when = DateTime.Now.NextMinute();

            for (int loop = 0; loop < LoopCount; loop++) {
                DateTime calc = cron.CalcNextTime(when);

                when = when.AddMinutes(1);

                Assert.AreEqual(when, calc);
                Assert.IsTrue(cron.Matches(when));
            }
        }

        ///////////////////////////////////////////////////////////////////////
        [Test]
        public void SimpleExpressionsTest() {
            // 0 0 1 1 *
        }

        ///////////////////////////////////////////////////////////////////////
        [Test]
        public void MessyExpressionsTest() {
            // * 0 0,12 */2 1-3,10-12 *
        }

        ///////////////////////////////////////////////////////////////////////
        [Test]
        public void SundayTest() {
            CronExpr cron = new CronExpr("* * * * 7");
            DateTime when = cron.CalcNextTime(DateTime.Now);
            Assert.IsTrue(when.DayOfWeek == DayOfWeek.Sunday);

            cron = new CronExpr("* * * * 0");
            when = cron.CalcNextTime(DateTime.Now);
            Assert.IsTrue(when.DayOfWeek == DayOfWeek.Sunday);

            cron = new CronExpr("* * * * 0,7");
            when = cron.CalcNextTime(DateTime.Now);
            Assert.IsTrue(when.DayOfWeek == DayOfWeek.Sunday);

            cron = new CronExpr("* * * * 1-6");
            when = cron.CalcNextTime(DateTime.Now);
            Assert.IsFalse(when.DayOfWeek == DayOfWeek.Sunday);
        }
    }
}
