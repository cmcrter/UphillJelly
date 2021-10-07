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
        public const int FriendInvitationInviteeCountMax = 16;
        public const long FriendInvitationApplicationParameterSizeMax = 1024;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct FriendInvitationGameModeDescription
    {
        public const int TextSize = 192;

        public string enUs
        {
            get { return Get(Language.EnUs); }
            set { Set(value, Language.EnUs); }
        }
        public string enGb
        {
            get { return Get(Language.EnGb); }
            set { Set(value, Language.EnGb); }
        }
        public string ja
        {
            get { return Get(Language.Ja); }
            set { Set(value, Language.Ja); }
        }
        public string fr
        {
            get { return Get(Language.Fr); }
            set { Set(value, Language.Fr); }
        }
        public string de
        {
            get { return Get(Language.De); }
            set { Set(value, Language.De); }
        }
        public string es419
        {
            get { return Get(Language.Es419); }
            set { Set(value, Language.Es419); }
        }
        public string es
        {
            get { return Get(Language.Es); }
            set { Set(value, Language.Es); }
        }
        public string it
        {
            get { return Get(Language.It); }
            set { Set(value, Language.It); }
        }
        public string nl
        {
            get { return Get(Language.Nl); }
            set { Set(value, Language.Nl); }
        }
        public string frCa
        {
            get { return Get(Language.FrCa); }
            set { Set(value, Language.FrCa); }
        }
        public string pt
        {
            get { return Get(Language.Pt); }
            set { Set(value, Language.Pt); }
        }
        public string ru
        {
            get { return Get(Language.Ru); }
            set { Set(value, Language.Ru); }
        }
        public string zhHans
        {
            get { return Get(Language.ZhHans); }
            set { Set(value, Language.ZhHans); }
        }
        public string zhHant
        {
            get { return Get(Language.ZhHant); }
            set { Set(value, Language.ZhHant); }
        }
        public string ko
        {
            get { return Get(Language.Ko); }
            set { Set(value, Language.Ko); }
        }
        public string ptBr
        {
            get { return Get(Language.ptBr); }
            set { Set(value, Language.ptBr); }
        }
        
        private enum Language
        {
            EnUs = 0,
            EnGb,
            Ja,
            Fr,
            De,
            Es419,
            Es,
            It,
            Nl,
            FrCa,
            Pt,
            Ru,
            ZhHans,
            ZhHant,
            Ko,
            ptBr,

            Length,
        }

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = TextSize * (int)Language.Length)]
        private byte[] data;

        private string Get(Language language)
        {
            if (data == null)
            {
                return string.Empty;
            }
            return System.Text.Encoding.UTF8.GetString(data, (int)language * TextSize, TextSize);
        }

        private void Set(string value, Language language)
        {
            if (data == null)
            {
                data = new byte[TextSize * (int)Language.Length];
            }
            var tmp = System.Text.Encoding.UTF8.GetBytes(value);
            var length = System.Math.Min(tmp.Length, TextSize - 1);
            System.Array.Copy(tmp, 0, data, (int)language * TextSize, (long)length);
            data[(int)language * TextSize + tmp.Length] = 0;
        }
    }
}
#endif
