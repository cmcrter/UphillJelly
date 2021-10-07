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
    public static partial class Friends
    {
#if !UNITY_SWITCH || UNITY_EDITOR
        public static void Initialize()
        {
        }

        public static nn.Result GetFriendList(
            ref int outCount, nn.account.NetworkServiceAccountId[] outAccountIds,
            nn.account.Uid uid, int offset, FriendFilter filter)
        {
            return new nn.Result();
        }

        public static nn.Result GetFriendList(
            ref int outCount, nn.account.NetworkServiceAccountId[] outAccountIds,
            int offset, FriendFilter filter)
        {
            return new nn.Result();
        }

        public static nn.Result GetFriendList(
            ref int outCount, Friend[] outFriends,
            nn.account.Uid uid, int offset, FriendFilter filter)
        {
            return new nn.Result();
        }

        public static nn.Result GetFriendList(
            ref int outCount, Friend[] outFriends,
            int offset, FriendFilter filter)
        {
            return new nn.Result();
        }

        public static nn.Result UpdateFriendInfo(
            Friend[] outFriends,
            nn.account.Uid uid, nn.account.NetworkServiceAccountId[] accountIds)
        {
            return new nn.Result();
        }

        public static nn.Result UpdateFriendInfo(
            Friend[] outFriends,
            nn.account.NetworkServiceAccountId[] accountIds)
        {
            return new nn.Result();
        }

        public static nn.Result CheckFriendListAvailability(ref bool outIsAvailable, nn.account.Uid uid)
        {
            return new nn.Result();
        }

        public static nn.Result CheckFriendListAvailability(ref bool outIsAvailable)
        {
            return new nn.Result();
        }

        public static nn.Result EnsureFriendListAvailable(nn.friends.AsyncContext outAsync, nn.account.Uid uid)
        {
            return new nn.Result();
        }

        public static nn.Result EnsureFriendListAvailable(nn.friends.AsyncContext outAsync)
        {
            return new nn.Result();
        }

        public static nn.Result GetBlockedUserList(
            ref int outCount, nn.account.NetworkServiceAccountId[] outAccountIds,
            nn.account.Uid uid, int offset)
        {
            return new nn.Result();
        }

        public static nn.Result GetBlockedUserList(
            ref int outCount, nn.account.NetworkServiceAccountId[] outAccountIds,
            int offset)
        {
            return new nn.Result();
        }

        public static nn.Result CheckBlockedUserListAvailability(ref bool outIsAvailable, nn.account.Uid uid)
        {
            return new nn.Result();
        }

        public static nn.Result CheckBlockedUserListAvailability(ref bool outIsAvailable)
        {
            return new nn.Result();
        }

        public static nn.Result EnsureBlockedUserListAvailable(nn.friends.AsyncContext outAsync, nn.account.Uid uid)
        {
            return new nn.Result();
        }

        public static nn.Result EnsureBlockedUserListAvailable(nn.friends.AsyncContext outAsync)
        {
            return new nn.Result();
        }

        public static nn.Result GetProfileList(
            AsyncContext outAsync, Profile[] outProfiles,
            nn.account.Uid uid, nn.account.NetworkServiceAccountId[] accountIds)
        {
            return new nn.Result();
        }

        public static nn.Result GetProfileList(
            AsyncContext outAsync, Profile[] outProfiles,
            nn.account.NetworkServiceAccountId[] accountIds)
        {
            return new nn.Result();
        }

        public static nn.Result DeclareOpenOnlinePlaySession(nn.account.Uid uid)
        {
            return new nn.Result();
        }

        public static nn.Result DeclareOpenOnlinePlaySession()
        {
            return new nn.Result();
        }

        public static nn.Result DeclareCloseOnlinePlaySession(nn.account.Uid uid)
        {
            return new nn.Result();
        }

        public static nn.Result DeclareCloseOnlinePlaySession()
        {
            return new nn.Result();
        }

        public static nn.Result GetPlayHistoryRegistrationKey(
            ref PlayHistoryRegistrationKey outKey,
            nn.account.Uid uid, bool isLocalPlay)
        {
            return new nn.Result();
        }

        public static nn.Result GetPlayHistoryRegistrationKey(
            ref PlayHistoryRegistrationKey outKey,
            bool isLocalPlay)
        {
            return new nn.Result();
        }

        public static nn.Result AddPlayHistory(
            nn.account.Uid uid,
            PlayHistoryRegistrationKey key,
            InAppScreenName inAppScreenName, InAppScreenName myInAppScreenName)
        {
            return new nn.Result();
        }

        public static nn.Result AddPlayHistory(
            PlayHistoryRegistrationKey key,
            InAppScreenName inAppScreenName, InAppScreenName myInAppScreenName)
        {
            return new nn.Result();
        }
#else
        [DllImport(Nn.DllName,
            CallingConvention = CallingConvention.Cdecl,
            EntryPoint = "nn_friends_Initialize")]
        public static extern void Initialize();

        public static nn.Result GetFriendList(
            ref int outCount, nn.account.NetworkServiceAccountId[] outAccountIds,
            nn.account.Uid uid, int offset, FriendFilter filter)
        {
            return GetFriendList(ref outCount, outAccountIds, uid, offset, outAccountIds.Length, filter);
        }

        [DllImport(Nn.DllName,
            CallingConvention = CallingConvention.Cdecl,
            EntryPoint = "nn_friends_GetFriendList1")]
        private static extern nn.Result GetFriendList(
            ref int outCount, [In, Out] nn.account.NetworkServiceAccountId[] outAccountIds,
            nn.account.Uid uid, int offset, int count, FriendFilter filter);

        public static nn.Result GetFriendList(
            ref int outCount, nn.account.NetworkServiceAccountId[] outAccountIds,
            int offset, FriendFilter filter)
        {
            return GetFriendList(ref outCount, outAccountIds, offset, outAccountIds.Length, filter);
        }

        [DllImport(Nn.DllName,
            CallingConvention = CallingConvention.Cdecl,
            EntryPoint = "nn_friends_GetFriendList2")]
        private static extern nn.Result GetFriendList(
            ref int outCount, [In, Out] nn.account.NetworkServiceAccountId[] outAccountIds,
            int offset, int count, FriendFilter filter);

        public static nn.Result GetFriendList(
            ref int outCount, Friend[] outFriends,
            nn.account.Uid uid, int offset, FriendFilter filter)
        {
            return GetFriendList(ref outCount, outFriends, uid, offset, outFriends.Length, filter);
        }

        [DllImport(Nn.DllName,
            CallingConvention = CallingConvention.Cdecl,
            EntryPoint = "nn_friends_GetFriendList3")]
        private static extern nn.Result GetFriendList(
            ref int outCount, [In, Out] Friend[] outFriends,
            nn.account.Uid uid, int offset, int count, FriendFilter filter);

        public static nn.Result GetFriendList(
            ref int outCount, Friend[] outFriends,
            int offset, FriendFilter filter)
        {
            return GetFriendList(ref outCount, outFriends, offset, outFriends.Length, filter);
        }

        [DllImport(Nn.DllName,
            CallingConvention = CallingConvention.Cdecl,
            EntryPoint = "nn_friends_GetFriendList4")]
        private static extern nn.Result GetFriendList(
            ref int outCount, [In, Out] Friend[] outFriends,
            int offset, int count, FriendFilter filter);

        public static nn.Result UpdateFriendInfo(
            Friend[] outFriends,
            nn.account.Uid uid, nn.account.NetworkServiceAccountId[] accountIds)
        {
            return UpdateFriendInfo(outFriends, uid, accountIds, accountIds.Length);
        }

        [DllImport(Nn.DllName,
            CallingConvention = CallingConvention.Cdecl,
            EntryPoint = "nn_friends_UpdateFriendInfo1")]
        private static extern nn.Result UpdateFriendInfo(
            [In, Out] Friend[] outFriends,
            nn.account.Uid uid, [In] nn.account.NetworkServiceAccountId[] accountIds, int count);

        public static nn.Result UpdateFriendInfo(
            Friend[] outFriends,
            nn.account.NetworkServiceAccountId[] accountIds)
        {
            return UpdateFriendInfo(outFriends, accountIds, accountIds.Length);
        }

        [DllImport(Nn.DllName,
            CallingConvention = CallingConvention.Cdecl,
            EntryPoint = "nn_friends_UpdateFriendInfo2")]
        private static extern nn.Result UpdateFriendInfo(
            [In, Out] Friend[] outFriends,
            nn.account.NetworkServiceAccountId[] accountIds, int count);

        public static nn.Result EnsureFriendListAvailable(nn.friends.AsyncContext outAsync, nn.account.Uid uid)
        {
            return EnsureFriendListAvailable(outAsync._context, uid);
        }

        [DllImport(Nn.DllName,
            CallingConvention = CallingConvention.Cdecl,
            EntryPoint = "nn_friends_CheckFriendListAvailability1")]
        public static extern nn.Result CheckFriendListAvailability([MarshalAs(UnmanagedType.U1)] ref bool outIsAvailable, nn.account.Uid uid);

        [DllImport(Nn.DllName,
            CallingConvention = CallingConvention.Cdecl,
            EntryPoint = "nn_friends_CheckFriendListAvailability2")]
        public static extern nn.Result CheckFriendListAvailability([MarshalAs(UnmanagedType.U1)] ref bool outIsAvailable);

        [DllImport(Nn.DllName,
            CallingConvention = CallingConvention.Cdecl,
            EntryPoint = "nn_friends_EnsureFriendListAvailable1")]
        private static extern nn.Result EnsureFriendListAvailable(IntPtr outAsync, nn.account.Uid uid);

        public static nn.Result EnsureFriendListAvailable(nn.friends.AsyncContext outAsync)
        {
            return EnsureFriendListAvailable(outAsync._context);
        }

        [DllImport(Nn.DllName,
            CallingConvention = CallingConvention.Cdecl,
            EntryPoint = "nn_friends_EnsureFriendListAvailable2")]
        private static extern nn.Result EnsureFriendListAvailable(IntPtr outAsync);

        public static nn.Result GetBlockedUserList(
            ref int outCount, nn.account.NetworkServiceAccountId[] outAccountIds,
            nn.account.Uid uid, int offset)
        {
            return GetBlockedUserList(ref outCount, outAccountIds, uid, offset, outAccountIds.Length);
        }

        [DllImport(Nn.DllName,
            CallingConvention = CallingConvention.Cdecl,
            EntryPoint = "nn_friends_GetBlockedUserList1")]
        private static extern nn.Result GetBlockedUserList(
            ref int outCount, [In, Out] nn.account.NetworkServiceAccountId[] outAccountIds,
            nn.account.Uid uid, int offset, int count);

        public static nn.Result GetBlockedUserList(
            ref int outCount, nn.account.NetworkServiceAccountId[] outAccountIds,
            int offset)
        {
            return GetBlockedUserList(ref outCount, outAccountIds, offset, outAccountIds.Length);
        }

        [DllImport(Nn.DllName,
            CallingConvention = CallingConvention.Cdecl,
            EntryPoint = "nn_friends_GetBlockedUserList2")]
        private static extern nn.Result GetBlockedUserList(
            ref int outCount, [In, Out] nn.account.NetworkServiceAccountId[] outAccountIds,
            int offset, int count);

        public static nn.Result GetProfileList(
            AsyncContext outAsync, Profile[] outProfiles,
            nn.account.Uid uid, nn.account.NetworkServiceAccountId[] accountIds)
        {
            return GetProfileList(outAsync._context, outProfiles, uid, accountIds, accountIds.Length);
        }

        [DllImport(Nn.DllName,
            CallingConvention = CallingConvention.Cdecl,
            EntryPoint = "nn_friends_CheckBlockedUserListAvailability1")]
        public static extern nn.Result CheckBlockedUserListAvailability([MarshalAs(UnmanagedType.U1)] ref bool outIsAvailable, nn.account.Uid uid);

        [DllImport(Nn.DllName,
            CallingConvention = CallingConvention.Cdecl,
            EntryPoint = "nn_friends_CheckBlockedUserListAvailability2")]
        public static extern nn.Result CheckBlockedUserListAvailability([MarshalAs(UnmanagedType.U1)] ref bool outIsAvailable);

        public static nn.Result EnsureBlockedUserListAvailable(nn.friends.AsyncContext outAsync, nn.account.Uid uid)
        {
            return EnsureBlockedUserListAvailable(outAsync._context, uid);
        }

        [DllImport(Nn.DllName,
            CallingConvention = CallingConvention.Cdecl,
            EntryPoint = "nn_friends_EnsureBlockedUserListAvailable1")]
        private static extern nn.Result EnsureBlockedUserListAvailable(IntPtr outAsync, nn.account.Uid uid);

        public static nn.Result EnsureBlockedUserListAvailable(nn.friends.AsyncContext outAsync)
        {
            return EnsureBlockedUserListAvailable(outAsync._context);
        }

        [DllImport(Nn.DllName,
            CallingConvention = CallingConvention.Cdecl,
            EntryPoint = "nn_friends_EnsureBlockedUserListAvailable2")]
        private static extern nn.Result EnsureBlockedUserListAvailable(IntPtr outAsync);

        [DllImport(Nn.DllName,
            CallingConvention = CallingConvention.Cdecl,
            EntryPoint = "nn_friends_GetProfileList1")]
        private static extern nn.Result GetProfileList(
            IntPtr outAsync, [In, Out] Profile[] outProfiles,
            nn.account.Uid uid, [In] nn.account.NetworkServiceAccountId[] accountIds, int count);

        public static nn.Result GetProfileList(
            AsyncContext outAsync, Profile[] outProfiles,
            nn.account.NetworkServiceAccountId[] accountIds)
        {
            return GetProfileList(outAsync._context, outProfiles, accountIds, accountIds.Length);
        }

        [DllImport(Nn.DllName,
            CallingConvention = CallingConvention.Cdecl,
            EntryPoint = "nn_friends_GetProfileList2")]
        private static extern nn.Result GetProfileList(
            IntPtr outAsync, [In, Out] Profile[] outProfiles,
            [In] nn.account.NetworkServiceAccountId[] accountIds, int count);

        [DllImport(Nn.DllName,
            CallingConvention = CallingConvention.Cdecl,
            EntryPoint = "nn_friends_DeclareOpenOnlinePlaySession1")]
        public static extern nn.Result DeclareOpenOnlinePlaySession(nn.account.Uid uid);

        [DllImport(Nn.DllName,
            CallingConvention = CallingConvention.Cdecl,
            EntryPoint = "nn_friends_DeclareOpenOnlinePlaySession2")]
        public static extern nn.Result DeclareOpenOnlinePlaySession();

        [DllImport(Nn.DllName,
            CallingConvention = CallingConvention.Cdecl,
            EntryPoint = "nn_friends_DeclareCloseOnlinePlaySession1")]
        public static extern nn.Result DeclareCloseOnlinePlaySession(nn.account.Uid uid);

        [DllImport(Nn.DllName,
            CallingConvention = CallingConvention.Cdecl,
            EntryPoint = "nn_friends_DeclareCloseOnlinePlaySession2")]
        public static extern nn.Result DeclareCloseOnlinePlaySession();

        [DllImport(Nn.DllName,
            CallingConvention = CallingConvention.Cdecl,
            EntryPoint = "nn_friends_GetPlayHistoryRegistrationKey1")]
        public static extern nn.Result GetPlayHistoryRegistrationKey(
            ref PlayHistoryRegistrationKey outKey,
            nn.account.Uid uid, [MarshalAs(UnmanagedType.U1)] bool isLocalPlay);

        [DllImport(Nn.DllName,
            CallingConvention = CallingConvention.Cdecl,
            EntryPoint = "nn_friends_GetPlayHistoryRegistrationKey2")]
        public static extern nn.Result GetPlayHistoryRegistrationKey(
            ref PlayHistoryRegistrationKey outKey,
            [MarshalAs(UnmanagedType.U1)] bool isLocalPlay);

        [DllImport(Nn.DllName,
            CallingConvention = CallingConvention.Cdecl,
            EntryPoint = "nn_friends_AddPlayHistory1")]
        public static extern nn.Result AddPlayHistory(
            nn.account.Uid uid,
            PlayHistoryRegistrationKey key,
            InAppScreenName inAppScreenName, InAppScreenName myInAppScreenName);

        [DllImport(Nn.DllName,
            CallingConvention = CallingConvention.Cdecl,
            EntryPoint = "nn_friends_AddPlayHistory2")]
        public static extern nn.Result AddPlayHistory(
            PlayHistoryRegistrationKey key,
            InAppScreenName inAppScreenName, InAppScreenName myInAppScreenName);
#endif
    }
}
#endif
