﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using Newtonsoft.Json;

public class FileUtility {

	public List<string> ReadFromFile(string filePath)
    {
        List<string> values = new List<string>();
        StreamReader reader = new StreamReader(filePath);
        while (!reader.EndOfStream)
        {
            values.Add(reader.ReadLine());
        }
        reader.Close();
        return values;
    }

    public string ReadJson(string filePath)
    {
        StreamReader reader = new StreamReader(filePath);
        string json = reader.ReadToEnd();
        reader.Close();
        return json;
    }

    public void WriteToFile(string filePath, List<string> values)
    {
        StreamWriter writer = new StreamWriter(filePath, true);
        values.ForEach(writer.WriteLine);
        writer.Close();
    }

    public void WriteJson(string filePath, string json)
    {
        File.WriteAllText(filePath, json);
    }

    public void ClearFile(string filePath)
    {
        FileStream fileStream = File.Open(filePath, FileMode.Open);

        /* 
         * Set the length of filestream to 0 and flush it to the physical file.
         *
         * Flushing the stream is important because this ensures that
         * the changes to the stream trickle down to the physical file.
         * 
         */
        fileStream.SetLength(0);
        fileStream.Close();
    }
}
