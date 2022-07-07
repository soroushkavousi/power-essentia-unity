using System;
using System.IO;
using System.Text;
using UnityEngine;

public static class StorageGateway
{
    private static readonly string _rootFolderName = Application.persistentDataPath + "/data";
    //private static readonly string _rootFolderName = Application.streamingAssetsPath + "/data";
    //private static readonly string _rootFolderName = "Data";

    private static string GetAbsolutePath(string relativeFilePath)
    {
        try
        {
            var absoluteFilePath = Path.Combine(_rootFolderName, relativeFilePath);
            return absoluteFilePath;
        }
        catch (Exception ex)
        {
            Debug.LogError(ex);
        }
        return default;
    }

    public static bool DoesDataExist(string relativeDataFilePath)
    {
        var absoluteDataFilePath = GetAbsolutePath(relativeDataFilePath);
        return File.Exists(absoluteDataFilePath);
    }

    public static string ReadData(string relativeDataFilePath)
    {
        var absoluteDataFilePath = GetAbsolutePath(relativeDataFilePath);
        if (File.Exists(absoluteDataFilePath) == false)
            return "";

        var text = File.ReadAllText(absoluteDataFilePath, Encoding.UTF8);
        return text;
    }

    public static T ReadData<T>(string relativeDataFilePath)
    {
        var jsonText = ReadData(relativeDataFilePath);
        var obj = jsonText.FromJson<T>();
        return obj;
    }

    public static void WriteData(string relativeDataFilePath, string contents)
    {
        var absoluteDataFilePath = GetAbsolutePath(relativeDataFilePath);
        CreateDirectoryIfNotExists(absoluteDataFilePath);
        File.WriteAllText(absoluteDataFilePath, contents, Encoding.UTF8);
    }

    public static void WriteData(string relativeDataFilePath, object obj)
    {
        var jsonText = obj.ToJson();
        WriteData(relativeDataFilePath, jsonText);
    }

    public static void CreateDirectoryIfNotExists(string path)
    {
        var directoryPath = new DirectoryInfo(path).Parent.FullName;
        Directory.CreateDirectory(directoryPath);
    }

    public static void DeleteFile(string relativeDataFilePath)
    {
        var absoluteDataFilePath = GetAbsolutePath(relativeDataFilePath);
        File.Delete(absoluteDataFilePath);
    }
}