//=============================================================================
// Copyright © Jason Heddings, All Rights Reserved
// $Id: CronTrigger.cs 83 2013-11-06 23:34:18Z jheddings $
//=============================================================================
using System;
using System.Xml.Serialization;
using Flynn.Cron;

// implements a trigger that fires according to a cron expression

namespace Flynn.Core.Triggers {

    [XmlRoot("CronTrigger", Namespace = "Flynn.Core.Triggers")]
    public sealed class CronTrigger : TimeTrigger {

		///////////////////////////////////////////////////////////////////////
		public CronExpr Expression { get; set; }

        ///////////////////////////////////////////////////////////////////////
        public CronTrigger() {
        }

        ///////////////////////////////////////////////////////////////////////
        public CronTrigger(CronExpr expr) {
            Expression = expr;
        }

        ///////////////////////////////////////////////////////////////////////
        public CronTrigger(String expr) {
            Expression = new CronExpr(expr);
        }

        ///////////////////////////////////////////////////////////////////////
        protected override DateTime CalcNextTime(DateTime timebase) {
			var next = DateTime.MaxValue;

			if (Expression != null) {
                next = Expression.CalculateNext(timebase);
            }

            return next;
        }
    }
}
