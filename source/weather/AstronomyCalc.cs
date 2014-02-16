//=============================================================================
// Copyright © Jason Heddings, All Rights Reserved
// $Id: AstronomyCalc.cs 86 2013-11-11 17:04:12Z jheddings $
//=============================================================================
using System;
using Flynn.Utilities;
using Flynn.Weather.Services;
using Flynn.Weather.Properties;

namespace Flynn.Weather {
    public sealed class AstronomyCalc {

        private static readonly Logger _logger = Logger.Get(typeof(AstronomyCalc));
        private static readonly GeoDataService _geodata = new GeoDataService();

        // we only need to keep astronomy data for a couple of days...
        private static readonly ObjectCache<WhenAndWhere, AstronomicalData> _cache =
            new ObjectCache<WhenAndWhere, AstronomicalData>(TimeSpan.FromHours(48));

        ///////////////////////////////////////////////////////////////////////
        public AstronomicalData GetAstroData(String woeid) {
            return GetAstroData(woeid, DateTime.Now);
        }
        
        ///////////////////////////////////////////////////////////////////////
        public AstronomicalData GetAstroData(String woeid, DateTime date) {
            _logger.Debug("Get astronomy data: {0} @ {1:yyyy-MM-dd}", woeid, date);
            
			var geo = _geodata.GetGeoData(woeid);
            if (geo == null) { return null; }

            // depending on the fields we end up with in AstroData, we may
            // not want to cache based on date; e.g. current sun position...
            // in fact, there may be a case not to cache this at all --jah
			var ww = new WhenAndWhere { When = date.Date, Where = geo };

            AstronomicalData data = null;
            
            lock (_cache) {
                if (Settings.Default.AstroData_CacheEnabled) {
                    data = _cache[ww];
                }
                
                if (data == null) {
                    data = GetNewData(ww);
                    _cache[ww] = data;
                }
            }
            
            if (data == null) {
                _logger.Warn("Update failed: {0}", woeid);
            }

            return data;
        }

        ///////////////////////////////////////////////////////////////////////
        private AstronomicalData GetNewData(WhenAndWhere ww) {
            _logger.Info("Retrieving current astro data: {0}", ww);

            AstronomicalData data = null;

            try {
                data = GeoNames.GetAstroData(ww.Where, ww.When);

                _logger.Debug("New data received: {0}", ww);

            } catch (Exception e) {
                _logger.Warn(e);
            }

            return data;
        }
    }
}
