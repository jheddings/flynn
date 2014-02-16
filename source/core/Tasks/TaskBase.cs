//=============================================================================
// Copyright © Jason Heddings, All Rights Reserved
// $Id: TaskBase.cs 83 2013-11-06 23:34:18Z jheddings $
//=============================================================================
using System;

// TODO it would be really hepful to add a name to logger statements...

namespace Flynn.Core.Tasks {
    public abstract class TaskBase : ITask {

		///////////////////////////////////////////////////////////////////////
		public String Name { get; set; }
        
        ///////////////////////////////////////////////////////////////////////
        public String Description { get; set; }

        ///////////////////////////////////////////////////////////////////////
        public abstract bool Enabled { get; set; }

        ///////////////////////////////////////////////////////////////////////
        public override String ToString() {
            return String.Format("[Task: {0}]", Name);
        }
    }
}
