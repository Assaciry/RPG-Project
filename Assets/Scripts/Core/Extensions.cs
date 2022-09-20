using UnityEngine;
using RPG.ScratchSaving;

namespace RPG.Core
{
    public static class Extensions
    {
        public static SerializableVector3 ToSerializable(this Vector3 vector)
        {
            return new SerializableVector3(vector);
        }

        public static Vector3 ToVector3(this SerializableVector3 sv)
        {
            return new Vector3(sv.x, sv.y, sv.z);
        }
    }
}
