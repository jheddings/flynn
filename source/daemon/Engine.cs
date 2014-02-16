//=============================================================================
// Copyright © Jason Heddings, All Rights Reserved
// $Id: Engine.cs 81 2013-11-06 04:44:58Z jheddings $
//=============================================================================
using System;
using System.Collections.Generic;
using System.IO;
using Flynn.Daemon.Engines;
using Flynn.Utilities;

// engines provide the context for configuration and execution; this class may
// be extended to provide customized engines for additional languages, e.g. lua

// TODO add interactive support for engines (like a REPL or something)

namespace Flynn.Daemon {
    public abstract class Engine : IDisposable {

        private static readonly Logger _logger = Logger.Get(typeof(Engine));

        private static Dictionary<String, Engine> _engines =
            new Dictionary<String, Engine>();

        ///////////////////////////////////////////////////////////////////////
        public static void ProcessConfig(String path) {
            if (Directory.Exists(path)) {
                ProcessDir(path);
            } else if (File.Exists(path)) {
                ProcessFile(path);
            } else {
                _logger.Error("invalid path: {0}", path);
            }
        }

        ///////////////////////////////////////////////////////////////////////
        public abstract void Dispose();

        ///////////////////////////////////////////////////////////////////////
        protected abstract void Process(String file);

        ///////////////////////////////////////////////////////////////////////
        protected static void ProcessFile(String file) {
            FileInfo info = new FileInfo(file);
            String type = info.Extension;

            _logger.Info("ProcessFile: {0}", info.FullName);

            Engine engine = GetEngine(type);

            if (engine == null) {
                _logger.Debug("Could not process config file: {0}", file);
                return;
            }

            try {
                engine.Process(file);
            } catch (Exception e) {
                _logger.Warn(e);
            }
        }

        ///////////////////////////////////////////////////////////////////////
        protected static void ProcessDir(String dir) {
            DirectoryInfo info = new DirectoryInfo(dir);

            _logger.Info("ProcessDir: {0}", info.FullName);

            foreach (FileInfo file in info.GetFiles()) {
                ProcessFile(file.FullName);
            }
        }

        ///////////////////////////////////////////////////////////////////////
        private static Engine GetEngine(String type) {
            Engine engine = null;

            if (_engines.ContainsKey(type)) {
                engine = _engines[type];

            } else {
                engine = CreateEngine(type);

                if (engine != null) {
                    _engines[type] = engine;
                }
            }

            return engine;
        }

        ///////////////////////////////////////////////////////////////////////
        private static Engine CreateEngine(String type) {
            // we could get clever and do this with attributes someday...

            switch (type) {
                case ".cs": return new CSharpEngine();
                case ".xml": return new XmlEngine();
            }

            return null;
        }
    }
}
