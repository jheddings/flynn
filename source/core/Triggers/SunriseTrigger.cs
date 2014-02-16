//=============================================================================
// Copyright © Jason Heddings, All Rights Reserved
// $Id: SunriseTrigger.cs 84 2013-11-11 16:35:56Z jheddings $
//=============================================================================
using System;
using System.Xml.Serialization;
using Flynn.Weather;

namespace Flynn.Core.Triggers {

    [XmlRoot("SunriseTrigger", Namespace = "Flynn.Core.Triggers")]
    public sealed class SunriseTrigger : AstroDataTrigger {

        ///////////////////////////////////////////////////////////////////////
        public SunriseTrigger() {
        }
        
        ///////////////////////////////////////////////////////////////////////
        public SunriseTrigger(String location) : base(location) {
        }
        
        ///////////////////////////////////////////////////////////////////////
        public SunriseTrigger(int offset) : base(offset) {
        }
        
        ///////////////////////////////////////////////////////////////////////
        protected override DateTime ReadDataField(AstronomicalData data) {
            return data.Sunrise;
        }
    }
}
