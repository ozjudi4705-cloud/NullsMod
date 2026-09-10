using MiraAPI.GameOptions;
using MiraAPI.Utilities.Assets;
using TownOfUs.Modules.Wiki;
using TownOfUs.Modifiers;
using TownOfUs.Utilities;
using TownOfUs.Modifiers.Game;
using UnityEngine;
using NullsMod.Assets;
using NullsMod.Options.Modifiers;

namespace NullsMod.Modifiers.Game.Crewmate;

public sealed class ShackledModifier : UniversalGameModifier, IWikiDiscoverable
{
    public override ModifierUiConfiguration Configuration => new(NullsColors.Shackled, TmpSpriteUtils.CreateSpriteAsset(NullsIcons.Shackled.LoadAsset(), "Shackled", 1.45f));
    public override string IdPart => "Shackled";
    public override string ModifierName => "Shackled";
    public override string IntroInfo => "Shackle your Killer!";

    public override string GetDescription()
    {
        return "Shackle your Killer forcing them to drag your dead body";
    }

    public string GetAdvancedDescription()
    {
        return "The Shackled Modifier is a Universal Postmortem modifier that causes" +
        " your killer to drag your dead body along with them for a specified duration." +
        MiscUtils.AppendOptionsText(GetType());
    }

    public override LoadableAsset<Sprite>? ModifierIcon => NullsIcons.Shackled;

    public override ModifierFaction FactionType => ModifierFaction.UniversalPostmortem;
    public override Color FreeplayFileColor => NullsColors.Shackled;

    public List<CustomButtonWikiDescription> Abilities { get; } = [];

    public override int GetAssignmentChance()
    {
        return (int)OptionGroupSingleton<ShackledOptions>.Instance.ShackledChance;
    }

    public override int GetAmountPerGame()
    {
        return (int)OptionGroupSingleton<ShackledOptions>.Instance.ShackledAmount;
    }
}