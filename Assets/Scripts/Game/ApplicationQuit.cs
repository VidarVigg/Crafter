using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplicationQuit : MonoBehaviour
{
    public static ApplicationQuit INSTANCE;

    public delegate void OnQuit();
    public OnQuit onQuit;

    private void Awake()
    {
        DontDestroyOnLoad(this);
        INSTANCE = this;
    }
    void OnDestroy()
    {
        #if UNITY_EDITOR
        Debug.Log("On Destroy called");
        OnApplicationQuit();
        #endif
    }

    void OnApplicationQuit()
    {

    }
}
