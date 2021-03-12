using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace UnityEngine
{
    public static class MeguminExtension_2B5D73B2
    {
        public static bool HasError(this UnityWebRequest uwr)
        {
            bool error = false;
#if UNITY_2020_1_OR_NEWER
            error = uwr.result == UnityWebRequest.Result.ConnectionError || uwr.result == UnityWebRequest.Result.ProtocolError;
#else
            error = uwr.isNetworkError || uwr.isHttpError;
#endif
            return error;
        }
    }
}




