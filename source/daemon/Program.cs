//=============================================================================
// Copyright © Jason Heddings, All Rights Reserved
// $Id: Program.cs 88 2013-12-23 18:04:48Z jheddings $
//=============================================================================
using System;
using System.ServiceProcess;
using System.Threading;
using Flynn.Utilities;
using Flynn.Daemon.Properties;
using Mono.Options;

namespace Flynn.Daemon {
    internal class Program : ServiceBase {

        private static readonly Logger _logger = Logger.Get(typeof(Program));

        // used for managing console application lifecycle
        private static AutoResetEvent _stopsig;

        // the console instance of the daemon
        private static Program _prog;

        // runtime configuration
        private static Settings _conf = Settings.Default;

        ///////////////////////////////////////////////////////////////////////
        public static void Main(String[] args) {
            _logger.Info("{0}: {1}", VersionFu.ProgramName, VersionFu.ApplicationVersion);

            InitProgram();

            GetOptions(args);

            if (_conf.RunAsService) {
                ServiceBase.Run(_prog);
            } else {
                RunAsConsole();
            }

            _logger.Info("Program EXIT");
        }

        ///////////////////////////////////////////////////////////////////////
        private static void GetOptions(String[] args) {
            bool opt_help = false;
            bool opt_version = false;

			var opts = new OptionSet() {
                //{ "c|conf=", "use provided config path [ConfigPath]", v => _conf.ConfigPath = v },
                //{ "d|daemon", "run as a service [RunAsService]", v => _conf.RunAsService = true },
                { "h|help", "display program help", v => opt_help = true },
                { "V|version", "display version information", v => opt_version = true }
            };

            try {
                opts.Parse(args);
            } catch (Exception) {
                DisplayHelp(opts);
                Environment.Exit(1);
            }

            if (opt_help) {
                DisplayHelp(opts);
                Environment.Exit(0);
            }

            if (opt_version) {
                DisplayVersion();
                Environment.Exit(0);
            }
        }

        ///////////////////////////////////////////////////////////////////////
        private static void InitProgram() {
            _logger.Debug("Program INIT");

            AppDomainSetup setup = AppDomain.CurrentDomain.SetupInformation;
            _logger.Debug("app.config: {0}", setup.ConfigurationFile);

			Console.CancelKeyPress += OnCancelKeyPress;

			AppDomain.CurrentDomain.UnhandledException += OnUnhandledException;

            _prog = new Program();

            _stopsig = new AutoResetEvent(false);
        }

        ///////////////////////////////////////////////////////////////////////
        private static void RunAsConsole() {
            _logger.Debug("Program RUN");

            String[] args = Environment.GetCommandLineArgs();

            _prog.OnStart(args);

            // wait for an exit event
            _stopsig.WaitOne();

            _prog.OnStop();

            _logger.Debug("Program stopped");
        }

        ///////////////////////////////////////////////////////////////////////
        private static void DisplayHelp(OptionSet opts) {
            Console.WriteLine("usage: {0} [<options>]+ ", VersionFu.ProgramName);
            Console.WriteLine();
            Console.WriteLine("where <options> is one or more of the following:");

            opts.WriteOptionDescriptions(Console.Out);
        }

        ///////////////////////////////////////////////////////////////////////
        private static void DisplayVersion() {
            Console.WriteLine("{0} - {1}", VersionFu.ApplicationTitle, VersionFu.ApplicationDesc);

            Console.Write("  {0}", VersionFu.ProgramName);
            Console.Write(": {0}", VersionFu.ApplicationVersion);
            Console.WriteLine(" [{0}]", VersionFu.BuildStamp);

            Console.WriteLine("  {0}", VersionFu.Framework);
        }

        ///////////////////////////////////////////////////////////////////////
        private static void OnCancelKeyPress(Object sender, ConsoleCancelEventArgs args) {
            _logger.Info("Program canceled by user");

            args.Cancel = true;

            _stopsig.Set();
        }

        ///////////////////////////////////////////////////////////////////////
        private static void OnUnhandledException(Object sender, UnhandledExceptionEventArgs args) {
            if (args.IsTerminating) {
                _logger.Fatal("fatal exception occured");
            } else {
                _logger.Error("unhandled exception occurred");
            }

            Exception e = args.ExceptionObject as Exception;

            while (e != null) {
                Console.WriteLine("-----------------------------------------");
                Console.WriteLine(e.StackTrace);
                e = e.InnerException;
            }
        }

        ///////////////////////////////////////////////////////////////////////
		private readonly Thread _maintenanceThread;

        ///////////////////////////////////////////////////////////////////////
        public Program() {
            this.ServiceName = VersionFu.ProgramName;

            _maintenanceThread = new Thread(MaintenanceThreadLoop);
        }

        ///////////////////////////////////////////////////////////////////////
        protected override void OnStart(String[] args) {
            _logger.Debug("Program::OnStart()");

            base.OnStart(args);

            Engine.ProcessConfig(_conf.ConfigPath);

            _maintenanceThread.Start();
        }

        ///////////////////////////////////////////////////////////////////////
        protected override void OnStop() {
            _logger.Debug("Program::OnStop()");

            _maintenanceThread.Interrupt();

            // give the thread a few seconds to finish, then kill it
            if (! _maintenanceThread.Join(5 * 1000)) {
                _logger.Debug("Aborting maintenance thread");

                _maintenanceThread.Abort();
                _maintenanceThread.Join();
            }

            base.OnStop();
        }

        ///////////////////////////////////////////////////////////////////////
        private void MaintenanceThreadLoop() {
            _logger.Debug("Maintenance thread started");

            uint deepTickCount = 0;
            uint shallowTickCount = 0;

            while (true) {

                // sleep for a minute at a time
                try { Thread.Sleep(1000 * 60); }
                catch (ThreadInterruptedException) {
                    break;
                }

                // perform shallow maintenance when needed...
                if (++shallowTickCount >= _conf.ShallowMaintInterval_Min) {
                    try {
                        ShallowMaintenance();
                    } catch (ThreadAbortException) {
                        _logger.Debug("Shallow maintenance aborted");
                        break;
                    }
                    shallowTickCount = 0;
                }

                // also perform deep maintenance when needed...
                if (++deepTickCount >= _conf.DeepMaintInterval_Min) {
                    try {
                        DeepMaintenance();
                    } catch (ThreadAbortException) {
                        _logger.Debug("Deep maintenance aborted");
                        break;
                    }
                    deepTickCount = 0;
                }
            }

            _logger.Debug("Maintenance thread exit");
        }

        ///////////////////////////////////////////////////////////////////////
        private void ShallowMaintenance() {
            _logger.Info("Performing shallow maintenance...");
        }

        ///////////////////////////////////////////////////////////////////////
        private void DeepMaintenance() {
            _logger.Info("Performing deep maintenance...");
        }
    }
}
