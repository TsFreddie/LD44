using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SingletonMonoBehaviour<T> : MonoBehaviour where T : SingletonMonoBehaviour<T>
{
    private static T instance;
    public static T I {
        get {
            if (instance != null) {
                return instance;
            } else {
                return default(T);
            }
        }
    }
    
    void Awake() {
        if (instance != null) {
            Destroy(this);
            return;
        }
        instance = (T)this;
        SingletonAwake();
    }

    protected virtual void SingletonAwake() {}
}
