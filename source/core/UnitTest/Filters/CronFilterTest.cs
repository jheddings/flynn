//=============================================================================
// $Id: CronFilterTest.cs 81 2013-11-06 04:44:58Z jheddings $
// Copyright © 2012, Jason Heddings - All Rights Reserved
//=============================================================================
using System;
using Flynn.Core.Filters;
using NUnit.Framework;

namespace Flynn.Core.UnitTest.Filters {

    [TestFixture]
    public class CronFilterTest {

        ///////////////////////////////////////////////////////////////////////
        [Test]
        public void SimpleFilterTest() {

            // to accept everything we must specify seconds
            CronFilter filter = new CronFilter("* * * * * *");

            Assert.IsTrue(filter.Accept());

            DateTime when = DateTime.Now;
            for (int count = 0; count < 1000; count++) {
                Assert.IsTrue(filter.Accept(when));
                when = when.AddMinutes(1);
            }
        }
    }
}
