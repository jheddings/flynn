// =============================================================================
//  Copyright © Jason Heddings, All Rights Reserved
//  $Id: MatchAnyFilter.cs 85 2013-11-11 16:38:38Z jheddings $
// =============================================================================
using System.Collections.Generic;

namespace Flynn.Core.Filters {
	public sealed class MatchAnyFilter : CompositeFilter {

        ///////////////////////////////////////////////////////////////////////
        public MatchAnyFilter() {
        }

        ///////////////////////////////////////////////////////////////////////
        public MatchAnyFilter(params IFilter[] filters) : base(filters) {
        }

		///////////////////////////////////////////////////////////////////////
		public override bool Accept(List<IFilter> filters) {
			foreach (var filter in filters) {
				if (filter.Accept()) {
					return true;
				}
			}

			return false;
		}
    }
}
