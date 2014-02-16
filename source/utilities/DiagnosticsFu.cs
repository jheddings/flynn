//=============================================================================
// Copyright © Jason Heddings, All Rights Reserved
// $Id: DiagnosticsFu.cs 81 2013-11-06 04:44:58Z jheddings $
//=============================================================================
using System;
using System.Diagnostics;

namespace Flynn.Utilities {
    public static class DiagnosticsFu {

        ///////////////////////////////////////////////////////////////////////
        // a convenience method for timing a specific action using a Stopwatch
        public static void Clock(this Stopwatch swatch, Action action) {
            swatch.Reset();
            swatch.Start();
            action.Invoke();
            swatch.Stop();
        }
    }
}
