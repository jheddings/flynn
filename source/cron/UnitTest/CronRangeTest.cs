//=============================================================================
// Copyright © Jason Heddings, All Rights Reserved
// $Id: CronRangeTest.cs 81 2013-11-06 04:44:58Z jheddings $
//=============================================================================
using System;
using NUnit.Framework;

// XXX most methods could construct the CronRange directly, but the parser is
// used to get more thorough test coverage since that is the common use case

namespace Flynn.Cron.UnitTest {

    [TestFixture]
    public class CronRangeTest {

        public const int TestValueMin = -100;
        public const int TestValueMax = 100;

        ///////////////////////////////////////////////////////////////////////
        [Test]
        public void ParseGoodFormatsTest() {
            TryParse("0", 0, 0, 1);
            TryParse("-10", -10, -10, 1);
            TryParse("10", 10, 10, 1);
            TryParse("1-5", 1, 5, 1);
            TryParse("*/2", long.MinValue, long.MaxValue, 2);
            TryParse("5/5", 5, long.MaxValue, 5);
            TryParse("1-30/3", 1, 30, 3);
        }

        ///////////////////////////////////////////////////////////////////////
        [Test]
        public void ParseBadFormatsTest() {
            // TODO
        }

        ///////////////////////////////////////////////////////////////////////
        [Test]
        public void LiteralTest(
            [Range(-10, 10)] int value
        ) {
            CronRange range = new CronRange(value);

            TestStepValues(range);
        }

        ///////////////////////////////////////////////////////////////////////
        [Test]
        public void LiteralStepTest(
            [Range(-10, 10)] int start,
            [Range(1, 10)] int step
        ) {
            String expr = String.Format("{0}/{1}", start, step);
            CronRange range = TryParse(expr, start, long.MaxValue, (uint) step);

            TestStepValues(range);
            //TestEnumerator(range);
        }

        ///////////////////////////////////////////////////////////////////////
        [Test]
        public void OddStepTest() {
            CronRange range = CronRange.Parse("1/2");

            for (int val = 0; val <= TestValueMax; val += 2) {
                Assert.IsFalse(range.Contains(val));
            }

            for (int val = 1; val <= TestValueMax; val += 2) {
                Assert.IsTrue(range.Contains(val));
            }
        }

        ///////////////////////////////////////////////////////////////////////
        [Test]
        public void WildcardTest() {
            CronRange range = TryParse("*", long.MinValue, long.MaxValue, 1);

            TestStepValues(range);
        }

        ///////////////////////////////////////////////////////////////////////
        [Test]
        public void WildcardStepTest(
            [Range(1, 10)] int step
        ) {
            String expr = String.Format("*/{0}", step);
            CronRange range = TryParse(expr, long.MinValue, long.MaxValue, (uint) step);

            TestStepValues(range);
        }

        ///////////////////////////////////////////////////////////////////////
        [Test, Pairwise]
        public void RangeTest(
            [Values(-50, -50, 25)] int min,
            [Values(-25,  50, 50)] int max
        ) {
            String expr = String.Format("{0}-{1}", min, max);
            CronRange range = TryParse(expr, min, max, 1);

            TestStepValues(range);
        }

        ///////////////////////////////////////////////////////////////////////
        [Test, Pairwise]
        public void RangeStepTest(
            [Values(-86, -2, 10, 21)] int min,
            [Values(-70, 18, 30, 22)] int max,
            [Range(1, 10)] int step
        ) {
            String expr = String.Format("{0}-{1}/{2}", min, max, step);
            CronRange range = TryParse(expr, min, max, (uint) step);

            TestStepValues(range);
        }

        ///////////////////////////////////////////////////////////////////////
        [Test, Pairwise, Explicit("long running & uncommon")]
        public void BigNumbersTest() {
            // TODO see how the CronRange handles large numbers and spans
            // XXX don't use the parser here
        }

        ///////////////////////////////////////////////////////////////////////
        private CronRange TryParse(String expr, long min, long max, uint step) {
            CronRange range = CronRange.Parse(expr);

            Assert.AreEqual(min, range.Minimum);
            Assert.AreEqual(max, range.Maximum);
            Assert.AreEqual(step, range.Step);

            return range;
        }

        ///////////////////////////////////////////////////////////////////////
        private void TestStepValues(CronRange range) {
            long start = Math.Max(range.Minimum, TestValueMin);
            long stop = Math.Min(range.Maximum, TestValueMax);

            // make sure all values in the range are present
            for (long val = start; val <= stop; val++) {
                if ((val - range.Minimum) % range.Step == 0) {
                    Assert.IsTrue(range.Contains(val));
                } else {
                    Assert.IsFalse(range.Contains(val));
                }
            }

            // make sure all values below the range are not present
            if (range.Minimum > long.MinValue) {
                for (long val = TestValueMin; val < range.Minimum; val++) {
                    Assert.IsFalse(range.Contains(val));
                }
            }

            // make sure all values above the range are not present
            if (range.Maximum > long.MaxValue) {
                for (long val = range.Maximum+1; val <= TestValueMax; val++) {
                    Assert.IsFalse(range.Contains(val));
                }
            }
        }
    }
}
