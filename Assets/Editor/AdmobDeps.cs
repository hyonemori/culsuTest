// <copyright file="AnalyticsDeps.cs" company="Google Inc.">
// Copyright (C) 2016 Google Inc. All Rights Reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEditor;

/// <summary>
/// This file is used to define dependencies, and pass them along to a system
/// which can resolve dependencies.
/// </summary>
[InitializeOnLoad]
public class AdmobDeps : AssetPostprocessor
{
    /// <summary>
    /// This is the entry point for "InitializeOnLoad". It will register the
    /// dependencies with the dependency tracking system.
    /// </summary>
    static AdmobDeps()
    {
        SetupDeps();
    }

    static void SetupDeps()
    {
#if UNITY_IOS
        Type iosResolver = Google.VersionHandler.FindClass
        (
            "Google.IOSResolver",
            "Google.IOSResolver");
        if (iosResolver == null)
        {
            return;
        }
        Google.VersionHandler.InvokeStaticMethod
        (
            iosResolver,
            "AddPod",
            new object[]
            {
                "GoogleMobileAdsMediationUnity"
            },
            new Dictionary<string, object>()
            {
                {
                    "version", null
                },
                {
                    "minTargetSdk", null
                },
                {
                    "sources", null
                }
            });
#endif
    }

    // Handle delayed loading of the dependency resolvers.
    private static void OnPostprocessAllAssets
    (
        string[] importedAssets,
        string[] deletedAssets,
        string[] movedAssets,
        string[] movedFromPath)
    {
        foreach (string asset in importedAssets)
        {
            if (asset.Contains("IOSResolver") ||
                asset.Contains("JarResolver"))
            {
                SetupDeps();
                break;
            }
        }
    }
}