//=============================================================================
// Copyright © Jason Heddings, All Rights Reserved
// $Id: CompositeTriggerTest.cs 81 2013-11-06 04:44:58Z jheddings $
//=============================================================================
using System;
using System.Collections.Generic;
using System.Threading;
using Flynn.Core.Properties;
using Flynn.Core.Triggers;
using NUnit.Framework;

namespace Flynn.Core.UnitTest.Triggers {

    [TestFixture]
    public class CompositeTriggerTest {

        private bool _simpleHandlerFired;

        ///////////////////////////////////////////////////////////////////////
        [Test]
        public void MatchAny_SimpleTest() {
            MatchAnyTrigger comp = new MatchAnyTrigger();
            comp.Fire += new EventHandler(SimpleTriggerHandler);

            List<ManualTrigger> triggers = new List<ManualTrigger>();

            for (int count = 0; count < 100; count++) {
                ManualTrigger trigger = new ManualTrigger();
                triggers.Add(trigger);
                comp.Add(trigger);
            }

            // make sure we fire for every trigger in the composite
            for (int count = 0; count < 100; count++) {
                _simpleHandlerFired = false;
                triggers[count].FireOne();

                if (Settings.Default.TriggerAsyncFire) {
                    Thread.Yield();
                }

                Assert.IsTrue(_simpleHandlerFired, "[{0}] handler did not fire", count);
            }
        }

        ///////////////////////////////////////////////////////////////////////
        [Test]
        public void MatchAll_SimpleTest() {
            MatchAllTrigger comp = new MatchAllTrigger();
            comp.Fire += new EventHandler(SimpleTriggerHandler);

            List<ManualTrigger> triggers = new List<ManualTrigger>();

            for (int count = 0; count < 100; count++) {
                ManualTrigger trigger = new ManualTrigger();
                triggers.Add(trigger);
                comp.Add(trigger);
            }

            _simpleHandlerFired = false;

            for (int count = 0; count < 99; count++) {
                triggers[count].FireOne();

                // XXX this could cause a problem for triggers needing to fire "at once"
                if (Settings.Default.TriggerAsyncFire) {
                    Thread.Yield();
                }

                Assert.IsFalse(_simpleHandlerFired, "[{0}] handler fired early", count);
            }

            triggers[99].FireOne();

            if (Settings.Default.TriggerAsyncFire) {
                Thread.Yield();
            }

            Assert.IsTrue(_simpleHandlerFired, "handler did not fire");
        }

        ///////////////////////////////////////////////////////////////////////
        [Test]
        public void Gated_SimpleTest() {
            GatedTrigger comp = new GatedTrigger();
            comp.Fire += new EventHandler(SimpleTriggerHandler);

            List<ManualTrigger> triggers = new List<ManualTrigger>();

            for (int count = 0; count < 100; count++) {
                ManualTrigger trigger = new ManualTrigger();
                triggers.Add(trigger);
                comp.Add(trigger);
            }

            _simpleHandlerFired = false;

            for (int count = 0; count < 99; count++) {
                triggers[count].FireOne();

                Thread.Sleep(10);  // create some separation between events

                Assert.IsFalse(_simpleHandlerFired, "[{0}] handler fired early", count);
            }

            Thread.Sleep(1000);  // create additional separation between events
            Assert.IsFalse(_simpleHandlerFired, "handler fired early");

            triggers[99].FireOne();

            if (Settings.Default.TriggerAsyncFire) {
                Thread.Yield();
            }

            Assert.IsTrue(_simpleHandlerFired, "handler did not fire");
        }

        ///////////////////////////////////////////////////////////////////////
        private void SimpleTriggerHandler(Object sender, EventArgs args) {
            _simpleHandlerFired = true;
        }
    }
}
