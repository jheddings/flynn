//=============================================================================
// Copyright © Jason Heddings, All Rights Reserved
// $Id: NullTrigger.cs 83 2013-11-06 23:34:18Z jheddings $
//=============================================================================
using System;
using System.Xml.Serialization;

// implements a trigger that never fires (null, never, none)

namespace Flynn.Core.Triggers {

    [XmlRoot("NullTrigger", Namespace = "Flynn.Core.Triggers")]
    public sealed class NullTrigger : ITrigger {

        ///////////////////////////////////////////////////////////////////////
        public event EventHandler Fire;

        ///////////////////////////////////////////////////////////////////////
        public DateTime LastTime {
            get { return DateTime.MinValue; }
        }

        ///////////////////////////////////////////////////////////////////////
        public NullTrigger() {
			Fire += NullTrigger_OnFire;
        }

        ///////////////////////////////////////////////////////////////////////
        private void NullTrigger_OnFire(Object sender, EventArgs args) {
            throw new NotImplementedException();
        }

        ///////////////////////////////////////////////////////////////////////
        private void DummyNotifyToAvoidCompilerWarnings() {
            Fire(this, EventArgs.Empty);
        }
    }
}
