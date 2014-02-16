//=============================================================================
// Copyright © Jason Heddings, All Rights Reserved
// $Id: X10Action.cs 83 2013-11-06 23:34:18Z jheddings $
//=============================================================================
using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using Flynn.Utilities;
using Flynn.X10;

namespace Flynn.Core.Actions {

    [XmlRoot("X10Action", Namespace = "Flynn.Core.Actions")]
    public sealed class X10Action : ActionBase {

        private static readonly Logger _logger = Logger.Get(typeof(X10Action));

        ///////////////////////////////////////////////////////////////////////
        private IX10Remote _controller;
        public IX10Remote Controller {
            get {
				return (_controller ?? X10Controller.Default);
            }

            set { _controller = value; }
        }

        ///////////////////////////////////////////////////////////////////////
        private readonly List<X10Command> _commands = new List<X10Command>();
        public List<X10Command> Commands {
            get { return _commands; }
        }

        ///////////////////////////////////////////////////////////////////////
        public X10Action() {
        }

        ///////////////////////////////////////////////////////////////////////
        public X10Action(params String[] cmds) {
            foreach (String cmd in cmds) {
                _commands.Add(X10Command.Parse(cmd));
            }
        }

        ///////////////////////////////////////////////////////////////////////
        protected override void PerformAction() {
            if ((_commands == null) || (_commands.Count == 0)) {
                return;
            }

			foreach (var cmd in _commands) {
                _logger.Info("send: {0}", cmd);

                Controller.Send(cmd);
            }
        }
    }
}
