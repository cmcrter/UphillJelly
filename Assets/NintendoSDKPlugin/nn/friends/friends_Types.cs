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
        public const int FriendCountMax = 300;
        public const int BlockedUserCountMax = 100;
        public const long PresenceAppFieldSize = 192;
        public const int InAppScreenNameLengthMax = 20;
        public const int GetProfileCountMax = 100;
        public const long ProfileImageSizeMax = 128 * 1024;
        public const int NotificationCountMax = 100;
    }

    public enum PresenceStatus
    {
        Offline = 0,
        Online = 1,
        OnlinePlay = 2,
    }

    public enum PresenceStatusFilter
    {
        None = 0,
        Online = 1,
        OnlinePlay = 2,
        OnlineOrOnlinePlay = 3,
    }

    public enum ImageSize
    {
        ImageSize64x64 = 64,
        ImageSize128x128 = 128,
        ImageSize256x256 = 256,

        ImageSizeStandard = ImageSize256x256
    }

    public enum NotificationType
    {
        Unkown = 0,
        FriendListUpdated = 1,
        FriendPresenceUpdated = 2,
        
        FriendRequestReceived = 101
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct InAppScreenName
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
        private byte[] name;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
        private char[] language;

        public InAppScreenName(string name, string language = "")
        {
            this.name = new byte[64];
            this.language = new char[8];
            Name = name;
            Language = language;
        }

        public string Name
        {
            get
            {
                if (name == null)
                {
                    return string.Empty;
                }
                return System.Text.Encoding.UTF8.GetString(name);
            }
            set
            {
                if (name == null)
                {
                    name = new byte[64];
                }
                var length = System.Math.Min(value.Length, nn.friends.Friends.InAppScreenNameLengthMax);
                var subStr = value.Substring(0, length);
                var tmp = System.Text.Encoding.UTF8.GetBytes(subStr);
                tmp.CopyTo(name, 0);
                name[tmp.Length] = 0;
            }
        }

        public string Language
        {
            get
            {
                if (language == null)
                {
                    return string.Empty;
                }
                int length = 0;
                for (; length < 7; length++)
                {
                    if (language[length] == '\0')
                    {
                        break;
                    }
                }
                return new string(language, 0, length);
            }
            set
            {
                if (language == null)
                {
                    language = new char[8];
                }
                var count = System.Math.Min(value.Length, 7);
                value.CopyTo(0, language, 0, count);
                language[count] = '\0';
            }
        }

        public override string ToString()
        {
            return string.Format("name:{0} language:{1}", Name, Language);
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct FriendFilter
    {
        public PresenceStatusFilter presenceStatus;
        [MarshalAs(UnmanagedType.U1)]
        public bool isFavoriteOnly;
        [MarshalAs(UnmanagedType.U1)]
        public bool isSameAppPresenceOnly;
        [MarshalAs(UnmanagedType.U1)]
        public bool isSameAppPlayedOnly;
        [MarshalAs(UnmanagedType.U1)]
        public bool isArbitraryAppPlayedOnly;
        public ulong presenceGroupId;

        public override string ToString()
        {
            return string.Format("{0} {1} {2} {3} {4}",
                presenceStatus, isFavoriteOnly, isSameAppPresenceOnly, isSameAppPlayedOnly, isArbitraryAppPlayedOnly, presenceGroupId);
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct ApplicationInfo
    {
        public ulong appId;
        public ulong presenceGroupId;

        public override string ToString()
        {
            return string.Format("{0} {1}",
                appId, presenceGroupId);
        }
    }

    [StructLayout(LayoutKind.Sequential, Size = Size)]
    public struct PlayHistoryRegistrationKey
    {
        private const int Size = 64;
        public byte[] GetValue()
        {
            byte[] value = new byte[Size];
            GCHandle handle = GCHandle.Alloc(this, GCHandleType.Pinned);
            Marshal.Copy(handle.AddrOfPinnedObject(), value, 0, Size);
            handle.Free();
            return value;
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct NotificationInfo
    {
        public NotificationType type;
        public nn.account.NetworkServiceAccountId accountId;

        public override string ToString()
        {
            return string.Format("{0} {1}",
                type, accountId);
        }
    }
}
#endif
