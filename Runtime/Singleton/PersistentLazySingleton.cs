using UnityEngine;

// TODO: REfactor
namespace GameStack.Singleton
{
    /// <summary>
    /// This is lazy implementation of Singleton Design Pattern.
    /// Instance is created when someone call Instance property.
    /// Additionally, with this implementation you have same instance when
    /// moving to different scene.
    /// </summary>
    public class PersistentLazySingleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        // Flag used to mark singleton destruction.
        private static bool _singletonDestroyed = false;
        // Reference to our singular instance.
        private static T _instance;

        public static T Instance
        {
            get
            {
                // If game is closing and we already destroyed instance, we
                // shouldn't create new one!
                if (_singletonDestroyed)
                {
                    return null;
                }

                // If there is no object already, we should create new one.
                if (!_instance)
                {
                    // Creating new game object with singleton component.
                    // We don't need to assign reference here as Awake() will be
                    // called immediately after coponent is added.
                    new GameObject(typeof(T).ToString()).AddComponent<T>();
                    // And now we are making sure that object won't be destroyed
                    // when we will move to other scene.
                    DontDestroyOnLoad(_instance);
                }

                return _instance;
            }
        }

        /// <summary>
        /// Unity method called just after object creation - like constructor.
        /// </summary>
        protected virtual void Awake()
        {
            // If we don't have reference to instance and we didn't destroy
            // instance yet than this object will take control
            if (_instance == null && !_singletonDestroyed)
            {
                _instance = this as T;
            }
            else if (_instance != this)
            { // Else this is other instance and we should destroy it!
                Destroy(this);
            }
        }

        /// <summary>
        /// Unity method called before object destruction.
        /// </summary>
        protected virtual void OnDestroy()
        {
            // Skip if instance is other than this object.
            if (_instance != this)
            {
                return;
            }

            _singletonDestroyed = true;
            _instance = null;
        }
    }
}
