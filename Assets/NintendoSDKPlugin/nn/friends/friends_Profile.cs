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
using System.Text;
using System.Runtime.InteropServices;

namespace nn.friends
{
    [StructLayout(LayoutKind.Sequential, Size = 256)]
    public struct Profile
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

        public nn.Result GetProfileImageUrl(ref string outUrl, ImageSize imageSize)
        {
            return new nn.Result();
        }

        public bool IsValid()
        {
            return false;
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

        public nn.Result GetProfileImageUrl(ref string outUrl, ImageSize imageSize)
        {
            StringBuilder sb = new StringBuilder(160);
            nn.Result result = GetProfileImageUrl(this, sb, imageSize);
            outUrl = sb.ToString();
            return result;
        }

        public bool IsValid()
        {
            return IsValid(this);
        }

        [DllImport(Nn.DllName,
            CallingConvention = CallingConvention.Cdecl,
            EntryPoint = "nn_friends_Profile_GetAccountId")]
        private static extern nn.account.NetworkServiceAccountId GetAccountId(Profile profile);

        [DllImport(Nn.DllName,
            CallingConvention = CallingConvention.Cdecl,
            EntryPoint = "nn_friends_Profile_GetNickname")]
        private static extern nn.account.Nickname GetNickname(Profile profile);

        [DllImport(Nn.DllName,
            CallingConvention = CallingConvention.Cdecl,
            EntryPoint = "nn_friends_Profile_GetProfileImageUrl")]
        private static extern nn.Result GetProfileImageUrl(Profile profile,
            [In, Out, MarshalAs(UnmanagedType.LPStr, SizeConst = 160)] StringBuilder outUrl, ImageSize imageSize);

        [DllImport(Nn.DllName,
            CallingConvention = CallingConvention.Cdecl,
            EntryPoint = "nn_friends_Profile_IsValid")]
        [return: MarshalAs(UnmanagedType.U1)]
        private static extern bool IsValid(Profile profile);
#endif
    }
}
#endif