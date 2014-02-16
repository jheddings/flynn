//=============================================================================
// Copyright © Jason Heddings, All Rights Reserved
// $Id: CollectionFu.cs 81 2013-11-06 04:44:58Z jheddings $
//=============================================================================
using System;
using System.Collections;

namespace Flynn.Utilities {
    public static class CollectionFu {

        ///////////////////////////////////////////////////////////////////////
        public static void DisposeAll(this IEnumerable set) {
            foreach (Object obj in set) {
                IDisposable disp = obj as IDisposable;
                if (disp != null) { disp.Dispose(); }
            }
        }
    }
}
