using BepInEx;
using BepInEx.Unity.IL2CPP;
using BepInEx.Configuration;
using HarmonyLib;
using MiraAPI.PluginLoading;
using Reactor;
using Reactor.Networking;
using Reactor.Networking.Attributes;
using Reactor.Utilities;
using UnityEngine;

namespace NullsMod;

[BepInAutoPlugin("NullsMod", "Nulls Mod")]
[BepInProcess("Among Us.exe")]
[BepInDependency(ReactorPlugin.Id)]
[BepInDependency("mira.api")]
[BepInDependency("auavengers.tou.mira")]
[ReactorModFlags(ModFlags.RequireOnAllClients)]
public partial class NullsPlugin : BasePlugin, IMiraPlugin
{
    public Harmony Harmony { get; } = new(Id);

        /// <summary>
    ///     Determines if the current build is a dev build or not. This will change certain visuals as well as always grab news locally to be up to date.
    /// </summary>
    public static bool IsDevBuild => IsBetaBuild || IsWipBuild;

    /// <summary>
    ///     Determines if the current build is a beta build. Beta builds are dev builds but should have restricted features like /up command.
    /// </summary>
    public static bool IsBetaBuild => Version.Contains("beta", StringComparison.OrdinalIgnoreCase) || Version.Contains("prerelease", StringComparison.OrdinalIgnoreCase);

    /// <summary>
    ///     Determines if the current build is a beta build. Beta builds are dev builds but should have restricted features like /up command.
    /// </summary>
    public static bool IsWipBuild => Version.Contains("dev", StringComparison.OrdinalIgnoreCase) || Version.Contains("ci", StringComparison.OrdinalIgnoreCase);

    public string OptionsTitleText => "Null's Mod";


    public ConfigFile GetConfigFile()
    {
        return Config;
    }
    public override void Load()
    {
        ReactorCredits.Register("Null's Mod", Version, IsDevBuild, ReactorCredits.AlwaysShow);
        try
        {
            Harmony.PatchAll();
        }
        catch(System.Exception e)
        {
            _ = ConstantlyError(e.ToString());
        }

        Log.LogInfo($"Nulls Mod loaded successfully!");
    }

    private static async Task ConstantlyError(string e)
    {
        while(true)
        {
            await Task.Delay(100);
            Fatal(e);

            if (Time.deltaTime > 1) break;
        }
    }
}