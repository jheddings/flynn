// =============================================================================
//  Copyright © Jason Heddings, All Rights Reserved
//  $Id: GeoNames.cs 86 2013-11-11 17:04:12Z jheddings $
// =============================================================================
using System;
using System.Text;
using System.Xml.Linq;
using Flynn.Utilities;
using Flynn.Weather.Properties;

// service - http://www.geonames.org/

namespace Flynn.Weather.Services {
    internal static class GeoNames {

        private static readonly Logger _logger = Logger.Get(typeof(GeoNames));

        ///////////////////////////////////////////////////////////////////////
        public static GeoData GetGeoData(String geonameId) {
            StringBuilder url = new StringBuilder("http://api.geonames.org/get?");
            url.Append("geonameId=").Append(geonameId);
            url.Append("&username=").Append(Settings.Default.GeoNames_Username);

            _logger.Debug("GetGeoData => {0}", url);

            XElement elem = XDocument.Load(url.ToString()).Root;
            String tz = elem.Element("timezone").Value;

			var data = new GeoData {
                LocationID = elem.Element("geonameId").Value,
                
                Name = elem.Element("name").Value,
                Country = elem.Element("country").Value,
                Region = elem.Element("adminName1").Value,
                
                Latitude = decimal.Parse(elem.Element("lat").Value),
                Longitude = decimal.Parse(elem.Element("lng").Value),

                TimeZone = TimeZoneInfo.FindSystemTimeZoneById(tz),
                
                LastUpdated = DateTime.Now
            };
            
            return data;
        }

        ///////////////////////////////////////////////////////////////////////
        public static TimeZoneInfo GetTimeZoneInfo(decimal latitude, decimal longitude) {
			var url = new StringBuilder("http://api.geonames.org/timezone?");
            url.Append("lat=").Append(latitude).Append("&lng=").Append(longitude);
            url.Append("&username=").Append(Settings.Default.GeoNames_Username);

            _logger.Debug("GetTimeZoneInfo => {0}", url);

            XElement root = XDocument.Load(url.ToString()).Root;

            XElement elem = root.Element("timezone");
            if (elem == null) { return null; }

            String id = elem.Element("timezoneId").Value;
            TimeZoneInfo tz = TimeZoneInfo.Local;

            try {
                tz = TimeZoneInfo.FindSystemTimeZoneById(id);
            } catch (TimeZoneNotFoundException) {
                // TODO find another way to make a tz info
                tz = null;
            }

            _logger.Debug("TimeZoneInfo({0}, {1}) => {2}", latitude, longitude, tz);

            return tz;
        }

        ///////////////////////////////////////////////////////////////////////
        public static AstronomicalData GetAstroData(GeoData geo, DateTime date) {
			var url = new StringBuilder("http://api.geonames.org/timezone?");
            url.Append("lat=").Append(geo.Latitude).Append("&lng=").Append(geo.Longitude);
            url.Append("&date=").Append(date.ToString("yyyy-MM-dd"));
            url.Append("&username=").Append(Settings.Default.GeoNames_Username);

            _logger.Debug("GetAstroData => {0}", url);
            
            XElement root = XDocument.Load(url.ToString()).Root;
            
            XElement elem = root.Element("timezone");
            if (elem == null) { return null; }
            
            XElement sunr = elem.Element("date");
            if (sunr == null) { return null; }
            
			var data = new AstronomicalData {
                Sunrise = DateTime.Parse(sunr.Element("sunrise").Value),
				Sunset = DateTime.Parse(sunr.Element("sunset").Value),

				Source = new WhenAndWhere {
					When = date, Where = geo
				}
            };

            return data;
        }
    }
}
