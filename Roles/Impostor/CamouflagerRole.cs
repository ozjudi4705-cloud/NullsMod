using AmongUs.GameOptions;
using UnityEngine;
using Il2CppInterop.Runtime.Attributes;
using MiraAPI.Hud;
using MiraAPI.Utilities.Assets;
using MiraAPI.Patches.Stubs;
using MiraAPI.Roles;
using TownOfUs.Buttons.Impostor;
using TownOfUs;
using TownOfUs.Modules.Wiki;
using TownOfUs.Utilities;
using TownOfUs.Assets;
using TownOfUs.Roles;
using TownOfUs.Roles.Crewmate;
using TownOfUs.Extensions;
using NullsMod.Assets;
using NullsMod.Options.Roles.Impostor;

namespace NullsMod.Roles.Impostor;

public sealed class CamouflagerRole(IntPtr cppPtr) : ImpostorRole(cppPtr), ITownOfUsRole, IWikiDiscoverable, IDoomable
{
    // public RoleBehaviour CrewVariant => RoleManager.Instance.GetRole((RoleTypes)RoleId.Get<HunterRole>());
    public Color RoleColor => TownOfUsColors.Impostor;
    public string RoleName => "Camouflager";
    public ModdedRoleTeams Team => ModdedRoleTeams.Impostor;
    public RoleAlignment RoleAlignment => RoleAlignment.ImpostorConcealing;
    public DoomableType DoomHintType => DoomableType.Perception;

    public string RoleDescription => "Use your Camouflage ability to disguise evryone!";
    public string RoleMedDescriptionLocale => "Use your abiliy to cause Confusion and get Sneaky kills.";
    public string RoleLongDescription => "You can Camouflage to change everyone's appearance to Gray";
    public string GetAdvancedDescription()
    {
        return
            $"The Camouflager is a Impostor Concealing that can use their Camouflage ability to make everyone Camouflaged." +
            MiscUtils.AppendOptionsText(GetType());
    }

    public CustomRoleConfiguration Configuration => new(this)
    {
        IconTmp = TmpSpriteUtils.CreateSpriteAsset(NullsIcons.Camouflager.LoadAsset(), "Camouflager", 1.45f),
        OptionsScreenshot = TouBanners.ImpostorRoleBanner,
        Icon = NullsIcons.Camouflager
    };

    public override void Initialize(PlayerControl player)
    {
        RoleBehaviourStubs.Initialize(this, player);
    }

    [HideFromIl2Cpp]
    public List<CustomButtonWikiDescription> Abilities
    {
        get
        {
            return
            [
                new($"Camouflage",
                    $"Camouflage.WikiDescription",
                    TouImpAssets.CamouflageSprite),
            ];
        }
    }
}