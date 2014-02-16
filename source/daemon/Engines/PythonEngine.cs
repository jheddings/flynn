//=============================================================================
// Copyright © Jason Heddings, All Rights Reserved
// $Id: PythonEngine.cs 81 2013-11-06 04:44:58Z jheddings $
//=============================================================================
using System;

// IronPython
// http://tinyurl.com/dbusyf
// http://tinyurl.com/9domyww

namespace Flynn.Daemon.Engines {
    internal sealed class PythonEngine : Engine {

        ///////////////////////////////////////////////////////////////////////
        public PythonEngine() {
        }

        ///////////////////////////////////////////////////////////////////////
        protected override void Process(String file) {
            throw new NotImplementedException();
        }

        ///////////////////////////////////////////////////////////////////////
        public override void Dispose() {
        }
    }
}
