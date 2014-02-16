//=============================================================================
// Copyright © Jason Heddings, All Rights Reserved
// $Id: CronSetTest.cs 81 2013-11-06 04:44:58Z jheddings $
//=============================================================================
using System;
using NUnit.Framework;

namespace Flynn.Cron.UnitTest {

    [TestFixture]
    public class CronSetTest {

        ///////////////////////////////////////////////////////////////////////
        [Test]
        public void SimpleWildcardTest() {
            CronSet set = CronSet.Parse("*");
            for (int loop = -100; loop <= 100; loop++) {
                Assert.IsTrue(set.Contains(loop));
            }
        }

        ///////////////////////////////////////////////////////////////////////
        [Test]
        public void SimpleValuesTest() {
            CronSet set = CronSet.Parse("2,4,6,8,10");

            Assert.IsTrue(set.Contains(2, 4, 6, 8, 10));
            Assert.IsFalse(set.Contains(1, 3, 5, 7, 9));
        }

        ///////////////////////////////////////////////////////////////////////
        [Test]
        public void SimpleRangeTest() {
            CronSet set = CronSet.Parse("1-5,10-20,30-60");

            Assert.IsTrue(set.Contains(1, 5, 16, 42, 60));
            Assert.IsFalse(set.Contains(-1, 0, 7, 28, 61, 99));
        }

        ///////////////////////////////////////////////////////////////////////
        [Test]
        public void SundayTest() {
            Assert.IsTrue(CronSet.Parse("*").Contains(DayOfWeek.Sunday));
            Assert.IsTrue(CronSet.Parse("1-7").Contains(DayOfWeek.Sunday));
            Assert.IsTrue(CronSet.Parse("0-6").Contains(DayOfWeek.Sunday));
            Assert.IsTrue(CronSet.Parse("0").Contains(DayOfWeek.Sunday));
            Assert.IsTrue(CronSet.Parse("7").Contains(DayOfWeek.Sunday));
            Assert.IsTrue(CronSet.Parse("0/2").Contains(DayOfWeek.Sunday));
            Assert.IsTrue(CronSet.Parse("1/2").Contains(DayOfWeek.Sunday));

            Assert.IsFalse(CronSet.Parse("1-5").Contains(DayOfWeek.Sunday));
            Assert.IsFalse(CronSet.Parse("2,4,6").Contains(DayOfWeek.Sunday));
            Assert.IsFalse(CronSet.Parse("2/3").Contains(DayOfWeek.Sunday));
        }
    }
}
