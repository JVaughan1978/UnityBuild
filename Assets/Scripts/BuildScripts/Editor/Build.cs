using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor;

public class Build
{
    private const string buildPathRoot = "d:/builds/";

    [MenuItem("Build/Build Android")]
    static void BuildAndroid()
    {
        Debug.Log("Starting Android Build");

        if(!CheckAndroidPaths())
        {
            Debug.Log("ERROR");
            return;
        }

        string path = buildPathRoot + "android/";
        CheckDirectory(path);
        path += Application.productName + ".apk";

        BuildPlayerOptions options = new BuildPlayerOptions
        {
            scenes= GetScenesInternal(),
            locationPathName= path,
            target= BuildTarget.Android,
            options= BuildOptions.Il2CPP
        };
        InternalBuild(options);
    }

#if UNITY_STANDALONE_OSX
    [MenuItem("Build/Build iOS")]
    public static void BuildiOS()
    {
        Debug.Log("Starting iOS Build");

        string path= buildPathRoot + "ios/";
        CheckDirectory(path);
        path += Application.productName;

        BuildPlayerOptions options = new BuildPlayerOptions
        {
            scenes= GetScenesInternal(),
            locationPathName= path,
            target= BuildTarget.iOS,
            options= BuildOptions.Il2CPP
        };
        InternalBuild(options);
    }

    [MenuItem("Build/Build Standalone OSX")]
    public static void BuildMac()
    {
        Debug.Log("Starting Standalone OSX Build");

        string path= buildPathRoot + "osx/";
        CheckDirectory(path);
        path += Application.productName + ".app";

        BuildPlayerOptions options = new BuildPlayerOptions
        {
            scenes= GetScenesInternal(),
            locationPathName= path,
            target= BuildTarget.StandaloneOSX,
            options= BuildOptions.Il2CPP
        };
        InternalBuild(options);
    }
#endif

#if UNITY_STANDALONE_WIN
    [MenuItem("Build/Build Standalone Windows 64")]
    static void BuildWindows()
    {
        Debug.Log("Starting Windows 64 Build");

        string path = buildPathRoot + "win64/";
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
#endif

    public static void Android()
    {
        //TBD
    }

    public static void iOS()
    {
        //TBD
    }

    public static void Win64()
    {
    }

    public static void OSX()
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

    static bool CheckAndroidPaths()
    {
        string test = EditorPrefs.GetString("AndroidSdkRoot");
        if(string.IsNullOrEmpty(test))
        {
            Debug.Log("ERROR: NO ANDROID SDK FOUND");
            return false;
        }

        test = EditorPrefs.GetString("AndroidNdkRoot");
        if(string.IsNullOrEmpty(test))
        {
            Debug.Log("ERROR: NO ANDROID NDK FOUND");
            return false;
        }
        return true;
    }
}
