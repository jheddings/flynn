//=============================================================================
// Copyright © Jason Heddings, All Rights Reserved
// $Id: WeatherData.cs 81 2013-11-06 04:44:58Z jheddings $
//=============================================================================
using System;

namespace Flynn.Weather {
    public class WeatherData {

        ///////////////////////////////////////////////////////////////////////
        private int _temp = int.MinValue;
        public int Temperature {
            get { return _temp; }
            set { _temp = value; }
        }

        ///////////////////////////////////////////////////////////////////////
        private int _humidity = int.MinValue;
        public int Humidity {
            get { return _humidity; }
            set { _humidity = value; }
        }

        ///////////////////////////////////////////////////////////////////////
        private int _windSpeed = int.MinValue;
        public int WindSpeed {
            get { return _windSpeed; }
            set { _windSpeed = value; }
        }

        ///////////////////////////////////////////////////////////////////////
        private decimal _pressure = decimal.MinusOne;
        public decimal Pressure {
            get { return _pressure; }
            set { _pressure = value; }
        }

        ///////////////////////////////////////////////////////////////////////
        public override int GetHashCode() {
            return (_temp + _humidity);
        }
        
        ///////////////////////////////////////////////////////////////////////
        public override bool Equals(Object obj) {
            WeatherData wx = obj as WeatherData;
            if (wx == null) { return false; }
            
            if (wx.Temperature != this.Temperature) { return false; }
            if (wx.Humidity != this.Humidity) { return false; }
            if (wx.WindSpeed != this.WindSpeed) { return false; }
            if (wx.Pressure != this.Pressure) { return false; }
            
            return true;
        }
    }
}