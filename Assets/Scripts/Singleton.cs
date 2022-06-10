using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton : MonoBehaviour
{
    public static Singleton Instance;

    // Start is called before the first frame update
    void Start()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
