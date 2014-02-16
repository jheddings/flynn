// =============================================================================
//  Copyright © Jason Heddings, All Rights Reserved
//  $Id: GeoDataService.cs 87 2013-11-11 17:05:49Z jheddings $
// =============================================================================
using System;
using Flynn.Utilities;
using Flynn.Weather.Properties;
using Flynn.Weather.Services;

namespace Flynn.Weather {
    public sealed class GeoDataService {

        private static readonly Logger _logger = Logger.Get(typeof(GeoDataService));

        // initialize the object cache to store information for 30 days
        private static readonly ObjectCache<String, GeoData> _cache =
            new ObjectCache<String, GeoData>(TimeSpan.FromDays(30));
        
        ///////////////////////////////////////////////////////////////////////
        public GeoData GetGeoData(String woeid) {
            _logger.Debug("Get geo data: {0}", woeid);

            GeoData data = null;
            
            lock (_cache) {
                if (Settings.Default.GeoData_CacheEnabled) {
                    data = _cache[woeid];
                }

                if (data == null) {
                    data = GetNewData(woeid);
                    _cache[woeid] = data;
                }
            }

            if (data == null) {
                _logger.Warn("Update failed: {0}", woeid);
            }

            return data;
        }

        ///////////////////////////////////////////////////////////////////////
        private static GeoData GetNewData(String woeid) {
            _logger.Info("Retrieving geo data: {0}", woeid);

            GeoData data = null;

            try {
				data = YahooServices.GetGeoData(woeid);

                // yahoo doesn't fill in all the fields at once...
                data.TimeZone = GeoNames.GetTimeZoneInfo(data.Latitude, data.Longitude);

                _logger.Debug("New data received: {0}", data.Name);

            } catch (Exception e) {
                _logger.Warn(e);
                data = null;
            }

            return data;
        }
    }
}
