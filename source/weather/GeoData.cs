// =============================================================================
//  Copyright © Jason Heddings, All Rights Reserved
//  $Id: GeoData.cs 84 2013-11-11 16:35:56Z jheddings $
// =============================================================================
using System;

namespace Flynn.Weather {
    public class GeoData {

		public String LocationID { get; set; }
		public String Name { get; set; }

		public String Region { get; set; }
		public String Country { get; set; }
		public TimeZoneInfo TimeZone { get; set; }

		public decimal Latitude { get; set; }
		public decimal Longitude { get; set; }

		public DateTime LastUpdated { get; set; }

		///////////////////////////////////////////////////////////////////////
		public GeoData() {
			LastUpdated = DateTime.MinValue;
			TimeZone = TimeZoneInfo.Utc;
		}

        ///////////////////////////////////////////////////////////////////////
        public override bool Equals(Object obj) {
            GeoData geo = obj as GeoData;
            if (geo == null) { return false; }

            if (geo.LocationID != LocationID) { return false; }
            if (geo.Name != Name) { return false; }

            if (geo.Latitude != Latitude) { return false; }
            if (geo.Longitude != Longitude) { return false; }

            if (geo.Country != Country) { return false; }
            if (geo.Region != Region) { return false; }
            if (geo.TimeZone != TimeZone) { return false; }

            if (geo.LastUpdated != LastUpdated) { return false; }

            return true;
        }

        ///////////////////////////////////////////////////////////////////////
        public override int GetHashCode() {
            int hash = LastUpdated.GetHashCode();
            
            if (LocationID != null) {
                hash += LocationID.GetHashCode();
            }
            
            return hash;
        }
    }
}
