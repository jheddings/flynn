//=============================================================================
// Copyright © Jason Heddings, All Rights Reserved
// $Id: MathFuTest.cs 81 2013-11-06 04:44:58Z jheddings $
//=============================================================================
using System;
using NUnit.Framework;

namespace Flynn.Utilities.UnitTest {

    [TestFixture]
    public class MathFuTest {

        ///////////////////////////////////////////////////////////////////////
        [Test]
        public void MinMaxTest_DateTime() {
            DateTime min = MathFu.Min<DateTime>(DateTime.MinValue, DateTime.MaxValue);
            DateTime max = MathFu.Max<DateTime>(DateTime.MinValue, DateTime.MaxValue);

            Assert.AreEqual(min, DateTime.MinValue);
            Assert.AreEqual(max, DateTime.MaxValue);

            Assert.Less(min, max);
            Assert.Greater(max, min);
        }
    }
}
