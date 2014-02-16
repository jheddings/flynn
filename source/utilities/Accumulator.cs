//=============================================================================
// Copyright © Jason Heddings, All Rights Reserved
// $Id: Accumulator.cs 81 2013-11-06 04:44:58Z jheddings $
//=============================================================================
using System;
using System.Text;

namespace Flynn.Utilities {
    public sealed class Accumulator {

        ///////////////////////////////////////////////////////////////////////
        private String _name;
        public String Name {
            get { return _name; }
        }

        ///////////////////////////////////////////////////////////////////////
        private int _value;
        public int Value {
            get { return _value; }
        }

        ///////////////////////////////////////////////////////////////////////
        public Accumulator() {
        }

        ///////////////////////////////////////////////////////////////////////
        public Accumulator(String name) : this() {
            _name = name;
        }

        ///////////////////////////////////////////////////////////////////////
        public void Reset() {
            _value = 0;
        }

        ///////////////////////////////////////////////////////////////////////
        public void Pulse() {
            _value++;
        }

        ///////////////////////////////////////////////////////////////////////
        public void Pulse(int count) {
            _value += count;
        }

        ///////////////////////////////////////////////////////////////////////
        public override String ToString() {
            StringBuilder str = new StringBuilder("{Accumulator (");

            if (_name == null) {
                str.Append("anon");
            } else {
                str.Append(_name);
            }

            str.Append(") = ").Append(_value).Append("}");

            return str.ToString();
        }
    }
}
