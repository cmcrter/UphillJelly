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

namespace nn.oe
{
    public static partial class ProgramTotalActiveTime
    {
#if !UNITY_SWITCH || UNITY_EDITOR
        public static System.TimeSpan Get()
        {
            return new System.TimeSpan();
        }

        public static long GetNanoseconds()
        {
            return 0L;
        }
#else
        public static System.TimeSpan Get()
        {
            return System.TimeSpan.FromMilliseconds(GetNanoseconds() / 1000000.0);
        }

        [DllImport(Nn.DllName,
            CallingConvention = CallingConvention.Cdecl,
            EntryPoint = "nn_oe_GetProgramTotalActiveTime")]
        public static extern long GetNanoseconds();
#endif
    }
}
#endif
