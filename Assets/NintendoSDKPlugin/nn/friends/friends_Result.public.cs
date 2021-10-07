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
        public static ErrorRange ResultNotInitialized { get { return new ErrorRange(121, 1, 2); } }
        public static ErrorRange ResultInvalidArgument { get { return new ErrorRange(121, 2, 3); } }
        public static ErrorRange ResultUserNotOpened { get { return new ErrorRange(121, 3, 4); } }
        public static ErrorRange ResultNetworkServiceAccountNotLinked { get { return new ErrorRange(121, 4, 5); } }
        public static ErrorRange ResultOwnNetworkServiceAccountSpecified { get { return new ErrorRange(121, 5, 6); } }
        public static ErrorRange ResultInternetRequestNotAccepted { get { return new ErrorRange(121, 6, 7); } }
        public static ErrorRange ResultNotCalled { get { return new ErrorRange(121, 7, 8); } }
        public static ErrorRange ResultCallInProgress { get { return new ErrorRange(121, 8, 9); } }
        public static ErrorRange ResultCanceled { get { return new ErrorRange(121, 9, 10); } }
        public static ErrorRange ResultProfileImageCacheNotFound { get { return new ErrorRange(121, 10, 11); } }
        public static ErrorRange ResultOutOfMemory { get { return new ErrorRange(121, 11, 12); } }
        public static ErrorRange ResultOutOfResource { get { return new ErrorRange(121, 12, 13); } }
        public static ErrorRange ResultReservedKey { get { return new ErrorRange(121, 13, 14); } }
        public static ErrorRange ResultDuplicatedKey { get { return new ErrorRange(121, 14, 15); } }
        public static ErrorRange ResultNotificationNotFound { get { return new ErrorRange(121, 15, 16); } }
        public static ErrorRange ResultPlayHistoryRegistrationKeyBroken { get { return new ErrorRange(121, 21, 22); } }
        public static ErrorRange ResultOwnPlayHistoryRegistrationKey { get { return new ErrorRange(121, 22, 23); } }
        public static ErrorRange ResultAppletCanceled { get { return new ErrorRange(121, 30, 31); } }
        public static ErrorRange ResultApplicationInfoNotRegistered { get { return new ErrorRange(121, 40, 41); } }
        public static ErrorRange ResultNotPermitted { get { return new ErrorRange(121, 90, 91); } }
        public static ErrorRange ResultInvalidOperation { get { return new ErrorRange(121, 92, 93); } }
        public static ErrorRange ResultNotImplemented { get { return new ErrorRange(121, 99, 100); } }
        public static ErrorRange ResultResponseFormatError { get { return new ErrorRange(121, 100, 200); } }
        public static ErrorRange ResultHttpError { get { return new ErrorRange(121, 1000, 2000); } }
        public static ErrorRange ResultServerError { get { return new ErrorRange(121, 2000, 4000); } }
    }
}
#endif