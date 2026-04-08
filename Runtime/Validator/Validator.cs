using System.Collections;
using UnityEngine;

namespace LK.Runtime.Validator
{
    public abstract class Validator : ScriptableObject
    {
        public abstract bool Result { get; protected set; }

        public abstract IEnumerator Verify();
    }
}
