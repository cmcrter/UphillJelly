/*--------------------------------------------------------------------------------*
  Copyright (C)Nintendo All rights reserved.

  These coded instructions, statements, and computer programs contain proprietary
  information of Nintendo and/or its licensed developers and are protected by
  national and international copyright laws. They may not be disclosed to third
  parties or copied or duplicated in any form, in whole or in part, without the
  prior written consent of Nintendo.

  The content herein is highly confidential and should be handled accordingly.
 *--------------------------------------------------------------------------------*/

#if UNITY_SWITCH || UNITY_EDITOR || NN_PLUGIN_ENABLE
using System;
using System.Runtime.InteropServices;

namespace nn.friends
{
    public sealed class NotificationQueue : IDisposable
    {
#pragma warning disable 0414
        private IntPtr queue = IntPtr.Zero;
#pragma warning restore 0414
        public NotificationQueue()
        {
#if !(!UNITY_SWITCH || UNITY_EDITOR)
            queue = Create();
#endif
        }

        ~NotificationQueue()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

#if !UNITY_SWITCH || UNITY_EDITOR
        private void Dispose(bool disposing)
        {
        }

        public nn.Result Initialize(nn.account.Uid uid)
        {
            return new nn.Result();
        }

        public nn.Result Initialize()
        {
            return new nn.Result();
        }

        public void Terminate()
        {
        }

        public nn.account.Uid GetUid()
        {
            return new nn.account.Uid();
        }

        public nn.Result Clear()
        {
            return new nn.Result();
        }

        public nn.Result Pop(ref NotificationInfo outInfo)
        {
            return new nn.Result();
        }

        public bool Exists()
        {
            return false;
        }
#else
        private void Dispose(bool disposing)
        {
            if (queue != IntPtr.Zero)
            {
                Destroy(queue);
                queue = IntPtr.Zero;
            }
        }

        public nn.Result Initialize(nn.account.Uid uid)
        {
            return Initialize(queue, uid);
        }

        public nn.Result Initialize()
        {
            return Initialize(queue);
        }

        public void Terminate()
        {
            Terminate(queue);
        }

        public nn.account.Uid GetUid()
        {
            return GetUid(queue);
        }

        public nn.Result Clear()
        {
            return Clear(queue);
        }

        public nn.Result Pop(ref NotificationInfo outInfo)
        {
            return Pop(queue, ref outInfo);
        }
        
        public bool Exists()
        {
            return Exists(queue);
        }

        [DllImport(Nn.DllName,
            CallingConvention = CallingConvention.Cdecl,
            EntryPoint = "nn_friends_NotificationQueueCreate")]
        private static extern IntPtr Create();

        [DllImport(Nn.DllName,
            CallingConvention = CallingConvention.Cdecl,
            EntryPoint = "nn_friends_NotificationQueueDestroy")]
        private static extern void Destroy(IntPtr pQueue);

        [DllImport(Nn.DllName,
            CallingConvention = CallingConvention.Cdecl,
            EntryPoint = "nn_friends_NotificationQueueInitialize1")]
        private static extern nn.Result Initialize(IntPtr pQueue, nn.account.Uid uid);

        [DllImport(Nn.DllName,
            CallingConvention = CallingConvention.Cdecl,
            EntryPoint = "nn_friends_NotificationQueueInitialize2")]
        private static extern nn.Result Initialize(IntPtr pQueue);

        [DllImport(Nn.DllName,
            CallingConvention = CallingConvention.Cdecl,
            EntryPoint = "nn_friends_NotificationQueueTerminate")]
        private static extern void Terminate(IntPtr pQueue);

        [DllImport(Nn.DllName,
            CallingConvention = CallingConvention.Cdecl,
            EntryPoint = "nn_friends_NotificationQueueGetUid")]
        private static extern nn.account.Uid GetUid(IntPtr pQueue);

        [DllImport(Nn.DllName,
            CallingConvention = CallingConvention.Cdecl,
            EntryPoint = "nn_friends_NotificationQueueClear")]
        private static extern nn.Result Clear(IntPtr pQueue);

        [DllImport(Nn.DllName,
            CallingConvention = CallingConvention.Cdecl,
            EntryPoint = "nn_friends_NotificationQueuePop")]
        private static extern nn.Result Pop(IntPtr pQueue, ref NotificationInfo outInfo);

        [DllImport(Nn.DllName,
            CallingConvention = CallingConvention.Cdecl,
            EntryPoint = "nn_friends_NotificationQueueExists")]
        [return: MarshalAs(UnmanagedType.U1)]
        private static extern bool Exists(IntPtr pQueue);
#endif
    }
}
#endif
