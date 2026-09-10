using MiraAPI.Utilities.Assets;
using UnityEngine;

namespace NullsMod.Assets;

public static class NullsIcons
{
    // THIS FILE SHOULD ONLY HOLD ROLE ICONS

    private const string ShortPath = "NullsMod.Resources";

    public static LoadableAsset<Sprite> Micromanager { get; } = new LoadableResourceAsset($"{ShortPath}.RoleIcons.Micromanager.png", 200);
    public static LoadableAsset<Sprite> Workaholic { get; } = new LoadableResourceAsset($"{ShortPath}.RoleIcons.Workaholic.png", 200);
    public static LoadableAsset<Sprite> Shackled { get; } = new LoadableResourceAsset($"{ShortPath}.RoleIcons.Shackled.png", 200);
    public static LoadableAsset<Sprite> Mortician { get; } = new LoadableResourceAsset($"{ShortPath}.RoleIcons.Mortician.png", 200);
    public static LoadableAsset<Sprite> MorticianAbility { get; } = new LoadableResourceAsset($"{ShortPath}.RoleIcons.MorticianAbility.png", 200);

}