// =============================================================================
//  Copyright © Jason Heddings, All Rights Reserved
//  $Id: AstronomyData.cs 84 2013-11-11 16:35:56Z jheddings $
// =============================================================================
using System;

namespace Flynn.Weather {
    public class AstronomicalData {

		public DateTime Sunrise { get; set; }
		public DateTime Sunset { get; set; }

		public WhenAndWhere Source { get; set; }

		///////////////////////////////////////////////////////////////////////
        public override int GetHashCode() {
            int hash = 0;
            
            hash += Sunrise.GetHashCode();
            hash += Sunset.GetHashCode();
            
            return hash;
        }

        ///////////////////////////////////////////////////////////////////////
        public override bool Equals(Object obj) {
			var astro = obj as AstronomicalData;
            if (astro == null) { return false; }

            if (astro.Sunrise != Sunrise) { return false; }
            if (astro.Sunset != Sunset) { return false; }

            return true;
        }
    }
}
