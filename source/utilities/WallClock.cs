//=============================================================================
// Copyright © Jason Heddings, All Rights Reserved
// $Id: WallClock.cs 81 2013-11-06 04:44:58Z jheddings $
//=============================================================================
using System;
using System.Threading;
using Flynn.Utilities.Properties;

// wall clocks are only good for 1-second resolution, so of course they cannot
// be relied on for precise time measurements; however they are quite good for
// occasional tasks and can be trusted to notify users at regular intervals

namespace Flynn.Utilities {
    public sealed class WallClock : IDisposable {

        private static readonly Logger _logger = Logger.Get(typeof(WallClock));

        public event EventHandler HourTick;
        public event EventHandler MinuteTick;
        public event EventHandler SecondTick;

        private Thread _thread;
        private RateMonitor _monitor;

        private readonly Settings _settings;

        private const int TickRateMs = 1000;
        private const int DriftThresholdMs = 500;

        ///////////////////////////////////////////////////////////////////////
        private static WallClock _instance;
        public static WallClock Instance {
            get {
                if (_instance == null) {
                    _instance = new WallClock();

                } else if (! _instance.IsRunning) {
                    _instance.Start();
                }

                return _instance;
            }
        }

        ///////////////////////////////////////////////////////////////////////
        public bool IsRunning {
            get { return (_thread != null); }
        }

        ///////////////////////////////////////////////////////////////////////
        private WallClock() {
            _monitor = new RateMonitor();
            _settings = Settings.Default;

            Start();
        }

        ///////////////////////////////////////////////////////////////////////
        public void Dispose() {
            if (this.IsRunning) {
                Stop();
            }

            _thread = null;
        }

        ///////////////////////////////////////////////////////////////////////
        private void Start() {
            if (this.IsRunning) {
                Stop();
            }

            _thread = new Thread(Run);
            _thread.IsBackground = true;
            _thread.Start();
        }

        ///////////////////////////////////////////////////////////////////////
        private void Stop() {
            if (_thread != null) {
                _thread.Abort();
                _thread.Join();
            }
        }

        ///////////////////////////////////////////////////////////////////////
        private void Run() {
            int startDelay = _settings.WallClockStartDelay_Sec;

            _logger.Debug("Wall Clock starting [delay: {0} sec]", startDelay);

            if (startDelay > 0) {
                try {
                    Thread.Sleep(startDelay * 1000);
                } catch (ThreadInterruptedException) {
                    _logger.Warn("startup interrupted");
                    return;
                }
            }

            _logger.Debug("Wall Clock started");

            while (true) {
                try {
                    TimerTick(null);
                } catch {
                    /* toss */
                }

                try {
                    SuspendRunLoop();
                } catch (ThreadInterruptedException) {
                    _logger.Debug("thread interrupted");
                    break;
                } catch (Exception e) {
                    _logger.Error(e);
                    break;
                }
            }

            _logger.Debug("Wall Clock stopped");
        }

        ///////////////////////////////////////////////////////////////////////
        private void TimerTick(Object data) {
            DateTime now = DateTime.Now;

            _monitor.Pulse();

            // handle misfires before the current tick
            if (_monitor.Skip > 1) {
                _logger.Warn("Misfire: {0} sec", _monitor.Skip);

                // go back to notify for missed seconds -- kind of a hack
                int goback = -1 * (_monitor.Skip - 1);
                DateTime replay = now.AddSeconds(goback);
                while (replay < now) {
                    _logger.Debug("Replay: {0}", replay);
                    NotifyDelegates(replay);
                    replay = replay.AddSeconds(1);
                }
            }

            // notify all delegates of the current tick
            if (_settings.WallClockAsyncNotify) {
                ThreadPool.QueueUserWorkItem(
                    delegate { NotifyDelegates(now); }
                );
            } else {
                NotifyDelegates(now);
            }
        }

        ///////////////////////////////////////////////////////////////////////
        private void SuspendRunLoop() {
            DateTime now = DateTime.Now;
            DateTime next = now.NextSecond();

            while (now < next) {
                TimeSpan diff = next - now;
                Thread.Sleep(diff);
                now = DateTime.Now;

                // XXX should we watch the retry count?
            }
        }

        ///////////////////////////////////////////////////////////////////////
        private void NotifyDelegates(DateTime now) {
            if (SecondTick != null) {
                SecondTick(this, EventArgs.Empty);
            }

            if (now.Second == 0) {
                if (MinuteTick != null) {
                    MinuteTick(this, EventArgs.Empty);
                }
            }

            if ((now.Second == 0) && (now.Minute == 0)) {
                if (HourTick != null) {
                    HourTick(this, EventArgs.Empty);
                }
            }
        }
    }
}
