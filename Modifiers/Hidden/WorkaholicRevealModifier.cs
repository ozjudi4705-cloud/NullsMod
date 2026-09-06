using AmongUs.GameOptions;
using TownOfUs.Modifiers;
using MiraAPI.Roles;
using NullsMod.Roles.Neutral;

namespace NullsMod.Modifiers.Hidden;

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