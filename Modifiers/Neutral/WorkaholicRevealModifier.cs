using AmongUs.GameOptions;
using MiraAPI.GameOptions;
using TownOfUs.Modifiers;
using MiraAPI.Modifiers;
using MiraAPI.Patches.Stubs;
using MiraAPI.Roles;
using TownOfUs.Roles;
using TownOfUs.Roles.Neutral;
using TownOfUs.Modules;
using MiraAPI.Utilities;

namespace TownOfUs.Modifiers.Neutral;

public sealed class WorkaholicRevealModifier
    : BaseRevealModifier
{
    public override string ModifierName => "Workaholic Reveal";

    public override ChangeRoleResult ChangeRoleResult { get; set; } =
        ChangeRoleResult.UpdateInfo;

    public override bool RevealRole { get; set; } = true;

    public override RoleBehaviour? ShownRole
    {
        get
        {
            return RoleManager.Instance.GetRole(
                (RoleTypes)RoleId.Get<WorkaholicRole>());
        }
    }

    public override void OnDeath(DeathReason reason)
    {
        base.OnDeath(reason);
    }
}