using MiraAPI.GameOptions;
using MiraAPI.Modifiers;
using MiraAPI.Keybinds;
using MiraAPI.Utilities;
using MiraAPI.Utilities.Assets;
using TownOfUs;
using TownOfUs.Utilities;
using TownOfUs.Assets;
using TownOfUs.Modifiers;
using TownOfUs.Roles.Impostor;
using TownOfUs.Buttons;
using TownOfUs.Options.Maps;
using UnityEngine;
using UnityEngine.UI;
using NullsMod.Options.Roles.Impostor;
using NullsMod.Assets;
using NullsMod.Modifiers.Hidden;
using NullsMod.Roles.Impostor;

namespace NullsMod.Buttons.Roles.Impostor;

public sealed class CamouflagerAbilityButton : TownOfUsRoleButton<CamouflagerRole>, IAftermathableButton, ILegacyCapable
{
    public override string Name => "Camouflage";
    public override Color TextOutlineColor => TownOfUsColors.Impostor;
    public override BaseKeybind Keybind => Keybinds.SecondaryAction;
    public override LoadableAsset<Sprite> Sprite => LegacyAssets.IsLegacy ? NullsIcons.CamouflagerButton : NullsIcons.CamouflagerButton;
    public override float Cooldown => Math.Clamp(OptionGroupSingleton<CamouflagerOptions>.Instance.CamoCooldown + MapCooldown, 5f, 120f);
    public override float EffectDuration => OptionGroupSingleton<CamouflagerOptions>.Instance.CamoDuration;

    public override void ClickHandler()
    {
        if (!CanUse())
        {
            return;
        }

        // if (EffectActive)
        // {
        //     Timer = Cooldown;
        //     EffectActive = false;
        //     Button?.SetDisabled();
        //     OnEffectEnd();
        //     return;
        // }

        OnClick();
        // Button?.SetDisabled();

        if (HasEffect)
        {
            EffectActive = true;
            Timer = EffectDuration;
        }
        else
        {
            Timer = Cooldown;
        }
    }
    
    public override bool CanUse()
    {
        if (HudManager.Instance.Chat.IsOpenOrOpening || MeetingHud.Instance)
        {
            return false;
        }

        if (PlayerControl.LocalPlayer.GetModifiers<DisabledModifier>().Any(x => !x.CanUseAbilities))
        {
            return false;
        }

        bool commsActive;

        switch ((ExpandedMapNames)GameOptionsManager.Instance.currentGameOptions.MapId)
        {
            case ExpandedMapNames.MiraHq:
            case ExpandedMapNames.Fungle:
                var hqComms = ShipStatus.Instance.Systems[SystemTypes.Comms]
                    .Cast<HqHudSystemType>();

                commsActive = hqComms.IsActive;
                break;

            default:
                var hudComms = ShipStatus.Instance.Systems[SystemTypes.Comms]
                    .Cast<HudOverrideSystemType>();

                commsActive = hudComms.IsActive;
                break;
        }
        if(commsActive && TownOfUsMapOptions.IsCamoCommsOn())
        {
            return false;
        }

        return Timer <= 0 && !EffectActive;
    }

    public void AftermathHandler()
    {
        ClickHandler();
    }

    protected override void OnClick()
    {
        // PlayerControl.LocalPlayer.RpcAddModifier<CamouflagerCamoModifier>();

        // foreach (var player in PlayerControl.AllPlayerControls)
        // {
        //     if (player.Data.IsDead || player.Data.Disconnected || player.AmOwner)
        //     {
        //         continue;
        //     }

        //     player.RpcAddModifier<CamouflagerCamoModifier>();
        // }
        foreach (var player in Helpers.GetAlivePlayers())
        {
            player.RpcAddModifier<CamouflagerCamoModifier>();
        }
    }

    public override void OnEffectEnd()
    {
        var camoMod = PlayerControl.LocalPlayer.GetModifier<CamouflagerCamoModifier>();

        if (camoMod != null)
        {
            PlayerControl.LocalPlayer.RpcRemoveModifier(camoMod.UniqueId);
        }

        var camoMods = ModifierUtils.GetActiveModifiers<CamouflagerCamoModifier>().ToList();
        foreach (var camo in camoMods)
        {
            camo.Player.RpcRemoveModifier(camo.UniqueId);
        }
    }
}