//=============================================================================
// Copyright © Jason Heddings, All Rights Reserved
// $Id: CSharpEngine.cs 84 2013-11-11 16:35:56Z jheddings $
//=============================================================================
using System;
using System.IO;
using Flynn.Daemon.Properties;
using Flynn.Utilities;
using Mono.CSharp;

namespace Flynn.Daemon.Engines {
    internal sealed class CSharpEngine : Engine {

        private static readonly Logger _logger = Logger.Get(typeof(CSharpEngine));

        private Evaluator _eval;

        ///////////////////////////////////////////////////////////////////////
        public CSharpEngine() {
        }

        ///////////////////////////////////////////////////////////////////////
        protected override void Process(String file) {
            if (_eval == null) {
                Initialize();
            }

            _logger.Debug("Evaluate: {0}", file);

            String source = File.ReadAllText(file);

            // TODO what should the return be?  a delegate, IDisposable, nada?
            // TODO use a timer to make sure evaluation doesn't take too long

            _eval.Run(source);
        }

        ///////////////////////////////////////////////////////////////////////
        public override void Dispose() {
            // TODO allow the script to dispose of it's objects somehow

            _eval.Interrupt();
        }

        ///////////////////////////////////////////////////////////////////////
        private void Initialize() {
            CompilerSettings sett = new CompilerSettings();
            ReportPrinter prnt = new ConsoleReportPrinter();

			// TODO programatically add references for all flynn assemblies

            sett.AssemblyReferences.Add("Flynn.Core.dll");
            sett.AssemblyReferences.Add("Flynn.X10.dll");
            sett.AssemblyReferences.Add("Flynn.Cron.dll");
            sett.AssemblyReferences.Add("Flynn.Utilities.dll");

            CompilerContext ctx = new CompilerContext(sett, prnt);

            _eval = new Evaluator(ctx);

            _eval.Run(Resources.CSharpEngine_InitUsings);
            _eval.Run(Resources.CSharpEngine_InitScript);
        }
    }
}
