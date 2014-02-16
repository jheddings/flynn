//=============================================================================
// Copyright © Jason Heddings, All Rights Reserved
// $Id: EggTimer.cs 81 2013-11-06 04:44:58Z jheddings $
//=============================================================================
using System;
using System.Timers;

// egg timers are useful for being notified after a period of time has elapsed
// because of their precision, egg timers should only be considered accurate to
// the second; callbacks should generally happen within 250 ms of the event

namespace Flynn.Utilities {

    public delegate void TimerExpiredHandler(Object sender, DateTime when);
    public delegate void TimerStartedHandler(Object sender, DateTime when);
    public delegate void TimerStoppedHandler(Object sender, DateTime when);

    public sealed class EggTimer : IDisposable {

        public event TimerExpiredHandler TimerExpired;
        public event TimerStartedHandler TimerStarted;
        public event TimerStoppedHandler TimerStopped;

        private Timer _timer;
        private int _remain;
        private int _initial;

        ///////////////////////////////////////////////////////////////////////
        public int InitialValue {
            get { return _initial; }
        }

        ///////////////////////////////////////////////////////////////////////
        public int Remaining {
            get { return _remain; }
        }

        ///////////////////////////////////////////////////////////////////////
        public bool IsRunning {
            get { return (_timer.Enabled); }
        }

        ///////////////////////////////////////////////////////////////////////
        public EggTimer(int seconds) {
            _remain = seconds;
            _initial = seconds;

            _timer = new Timer();
            _timer.Elapsed += new ElapsedEventHandler(OnTimerTick);
            _timer.AutoReset = true;
            _timer.Interval = 1000;
        }

        ///////////////////////////////////////////////////////////////////////
        // Start the timer from the current remaining time.
        public void Start() {
            if (! IsRunning) {
                _timer.Start();

                if (TimerStarted != null) {
                    TimerStarted(this, DateTime.Now);
                }
            }
        }

        ///////////////////////////////////////////////////////////////////////
        // Stop the timer and reset to the original countdown value.
        public void Reset() {
            Stop();
            _remain = _initial;
        }

        ///////////////////////////////////////////////////////////////////////
        // Restart the timer from the given countdown value.
        public void Restart(int countdown) {
            _initial = countdown;
            Restart();
        }

        ///////////////////////////////////////////////////////////////////////
        // Restart the timer from the original countdown value.
        public void Restart() {
            Reset();
            Start();
        }

        ///////////////////////////////////////////////////////////////////////
        // Stop the timer at the current remaining time.
        public void Stop() {
            if (IsRunning) {
                _timer.Stop();

                if (TimerStopped != null) {
                    TimerStopped(this, DateTime.Now);
                }
            }
        }

        ///////////////////////////////////////////////////////////////////////
        // Stop the timer and clear all remaining time.  Note that this will
        // not fire the TimerExpired event; all others will process normally.
        public void Cancel() {
            Stop();
            _remain = 0;
        }

        ///////////////////////////////////////////////////////////////////////
        // Expire the timer, regardless of the remaining time.
        public void Expire() {
            if (IsRunning) {
                Cancel();

                if (TimerExpired != null) {
                    TimerExpired(this, DateTime.Now);
                }
            }
        }

        ///////////////////////////////////////////////////////////////////////
        // Completely dispose of this timer and release all its resources.
        public void Dispose() {
            _timer.Dispose();
        }

        ///////////////////////////////////////////////////////////////////////
        private void OnTimerTick(Object sender, EventArgs args) {
            _remain -= 1;

            if (_remain <= 0) {
                Expire();
            }
        }
    }
}
