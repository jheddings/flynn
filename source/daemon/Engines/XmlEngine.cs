//=============================================================================
// Copyright © Jason Heddings, All Rights Reserved
// $Id: XmlEngine.cs 81 2013-11-06 04:44:58Z jheddings $
//=============================================================================
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using Flynn.Core;
using Flynn.Utilities;

namespace Flynn.Daemon.Engines {
    internal sealed class XmlEngine : Engine {

        private static readonly Logger _logger = Logger.Get(typeof(XmlEngine));

        private Dictionary<String, Object> _refs = new Dictionary<String, Object>();

        private List<IAction> _actions = new List<IAction>();
        private List<ITrigger> _triggers = new List<ITrigger>();
        private List<IFilter> _filters = new List<IFilter>();
        private List<ITask> _tasks = new List<ITask>();

        ///////////////////////////////////////////////////////////////////////
        public XmlEngine() {
        }

        ///////////////////////////////////////////////////////////////////////
        public override void Dispose() {
            _logger.Debug("Disposing...");

            _actions.DisposeAll();
            _tasks.DisposeAll();
            _triggers.DisposeAll();
            _filters.DisposeAll();

            _logger.Debug("Disposed");
        }

        ///////////////////////////////////////////////////////////////////////
        protected override void Process(String file) {
            XmlDocument doc = new XmlDocument();

            try {
                doc.Load(file);
            } catch (XmlException e) {
                _logger.Error("invalid configuration file: {0}", e.Message);
            }

            XmlNode root = doc.DocumentElement;
            if (root == null) { return; }

            foreach (XmlNode child in root.ChildNodes) {
                if (child.NodeType != XmlNodeType.Element) {
                    continue;
                }

                if ((child.Prefix == null) || (child.Prefix == String.Empty)) {
                    ProcessStandardElement(child);
                } else {
                    ProcessExtendedElement(child);
                }
            }
        }

        ///////////////////////////////////////////////////////////////////////
        private void ProcessStandardElement(XmlNode node) {
            switch (node.LocalName) {
                case "include":
                    ProcessNode_include(node);
                    break;

                default:
                    // TODO improve this error message (ideally with line number & file)
                    _logger.Error("illegal configuration element: {0}", node.Name);
                    break;
            }
        }

        ///////////////////////////////////////////////////////////////////////
        private void ProcessExtendedElement(XmlNode node) {
            Object obj = Load(node);

            ITrigger trigger = obj as ITrigger;
            if (trigger != null) {
                _triggers.Add(trigger);
            }

            IFilter filter = obj as IFilter;
            if (filter != null) {
                _filters.Add(filter);
            }

            IAction action = obj as IAction;
            if (action != null) {
                _actions.Add(action);
            }

            ITask task = obj as ITask;
            if (task != null) {
                _tasks.Add(task);
                task.Enabled = true;
            }

            // store ID for future cross-referencing
            XmlAttribute idAttr = node.Attributes["id"];
            if ((idAttr != null) && (idAttr.Value != null)) {
                _refs[idAttr.Value] = obj;
            }
        }

        ///////////////////////////////////////////////////////////////////////
        private void ProcessNode_include(XmlNode node) {
            XmlAttribute dirAttr = node.Attributes["dir"];
            XmlAttribute fileAttr = node.Attributes["file"];
            XmlAttribute pathAttr = node.Attributes["path"];

            if (dirAttr != null) {
                ProcessDir(dirAttr.Value);
            }

            if (fileAttr != null) {
                ProcessFile(fileAttr.Value);
            }

            if (pathAttr != null) {
                ProcessConfig(pathAttr.Value);
            }
        }

        ///////////////////////////////////////////////////////////////////////
        private Object Load(XmlNode node) {
            Object obj = null;

            XmlAttribute xrefAttr = node.Attributes["xref"];
            if (xrefAttr != null) {
                String xref = xrefAttr.Value;

                if (! _refs.ContainsKey(xref)) {
                    throw new Exception("invalid reference: " + xref);
                }

                obj = _refs[xref];

            } else {

                obj = LoadObject(node);
            }

            return obj;
        }

        ///////////////////////////////////////////////////////////////////////
        private Object LoadObject(XmlNode node) {
            Object obj = null;

            StringBuilder str = new StringBuilder();
            if ((node.Prefix != null) && (node.Prefix != "")) {
                str.Append(node.NamespaceURI).Append('.');
            }
            str.Append(node.LocalName);

            Type type = AssemblyFu.GetType(str.ToString());
            XmlSerializer xser = new XmlSerializer(type);

            using (XmlNodeReader reader = new CustomXmlNodeReader(node)) {
                obj = xser.Deserialize(reader);
            }

            return obj;
        }

        // helper class to ignore namespaces when de-serializing
        // adapted from - http://stackoverflow.com/questions/870293/
        private class CustomXmlNodeReader : XmlNodeReader {

            private String _namespace;

            public CustomXmlNodeReader(XmlNode node) : base(node) {
                _namespace = node.NamespaceURI;
            }

            public override String NamespaceURI {
                get { return _namespace; }
            }
        }
    }
}
