//=============================================================================
// Copyright © Jason Heddings, All Rights Reserved
// $Id: DaylightSavingsFilter.cs 84 2013-11-11 16:35:56Z jheddings $
//=============================================================================
using System;

// filter based on daylight savings time

namespace Flynn.Core.Filters {
    public class DaylightSavingsFilter : FilterBase {

        ///////////////////////////////////////////////////////////////////////
		public override bool Accept(DateTime when) {
			return when.IsDaylightSavingTime();
        }
    }
}
