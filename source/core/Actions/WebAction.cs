//=============================================================================
// Copyright © Jason Heddings, All Rights Reserved
// $Id: WebAction.cs 83 2013-11-06 23:34:18Z jheddings $
//=============================================================================
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Xml.Serialization;
using Flynn.Utilities;

// TODO what about setting cookies, user agent, etc?

namespace Flynn.Core.Actions {

    [XmlRoot("WebAction", Namespace = "Flynn.Core.Actions")]
    public sealed class WebAction : ActionBase {

        private static readonly Logger _logger = Logger.Get(typeof(WebAction));

        private readonly Dictionary<String, String> _headers =
            new Dictionary<String, String>();

		///////////////////////////////////////////////////////////////////////
		public String URL { get; set; }

		///////////////////////////////////////////////////////////////////////
		public String Method { get; set; }

		///////////////////////////////////////////////////////////////////////
		public bool DoAsyncRequest { get; set; }

		///////////////////////////////////////////////////////////////////////
		public String OutputFile { get; set; }

		///////////////////////////////////////////////////////////////////////
		public String Data { get; set; }

        ///////////////////////////////////////////////////////////////////////
        public Dictionary<String, String> Headers {
            get { return _headers; }
        }

        ///////////////////////////////////////////////////////////////////////
        public WebAction() {
        }

        ///////////////////////////////////////////////////////////////////////
        public WebAction(String url) {
            URL = url;
        }

        ///////////////////////////////////////////////////////////////////////
        protected override void PerformAction() {
            _logger.Info("{1}: {0}", URL, Method);

			var req = WebRequest.Create(URL);

            req.Method = Method;

			foreach (var pair in _headers) {
                req.Headers.Add(pair.Key, pair.Value);
            }

            SendRequestData(req);

            if (DoAsyncRequest) {
                AsyncRequest(req);
            } else {
                SyncRequest(req);
            }
        }

        ///////////////////////////////////////////////////////////////////////
        private void SyncRequest(WebRequest req) {
            HandleWebResponse(req.GetResponse());
        }

        ///////////////////////////////////////////////////////////////////////
        private void AsyncRequest(WebRequest req) {
            req.BeginGetResponse(new AsyncCallback(ResponseCallback), req);
        }

        ///////////////////////////////////////////////////////////////////////
        private void ResponseCallback(IAsyncResult result) {
			var req = (WebRequest) result.AsyncState;
            HandleWebResponse(req.EndGetResponse(result));
        }

        ///////////////////////////////////////////////////////////////////////
        private void SendRequestData(WebRequest req) {
            if (Data == null) {
                req.ContentLength = 0;

            } else {
                Stream output = req.GetRequestStream();
                byte[] bytes = Encoding.UTF8.GetBytes(Data);
                req.ContentLength = bytes.Length;
                output.Write(bytes, 0, bytes.Length);
                output.Close();
            }
        }

        ///////////////////////////////////////////////////////////////////////
        private void HandleWebResponse(WebResponse resp) {
            _logger.Debug("received: {0} bytes", resp.ContentLength);

            Stream input = resp.GetResponseStream();
            Stream output = null;

			if (OutputFile == null) {
				output = new MemoryStream();
            } else {
				output = new FileStream(OutputFile, FileMode.Create);
            }

            input.CopyTo(output);

            resp.Close();
            output.Close();
        }
    }
}
