//=============================================================================
// Copyright © Jason Heddings, All Rights Reserved
// $Id: RubyEngine.cs 81 2013-11-06 04:44:58Z jheddings $
//=============================================================================
using System;

// IronRuby

namespace Flynn.Daemon.Engines {
    internal sealed class RubyEngine : Engine {

        ///////////////////////////////////////////////////////////////////////
        public RubyEngine() {
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
