using UnityEngine;
using System.Collections;
using MiraAPI.Modifiers;
using TownOfUs.Utilities;
using Reactor.Utilities;


namespace NullsMod.Modifiers.Hidden;

public sealed class MicromanagerManageTaskModifier(uint task) : BaseModifier
{
    public override string ModifierName => "Manage Task Modifier";
    public override bool HideOnUi => true;
    public override void OnActivate()
    {
        base.OnActivate();
        Coroutines.Start(ManageRandomTask());
    }

    private IEnumerator ManageRandomTask()
    {
        if (Player.AmOwner)
        {
            Player.RpcCompleteTask(task);
            yield return new WaitForSeconds(0.5f);
            Player.RpcRemoveModifier<MicromanagerManageTaskModifier>();
        }
    }
}