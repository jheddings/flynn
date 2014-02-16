//=============================================================================
// Copyright © Jason Heddings, All Rights Reserved
// $Id: SpecialCaseTest.cs 81 2013-11-06 04:44:58Z jheddings $
//=============================================================================
using System;
using NUnit.Framework;

namespace Flynn.Cron.UnitTest {

    [TestFixture]
    public class SpecialCaseTest {

        ///////////////////////////////////////////////////////////////////////
        [Test]
        public void NextLeapYearTest() {
            // Feb 29th would only occur at the next leap year
            CronExpr expr = new CronExpr("0 0 29 2 *");
            DateTime next = expr.Next;

            Assert.IsTrue(DateTime.IsLeapYear(next.Year));
        }
    }
}
