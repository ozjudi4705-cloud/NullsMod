using MiraAPI.GameOptions;
using MiraAPI.GameOptions.Attributes;
using MiraAPI.Utilities;
using NullsMod.Roles.Impostor;

namespace NullsMod.Options.Roles.Impostor;

public sealed class CamouflagerOptions : AbstractRoleOptionGroup<CamouflagerRole>
{
    public override string GroupName => "Camouflager";

    [ModdedNumberOption("Ability Cooldown", 5f, 120f, 2.5f, MiraNumberSuffixes.Seconds)]
    public float CamoCooldown { get; set; } = 25f;

    [ModdedNumberOption("Ability Duration", 5f, 25f, suffixType: MiraNumberSuffixes.Seconds)]
    public float CamoDuration { get; set; } = 15f;

    // [ModdedToggleOption("Passive Camo Vision")]
    // public bool CamoVision { get; set; } = true;
}