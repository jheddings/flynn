//=============================================================================
// Copyright © Jason Heddings, All Rights Reserved
// $Id: ActionBase.cs 82 2013-11-06 22:04:47Z jheddings $
//=============================================================================
using System;
using System.Diagnostics;
using Flynn.Utilities;

// TODO it would be really hepful to add a name to logger statements...

namespace Flynn.Core.Actions {
    public abstract class ActionBase : IAction {

        private static readonly Logger _logger = Logger.Get(typeof(ActionBase));

        // XXX should we disable this for performance?
        private readonly Stopwatch _swatch = new Stopwatch();

		///////////////////////////////////////////////////////////////////////
		public String Name { get; set; }
        
        ///////////////////////////////////////////////////////////////////////
        protected abstract void PerformAction();

        ///////////////////////////////////////////////////////////////////////
        public void Invoke() {
            try {

                _swatch.Clock(delegate {
                    PerformAction();
                });

				_logger.Debug("action complete; {0} ms", _swatch.ElapsedMilliseconds);

            } catch (Exception e) {
                _logger.Error(e);
            }
        }

        ///////////////////////////////////////////////////////////////////////
        public override String ToString() {
            return String.Format("[Action: {0}]", Name);
        }
    }
}
