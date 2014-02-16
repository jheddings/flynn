//=============================================================================
// Copyright © Jason Heddings, All Rights Reserved
// $Id: IAction.cs 81 2013-11-06 04:44:58Z jheddings $
//=============================================================================
using System;

namespace Flynn.Core {
    public interface IAction {

        ///////////////////////////////////////////////////////////////////////
        void Invoke();

        /* XXX would these be helpful?
        IAsyncResult BeginInvoke(AsyncCallback callback, Object obj);
        void EndInvoke(IAsyncResult result);
        */
    }
}
