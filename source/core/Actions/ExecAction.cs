//=============================================================================
// Copyright © Jason Heddings, All Rights Reserved
// $Id: ExecAction.cs 82 2013-11-06 22:04:47Z jheddings $
//=============================================================================
using System;
using System.Diagnostics;
using System.Threading;
using System.Xml.Serialization;
using Flynn.Utilities;

// TODO support username / password ?
// TODO support environment variables ?

namespace Flynn.Core.Actions {

    [XmlRoot("ExecAction", Namespace = "Flynn.Core.Actions")]
    public sealed class ExecAction : ActionBase {

        private static readonly Logger _logger = Logger.Get(typeof(ExecAction));

        private readonly ProcessStartInfo _info = new ProcessStartInfo();

        ///////////////////////////////////////////////////////////////////////
        public String Command {
            get { return _info.FileName; }
            set { _info.FileName = value; }
        }

        ///////////////////////////////////////////////////////////////////////
        public String Arguments {
            get { return _info.Arguments; }
            set { _info.Arguments = value; }
        }

        ///////////////////////////////////////////////////////////////////////
        public String Directory {
            get { return _info.WorkingDirectory; }
            set { _info.WorkingDirectory = value; }
        }

		///////////////////////////////////////////////////////////////////////
		public bool Fork { get; set; }

        ///////////////////////////////////////////////////////////////////////
        public ExecAction() {
            _info.CreateNoWindow = true;
            _info.ErrorDialog = false;
            _info.LoadUserProfile = false;
            _info.UseShellExecute = false;
        }

        ///////////////////////////////////////////////////////////////////////
        public ExecAction(String cmd) : this() {
            Command = cmd;
        }

        ///////////////////////////////////////////////////////////////////////
        public ExecAction(String cmd, String args) : this(cmd) {
            Arguments = args;
        }

        ///////////////////////////////////////////////////////////////////////
        protected override void PerformAction() {
            _logger.Info("exec");

            if (Fork) {
                StartProcessAsync();
            } else {
                StartProcessSync();
            }
        }

        ///////////////////////////////////////////////////////////////////////
        private void StartProcessAsync() {
            ThreadPool.QueueUserWorkItem(
                delegate {
                    _logger.Debug("started");

                    StartProcessSync();

                    _logger.Debug("complete");
                }
            );

            _logger.Debug("queued");
        }

        ///////////////////////////////////////////////////////////////////////
        private void StartProcessSync() {
            // TODO support named locks / mutexes

            _logger.Debug("{0}({1})", _info.FileName, _info.Arguments);

            try {
                Process proc = Process.Start(_info);
                proc.WaitForExit();
            } catch (Exception e) {
                _logger.Error(e);
            }

            // TODO support process timeout
        }
    }
}
