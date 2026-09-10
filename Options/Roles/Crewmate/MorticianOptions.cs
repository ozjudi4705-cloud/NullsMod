using MiraAPI.GameOptions;
using MiraAPI.GameOptions.Attributes;
using NullsMod.Roles.Crewmate;

namespace NullsMod.Options.Roles.Crewmate;

public sealed class MorticianOptions : AbstractRoleOptionGroup<MorticianRole>
{
    public override string GroupName => "Mortician";

    [ModdedNumberOption("Initial Ability Uses", 0f, 8f)]
    public float InitialAbilityUses { get; set; } = 1f;

    [ModdedNumberOption("Tasks per Ability Use", 1f, 5f)]
    public float TasksPerAbilityUse { get; set; } = 1f;

    [ModdedToggleOption("Get Autopsy Report on Report")]
    public bool GetAutopsyChat { get; set;} = false;

    [ModdedToggleOption("Show Modifiers on Autopsy")]
    public bool ShowModifiers { get; set;} = false;
}