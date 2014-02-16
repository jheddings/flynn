//=============================================================================
// Copyright © Jason Heddings, All Rights Reserved
// $Id: WeatherTrigger.cs 81 2013-11-06 04:44:58Z jheddings $
//=============================================================================
using System;
using System.Xml.Serialization;
using Flynn.Weather;

// TODO based on temperature, wind speed, etc

namespace Flynn.Core.Triggers {
    public abstract class WeatherTrigger : TriggerBase {

        ///////////////////////////////////////////////////////////////////////
        public WeatherTrigger() {
        }

        ///////////////////////////////////////////////////////////////////////
        protected abstract DateTime ReadDataField(WeatherData data);
    }
}
