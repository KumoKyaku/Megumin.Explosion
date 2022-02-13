using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Megumin
{
    [Serializable]
    public class Version4Unity : ISerializationCallbackReceiver
    {
        [SerializeField]
        private Vector3Int VersionV3; //Vector4Int
        public Version Version => new Version(VersionV3.x, VersionV3.y, VersionV3.z);

        public void OnBeforeSerialize()
        {
            //VersionV3 = new Vector3Int(Version.Major, Version.Minor, Version.Build);
        }

        public void OnAfterDeserialize()
        {
            //Version = new Version(VersionV3.x, VersionV3.y, VersionV3.z);
        }

        public static implicit operator Version4Unity(Version version)
        {
            return new Version4Unity() { VersionV3 = new Vector3Int(version.Major, version.Minor, version.Build) };
        }
    }
}

