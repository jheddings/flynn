//=============================================================================
// Copyright © Jason Heddings, All Rights Reserved
// $Id: TimeOfDayFilterTest.cs 81 2013-11-06 04:44:58Z jheddings $
//=============================================================================
using System;
using Flynn.Core.Filters;
using NUnit.Framework;

namespace Flynn.Core.UnitTest.Filters {

    [TestFixture]
    public class TimeOfDayFilterTest {

        ///////////////////////////////////////////////////////////////////////
        [Test]
        public void DefaultValuesTest() {
            TimeOfDayFilter filter = new TimeOfDayFilter();

            for (int hour = 0; hour < 24; hour++) {
                for (int minute = 0; minute < 60; minute++) {
                    for (int second = 0; second < 60; second++) {
                        TimeSpan when = new TimeSpan(hour, minute, second);
                        Assert.IsTrue(filter.Accept(when));
                    }
                }
            }
        }

        ///////////////////////////////////////////////////////////////////////
        [Test]
        public void SimpleFilterTest() {
            TimeOfDayFilter filter = new TimeOfDayFilter();
            filter.Start = new TimeSpan(10, 0, 0);
            filter.Stop = new TimeSpan(18, 0, 0);

            TimeSpan noon = new TimeSpan(12, 0, 0);
            Assert.IsTrue(filter.Accept(noon));

            TimeSpan night = new TimeSpan(20, 0, 0);
            Assert.IsFalse(filter.Accept(night));
        }

        ///////////////////////////////////////////////////////////////////////
        [Test]
        public void WrapMidnightTest() {
            TimeOfDayFilter filter = new TimeOfDayFilter();
            filter.Start = new TimeSpan(22, 0, 0);
            filter.Stop = new TimeSpan(6, 0, 0);

            TimeSpan night = new TimeSpan(2, 0, 0);
            Assert.IsTrue(filter.Accept(night));

            TimeSpan noon = new TimeSpan(12, 0, 0);
            Assert.IsFalse(filter.Accept(noon));
        }
    }
}
