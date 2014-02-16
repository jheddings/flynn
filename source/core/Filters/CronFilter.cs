//=============================================================================
// Copyright © Jason Heddings, All Rights Reserved
// $Id: CronFilter.cs 84 2013-11-11 16:35:56Z jheddings $
//=============================================================================
using System;
using System.Xml.Serialization;
using Flynn.Cron;
using Flynn.Utilities;

// filters based on hitting a cron expression

namespace Flynn.Core.Filters {

    [XmlRoot("CronFilter", Namespace = "Flynn.Core.Filters")]
    public sealed class CronFilter : FilterBase {

        private static readonly Logger _logger = Logger.Get(typeof(CronFilter));

		///////////////////////////////////////////////////////////////////////
		public CronExpr Expression { get; set; }

        ///////////////////////////////////////////////////////////////////////
        public CronFilter(CronExpr cron) {
			Expression = cron;
        }

        ///////////////////////////////////////////////////////////////////////
        public CronFilter(String expr) {
			Expression = new CronExpr(expr);
        }

        ///////////////////////////////////////////////////////////////////////
		public override bool Accept(DateTime when) {
			_logger.Debug("accept: {{{0}}} :: {1}", Expression, when);

			return (Expression == null) || (Expression.Matches(when));
        }
    }
}
