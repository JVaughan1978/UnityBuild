﻿using SimpleJSON;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor;

public class Build
{
    //default build path can be overridden by creating UnityProjRoot/Config/config.json
#if UNITY_EDITOR_WIN
    private static string buildPathRoot= "c:/builds/";
#elif UNITY_EDITOR_OSX
    private static string buildPathRoot= "/Users/Shared/Builds/";
#endif

#region BUILD_MENU
    [MenuItem("Build/Build Android")]
    public static void BuildAndroid()
    {
        Debug.Log("Starting Android Build");

        if(InBatchMode())
        {
            Debug.Log("ERROR: EDITOR ONLY");
            return;
        }

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

    [MenuItem("Build/Build Linux")]
    static void BuildLinux()
    {
        Debug.Log("Starting Linux Build");

        if(InBatchMode())
        {
            Debug.Log("ERROR: EDITOR ONLY");
            return;
        }

        BuildPlayerOptions options= new BuildPlayerOptions
        {
            scenes= GetScenesFromSettings(),
            locationPathName= GetPath("linux", ".x86"),
            target= BuildTarget.StandaloneLinuxUniversal,
            options= BuildOptions.Il2CPP
        };
        _Build(options);
    }

    [MenuItem("Build/Build Linux Server")]
    static void BuildLinuxServer()
    {
        Debug.Log("Starting Linux Server Build");

        if(InBatchMode())
        {
            Debug.Log("ERROR: EDITOR ONLY");
            return;
        }

        BuildOptions headlessOption= BuildOptions.None;
        headlessOption |= BuildOptions.Il2CPP;
        headlessOption |= BuildOptions.EnableHeadlessMode;

        BuildPlayerOptions options= new BuildPlayerOptions
        {
            scenes= GetScenesFromSettings(),
            locationPathName= GetPath("linuxServer", ".x86_64"),
            target= BuildTarget.StandaloneLinux64,
            options= headlessOption 
        };
        _Build(options);
    }


#if UNITY_EDITOR_OSX
    [MenuItem("Build/Build iOS")]
    public static void BuildiOS()
    {
        Debug.Log("Starting iOS Build");

        if(InBatchMode())
        {
            Debug.Log("ERROR: EDITOR ONLY");
            return;
        }

        BuildPlayerOptions options= new BuildPlayerOptions
        {
            scenes= GetScenesFromSettings(),
            locationPathName= GetPath("ios", ""),
            target= BuildTarget.iOS,
            options= BuildOptions.Il2CPP
        };
        _Build(options);
    }

    [MenuItem("Build/Build  OSX")]
    public static void BuildMac()
    {
        Debug.Log("Starting OSX Build");

        if(InBatchMode())
        {
            Debug.Log("ERROR: EDITOR ONLY");
            return;
        }

        BuildPlayerOptions options= new BuildPlayerOptions
        {
            scenes= GetScenesFromSettings(),
            locationPathName= GetPath("osx", ""), 
            target= BuildTarget.StandaloneOSX,
            options= BuildOptions.Il2CPP
        };
        _Build(options);
    }
#endif

#if UNITY_EDITOR_WIN
    [MenuItem("Build/Build Windows 64")]
    static void BuildWindows()
    {
        Debug.Log("Starting Windows 64 Build");

        if(InBatchMode())
        {
            Debug.Log("ERROR: EDITOR ONLY");
            return;
        }

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

        if(!InBatchMode())
        {
            Debug.Log("ERROR: CLI USE ONLY");
            return;
        }

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

    public static void Linux()
    {
        Debug.Log("Starting Linux Build");

        if(!InBatchMode())
        {
            Debug.Log("ERROR: CLI USE ONLY");
            return;
        }

        string path = string.Empty;
        bool _build = GetBuildPathFromArgs(out path);
        if(!_build)
        {
            Debug.Log("ERROR: Invalid Path");
            return;
        }

        BuildPlayerOptions options= new BuildPlayerOptions
        {
            scenes= GetScenesFromSettings(),
            locationPathName= path,
            target= BuildTarget.StandaloneLinuxUniversal,
            options= BuildOptions.Il2CPP
        };
        _Build(options);
    }

    public static void LinuxServer()
    {
        Debug.Log("Starting Linux Build");

        if(!InBatchMode())
        {
            Debug.Log("ERROR: CLI USE ONLY");
            return;
        }

        string path = string.Empty;
        bool _build = GetBuildPathFromArgs(out path);
        if(!_build)
        {
            Debug.Log("ERROR: Invalid Path");
            return;
        }

        BuildOptions headlessOption= BuildOptions.None;
        headlessOption |= BuildOptions.Il2CPP;
        headlessOption |= BuildOptions.EnableHeadlessMode;

        BuildPlayerOptions options = new BuildPlayerOptions
        {
            scenes= GetScenesFromSettings(),
            locationPathName= path,
            target= BuildTarget.StandaloneLinuxUniversal,
            options= headlessOption,
        };
        _Build(options);
    }

#if UNITY_EDITOR_OSX
    public static void iOS()
    {
        Debug.Log("Starting iOS Build");

        if(!InBatchMode())
        {
            Debug.Log("ERROR: CLI USE ONLY");
            return;
        }

        string path = string.Empty;
        bool _build = GetBuildPathFromArgs(out path);
        if(!_build)
        {
            Debug.Log("ERROR: Invalid Path");
            return;
        }

        BuildPlayerOptions options = new BuildPlayerOptions
        {
            scenes= GetScenesFromSettings(),
            locationPathName= path,
            target= BuildTarget.iOS,
            options= BuildOptions.None
        };
        _Build(options);
    }

    public static void OSX()
    {
        Debug.Log("Starting OSX Build");

        if(!InBatchMode())
        {
            Debug.Log("ERROR: CLI USE ONLY");
            return;
        }

        string path = string.Empty;
        bool _build = GetBuildPathFromArgs(out path);
        if(!_build)
        {
            Debug.Log("ERROR: Invalid Path");
            return;
        }

        BuildPlayerOptions options = new BuildPlayerOptions
        {
            scenes= GetScenesFromSettings(),
            locationPathName= path,
            target= BuildTarget.StandaloneOSX,
            options= BuildOptions.None
        };
        _Build(options);
    }
#endif

#if UNITY_EDITOR_WIN
    public static void Win64()
    {
        Debug.Log("Starting Windows 64 Build");

        if(!InBatchMode())
        {
            Debug.Log("ERROR: CLI USE ONLY");
            return;
        }

        string path = string.Empty;
        bool _build = GetBuildPathFromArgs(out path);
        if(!_build)
        {
            Debug.Log("ERROR: Invalid Path");
            return;
        }

        BuildPlayerOptions options = new BuildPlayerOptions
        {
            scenes= GetScenesFromSettings(),
            locationPathName= path,
            target= BuildTarget.StandaloneWindows64,
            options= BuildOptions.None
        };
        _Build(options);
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
        //TODO: Depends on a few internal conditions
    }

    static string GetPath(string buildTarget, string extension)
    {
        string buildRoot= buildPathRoot;

        string path= string.Empty;
        DirectoryInfo rootPath= Directory.GetParent(Application.dataPath);        
        string configPath= rootPath.FullName + "/Config/config.json";
        if(File.Exists(configPath))
        {
            string jsonString= File.ReadAllText(configPath);
            var n= JSON.Parse(jsonString);
#if UNITY_EDITOR_WIN
            buildRoot= n["buildRoot"].Value;
            Debug.Log("get build root : " + n["buildRoot"]);
#elif UNITY_EDITOR_OSX
            buildRoot= n["osxBuildRoot"].Value;
            Debug.Log("get build root : " + n["osxBuildRoot"]);
#endif
        }
        path= buildRoot + buildTarget + "/";

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
        { //Create directory
            Directory.CreateDirectory(path);
        }
        else
        { //Clear directory
            DirectoryInfo di = new DirectoryInfo(path);
            foreach(FileInfo file in di.GetFiles())
            {
                if(!IsFileLocked(file))
                    file.Delete();
            }
            foreach(DirectoryInfo dir in di.GetDirectories())
            {
                dir.Delete(true);
            }
        }
    }

    static bool IsFileLocked(FileInfo file)
    {
        FileStream stream= null;

        try
        {
            stream= file.Open(FileMode.Open, FileAccess.Read, FileShare.None);
        }
        catch(IOException)
        {
            return true;
        }
        finally
        {
            if(stream != null)
                stream.Close();
        }
        return false;
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
        string commands= string.Empty;
        foreach(var command in commandlineArgs)
        {
            commands += command + Environment.NewLine;
        }
        Debug.Log(commands);

        for(int i= 0; i < commandlineArgs.Length; i++)
        {
            if(commandlineArgs[i].ToLower() == "-buildpath")
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

                CheckDirectory(Path.GetDirectoryName(path));
                return true;
            }
        }
        return false;
    }

    static bool InBatchMode()
    {
        string[] commandLineArgs = Environment.GetCommandLineArgs();
        foreach(string command in commandLineArgs)
        {
            if(command.ToLower().Contains("batchmode"))
            {
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
