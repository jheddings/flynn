//=============================================================================
// Copyright © Jason Heddings, All Rights Reserved
// $Id: CSharpEngine_InitUsings.cs 81 2013-11-06 04:44:58Z jheddings $
//=============================================================================
using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections;

using Flynn.Core;
using Flynn.Core.Triggers;
using Flynn.Core.Actions;
using Flynn.Core.Tasks;
using Flynn.Core.Filters;
using Flynn.Core.Devices;

// workaround for https://bugzilla.xamarin.com/show_bug.cgi?id=8607

public static class __dummy_extension_method_helper {
    // for some reason, we need to define an extension method
    // here before our real extenion methods will work...
    public static void __do_nothing(this Object obj) {
    }
}
