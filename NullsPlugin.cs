using BepInEx;
using BepInEx.Unity.IL2CPP;
using BepInEx.Configuration;
using HarmonyLib;
using MiraAPI.PluginLoading;
using Reactor;
using Reactor.Networking;
using Reactor.Networking.Attributes;

namespace NullsMod;

[BepInAutoPlugin("com.oziahscourtney.nullsmod", "Nulls Mod")]
[BepInProcess("Among Us.exe")]
[BepInDependency(ReactorPlugin.Id)]
[BepInDependency("mira.api")]
[BepInDependency("auavengers.tou.mira")]
[ReactorModFlags(ModFlags.RequireOnAllClients)]
public partial class NullsPlugin : BasePlugin, IMiraPlugin
{
    public Harmony Harmony { get; } = new(Id);

    public ConfigFile GetConfigFile()
    {
        return Config;
    }

    public string OptionsTitleText => "Nulls Mod";

    public override void Load()
    {
        Harmony.PatchAll();

        Log.LogInfo($"Nulls Mod loaded successfully!");
    }
}