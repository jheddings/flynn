//=============================================================================
// Copyright © Jason Heddings, All Rights Reserved
// $Id: AssemblyFu.cs 81 2013-11-06 04:44:58Z jheddings $
//=============================================================================
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Flynn.Utilities {
    public static class AssemblyFu {

        ///////////////////////////////////////////////////////////////////////////
        public static Type GetType(String typeName) {
            AppDomain domain = AppDomain.CurrentDomain;
            Assembly[] all = domain.GetAssemblies();

            foreach (Assembly assy in all) {
                Type type = assy.GetType(typeName);
                if (type != null) { return type; }
            }

            return null;
        }

        ///////////////////////////////////////////////////////////////////////////
        public static Object Activate(String typeName, params Object[] args) {
            Type type = GetType(typeName);

            if (type == null) {
                throw new ArgumentException("Unknown type: " + typeName);
            }

            return Activator.CreateInstance(type, args);
        }

        ///////////////////////////////////////////////////////////////////////////
        public static Object Activate(String typeName) {
            return Activate(typeName, null);
        }

        ///////////////////////////////////////////////////////////////////////////
        public static T Activate<T>(String typeName) {
            return Activate<T>(typeName, null);
        }

        ///////////////////////////////////////////////////////////////////////////
        public static T Activate<T>(String typeName, params Object[] args) {
            Type srcType = GetType(typeName);
            Type destType = typeof(T);

            if (destType.IsAssignableFrom(srcType)) {
                return (T) Activator.CreateInstance(srcType, args);
            }

            throw new InvalidCastException("incompatible type in activation");
        }

        ///////////////////////////////////////////////////////////////////////////
        public static PropertyInfo GetPropertyInfo(Object target, String name) {
            Type objectType = target.GetType();

            foreach (PropertyInfo prop in objectType.GetProperties()) {
                if (prop.Name == name) {
                    return prop;
                }
            }

            throw new ArgumentException("invalid property", name);
        }

        ///////////////////////////////////////////////////////////////////////////
        public static Object GetProperty(Object target, String name) {
            PropertyInfo prop = GetPropertyInfo(target, name);

            return prop.GetValue(name, null);
        }

        ///////////////////////////////////////////////////////////////////////////
        public static void SetProperty(Object target, String name, Object value) {
            PropertyInfo prop = GetPropertyInfo(target, name);

            if (prop.CanWrite) {
                prop.SetValue(target, value, null);
            } else {
                throw new ArgumentException("cannot write to property", name);
            }
        }

        ///////////////////////////////////////////////////////////////////////////
        public static List<Type> FindImplOf(Type search) {
            List<Type> avail = new List<Type>();
            Assembly assy = Assembly.GetExecutingAssembly();
            foreach (Type type in assy.GetTypes()) {
                if (type.Equals(search)) { continue; }
                if (search.IsAssignableFrom(type)) {
                    avail.Add(type);
                }
            }
            return avail;
        }

        ///////////////////////////////////////////////////////////////////////////
        public static String GetDescription(this Assembly assy) {
            Object[] attributes = assy.GetCustomAttributes(typeof(AssemblyDescriptionAttribute), false);
            
            if (attributes.Length == 0) {
                return null;
            }

            return ((AssemblyDescriptionAttribute) attributes[0]).Description;
        }

        ///////////////////////////////////////////////////////////////////////////
        public static String GetTitle(this Assembly assy) {
            Object[] attrs = assy.GetCustomAttributes(typeof(AssemblyTitleAttribute), false);
            
            if (attrs.Length == 0) {
                return null;
            }
            
            return ((AssemblyTitleAttribute) attrs[0]).Title;
        }
    }
}
