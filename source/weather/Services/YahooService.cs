//=============================================================================
// Copyright © Jason Heddings, All Rights Reserved
// $Id: YahooService.cs 86 2013-11-11 17:04:12Z jheddings $
//=============================================================================
using System;
using System.Xml.Linq;
using Flynn.Utilities;
using Flynn.Weather.Properties;

// TODO check for timeouts, bad responses, etc

namespace Flynn.Weather.Services {
    internal static class YahooServices {

        private static readonly Logger _logger = Logger.Get(typeof(YahooServices));
        public static readonly String _appid = Settings.Default.YahooApplicationID;

        ///////////////////////////////////////////////////////////////////////
        // http://developer.yahoo.com/geo/geoplanet/
		public static GeoData GetGeoData(String woeid) {
            String url = String.Format("http://where.yahooapis.com/v1/place/{0}?appid={1}", woeid, _appid);

            _logger.Debug("GetGeoData({0})", url);

            // the result declares a default namespace on the root node
            XElement elem = XDocument.Load(url).Root;
            XNamespace ns = elem.GetDefaultNamespace();

            // holds the lat/lon of the woeid region center
            XElement latlon = elem.Element(ns + "centroid");

			var data = new GeoData {
                LocationID = elem.Element(ns + "woeid").Value,

                Name = elem.Element(ns + "name").Value,
                Country = elem.Element(ns + "country").Value,
                Region = elem.Element(ns + "admin1").Value,

                Latitude = decimal.Parse(latlon.Element(ns + "latitude").Value),
                Longitude = decimal.Parse(latlon.Element(ns + "longitude").Value),

                LastUpdated = DateTime.Now
            };

			return data;
        }

        ///////////////////////////////////////////////////////////////////////
        // http://developer.yahoo.com/weather/
		public static WeatherData GetWeather(String woeid) {
			var url = String.Format("http://weather.yahooapis.com/forecastrss?u=c&w={0}", woeid);

            _logger.Debug("GetWeather({0})", url);

            XElement elem = XDocument.Load(url).Root;

            // find our elements of interest (quick-n-dirty)
            XElement cond = elem.Element("yweather:condition");
            XElement wind = elem.Element("yweather:wind");
            XElement atmos = elem.Element("yweather:atmosphere");

			var data = new WeatherData {
                Temperature = int.Parse(cond.Attribute("temp").Value),

                Humidity = int.Parse(atmos.Attribute("humidity").Value),
                Pressure = decimal.Parse(atmos.Attribute("pressure").Value),

                WindSpeed = int.Parse(wind.Attribute("speed").Value)
            };

			return data;
        }
    }
}
