//=============================================================================
// Copyright © Jason Heddings, All Rights Reserved
// $Id: VersionFu.cs 81 2013-11-06 04:44:58Z jheddings $
//=============================================================================
using System;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;

// maintains version information about the application & environment

namespace Flynn.Utilities {
    public static class VersionFu {

        ///////////////////////////////////////////////////////////////////////
        private static String _progname = null;
        public static String ProgramName {
            get {
                if (_progname == null) {
                    _progname = Assembly.GetEntryAssembly().GetName().Name;
                }
                return _progname;
            }
        }
        
        ///////////////////////////////////////////////////////////////////////
        private static String _desc = null;
        public static String ApplicationDesc {
            get {
                if (_desc == null) {
                    _desc = Assembly.GetEntryAssembly().GetDescription();
                }
                return _desc;
            }
        }

        ///////////////////////////////////////////////////////////////////////
        private static String _title = null;
        public static String ApplicationTitle {
            get {
                if (_title == null) {
                    _title = Assembly.GetEntryAssembly().GetTitle();
                }
                return _title;
            }
        }

        ///////////////////////////////////////////////////////////////////////
        private static String _appver = null;
        public static String ApplicationVersion {
            get {
                if (_appver == null) {
                    Assembly assy = Assembly.GetEntryAssembly();
                    _appver = assy.GetName().Version.ToString();
                }
                return _appver;
            }
        }

        ///////////////////////////////////////////////////////////////////////
        private static String _buildstamp = null;
        public static String BuildStamp {
            get {
                if (_buildstamp == null) {
                    // TODO VERSION-REPOREV;TIMESTAMP
                    StringBuilder str = new StringBuilder();
                    str.Append(TimeStamp.ToString("s"));
                    _buildstamp = str.ToString();
                }
                return _buildstamp;
            }
        }

        ///////////////////////////////////////////////////////////////////////
        private static String _os = null;
        public static String OperatingSystem {
            get {
                if (_os == null) {
                    // XXX for *nix, `uname -sr` would be better
                    _os = Environment.OSVersion.VersionString;
                }
                return _os;
            }
        }

        ///////////////////////////////////////////////////////////////////////
        private static String _framework = null;
        public static String Framework {
            get {
                if (_framework == null) {
                    _framework = ReadFrameworkVersion();
                }
                return _framework;
            }
        }

        ///////////////////////////////////////////////////////////////////////
        private static DateTime _timestamp = DateTime.MinValue;
        public static DateTime TimeStamp {
            get {
                if (_timestamp == DateTime.MinValue) {
                    _timestamp = GetBuildDateTime(Assembly.GetEntryAssembly());
                }
                return _timestamp;
            }
        }

        ///////////////////////////////////////////////////////////////////////
        private static IMAGE_FILE_HEADER GetHeader(String file) {
            int buflen = Math.Max(Marshal.SizeOf(typeof(IMAGE_FILE_HEADER)), 4);
            byte[] buffer = new byte[buflen];

            using (Stream stream = new FileStream(file, FileMode.Open, FileAccess.Read)) {
                // COFF header offset
                stream.Position = 0x3C;
                stream.Read(buffer, 0, 4);

                uint hdrpos = BitConverter.ToUInt32(buffer, 0);
                stream.Position = hdrpos;
                stream.Read(buffer, 0, 4);  // "PE\0\0" marker
                stream.Read(buffer, 0, buflen);
            }

            GCHandle handle = GCHandle.Alloc(buffer, GCHandleType.Pinned);
            IMAGE_FILE_HEADER header = IMAGE_FILE_HEADER.Empty;

            try {
                IntPtr addr = handle.AddrOfPinnedObject();
                header = (IMAGE_FILE_HEADER) Marshal.PtrToStructure(addr, typeof(IMAGE_FILE_HEADER));
            } finally {
                handle.Free();
            }

            return header;
        }

        ///////////////////////////////////////////////////////////////////////
        private static DateTime GetBuildDateTime(this Assembly assembly) {
            DateTime tstamp = DateTime.MinValue;
            if (File.Exists(assembly.Location)) {
                IMAGE_FILE_HEADER coff = GetHeader(assembly.Location);
                if (coff.TimeDateStamp != 0) {
                    tstamp = DateTimeFu.FromUnixTime(coff.TimeDateStamp);
                }
            }
            return tstamp;
        }

        ///////////////////////////////////////////////////////////////////////
        private static String GetMonoVersion() {
            // i.e. Mono JIT compiler version 2.10.1 (tarball Tue Apr 12 15:19:47 MDT 2011)

            String displayName = null;

            Type mono = Type.GetType("Mono.Runtime");

            MethodInfo monoGetDisplayName =
                mono.GetMethod("GetDisplayName", BindingFlags.NonPublic | BindingFlags.Static);

            if (monoGetDisplayName != null) {
                Object retval = monoGetDisplayName.Invoke(null, null);
                if (retval != null) { displayName = retval.ToString(); }
            }

            StringBuilder str = new StringBuilder("Mono ");

            if (displayName == null) {
                str.Append(Environment.Version);
            } else {
                str.Append(displayName);
            }

            return str.ToString();
        }

        ///////////////////////////////////////////////////////////////////////
        private static String ReadFrameworkVersion() {
            if (Type.GetType("Mono.Runtime") != null) {
                return GetMonoVersion();
            }

            return "unknown framework";
        }

        ///////////////////////////////////////////////////////////////////////
        // http://msdn.microsoft.com/en-us/library/ms680313
        struct IMAGE_FILE_HEADER {
            public static readonly IMAGE_FILE_HEADER Empty = new IMAGE_FILE_HEADER();

            public ushort Machine;
            public ushort NumberOfSections;
            public uint TimeDateStamp;
            public uint PointerToSymbolTable;
            public uint NumberOfSymbols;
            public ushort SizeOfOptionalHeader;
            public ushort Characteristics;
        };
    }
}
