//=============================================================================
// Copyright © Jason Heddings, All Rights Reserved
// $Id: MacroAction.cs 83 2013-11-06 23:34:18Z jheddings $
//=============================================================================
using System;
using System.Collections.Generic;
using Flynn.Utilities;

// FIXME this won't work with the way we deserialize the IAction types -- each
// element of the node is considered a property; we need <action> child nodes

// TODO create a "security action" based on a sequence of events with random time between

namespace Flynn.Core.Actions {
    public sealed class MacroAction : ActionBase {

        private static readonly Logger _logger = Logger.Get(typeof(MacroAction));

        ///////////////////////////////////////////////////////////////////////
        private readonly List<IAction> _actions = new List<IAction>();
        public List<IAction> Actions {
            get { return _actions; }
        }

        ///////////////////////////////////////////////////////////////////////
        public MacroAction() {
        }

        ///////////////////////////////////////////////////////////////////////
        public MacroAction(params IAction[] actions) {
            _actions.AddRange(actions);
        }

        ///////////////////////////////////////////////////////////////////////
        public void Add(IAction action) {
            _actions.Add(action);
        }

        ///////////////////////////////////////////////////////////////////////
        protected override void PerformAction() {
            _logger.Info("macro: {0}", _actions.Count);

            foreach (IAction action in _actions) {
                try {
                    action.Invoke();
                } catch (Exception e) {
                    _logger.Error(e);
                }
            }
        }
    }
}
