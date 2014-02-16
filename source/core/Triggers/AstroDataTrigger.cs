// =============================================================================
//  Copyright © Jason Heddings, All Rights Reserved
//  $Id: AstroDataTrigger.cs 84 2013-11-11 16:35:56Z jheddings $
// =============================================================================
using System;
using Flynn.Core.Triggers;
using Flynn.Weather;
using Flynn.Utilities;

namespace Flynn.Core.Triggers {
    public abstract class AstroDataTrigger : TimeTrigger {

        private static readonly Logger _logger = Logger.Get(typeof(AstroDataTrigger));

        private readonly AstronomyCalc _calc = new AstronomyCalc();

		public String LocationID { get; set; }

        ///////////////////////////////////////////////////////////////////////
		protected AstroDataTrigger() {
        }
        
        ///////////////////////////////////////////////////////////////////////
		protected AstroDataTrigger(String location) {
            LocationID = location;
        }
        
        ///////////////////////////////////////////////////////////////////////
        protected AstroDataTrigger(int offset) : base(offset) {
        }

        ///////////////////////////////////////////////////////////////////////
        protected sealed override DateTime CalcNextTime(DateTime timebase) {
            DateTime date = timebase.Date;
            DateTime next = DateTime.MaxValue;
            
            do {
                _logger.Debug("read data for {0} at {1}", LocationID, date);

				var data = _calc.GetAstroData(LocationID, date);
                next = ReadDataField(data);  // read the desired field

                date = date.AddDays(1);  // setup for the next loop if needed
            } while (next <= timebase);
            
            return next;
        }

        ///////////////////////////////////////////////////////////////////////
        protected abstract DateTime ReadDataField(AstronomicalData data);
    }
}
