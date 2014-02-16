//=============================================================================
// Copyright © Jason Heddings, All Rights Reserved
// $Id: SimpleTask.cs 83 2013-11-06 23:34:18Z jheddings $
//=============================================================================
using System;
using System.Xml.Serialization;
using Flynn.Utilities;

// a SimpleTask attaches to a trigger and invokes a corresponding action

namespace Flynn.Core.Tasks {

    [XmlRoot("SimpleTask", Namespace = "Flynn.Core.Tasks")]
    public sealed class SimpleTask : TaskBase, IDisposable {

        private static readonly Logger _logger = Logger.Get(typeof(SimpleTask));

		///////////////////////////////////////////////////////////////////////
		public override bool Enabled { get; set; }

        ///////////////////////////////////////////////////////////////////////
        private ITrigger _trigger;
        public ITrigger Trigger {
            get { return _trigger; }

            set {
                // unregister existing events
                if (_trigger != null) {
					_trigger.Fire -= Trigger_OnFire;
                }

                // register with the new trigger
                if (value != null) {
					value.Fire += Trigger_OnFire;
                }

                _trigger = value;
            }
        }

		///////////////////////////////////////////////////////////////////////
		public IAction Action { get; set; }

		///////////////////////////////////////////////////////////////////////
		public IFilter Filter { get; set; }

        ///////////////////////////////////////////////////////////////////////
        public SimpleTask() {
        }

        ///////////////////////////////////////////////////////////////////////
        public SimpleTask(ITrigger trigger, IAction action) {
            Action = action;
            Trigger = trigger;
        }

        ///////////////////////////////////////////////////////////////////////
        public SimpleTask(ITrigger trigger, IAction action, IFilter filter) {
            Action = action;
            Filter = filter;
            Trigger = trigger;
        }

        ///////////////////////////////////////////////////////////////////////
        public void Dispose() {
            Enabled = false;
            Action = null;
            Trigger = null;
        }

        ///////////////////////////////////////////////////////////////////////
        private void Trigger_OnFire(Object sender, EventArgs args) {
            if (! Enabled) { return; }

            if ((Filter != null) && (! Filter.Accept())) {
                _logger.Debug("trigger rejected by filter");
                return;
            }

            if (Action == null) {
                _logger.Debug("trigger fired; no action");
                return;
            }

            _logger.Debug("trigger fired");

            try {
                Action.Invoke();
            } catch (Exception e) {
                _logger.Error(e);
            }
        }
    }
}
