using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor;

public class Build
{
    //TODO: Common Config...
    private static string buildPathRoot= "d:/builds/";

#region BUILD_MENU
    [MenuItem("Build/Build Android")]
    static void BuildAndroid()
    {
        Debug.Log("Starting Android Build");

        if(!CheckAndroidPaths())
        {
            Debug.Log("ERROR: INVALID ANDROID PATHS");
            return;
        }

        BuildPlayerOptions options= new BuildPlayerOptions
        {
            scenes= GetScenesFromSettings(),
            locationPathName= GetPath("android", ".apk"),
            target= BuildTarget.Android,
            options= BuildOptions.Il2CPP
        };
        _Build(options);
    }

#if UNITY_EDITOR_OSX
    [MenuItem("Build/Build iOS")]
    public static void BuildiOS()
    {
        Debug.Log("Starting iOS Build");

        BuildPlayerOptions options= new BuildPlayerOptions
        {
            scenes= GetScenesFromSettings(),
            locationPathName= GetPath("ios", ""),
            target= BuildTarget.iOS,
            options= BuildOptions.Il2CPP
        };
        _Build(options);
    }

    [MenuItem("Build/Build Standalone OSX")]
    public static void BuildMac()
    {
        Debug.Log("Starting Standalone OSX Build");

        BuildPlayerOptions options= new BuildPlayerOptions
        {
            scenes= GetScenesFromSettings(),
            locationPathName= GetPath("osx", ""); 
            target= BuildTarget.StandaloneOSX,
            options= BuildOptions.Il2CPP
        };
        _Build(options);
    }
#endif

#if UNITY_EDITOR_WIN
    [MenuItem("Build/Build Standalone Windows 64")]
    static void BuildWindows()
    {
        Debug.Log("Starting Windows 64 Build");

        BuildPlayerOptions options= new BuildPlayerOptions
        {
            scenes= GetScenesFromSettings(),
            locationPathName= GetPath("win64",".exe"),
            target= BuildTarget.StandaloneWindows64,
            options= BuildOptions.None
        };
        _Build(options);
    }
#endif
#endregion

#region CLI_BUILD
    public static void Android()
    {
        Debug.Log("Starting Android Build");
        string path= string.Empty;
        bool _build= GetBuildPathFromArgs(out path);
        if(!_build)
        {
            Debug.Log("ERROR: Invalid Path");
            return;
        }

        BuildPlayerOptions options= new BuildPlayerOptions
        {
            scenes= GetScenesFromSettings(),
            locationPathName= path,
            target= BuildTarget.Android,
            options= BuildOptions.Il2CPP
        };
        _Build(options);
    }

#if UNITY_EDITOR_OSX
    public static void iOS()
    {
        //TBD
    }

    public static void OSX()
    {
        //TBD
    }
#endif

#if UNITY_EDITOR_WIN
    public static void Win64()
    {
        //TBD
    }
#endif
#endregion

#region COMMON_FUNCTIONS
    static void _Build(BuildPlayerOptions options)
    {
        PreBuild();
        BuildPipeline.BuildPlayer(options);
        Debug.Log("Build Complete!");
    }

    static void PreBuild()
    {
        //TDB...
    }

    static string GetPath(string buildTarget, string extension)
    {
        string path= buildPathRoot + buildTarget + "/";
        CheckDirectory(path);
        path += Application.productName + extension;
        return path;
    }

    static string[] GetScenesFromSettings()
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
        string test= EditorPrefs.GetString("AndroidSdkRoot");
        if(string.IsNullOrEmpty(test))
        {
            Debug.Log("ERROR: NO ANDROID SDK FOUND");
            return false;
        }

        test= EditorPrefs.GetString("AndroidNdkRoot");
        if(string.IsNullOrEmpty(test))
        {
            Debug.Log("ERROR: NO ANDROID NDK FOUND");
            return false;
        }

        test= EditorPrefs.GetString("JdkPath");
        if(string.IsNullOrEmpty(test))
        {
            Debug.Log("ERROR: NO JAVA SDK FOUND");
            return false;
        }
        return true;
    }

    static bool GetBuildPathFromArgs(out string path)
    {
        path= null;
        string[] commandlineArgs= Environment.GetCommandLineArgs();
        for(int i= 0; i < commandlineArgs.Length; i++)
        {
            if(commandlineArgs[i] == "-buildPath")
            {
                if(i == commandlineArgs.Length)
                {
                    Debug.Log("No next arg found");
                    return false;
                }

                path= commandlineArgs[i+1];

                if(string.IsNullOrEmpty(path))
                {
                    Debug.Log("Null or empty path.");
                    return false;
                }
                
                if(!Uri.IsWellFormedUriString(path, UriKind.Absolute))
                {
                    Debug.Log("Malformed Uri");
                    return false;
                }

                return true;
            }
        }
        return false;
    }

    static BuildOptions GetBuildOptionsFromArgs()
    {
        Debug.Log("Not ready yet...");
        return BuildOptions.None;
    }
#endregion
}
