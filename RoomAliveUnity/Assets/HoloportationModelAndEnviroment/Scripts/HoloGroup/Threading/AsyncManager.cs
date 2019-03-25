using UnityEngine;
using System.Collections.Generic;
using System;
using HoloGroup.Patterns;

#if UNITY_EDITOR || UNITY_STANDALONE
using System.Threading;
#endif

#if NETFX_CORE
using System.Threading.Tasks;
#endif

namespace HoloGroup.Threading
{
    /// <summary>
    /// This class give you ability to run your code in async way on different platforms.
    /// </summary>
    public class AsyncManager : MonoSingleton<AsyncManager>
    {
        /// <summary>
        /// Wrap your method in StandartAsyncProcess object, 
        /// so you can run it in secondary thread.
        /// </summary>
        /// <param name="method">Action method.</param>
        /// <returns></returns>
        public AsyncProcess MakeAsync(Action method)
        {
           
#if UNITY_EDITOR || UNITY_STANDALONE
            Thread thread = new Thread(new ThreadStart(method));
            return new AsyncProcess(thread);
#elif NETFX_CORE
        Task task = new Task(method);
        return new AsyncProcess(task);
#else
            return null;
#endif
           
        }
    }
}