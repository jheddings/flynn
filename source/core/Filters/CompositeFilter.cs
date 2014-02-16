// =============================================================================
//  Copyright © Jason Heddings, All Rights Reserved
//  $Id: CompositeFilter.cs 85 2013-11-11 16:38:38Z jheddings $
// =============================================================================
using System.Collections.Generic;

namespace Flynn.Core.Filters {
    public abstract class CompositeFilter : IFilter {

        protected readonly List<IFilter> _filters = new List<IFilter>();

        ///////////////////////////////////////////////////////////////////////
        protected CompositeFilter() {
        }

        ///////////////////////////////////////////////////////////////////////
        protected CompositeFilter(params IFilter[] filters) {
            _filters.AddRange(filters);
        }

        ///////////////////////////////////////////////////////////////////////
        public bool Accept() {
            lock (_filters) {
                return Accept(_filters);
            }
        }

        ///////////////////////////////////////////////////////////////////////
        public abstract bool Accept(List<IFilter> filters);

        ///////////////////////////////////////////////////////////////////////
        public void Add(IFilter filter) {
            lock (_filters) {
                _filters.Add(filter);
            }
        }
    }
}
