using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExecutionOrder : MonoBehaviour
{
    public static string filePath;
    // Start is called before the first frame update
    void Start()
    {
        filePath = Application.persistentDataPath;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
