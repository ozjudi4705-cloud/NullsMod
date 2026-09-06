using MiraAPI.Utilities.Assets;
using UnityEngine;

namespace NullsMod.Assets;

public static class NullsRoleIcons
{
    // THIS FILE SHOULD ONLY HOLD ROLE ICONS

    private const string ShortPath = "NullsMod.Resources";

    // Neutrals
    public static LoadableAsset<Sprite> Micromanager { get; } = new LoadableResourceAsset($"{ShortPath}.RoleIcons.Micromanager.png", 200);
    public static LoadableAsset<Sprite> Workaholic { get; } = new LoadableResourceAsset($"{ShortPath}.RoleIcons.Workaholic.png", 200);

}