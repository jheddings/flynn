//=============================================================================
// Copyright © Jason Heddings, All Rights Reserved
// $Id: NullFilter.cs 84 2013-11-11 16:35:56Z jheddings $
//=============================================================================
using System;

// a NullFilter doesn't actually filter anything; it accepts everything
// a "block everything" filter can be acheived by an InverseFilter(NullFilter)

namespace Flynn.Core.Filters {
	public class NullFilter : IFilter {

        ///////////////////////////////////////////////////////////////////////
		public bool Accept() { return true; }
    }
}
