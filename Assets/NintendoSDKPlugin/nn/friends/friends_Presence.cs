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
using System.Text;

namespace nn.friends
{
    [StructLayout(LayoutKind.Sequential, Size = 224)]
    public struct UserPresence
    {
#if !UNITY_SWITCH || UNITY_EDITOR
        public nn.Result Initialize(nn.account.Uid uid)
        {
            return new nn.Result();
        }

        public nn.Result Initialize()
        {
            return new nn.Result();
        }

        public void Clear()
        {
        }

        public nn.Result Commit()
        {
            return new nn.Result();
        }

        public void DeclareOpenOnlinePlaySession()
        {
        }

        public void DeclareCloseOnlinePlaySession()
        {
        }

        public nn.Result SetDescription(string description)
        {
            return new nn.Result();
        }
#else
        public nn.Result Initialize(nn.account.Uid uid)
        {
            return Initialize(ref this, uid);
        }

        public nn.Result Initialize()
        {
            return Initialize(ref this);
        }

        public void Clear()
        {
            Clear(ref this);
        }

        public nn.Result Commit()
        {
            return Commit(ref this);
        }

        public void DeclareOpenOnlinePlaySession()
        {
            DeclareOpenOnlinePlaySession(ref this);
        }

        public void DeclareCloseOnlinePlaySession()
        {
            DeclareCloseOnlinePlaySession(ref this);
        }

        public nn.Result SetDescription(string description)
        {
            return SetDescription(ref this, description);
        }

        [DllImport(Nn.DllName,
            CallingConvention = CallingConvention.Cdecl,
            EntryPoint = "nn_friends_UserPresence_Initialize1")]
        private static extern nn.Result Initialize(ref UserPresence pUserPresence, nn.account.Uid uid);

        [DllImport(Nn.DllName,
            CallingConvention = CallingConvention.Cdecl,
            EntryPoint = "nn_friends_UserPresence_Initialize2")]
        private static extern nn.Result Initialize(ref UserPresence pUserPresence);

        [DllImport(Nn.DllName,
            CallingConvention = CallingConvention.Cdecl,
            EntryPoint = "nn_friends_UserPresence_Clear")]
        private static extern void Clear(ref UserPresence pUserPresence);

        [DllImport(Nn.DllName,
            CallingConvention = CallingConvention.Cdecl,
            EntryPoint = "nn_friends_UserPresence_Commit")]
        private static extern nn.Result Commit(ref UserPresence pUserPresence);

        [DllImport(Nn.DllName,
            CallingConvention = CallingConvention.Cdecl,
            EntryPoint = "nn_friends_UserPresence_DeclareOpenOnlinePlaySession")]
        private static extern void DeclareOpenOnlinePlaySession(ref UserPresence pUserPresence);

        [DllImport(Nn.DllName,
            CallingConvention = CallingConvention.Cdecl,
            EntryPoint = "nn_friends_UserPresence_DeclareCloseOnlinePlaySession")]
        private static extern void DeclareCloseOnlinePlaySession(ref UserPresence pUserPresence);
        
        [DllImport(Nn.DllName,
            CallingConvention = CallingConvention.Cdecl,
            EntryPoint = "nn_friends_UserPresence_SetDescription")]
        private static extern nn.Result SetDescription(ref UserPresence pUserPresence, [MarshalAs(UnmanagedType.LPStr), In] string description);
#endif
    }

    [StructLayout(LayoutKind.Sequential, Size = 224)]
    public struct FriendPresence
    {
#if !UNITY_SWITCH || UNITY_EDITOR
        public string GetDescription()
        {
            return string.Empty;
        }

        public ApplicationInfo GetLastPlayedApplication()
        {
            return new ApplicationInfo();
        }

        public long GetLastUpdatePosixTime()
        {
            return 0L;
        }

        public PresenceStatus GetStatus()
        {
            return PresenceStatus.Offline;
        }

        public bool IsSamePresenceGroupApplication()
        {
            return false;
        }
#else
        public string GetDescription()
        {
            StringBuilder description = new StringBuilder(256);
            GetDescription(this, description, description.Capacity);
            return description.ToString();
        }

        public ApplicationInfo GetLastPlayedApplication()
        {
            return GetLastPlayedApplication(this);
        }

        public long GetLastUpdatePosixTime()
        {
            return GetLastUpdatePosixTime(this);
        }

        public PresenceStatus GetStatus()
        {
            return GetStatus(this);
        }

        public bool IsSamePresenceGroupApplication()
        {
            return IsSamePresenceGroupApplication(this);
        }

        [DllImport(Nn.DllName,
            CallingConvention = CallingConvention.Cdecl,
            EntryPoint = "nn_friends_FriendPresence_GetDescription")]
        [return: MarshalAs(UnmanagedType.LPStr)]
        private static extern void GetDescription(FriendPresence pFriendPresence,
            [MarshalAs(UnmanagedType.LPStr), In, Out] StringBuilder description, long size);

        [DllImport(Nn.DllName,
            CallingConvention = CallingConvention.Cdecl,
            EntryPoint = "nn_friends_FriendPresence_GetLastPlayedApplication")]
        private static extern ApplicationInfo GetLastPlayedApplication(FriendPresence pFriendPresence);

        [DllImport(Nn.DllName,
            CallingConvention = CallingConvention.Cdecl,
            EntryPoint = "nn_friends_FriendPresence_GetLastUpdatePosixTime")]
        private static extern long GetLastUpdatePosixTime(FriendPresence pFriendPresence);

        [DllImport(Nn.DllName,
            CallingConvention = CallingConvention.Cdecl,
            EntryPoint = "nn_friends_FriendPresence_GetStatus")]
        private static extern PresenceStatus GetStatus(FriendPresence pFriendPresence);

        [DllImport(Nn.DllName,
            CallingConvention = CallingConvention.Cdecl,
            EntryPoint = "nn_friends_FriendPresence_IsSamePresenceGroupApplication")]
        private static extern bool IsSamePresenceGroupApplication(FriendPresence pFriendPresence);
#endif
    }
}
#endif