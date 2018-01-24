using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor;

public class Build
{
    public static void Android()
    {
        //TBD
    }

    public static void iOS()
    {
        //TDB
    }

    [MenuItem("Build/Build Standalone Windows 64")]
    public static void BuildWindows()
    {
        Debug.Log("Starting Windows 64 Build");
        string path = "D:/builds/win64/";
        CheckDirectory(path);
        path += Application.productName + ".exe";
        BuildPlayerOptions options = new BuildPlayerOptions
        {
            scenes= GetScenesInternal(),
            locationPathName= path,
            target= BuildTarget.StandaloneWindows64,
            options= BuildOptions.None
        };
        InternalBuild(options);
    }

    [MenuItem("Build/Development/Build Dev Win64")]
    public static void BuildDevWindows()
    {
        Debug.Log("Starting Windows 64 Build");
        string path = "D:/builds/win64/";
        CheckDirectory(path);
        path += Application.productName + ".exe";
        BuildPlayerOptions options = new BuildPlayerOptions
        {
            scenes= GetScenesInternal(),
            locationPathName= path,
            target= BuildTarget.StandaloneWindows64,
            options= BuildOptions.Development
        };
        InternalBuild(options);
    }

    public static void Win64()
    {
        //GET OPTIONS FROM ARGS
        //exe path
        //

    }

    [MenuItem("Build/Build Standalone OSX")]
    public static void BuildMacintosh()
    {
        Debug.Log("Starting Standalone OSX Build");
        string path= "D:/builds/osx/"; //path must include executable name
        CheckDirectory(path);
        path += Application.productName;
        BuildPlayerOptions options = new BuildPlayerOptions
        {
            scenes= GetScenesInternal(),
            locationPathName= path,
            target= BuildTarget.StandaloneOSX,
            options= BuildOptions.None
        };
        InternalBuild(options);
    }

    public static void MacOS()
    {
        //TBD
    }

    static void InternalBuild(BuildPlayerOptions options)
    {
        BuildPipeline.BuildPlayer(options);
        Debug.Log("Complete!");
    }

    static string[] GetScenesInternal()
    {
        List<string> scenes= new List<string>();
        foreach(var scene in EditorBuildSettings.scenes)
        {
            if(scene.enabled)
            {
                scenes.Add(scene.path);
            }
        }
        return scenes.ToArray();
    }

    static void CheckDirectory(string path)
    {
        if(!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
    }
}
