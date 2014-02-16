//=============================================================================
// Copyright © Jason Heddings, All Rights Reserved
// $Id: WallClockTest.cs 81 2013-11-06 04:44:58Z jheddings $
//=============================================================================
using System;
using System.Threading;
using Flynn.Utilities.Properties;
using NUnit.Framework;

// TODO make sure the clock calls every second for a minute

namespace Flynn.Utilities.UnitTest {

    [TestFixture]
    public class WallClockTest {

        private WallClock _clock = WallClock.Instance;

        ///////////////////////////////////////////////////////////////////////
        [SetUp]
        public void SetupWallClockTests() {
            int startDelay = Settings.Default.WallClockStartDelay_Sec;

            if (startDelay > 0) {
                try {
                    Thread.Sleep(startDelay * 1000);
                } catch (ThreadInterruptedException) {
                    return;
                }
            }
        }

        ///////////////////////////////////////////////////////////////////////
        [Test]
        public void FireNextSecondTest() {
            bool fired = false;

            _clock.SecondTick += new EventHandler(delegate {
                fired = true;
            });

            // waite one second for the tick to occur
            Thread.Sleep(1000);

            Assert.IsTrue(fired, "trigger did not fire in time");
        }

        ///////////////////////////////////////////////////////////////////////
        [Test, Explicit]
        public void FireNextMinuteTest() {
            bool fired = false;

            _clock.MinuteTick += new EventHandler(delegate {
                fired = true;
            });

            // waite one minute for the tick to occur
            Thread.Sleep(60 * 1000);

            Assert.IsTrue(fired, "trigger did not fire in time");
        }
    }
}
