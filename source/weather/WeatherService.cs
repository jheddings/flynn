//=============================================================================
// Copyright © Jason Heddings, All Rights Reserved
// $Id: WeatherService.cs 84 2013-11-11 16:35:56Z jheddings $
//=============================================================================
using System;
using System.Collections.Generic;
using Flynn.Utilities;
using Flynn.Weather.Properties;
using Flynn.Weather.Services;

namespace Flynn.Weather {
    public sealed class WeatherService {

        private static readonly Logger _logger = Logger.Get(typeof(WeatherService));

        private static readonly ObjectCache<String, WeatherData> _cache =
            new ObjectCache<String, WeatherData>(TimeSpan.FromMinutes(30));

        ///////////////////////////////////////////////////////////////////////
        public WeatherData GetCurrentWeather(String woeid) {
            _logger.Debug("Get current weather: {0}", woeid);

            WeatherData weather = null;

            lock (_cache) {
                if (Settings.Default.Weather_CacheEnabled) {
                    weather = _cache[woeid];
                }

                if (weather == null) {
                    weather = GetNewData(woeid);
                    _cache[woeid] = weather;
                }
            }

            if (weather == null) {
                _logger.Warn("Update failed: {0}", woeid);
            }

            return weather;
        }

        ///////////////////////////////////////////////////////////////////////
        private WeatherData GetNewData(String woeid) {
            _logger.Info("Retrieving current weather: {0}", woeid);

            WeatherData weather = null;

            try {
				weather = YahooServices.GetWeather(woeid);

                _logger.Debug("New data received: {0}", woeid);

            } catch (Exception e) {
                _logger.Warn(e);
            }

            return weather;
        }
    }
}