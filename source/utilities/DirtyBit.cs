// =============================================================================
//  Copyright © Jason Heddings, All Rights Reserved
//  $Id: DirtyBit.cs 81 2013-11-06 04:44:58Z jheddings $
// =============================================================================
using System;
using System.Threading;

//  provides a thread-safe mechanism for conditional updates

namespace Flynn.Utilities {
    public sealed class DirtyBit {

        private bool _dirty;
        private AutoResetEvent _lock;

        ///////////////////////////////////////////////////////////////////////
        public bool IsSet {
            get { return _dirty; }
        }

        ///////////////////////////////////////////////////////////////////////
        public DirtyBit() {
            _lock = new AutoResetEvent(true);
        }

        ///////////////////////////////////////////////////////////////////////
        public DirtyBit(bool set) : this() {
            _dirty = set;
        }

        ///////////////////////////////////////////////////////////////////////
        public void Set() {
            _lock.WaitOne();
            _dirty = true;
            _lock.Set();
        }

        ///////////////////////////////////////////////////////////////////////
        public void Reset() {
            _lock.WaitOne();
            _dirty = false;
            _lock.Set();
        }
        
        ///////////////////////////////////////////////////////////////////////
        public void Update(Action doUpdate) {
            _lock.WaitOne();
            
            if (_dirty) {
                doUpdate();
            }

            _dirty = false;
            
            _lock.Set();
        }
    }
}
