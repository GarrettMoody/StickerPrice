using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExecutionOrder : MonoBehaviour
{
    public static string filePath;
    public static string readFilePath;
    // Start is called before the first frame update
    void Start()
    {
        filePath = Application.persistentDataPath;
        readFilePath = Application.streamingAssetsPath;
    }

}
