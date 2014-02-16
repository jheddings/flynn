//=============================================================================
// Copyright © Jason Heddings, All Rights Reserved
// $Id: DaylightFilter.cs 85 2013-11-11 16:38:38Z jheddings $
//=============================================================================
using System;
using Flynn.Weather;

// filter that only accepts while is is light outside; more specifically:
// if the current time is between sunrise and sunset at the given location
// to filter for dark hours, create an InverseFilter(DaylightFilter)

// XXX should we compare SourceTime of AstroData instead of now?

// XXX do we want control for minutes before / after daylight as offests?

namespace Flynn.Core.Filters {
	public sealed class DaylightFilter : AstroDataFilter {

		///////////////////////////////////////////////////////////////////////
        public DaylightFilter() {
        }

		///////////////////////////////////////////////////////////////////////
		public override bool Accept(AstronomicalData data) {
			DateTime now = DateTime.Now;

			return (now > data.Sunrise) && (now < data.Sunset);
		}
    }
}

