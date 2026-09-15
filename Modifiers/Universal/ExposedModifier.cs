using MiraAPI.GameOptions;
using MiraAPI.Utilities.Assets;
using TownOfUs.Modules.Wiki;
using TownOfUs.Modifiers;
using TownOfUs.Utilities;
using TownOfUs.Modifiers.Game;
using UnityEngine;
using NullsMod.Assets;
using NullsMod.Options.Modifiers;

namespace NullsMod.Modifiers.Universal;

public sealed class ExposedModifier : UniversalGameModifier, IWikiDiscoverable
{
    public override ModifierUiConfiguration Configuration => new(NullsColors.Exposed, TmpSpriteUtils.CreateSpriteAsset(NullsIcons.Exposed.LoadAsset(), "Exposed", 1.45f));
    public override string IdPart => "Exposed";
    public override string ModifierName => "Exposed";
    public override string IntroInfo => "Your vote is Exposed!";
    // public override bool HideOnUi => false;

    public override string GetDescription()
    {
        return "Your vote is visible to everyone in meetings";
    }

    public string GetAdvancedDescription()
    {
        return "The Exposed Modifier is a Universal Visibility modifier that causes " +
        "your vote to be exposed during meetings through Anonymous Votes." +
        MiscUtils.AppendOptionsText(GetType());
    }
    public string RoleMedDescriptionLocale => "Your votes are visible to everyone!";

    public override LoadableAsset<Sprite>? ModifierIcon => NullsIcons.Exposed;

    public override ModifierFaction FactionType => ModifierFaction.UniversalVisibility;
    public override Color FreeplayFileColor => NullsColors.Exposed;

    public List<CustomButtonWikiDescription> Abilities { get; } = [];

    public override int GetAssignmentChance()
    {
        return (int)OptionGroupSingleton<ExposedOptions>.Instance.ExposedChance;
    }

    public override int GetAmountPerGame()
    {
        return (int)OptionGroupSingleton<ExposedOptions>.Instance.ExposedAmount;
    }
}