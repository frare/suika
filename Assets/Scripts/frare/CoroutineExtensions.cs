using System.Collections;
using UnityEngine;

namespace frare.CoroutineHelper
{
    public class CoroutineHelper
    {
        /// <summary>
        /// Stops the previous instance running in the <paramref name="coroutine"/> and then starts a new one.
        /// Useful for coroutines that should only have one instance running at a time but that can be restarted at any time.
        /// </summary>
        /// <param name="owner"></param>
        /// <param name="coroutine"></param>
        /// <param name="enumerator"></param>
        public static void StartCoroutineOverride(MonoBehaviour owner, ref Coroutine coroutine, IEnumerator enumerator)
        {
            if (coroutine != null)
            {
                owner.StopCoroutine(coroutine);
            }

            coroutine = owner.StartCoroutine(enumerator);
        }

        /// <summary>
        /// Starts a coroutine only if <paramref name="coroutine"/> does not already have a running instance,
        /// preventing simultaneous instances completely.
        /// </summary>
        /// <param name="owner"></param>
        /// <param name="coroutine"></param>
        /// <param name="enumerator"></param>
        public static void StartCoroutineIfNone(MonoBehaviour owner, ref Coroutine coroutine, IEnumerator enumerator)
        {
            if (coroutine != null)
            {
                return;
            }

            coroutine = owner.StartCoroutine(enumerator);
        }
    }
}