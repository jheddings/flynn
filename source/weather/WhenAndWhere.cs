// =============================================================================
//  Copyright © Jason Heddings, All Rights Reserved
//  $Id: WhenAndWhere.cs 84 2013-11-11 16:35:56Z jheddings $
// =============================================================================
using System;
using System.Text;

namespace Flynn.Weather {
	public class WhenAndWhere {

		public DateTime When { get; set; }
		public GeoData Where { get; set; }

		///////////////////////////////////////////////////////////////////////
		public WhenAndWhere() {
			When = DateTime.Now;
		}

		///////////////////////////////////////////////////////////////////////
		public WhenAndWhere(DateTime when) {
			When = when;
		}

		///////////////////////////////////////////////////////////////////////
		public override int GetHashCode() {
			int hash = When.GetHashCode();

			if (Where != null) {
				hash += Where.GetHashCode();
			}

			return hash;
		}

		///////////////////////////////////////////////////////////////////////
		public override bool Equals(Object obj) {
			var ww = obj as WhenAndWhere;
			if (ww == null) {
				return false;
			}
            
			if (ww.When != When) {
				return false;
			}

			if (ww.Where != Where) {
				return false;
			}
            
			return true;
		}

		///////////////////////////////////////////////////////////////////////
		public override String ToString() {
			var str = new StringBuilder("[");

			if (Where == null) {
				str.Append("??");
			} else {
				str.Append(Where.Name);
			}

			str.Append(" @ ").Append(When.ToString("u")).Append(']');

			return str.ToString();
		}
	}
}
