using System;
using System.Collections.Generic;
using UnityEngine;

namespace CommonLayer.SceneControllers.Routines
{
    public class RoutineDispatcher : IRoutineDispatcher
    {
        private const int InitialSize = 16;

        private static IRoutineDispatcher _instance;

        private readonly object _runningAndQueueLock = new object();
        private readonly object _arrayLock = new object();

        private int _tail;
        private bool _running;
        private IEnumerator<TaskStatus>[] _routines = new IEnumerator<TaskStatus>[InitialSize];
        private readonly Queue<IEnumerator<TaskStatus>> _waitQueue = new Queue<IEnumerator<TaskStatus>>();

        public RoutineDispatcher()
        {
            if (_instance == null)
            {
                _instance = this;
            }
        }

        private static IRoutineDispatcher Instance => _instance ?? new RoutineDispatcher();

        #region IRoutineDispatcher Implementation

        public bool HaveActiveRoutines => _tail > 0 || _waitQueue.Count > 0;

        public void EnqueueRoutine(IEnumerator<TaskStatus> routine)
        {
            lock (_runningAndQueueLock)
            {
                if (_running)
                {
                    _waitQueue.Enqueue(routine);
                    return;
                }
            }

            lock (_arrayLock)
            {
                if (_routines.Length == _tail)
                {
                    Array.Resize(ref _routines, checked(_tail * 2));
                }

                _routines[_tail++] = routine;
            }
        }

        #endregion

        public static void Enqueue(IEnumerator<TaskStatus> routine)
        {
            Instance.EnqueueRoutine(routine);
        }

        protected void Do()
        {
            lock (_runningAndQueueLock)
            {
                _running = true;
            }

            lock (_arrayLock)
            {
                int j = _tail - 1;
                var i = 0;
                for (; i < _routines.Length; i++)
                {
                    IEnumerator<TaskStatus> coroutine = _routines[i];
                    if (coroutine != null)
                    {
                        try
                        {
                            if (!coroutine.MoveNext())
                            {
                                _routines[i] = null;
                            }
                            else
                            {
                                continue; // next i 
                            }
                        }
                        catch (Exception ex)
                        {
                            _routines[i] = null;
                            Debug.LogError(new CoroutineDispatcherException("Coroutine exception", ex));
                        }
                    }

                    while (i < j)
                    {
                        IEnumerator<TaskStatus> fromTail = _routines[j];
                        if (fromTail != null)
                        {
                            try
                            {
                                if (!fromTail.MoveNext())
                                {
                                    _routines[j] = null;
                                    j--;
                                }
                                else
                                {
                                    // swap
                                    _routines[i] = fromTail;
                                    _routines[j] = null;
                                    j--;
                                    goto NEXT_LOOP; // next i
                                }
                            }
                            catch (Exception ex)
                            {
                                _routines[j] = null;
                                j--;
                                Debug.LogError(new CoroutineDispatcherException("Coroutine exception", ex));
                            }
                        }
                        else
                        {
                            j--;
                        }
                    }

                    break; // LOOP END

                    NEXT_LOOP: ;
                }

                _tail = i;

                lock (_runningAndQueueLock)
                {
                    _running = false;
                    while (_waitQueue.Count != 0)
                    {
                        if (_routines.Length == _tail)
                        {
                            Array.Resize(ref _routines, checked(_tail * 2));
                        }

                        _routines[_tail++] = _waitQueue.Dequeue();
                    }
                }
            }
        }
    }
}