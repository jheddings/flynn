// =============================================================================
//  Copyright © Jason Heddings, All Rights Reserved
//  $Id: InsteonController.cs 88 2013-12-23 18:04:48Z jheddings $
// =============================================================================
using System;
using System.Diagnostics;
using System.Threading;
using Flynn.Utilities;
using Flynn.X10.Properties;

namespace Flynn.X10 {
    public abstract class InsteonController : IInsteonRemote {

        private static readonly Logger _logger = Logger.Get(typeof(InsteonController));

        private readonly Stopwatch _swatch;
        
        ///////////////////////////////////////////////////////////////////////
        private static IInsteonRemote _default;
        public static IInsteonRemote Default {
            get {
                if (_default == null) {
                    ConfigureDefaultController();
                }
                return _default;
            }
            
            set { _default = value; }
        }
        
        ///////////////////////////////////////////////////////////////////////
        protected InsteonController() {
            _swatch = new Stopwatch();
        }

        ///////////////////////////////////////////////////////////////////////
        public void Send(InsteonCommand command) {
            if (Settings.Default.SendAsync) {
                SendCommand_Async(command);
            } else {
                SendCommand_Sync(command);
            }
        }

        ///////////////////////////////////////////////////////////////////////
        protected abstract void SendCommand(InsteonCommand cmd);

        ///////////////////////////////////////////////////////////////////////
        private void SendCommand_Async(InsteonCommand command) {
            _logger.Debug("queueing {0}", command);
            
            ThreadPool.QueueUserWorkItem(delegate {
                SendCommand_Sync(command);
            });
        }
        
        ///////////////////////////////////////////////////////////////////////
        private void SendCommand_Sync(InsteonCommand command) {
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
