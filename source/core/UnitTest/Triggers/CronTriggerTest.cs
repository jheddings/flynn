//=============================================================================
// Copyright © Jason Heddings, All Rights Reserved
// $Id: CronTriggerTest.cs 81 2013-11-06 04:44:58Z jheddings $
//=============================================================================
using System;
using System.Threading;
using Flynn.Cron;
using Flynn.Core.Triggers;
using NUnit.Framework;

// TODO this doesn't work with the wall clock start delay; we either need to
// make a way around the delay, wait for it, or give MonitoredTrigger a thread

namespace Flynn.Core.UnitTest.Triggers {

    [TestFixture]
    public class CronTriggerTest {

        ///////////////////////////////////////////////////////////////////////
        [Test]
        public void SimpleFireTest() {
            bool fired = false;

            CronExpr cron = new CronExpr("* * * * * *");
            CronTrigger trigger = new CronTrigger(cron);
            Assert.AreEqual(cron.Next, trigger.NextTime);

            TimeSpan diff = cron.Next - DateTime.Now;
            Assert.LessOrEqual(diff.TotalSeconds, 1);

            trigger.Fire += new EventHandler(delegate {
                fired = true;
            });

            // give the trigger a chance to fire...
            Thread.Sleep((int) diff.TotalMilliseconds);

            Assert.IsTrue(fired, "trigger did not fire in time");
            Assert.LessOrEqual(trigger.LastTime, DateTime.Now);

            trigger.Dispose();
        }

        ///////////////////////////////////////////////////////////////////////
        [Test]
        public void VerifySimpleOffset(
            [Range(-90, 90, 3)] int offset
        ) {
            CronExpr cron = new CronExpr("0 0 * * *");
            DateTime nextCron = cron.Next;

            CronTrigger trigger = new CronTrigger(cron);
            trigger.Offset = offset;

            // make sure the trigger and the cron expression match
            DateTime nextTrigger = trigger.NextTime;

            TimeSpan diff = nextTrigger - nextCron;
            Assert.AreEqual(offset, diff.TotalMinutes);

            // TODO the next next time should be ...
        }

        ///////////////////////////////////////////////////////////////////////
        [Test]
        public void VerifyRandomOffset(
            [Range(-90, 90, 5)] int offset
        ) {
            CronExpr cron = new CronExpr("0 0 * * *");

            CronTrigger trigger = new CronTrigger(cron);
            trigger.Offset = offset;
            trigger.Randomize = true;

            TimeSpan diff = trigger.NextTime - cron.Next;

            if (offset < 0) {
                Assert.LessOrEqual(offset, diff.TotalMinutes);
            } else if (offset > 0) {
                Assert.LessOrEqual(diff.TotalMinutes, offset);
            } else {
                Assert.AreEqual(0, diff.TotalMinutes);
            }
        }
    }
}
