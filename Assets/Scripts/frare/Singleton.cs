using UnityEngine;

namespace frare
{
    public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        // attributes
        private static T instance;

        // properties
        protected virtual bool DontDestroyWhenLoad => true;

        // static instance property
        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    // try to find inside the scene
                    instance = FindAnyObjectByType<T>();

                    // still null? error
                    if (instance == null)
                    {
                        Debug.LogError($"Singleton of type {nameof(T)} is not initialized");
                    }
                }

                return instance;
            }
        }

        // initialize the singleton gameObject as the static instance
        // or delete a new instance when there is one already
        protected virtual void Awake()
        {
            if (instance == null)
            {
                instance = this as T;
                if (DontDestroyWhenLoad) DontDestroyOnLoad(transform.gameObject);
            }
            else if (instance != this) Destroy(this.gameObject);
        }

        // cleanup
        protected virtual void OnDestroy()
        {
            if (this == instance) instance = null;
        }
    }
}