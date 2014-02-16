// =============================================================================
//  Copyright © Jason Heddings, All Rights Reserved
//  $Id: AssemblyEngine.cs 85 2013-11-11 16:38:38Z jheddings $
// =============================================================================
using System;

// for processing a compiled assembly config file

// XXX what is the entry point / symbol we look for?  using attributes?

namespace Flynn.Daemon.Engines {
	internal sealed class AssemblyEngine : Engine {

		///////////////////////////////////////////////////////////////////////
		public AssemblyEngine() {
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
