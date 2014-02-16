//=============================================================================
// Copyright © Jason Heddings, All Rights Reserved
// $Id: ThreadFuTest.cs 81 2013-11-06 04:44:58Z jheddings $
//=============================================================================
using System;
using System.Diagnostics;
using NUnit.Framework;

namespace Flynn.Utilities.UnitTest {

    [TestFixture]
    public class ThreadFuTest {

        ///////////////////////////////////////////////////////////////////////
        [Test]
        public void TestSpinWait() {
            decimal avg = ClockSpinWait(1000, 1);

            // expect 1% precision or better
            Assert.LessOrEqual(avg, 0.01);
        }

        ///////////////////////////////////////////////////////////////////////
        private static decimal ClockSpinWait(int loop, int delay) {
            Stopwatch swatch = new Stopwatch();

            swatch.Clock(delegate {
                for (uint iter = 0; iter < loop; iter++) {
                    ThreadFu.SpinWaitMs(delay);
                }
            });

            decimal avg = (decimal) swatch.ElapsedMilliseconds / (decimal) loop;
            decimal prec = Math.Abs(avg - delay) / (decimal) delay;

            Console.Write("TestSpinWait target: {0} ms; iter: {1}", delay, loop);
            Console.WriteLine("; avg: {0}", avg);

            return prec;
        }
    }
}
