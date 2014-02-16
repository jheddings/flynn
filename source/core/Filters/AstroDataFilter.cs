//=============================================================================
// Copyright © Jason Heddings, All Rights Reserved
// $Id: AstroDataFilter.cs 85 2013-11-11 16:38:38Z jheddings $
//=============================================================================
using System;
using Flynn.Weather;
using Flynn.Utilities;

// provides a base class for filters that use astrononical data, i.e. sunrise
namespace Flynn.Core.Filters {
    public abstract class AstroDataFilter : FilterBase {

        private static readonly Logger _logger = Logger.Get(typeof(AstroDataFilter));
        private static readonly AstronomyCalc _calc = new AstronomyCalc();

        ///////////////////////////////////////////////////////////////////////
        public String LocationID { get; set; }

        ///////////////////////////////////////////////////////////////////////
        public override bool Accept(DateTime when) {
            _logger.Debug("read data for {0} at {1}", LocationID, when);

            var data = _calc.GetAstroData(LocationID, when);
			
            return Accept(data);
        }

        ///////////////////////////////////////////////////////////////////////
        public abstract bool Accept(AstronomicalData data);
    }
}
