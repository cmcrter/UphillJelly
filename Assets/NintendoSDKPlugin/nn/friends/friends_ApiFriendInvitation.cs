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
        public static bool TryPopFriendInvitationNotificationInfo(ref nn.account.Uid pOutUid, ref long pOutSize, byte[] pOutBuffer)
        {
            pOutUid = new nn.account.Uid();
            pOutSize = 0;
            pOutBuffer[0] = 0;
            return false;
        }
#else
        public static bool TryPopFriendInvitationNotificationInfo(ref nn.account.Uid pOutUid, ref long pOutSize, byte[] pOutBuffer)
        {
            return TryPopFriendInvitationNotificationInfo(ref pOutUid, ref pOutSize, pOutBuffer, pOutBuffer.LongLength);
        }

        [DllImport(Nn.DllName,
            CallingConvention = CallingConvention.Cdecl,
            EntryPoint = "nn_friends_TryPopFriendInvitationNotificationInfo")]
        private static extern bool TryPopFriendInvitationNotificationInfo(ref nn.account.Uid pOutUid, ref long pOutSize, [In, Out] byte[] pOutBuffer, long bufferSize);
#endif
    }
}
#endif
