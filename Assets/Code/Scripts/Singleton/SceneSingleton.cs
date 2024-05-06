using UnityEngine;

namespace Leon.Singleton
{
    public class SceneSingleton<T> : MonoBehaviour where T : Component
    {
        private static T instance;
        public static T Instance
        {
            get
            {
                if (instance == null) instance = FindObjectOfType<T>();
                return instance;
            }
        }

        protected virtual void Awake()
        {
            if (instance == null)
                instance = this as T;
            else
                Destroy(gameObject);
        }
    }
}