//=============================================================================
// Copyright © Jason Heddings, All Rights Reserved
// $Id: X10CommandTest.cs 81 2013-11-06 04:44:58Z jheddings $
//=============================================================================
using System;
using NUnit.Framework;

namespace Flynn.X10.UnitTest {

    [TestFixture]
    public class X10CommandTest {

        ///////////////////////////////////////////////////////////////////////
        [Test]
        public void ParseValidStrings() {
            for (char house = 'A'; house <= 'P'; house++) {

                // test all device-specific combinations
                for (uint device = 1; device <= 16; device++) {
                    TestParse(house, device, X10Command.Command.ON);
                    TestParse(house, device, X10Command.Command.OFF);
                }

                // test all non-device-specific combinations
                TestParse(house, 0, X10Command.Command.ALF);
                TestParse(house, 0, X10Command.Command.ALO);
                TestParse(house, 0, X10Command.Command.AUF);
                TestParse(house, 0, X10Command.Command.BRI);
                TestParse(house, 0, X10Command.Command.DIM);
            }
        }

        ///////////////////////////////////////////////////////////////////////
        private void TestParse(char house, uint device, X10Command.Command action) {
            String x10 = null;

            if (device == 0) {
                x10 = String.Format("{0}_{1}", house, action);
            } else {
                x10 = String.Format("{0}{1}_{2}", house, device, action);
            }

            X10Command cmd = X10Command.Parse(x10);

            Assert.AreEqual(house, cmd.House);
            Assert.AreEqual(device, cmd.Device);
            Assert.AreEqual(action, cmd.Action);
        }
    }
}
