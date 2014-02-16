//=============================================================================
// Copyright © Jason Heddings, All Rights Reserved
// $Id: SunsetTrigger.cs 84 2013-11-11 16:35:56Z jheddings $
//=============================================================================
using System;
using System.Xml.Serialization;
using Flynn.Weather;

namespace Flynn.Core.Triggers {

    [XmlRoot("SunsetTrigger", Namespace = "Flynn.Core.Triggers")]
    public sealed class SunsetTrigger : AstroDataTrigger {

        ///////////////////////////////////////////////////////////////////////
        public SunsetTrigger() {
        }

        ///////////////////////////////////////////////////////////////////////
        public SunsetTrigger(String location) : base(location) {
        }
        
        ///////////////////////////////////////////////////////////////////////
        public SunsetTrigger(int offset) : base(offset) {
        }

        ///////////////////////////////////////////////////////////////////////
        protected override DateTime ReadDataField(AstronomicalData data) {
            return data.Sunset;
        }
    }
}
