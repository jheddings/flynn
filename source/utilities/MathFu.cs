//=============================================================================
// Copyright © Jason Heddings, All Rights Reserved
// $Id: MathFu.cs 81 2013-11-06 04:44:58Z jheddings $
//=============================================================================
using System;

namespace Flynn.Utilities {
    public static class MathFu {

        ///////////////////////////////////////////////////////////////////////
        public static T Min<T>(params T[] items) where T : IComparable {
            if (items == null) {
                throw new ArgumentNullException("items");
            }

            if (items.Length == 0) {
                return default(T);
            }

            T val = items[0];

            for (int idx = 1; idx < items.Length; idx++) {
                if (val.CompareTo(items[idx]) > 0) {
                    val = items[idx];
                }
            }

            return val;
        }

        ///////////////////////////////////////////////////////////////////////
        public static T Max<T>(params T[] items) where T : IComparable {
            if (items == null) {
                throw new ArgumentNullException("items");
            }

            if (items.Length == 0) {
                return default(T);
            }

            T val = items[0];

            for (int idx = 1; idx < items.Length; idx++) {
                if (val.CompareTo(items[idx]) < 0) {
                    val = items[idx];
                }
            }

            return val;
        }
    }
}
