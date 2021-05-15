using System;
using System.Collections.Generic;
using UnityEngine;

namespace Networking
{
    public class ThreadManager
    {
        private static readonly List<Action> execOnMainThreadList = new List<Action>();
        private static bool actionsToExec = false;


        /// <summary>
        /// Call this method from outside Unity's main thread to add an Action to execOnMainThreadList.
        /// If updateMainThread() is called this List is emptied and all actions are executed.
        /// </summary>
        /// <param name="action">Action to execute on main thread.</param>
        private static void executeOnMainThread(Action action)
        {
            if (action == null)
            {
                Debug.LogWarning("Action to execute on main thread is null!");
                return;
            }

            // secure that execOnMainThreadList is not read at the same time.
            lock (execOnMainThreadList)
            {
                execOnMainThreadList.Add(action);
                actionsToExec = true;
            }
        }

        /// <summary>
        /// IMPORTANT!! This method must be called only from Unity's main thread!
        /// Copy the content of execOnMainThreadList to a second list and execute it.
        /// </summary>
        private static void updateMainThread()
        {
            if (!actionsToExec)
            {
                return;
            }
            
            List<Action> execOnMainThreadListCopied = new List<Action>();
            
            // lock execOnMainThreadList for as short as possible --> copy it
            lock (execOnMainThreadList)
            {
                execOnMainThreadListCopied.AddRange(execOnMainThreadList);
                execOnMainThreadList.Clear();
                actionsToExec = false;
            }

            foreach (Action action in execOnMainThreadListCopied)
            {
                action();
            }
        }
    }
}