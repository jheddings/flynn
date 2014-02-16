﻿//=============================================================================
// Copyright © Jason Heddings, All Rights Reserved
// $Id: ITask.cs 81 2013-11-06 04:44:58Z jheddings $
//=============================================================================
using System;

namespace Flynn.Core {
    public interface ITask {

        ///////////////////////////////////////////////////////////////////////
        String Description { get; }

        ///////////////////////////////////////////////////////////////////////
        bool Enabled { get; set; }
    }
}