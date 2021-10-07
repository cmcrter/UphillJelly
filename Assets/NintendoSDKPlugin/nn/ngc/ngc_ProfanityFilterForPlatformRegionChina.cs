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

namespace nn.ngc
{
    public static partial class ProfanityFilterForPlatformRegionChina
    {
#if !UNITY_SWITCH || UNITY_EDITOR
        public static nn.Result CheckProfanityWords(ref bool pOutCheckResult, string pText)
        {
            return new nn.Result();
        }

        public static nn.Result CheckProfanityWords(ref bool pOutCheckResult, StringBuilder pText)
        {
            return new nn.Result();
        }

        public static nn.Result MaskProfanityWords(StringBuilder pOutText)
        {
            return new nn.Result();
        }
#else
        [DllImport(Nn.DllName,
            CallingConvention = CallingConvention.Cdecl,
            EntryPoint = "nn_ngc_CheckProfanityWordsInTextForPlatformRegionChina",
            CharSet = CharSet.Ansi)]
        public static extern nn.Result CheckProfanityWords(
            [MarshalAs(UnmanagedType.U1)] ref bool pOutCheckResult, [In] string pText);

        [DllImport(Nn.DllName,
            CallingConvention = CallingConvention.Cdecl,
            EntryPoint = "nn_ngc_CheckProfanityWordsInTextForPlatformRegionChina",
            CharSet = CharSet.Ansi)]
        public static extern nn.Result CheckProfanityWords(
            [MarshalAs(UnmanagedType.U1)] ref bool pOutCheckResult, [In] StringBuilder pText);

#if NET_2_0 || NET_2_0_SUBSET
        public static nn.Result MaskProfanityWords(StringBuilder pOutText)
        {
            string text = pOutText.ToString();
            int byteCount = Encoding.UTF8.GetByteCount(text);
            byte[] bytes = new byte[byteCount + 1];
            Encoding.UTF8.GetBytes(text, 0, text.Length, bytes, 0);
            bytes[byteCount] = 0;

            nn.Result result = MaskProfanityWords(bytes);
            pOutText.Length = 0;
            pOutText.Append(Encoding.UTF8.GetString(bytes).TrimEnd('\0'));
            return result;
        }

        [DllImport(Nn.DllName,
            CallingConvention = CallingConvention.Cdecl,
            EntryPoint = "nn_ngc_MaskProfanityWordsInTextForPlatformRegionChina")]
        private static extern nn.Result MaskProfanityWords(
            [In, Out] byte[] pOutText);
#else
        [DllImport(Nn.DllName,
            CallingConvention = CallingConvention.Cdecl,
            EntryPoint = "nn_ngc_MaskProfanityWordsInTextForPlatformRegionChina", 
            CharSet = CharSet.Ansi)]
        public static extern nn.Result MaskProfanityWords(
            [In, Out] StringBuilder pOutText);
#endif // NET_LEGACY
#endif // !UNITY_SWITCH || UNITY_EDITOR
    }
}
#endif
