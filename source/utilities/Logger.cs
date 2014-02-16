//=============================================================================
// Copyright © Jason Heddings, All Rights Reserved
// $Id: Logger.cs 81 2013-11-06 04:44:58Z jheddings $
//=============================================================================
using System;
using System.Text;
using log4net;
using log4net.Config;

namespace Flynn.Utilities {
    public sealed class Logger {

        private ILog _impl;

        ///////////////////////////////////////////////////////////////////////
        public static Logger Get(Type type) {
            Logger logger = new Logger();
            logger._impl = LogManager.GetLogger(type);

            return logger;
        }

        ///////////////////////////////////////////////////////////////////////
        public static Logger Get(String name) {
            Logger logger = new Logger();
            logger._impl = LogManager.GetLogger(name);

            return logger;
        }

        ///////////////////////////////////////////////////////////////////////
        static Logger() {
            XmlConfigurator.Configure();
        }

        ///////////////////////////////////////////////////////////////////////
        private Logger() {
        }

        ///////////////////////////////////////////////////////////////////////
        public void Debug(String msg) {
            _impl.Debug(msg);
        }

        ///////////////////////////////////////////////////////////////////////
        public void Debug(String msg, params Object[] args) {
            _impl.DebugFormat(msg, args);
        }

        ///////////////////////////////////////////////////////////////////////
        public void Debug(Exception e) {
            _impl.Debug(GetExceptionString(e));
        }

        ///////////////////////////////////////////////////////////////////////
        public void Info(String msg) {
            _impl.Info(msg);
        }

        ///////////////////////////////////////////////////////////////////////
        public void Info(String msg, params Object[] args) {
            _impl.InfoFormat(msg, args);
        }

        ///////////////////////////////////////////////////////////////////////
        public void Info(Exception e) {
            _impl.Info(GetExceptionString(e));
        }

        ///////////////////////////////////////////////////////////////////////
        public void Warn(String msg) {
            _impl.Warn(msg);
        }

        ///////////////////////////////////////////////////////////////////////
        public void Warn(String msg, params Object[] args) {
            _impl.WarnFormat(msg, args);
        }

        ///////////////////////////////////////////////////////////////////////
        public void Warn(Exception e) {
            _impl.Warn(GetExceptionString(e));
        }

        ///////////////////////////////////////////////////////////////////////
        public void Error(String msg) {
            _impl.Error(msg);
        }

        ///////////////////////////////////////////////////////////////////////
        public void Error(String msg, params Object[] args) {
            _impl.ErrorFormat(msg, args);
        }

        ///////////////////////////////////////////////////////////////////////
        public void Error(Exception e) {
            _impl.Error(GetExceptionString(e));
        }

        ///////////////////////////////////////////////////////////////////////
        public void Fatal(String msg) {
            _impl.Fatal(msg);
        }

        ///////////////////////////////////////////////////////////////////////
        public void Fatal(String msg, params Object[] args) {
            _impl.FatalFormat(msg, args);
        }

        ///////////////////////////////////////////////////////////////////////
        public void Fatal(Exception e) {
            _impl.Fatal(GetExceptionString(e));
        }

        ///////////////////////////////////////////////////////////////////////
        private String GetExceptionString(Exception e) {
            Type type = e.GetType();

            StringBuilder str = new StringBuilder();
            str.Append(type.Name);
            str.Append(": ");
            str.AppendLine(e.Message);
            str.Append(e.StackTrace);

            return str.ToString();
        }
    }
}
