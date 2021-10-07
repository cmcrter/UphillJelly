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
    [StructLayout(LayoutKind.Sequential, Size = 512)]
    public struct Friend
    {
#if !UNITY_SWITCH || UNITY_EDITOR

        public nn.account.NetworkServiceAccountId GetAccountId()
        {
            return new nn.account.NetworkServiceAccountId();
        }

        public nn.account.Nickname GetNickname()
        {
            return new nn.account.Nickname();
        }

        public FriendPresence GetPresence()
        {
            return new FriendPresence();
        }

        public nn.Result GetProfileImage(ref long outSize, byte[] buffer)
        {
            return new nn.Result();
        }

        public bool IsFavorite()
        {
            return false;
        }

        public bool IsNewly()
        {
            return false;
        }

        public bool IsValid()
        {
            return false;
        }

        public nn.Result Update()
        {
            return new nn.Result();
        }
#else

        public nn.account.NetworkServiceAccountId GetAccountId()
        {
            return GetAccountId(this);
        }

        public nn.account.Nickname GetNickname()
        {
            return GetNickname(this);
        }

        public FriendPresence GetPresence()
        {
            FriendPresence presence = new FriendPresence();
            GetPresence(this, ref presence);
            return presence;
        }

        public nn.Result GetProfileImage(ref long outSize, byte[] buffer)
        {
            return GetProfileImage(this, ref outSize, buffer, buffer.LongLength);
        }

        public bool IsFavorite()
        {
            return IsFavorite(this);
        }

        public bool IsNewly()
        {
            return IsNewly(this);
        }

        public bool IsValid()
        {
            return IsValid(this);
        }

        public nn.Result Update()
        {
            return Update(ref this);
        }

        [DllImport(Nn.DllName,
            CallingConvention = CallingConvention.Cdecl,
            EntryPoint = "nn_friends_FriendGetAccountId")]
        private static extern nn.account.NetworkServiceAccountId GetAccountId(Friend friend);

        [DllImport(Nn.DllName,
            CallingConvention = CallingConvention.Cdecl,
            EntryPoint = "nn_friends_FriendGetNickname")]
        private static extern nn.account.Nickname GetNickname(Friend friend);

        [DllImport(Nn.DllName,
            CallingConvention = CallingConvention.Cdecl,
            EntryPoint = "nn_friends_FriendGetPresence")]
        private static extern void GetPresence(Friend friend, ref FriendPresence outPresence);

        [DllImport(Nn.DllName,
            CallingConvention = CallingConvention.Cdecl,
            EntryPoint = "nn_friends_FriendGetProfileImage")]
        private static extern nn.Result GetProfileImage(Friend friend, ref long outSize, [In, Out] byte[] outBuffer, long size);

        [DllImport(Nn.DllName,
            CallingConvention = CallingConvention.Cdecl,
            EntryPoint = "nn_friends_FriendIsFavorite")]
        [return: MarshalAs(UnmanagedType.U1)]
        private static extern bool IsFavorite(Friend friend);

        [DllImport(Nn.DllName,
            CallingConvention = CallingConvention.Cdecl,
            EntryPoint = "nn_friends_FriendIsNewly")]
        [return: MarshalAs(UnmanagedType.U1)]
        private static extern bool IsNewly(Friend friend);

        [DllImport(Nn.DllName,
            CallingConvention = CallingConvention.Cdecl,
            EntryPoint = "nn_friends_FriendIsValid")]
        [return: MarshalAs(UnmanagedType.U1)]
        private static extern bool IsValid(Friend friend);

        [DllImport(Nn.DllName,
            CallingConvention = CallingConvention.Cdecl,
            EntryPoint = "nn_friends_FriendUpdate")]
        private static extern nn.Result Update(ref Friend pFriend);
#endif
    }
}
#endif