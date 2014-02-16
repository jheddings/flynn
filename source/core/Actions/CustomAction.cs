//=============================================================================
// Copyright © Jason Heddings, All Rights Reserved
// $Id: CustomAction.cs 82 2013-11-06 22:04:47Z jheddings $
//=============================================================================
using System;
using Flynn.Utilities;

namespace Flynn.Core.Actions {
    public sealed class CustomAction : ActionBase {

        private static readonly Logger _logger = Logger.Get(typeof(CustomAction));

        ///////////////////////////////////////////////////////////////////////
		public Action Action { get; set; }

        ///////////////////////////////////////////////////////////////////////
        public CustomAction() {
        }

        ///////////////////////////////////////////////////////////////////////
        public CustomAction(Action action) {
            Action = action;
        }

        ///////////////////////////////////////////////////////////////////////
        protected override void PerformAction() {
            if (Action == null) { return; }

            try {
                Action.Invoke();
            } catch (Exception e) {
                _logger.Warn(e);
            }
        }
    }
}
