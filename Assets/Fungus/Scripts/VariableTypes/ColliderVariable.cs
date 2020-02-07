// This code is part of the Fungus library (https://github.com/snozbot/fungus)
// It is released for free under the MIT open source license (https://github.com/snozbot/fungus/blob/master/LICENSE)

/*This script has been, partially or completely, generated by the Fungus.GenerateVariableWindow*/

using UnityEngine;

namespace Fungus
{
    /// <summary>
    /// Collider variable type.
    /// </summary>
    [VariableInfo("Other", "Collider")]
    [AddComponentMenu("")]
    [System.Serializable]
    public class ColliderVariable : VariableBase<UnityEngine.Collider>
    { }

    /// <summary>
    /// Container for a Collider variable reference or constant value.
    /// </summary>
    [System.Serializable]
    public struct ColliderData
    {
        [SerializeField]
        [VariableProperty("<Value>", typeof(ColliderVariable))]
        public ColliderVariable colliderRef;

        [SerializeField]
        public UnityEngine.Collider colliderVal;

        public static implicit operator UnityEngine.Collider(ColliderData ColliderData)
        {
            return ColliderData.Value;
        }

        public ColliderData(UnityEngine.Collider v)
        {
            colliderVal = v;
            colliderRef = null;
        }

        public UnityEngine.Collider Value
        {
            get { return (colliderRef == null) ? colliderVal : colliderRef.Value; }
            set { if (colliderRef == null) { colliderVal = value; } else { colliderRef.Value = value; } }
        }

        public string GetDescription()
        {
            if (colliderRef == null)
            {
                return colliderVal != null ? colliderVal.ToString() : string.Empty;
            }
            else
            {
                return colliderRef.Key;
            }
        }
    }
}