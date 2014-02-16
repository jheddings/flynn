//=============================================================================
// Copyright © Jason Heddings, All Rights Reserved
// $Id: NullAction.cs 82 2013-11-06 22:04:47Z jheddings $
//=============================================================================
using System.Threading;
using System.Xml.Serialization;
using Flynn.Utilities;

// represents an action that does nothing, with an optional duration

namespace Flynn.Core.Actions {

    [XmlRoot("NullAction", Namespace = "Flynn.Core.Actions")]
    public sealed class NullAction : IAction {

        private static readonly Logger _logger = Logger.Get(typeof(NullAction));

		public int Delay { get; set; }

        ///////////////////////////////////////////////////////////////////////
        public NullAction() {
        }

        ///////////////////////////////////////////////////////////////////////
        public NullAction(int delay) {
            Delay = delay;
        }

        ///////////////////////////////////////////////////////////////////////
        public void Invoke() {
            _logger.Debug("NullAction::Invoke() [{0} ms]", Delay);

            if (Delay > 0) {
                Thread.Sleep(Delay);
            }
        }
    }
}
