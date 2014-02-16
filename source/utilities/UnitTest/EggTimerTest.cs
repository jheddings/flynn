//=============================================================================
// Copyright © Jason Heddings, All Rights Reserved
// $Id: EggTimerTest.cs 81 2013-11-06 04:44:58Z jheddings $
//=============================================================================
using System.Threading;
using NUnit.Framework;

namespace Flynn.Utilities.UnitTest {

    [TestFixture]
    public class EggTimerTest {

        ///////////////////////////////////////////////////////////////////////
        [Test]
        public void ExpirationTest_Short(
            [Range(1, 5)] int value
        ) {
            TestTimerExpiration(value);
        }

        ///////////////////////////////////////////////////////////////////////
        [Test, Explicit]
        public void ExpirationTest_Long(
            [Range(60, 300, 30)] int value
        ) {
            TestTimerExpiration(value);
        }

        ///////////////////////////////////////////////////////////////////////
        [Test]
        public void StartStopTest() {
            bool fired = false;
            bool running = false;

            EggTimer timer = new EggTimer(2);

            timer.TimerExpired += new TimerExpiredHandler(delegate {
                fired = true;
            });

            timer.TimerStarted += new TimerStartedHandler(delegate {
                running = true;
            });

            timer.TimerStopped += new TimerStoppedHandler(delegate {
                running = false;
            });

            timer.Start();

            Assert.IsTrue(timer.IsRunning);
            Assert.IsTrue(running);

            Thread.Sleep(1100);

            Assert.IsTrue(timer.IsRunning);
            Assert.IsTrue(running);

            timer.Stop();

            Assert.IsFalse(fired, "timer expired early");
            Assert.IsFalse(running);
            Assert.IsFalse(timer.IsRunning);

            timer.Start();

            Assert.IsTrue(timer.IsRunning);
            Assert.IsTrue(running);

            Thread.Sleep(1100);

            Assert.IsTrue(fired, "timer did not expire in time");
            Assert.IsFalse(running);
            Assert.IsFalse(timer.IsRunning);
        }

        ///////////////////////////////////////////////////////////////////////
        [Test]
        public void CancelRestartTest() {
            bool fired = false;
            bool running = false;

            EggTimer timer = new EggTimer(2);

            timer.TimerExpired += new TimerExpiredHandler(delegate {
                fired = true;
            });

            timer.TimerStarted += new TimerStartedHandler(delegate {
                running = true;
            });

            timer.TimerStopped += new TimerStoppedHandler(delegate {
                running = false;
            });

            timer.Start();

            Assert.IsTrue(timer.IsRunning);
            Assert.IsTrue(running);

            Thread.Sleep(1100);

            timer.Cancel();

            Assert.IsFalse(fired, "timer expired after cancel");
            Assert.IsFalse(timer.IsRunning);
            Assert.IsFalse(running);
            Assert.AreEqual(0, timer.Remaining);

            Thread.Sleep(1100);

            Assert.IsFalse(fired, "timer expired after cancel");
            Assert.AreEqual(0, timer.Remaining);

            timer.Restart();

            Assert.IsTrue(timer.IsRunning);
            Assert.IsTrue(running);

            Thread.Sleep(1100);

            Assert.IsTrue(timer.IsRunning);
            Assert.IsTrue(running);
            Assert.IsFalse(fired, "timer expired early");

            Thread.Sleep(1100);

            Assert.IsTrue(fired, "timer did not expire after restart");
            Assert.IsFalse(running);
            Assert.IsFalse(timer.IsRunning);
        }

        ///////////////////////////////////////////////////////////////////////
        [Test]
        public void ForcedExpirationTest() {
            bool fired = false;
            bool running = false;

            EggTimer timer = new EggTimer(2);

            timer.TimerExpired += new TimerExpiredHandler(delegate {
                fired = true;
            });

            timer.TimerStarted += new TimerStartedHandler(delegate {
                running = true;
            });

            timer.TimerStopped += new TimerStoppedHandler(delegate {
                running = false;
            });

            timer.Start();

            Assert.IsTrue(timer.IsRunning);
            Assert.IsTrue(running);

            Thread.Sleep(1100);

            Assert.IsTrue(timer.IsRunning);
            Assert.IsTrue(running);

            timer.Expire();

            Assert.IsTrue(fired, "timer did not expire after expire");
            Assert.IsFalse(timer.IsRunning);
            Assert.IsFalse(running);
            Assert.AreEqual(0, timer.Remaining);

            fired = false;

            Thread.Sleep(1100);

            Assert.IsFalse(fired, "timer expired again");
        }

        ///////////////////////////////////////////////////////////////////////
        private void TestTimerExpiration(int seconds) {
            bool fired = false;
            bool running = false;

            EggTimer timer = new EggTimer(seconds);

            timer.TimerExpired += new TimerExpiredHandler(delegate {
                fired = true;
            });

            timer.TimerStarted += new TimerStartedHandler(delegate {
                running = true;
            });

            timer.TimerStopped += new TimerStoppedHandler(delegate {
                running = false;
            });

            Assert.IsFalse(timer.IsRunning);
            Assert.AreEqual(seconds, timer.Remaining);

            timer.Start();

            // keep an eye on the timer and the flag...
            for (int sec = 0; sec < seconds; sec++) {
                Assert.IsFalse(fired, "timer expired early");
                Assert.IsTrue(timer.IsRunning);
                Assert.IsTrue(running);

                Thread.Sleep(1000);
            }

            // give the timer a little extra time (note: 250 ms is spec, but
            // we push it a little bit here to make sure we will hit the spec)
            Thread.Sleep(100);

            // we should certainly have expired by now...
            Assert.IsTrue(fired, "timer did not expire in time");

            // the timer should be dead, since it had expired
            Assert.IsFalse(timer.IsRunning);
            Assert.IsFalse(running);
            Assert.AreEqual(0, timer.Remaining);
        }
    }
}
