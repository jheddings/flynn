//=============================================================================
// Copyright © Jason Heddings, All Rights Reserved
// $Id: SystemTrigger.cs 81 2013-11-06 04:44:58Z jheddings $
//=============================================================================
using System;
using System.Xml.Serialization;

// TODO startup, shutdown, etc trigger

namespace Flynn.Core.Triggers {

    [XmlRoot("SystemTrigger", Namespace = "Flynn.Core.Triggers")]
    public sealed class SystemTrigger : TriggerBase {

        enum Kind { STARTUP, IDLE, SHUTDOWN }

        ///////////////////////////////////////////////////////////////////////
        public SystemTrigger() {
            throw new NotImplementedException();
        }
    }
}
