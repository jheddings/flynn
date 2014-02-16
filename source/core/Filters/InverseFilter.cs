//=============================================================================
// Copyright © Jason Heddings, All Rights Reserved
// $Id: InverseFilter.cs 84 2013-11-11 16:35:56Z jheddings $
//=============================================================================
using System;

// accepts the inverse of a given filter; specifically, this filter will accept
// everything the supplied filter rejects and will reject everything the given
// filter would accept

namespace Flynn.Core.Filters {
	public sealed class InverseFilter : IFilter {

		///////////////////////////////////////////////////////////////////////
        private readonly IFilter _filter;
		public IFilter OriginalFilter {
			get { return _filter; }
		}

        ///////////////////////////////////////////////////////////////////////
        public InverseFilter(IFilter filter) {
            _filter = filter;
        }

        ///////////////////////////////////////////////////////////////////////
        public bool Accept() {
            return (! _filter.Accept());
        }
    }
}
