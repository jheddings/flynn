//=============================================================================
// Copyright © Jason Heddings, All Rights Reserved
// $Id: Statistics.cs 81 2013-11-06 04:44:58Z jheddings $
//=============================================================================
using System;
using System.Text;

// XXX is there a way to get the mean, std dev, etc from a generic type?
// i.e. where T : IArithmetic

namespace Flynn.Utilities {
    public sealed class Statistics<T> where T : IComparable {

        ///////////////////////////////////////////////////////////////////////
        private String _name;
        public String Name {
            get { return _name; }
        }

        ///////////////////////////////////////////////////////////////////////
        private int _count = 0;
        public int Count {
            get { return _count; }
        }

        ///////////////////////////////////////////////////////////////////////
        private T _max = default(T);
        public T Maximum {
            get { return _max; }
        }

        ///////////////////////////////////////////////////////////////////////
        private T _min = default(T);
        public T Minimum {
            get { return _min; }
        }

        ///////////////////////////////////////////////////////////////////////
        public Statistics() {
        }

        ///////////////////////////////////////////////////////////////////////
        public Statistics(String name) : this() {
            _name = name;
        }

        ///////////////////////////////////////////////////////////////////////
        public void Update(T value) {
            if (_count == 0) {
                FirstUpdate(value);
            } else {
                NormalUpdate(value);
            }

            _count++;
        }

        ///////////////////////////////////////////////////////////////////////
        private void FirstUpdate(T value) {
            _min = value;
            _max = value;
        }

        ///////////////////////////////////////////////////////////////////////
        private void NormalUpdate(T value) {
            if (value.CompareTo(_min) < 0) {
                _min = value;
            }

            if (value.CompareTo(_max) > 0) {
                _max = value;
            }

            // TODO calculate _mean
            // TODO calculate _stddev
        }

        ///////////////////////////////////////////////////////////////////////
        public override String ToString() {
            StringBuilder str = new StringBuilder("{Statistics (");

            if (_name == null) {
                str.Append("anon");
            } else {
                str.Append(_name);
            }
            str.Append(") : ");

            str.Append("cnt=").Append(_count);
            str.Append(", min=").Append(_min);
            str.Append(", max=").Append(_max);
            
            str.Append("}");
            return str.ToString();
        }
    }
}
