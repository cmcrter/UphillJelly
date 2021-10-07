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
using System.Runtime.InteropServices;

namespace nn.friends
{
    public static partial class Friends
    {
#if !UNITY_SWITCH || UNITY_EDITOR
        public static nn.Result ShowFriendList(nn.account.Uid uid)
        {
            return new nn.Result();
        }

        public static nn.Result ShowUserDetailInfo(nn.account.Uid uid, nn.account.NetworkServiceAccountId accountId,
            InAppScreenName myInAppScreenName, InAppScreenName inAppScreenName)
        {
            return new nn.Result();
        }

        public static nn.Result StartSendingFriendRequest(nn.account.Uid uid, nn.account.NetworkServiceAccountId accountId,
            InAppScreenName myInAppScreenName, InAppScreenName inAppScreenName)
        {
            return new nn.Result();
        }

        public static nn.Result ShowMethodsOfSendingFriendRequest(nn.account.Uid uid)
        {
            return new nn.Result();
        }

        public static nn.Result StartFacedFriendRequest(nn.account.Uid uid)
        {
            return new nn.Result();
        }
                      
        public static nn.Result ShowReceivedFriendRequestList(nn.account.Uid uid)
        {
            return new nn.Result();
        }

        public static nn.Result ShowBlockedUserList(nn.account.Uid uid)
        {
            return new nn.Result();
        }

        public static nn.Result StartFriendInvitation(nn.account.Uid uid, int maxInviteeCount,
            nn.friends.FriendInvitationGameModeDescription description,
            byte[] pAppParameter)
        {
            return new nn.Result();
        }

        public static nn.Result StartSendingFriendInvitation(nn.account.Uid uid,
            nn.account.NetworkServiceAccountId[] pInvitees,
            nn.friends.FriendInvitationGameModeDescription description,
            byte[] pAppParameter)
        {
            return new nn.Result();
        }
#else
        [DllImport(Nn.DllName,
            CallingConvention = CallingConvention.Cdecl,
            EntryPoint = "nn_friends_ShowFriendList")]
        public static extern nn.Result ShowFriendList(nn.account.Uid uid);

        [DllImport(Nn.DllName,
            CallingConvention = CallingConvention.Cdecl,
            EntryPoint = "nn_friends_ShowUserDetailInfo")]
        public static extern nn.Result ShowUserDetailInfo(nn.account.Uid uid, nn.account.NetworkServiceAccountId accountId,
            InAppScreenName myInAppScreenName, InAppScreenName inAppScreenName);

        [DllImport(Nn.DllName,
            CallingConvention = CallingConvention.Cdecl,
            EntryPoint = "nn_friends_StartSendingFriendRequest")]
        public static extern nn.Result StartSendingFriendRequest(nn.account.Uid uid, nn.account.NetworkServiceAccountId accountId,
            InAppScreenName myInAppScreenName, InAppScreenName inAppScreenName);

        [DllImport(Nn.DllName,
            CallingConvention = CallingConvention.Cdecl,
            EntryPoint = "nn_friends_ShowMethodsOfSendingFriendRequest")]
        public static extern nn.Result ShowMethodsOfSendingFriendRequest(nn.account.Uid uid);

        [DllImport(Nn.DllName,
            CallingConvention = CallingConvention.Cdecl,
            EntryPoint = "nn_friends_StartFacedFriendRequest")]
        public static extern nn.Result StartFacedFriendRequest(nn.account.Uid uid);

        [DllImport(Nn.DllName,
            CallingConvention = CallingConvention.Cdecl,
            EntryPoint = "nn_friends_ShowReceivedFriendRequestList")]
        public static extern nn.Result ShowReceivedFriendRequestList(nn.account.Uid uid);

        [DllImport(Nn.DllName,
            CallingConvention = CallingConvention.Cdecl,
            EntryPoint = "nn_friends_ShowBlockedUserList")]
        public static extern nn.Result ShowBlockedUserList(nn.account.Uid uid);

        public static nn.Result StartFriendInvitation(nn.account.Uid uid, int maxInviteeCount,
            nn.friends.FriendInvitationGameModeDescription description,
            byte[] pAppParameter)
        {
            return StartFriendInvitation(uid, maxInviteeCount, description, pAppParameter, pAppParameter.LongLength);
        }

        [DllImport(Nn.DllName,
            CallingConvention = CallingConvention.Cdecl,
            EntryPoint = "nn_friends_StartFriendInvitation")]
        private static extern nn.Result StartFriendInvitation(nn.account.Uid uid, int maxInviteeCount,
            nn.friends.FriendInvitationGameModeDescription description,
            [In] byte[] pAppParameter, long appParameterSize);

        public static nn.Result StartSendingFriendInvitation(nn.account.Uid uid,
            nn.account.NetworkServiceAccountId[] pInvitees,
            nn.friends.FriendInvitationGameModeDescription description,
            byte[] pAppParameter)
        {
            return StartSendingFriendInvitation(uid, pInvitees, pInvitees.Length, description,
                pAppParameter, pAppParameter.LongLength);
        }

        [DllImport(Nn.DllName,
            CallingConvention = CallingConvention.Cdecl,
            EntryPoint = "nn_friends_StartSendingFriendInvitation")]
        private static extern nn.Result StartSendingFriendInvitation(nn.account.Uid uid,
            [In] nn.account.NetworkServiceAccountId[] pInvitees, int inviteeCount,
            nn.friends.FriendInvitationGameModeDescription description,
            [In] byte[] pAppParameter, long appParameterSize);
#endif
    }
}
#endif