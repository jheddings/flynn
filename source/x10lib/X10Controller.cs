//=============================================================================
// Copyright © Jason Heddings, All Rights Reserved
// $Id: X10Controller.cs 83 2013-11-06 23:34:18Z jheddings $
//=============================================================================
using System;
using System.Diagnostics;
using System.Threading;
using Flynn.Utilities;
using Flynn.X10.Properties;

namespace Flynn.X10 {
    public abstract class X10Controller : IX10Remote {

        private static readonly Logger _logger = Logger.Get(typeof(X10Controller));

        private readonly Stopwatch _swatch;

        ///////////////////////////////////////////////////////////////////////
        private static IX10Remote _default;
        public static IX10Remote Default {
            get {
                if (_default == null) {
                    ConfigureDefaultController();
                }
                return _default;
            }

            set { _default = value; }
        }


        ///////////////////////////////////////////////////////////////////////
        protected X10Controller() {
            _swatch = new Stopwatch();

            _logger.Debug("interval: {0}", Settings.Default.CM17A_SendDelayMs);
        }

        ///////////////////////////////////////////////////////////////////////
        public void Send(X10Command command) {
            if (Settings.Default.SendAsync) {
                SendCommand_Async(command);
            } else {
                SendCommand_Sync(command);
            }
        }

        ///////////////////////////////////////////////////////////////////////
        protected abstract void SendCommand(X10Command command);

        ///////////////////////////////////////////////////////////////////////
        private void SendCommand_Async(X10Command command) {
            _logger.Debug("queueing {0}", command);

            ThreadPool.QueueUserWorkItem(delegate {
                SendCommand_Sync(command);
            });
        }

        ///////////////////////////////////////////////////////////////////////
        private void SendCommand_Sync(X10Command command) {
            _logger.Info("send: {0}", command);

            _swatch.Clock(delegate {
                SendCommand(command);
            });

            _logger.Debug("{0} sent in {1} ms", command, _swatch.ElapsedMilliseconds);
        }

        ///////////////////////////////////////////////////////////////////////
        private static void ConfigureDefaultController() {
            // FIXME create the default controller instance from Settings
                
            throw new NotImplementedException();
        }
    }
}
