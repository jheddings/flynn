// =============================================================================
//  Copyright © Jason Heddings, All Rights Reserved
//  $Id: CompositeFilterTest.cs 85 2013-11-11 16:38:38Z jheddings $
// =============================================================================
using System;
using NUnit.Framework;
using Flynn.Core.Filters;

namespace Flynn.Core.UnitTest.Filters {

    [TestFixture]
    public class CompositeFilterTest {

        ///////////////////////////////////////////////////////////////////////
        [Test]
        public void MatchAnyTest() {
            var yes = new NullFilter();
            var no = new InverseFilter(yes);

            var any_yes = new MatchAnyFilter(no, no, yes, no);
            Assert.IsTrue(any_yes.Accept());

            var any_no = new MatchAnyFilter(no, no, no, no);
            Assert.IsFalse(any_no.Accept());
        }

        ///////////////////////////////////////////////////////////////////////
        [Test]
        public void MatchAllTest() {
            var yes = new NullFilter();
            var no = new InverseFilter(yes);

            var all_yes = new MatchAllFilter(yes, yes, yes, yes);
            Assert.IsTrue(all_yes.Accept());

            var all_no = new MatchAllFilter(yes, no, yes, yes);
            Assert.IsFalse(all_no.Accept());
        }
    }
}
