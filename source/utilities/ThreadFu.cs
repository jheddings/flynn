//=============================================================================
// Copyright © Jason Heddings, All Rights Reserved
// $Id: ThreadFu.cs 81 2013-11-06 04:44:58Z jheddings $
//=============================================================================
using System.Diagnostics;

namespace Flynn.Utilities {
    public static class ThreadFu {

        ///////////////////////////////////////////////////////////////////////
        // use with caution...  prevents thread context switching during sleep
        public static void SpinWaitMs(int milliseconds) {
            Stopwatch swatch = new Stopwatch();
            swatch.Start();

            while (swatch.ElapsedMilliseconds < milliseconds) {
                /* noop */
            }

            swatch.Stop();
        }
    }
}
