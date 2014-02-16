//=============================================================================
// Copyright © Jason Heddings, All Rights Reserved
// $Id: FilterBase.cs 84 2013-11-11 16:35:56Z jheddings $
//=============================================================================
using System;

// provides basic functionality for building time-based filters

namespace Flynn.Core.Filters {
    public abstract class FilterBase : IFilter {

		///////////////////////////////////////////////////////////////////////
		public String Name { get; set; }
        
        ///////////////////////////////////////////////////////////////////////
		public bool Accept() {
			return Accept(DateTime.Now);
		}

		///////////////////////////////////////////////////////////////////////
		public abstract bool Accept(DateTime when);

        ///////////////////////////////////////////////////////////////////////
        public override String ToString() {
			return String.Format("[Filter: {0}]", Name);
        }
    }
}
