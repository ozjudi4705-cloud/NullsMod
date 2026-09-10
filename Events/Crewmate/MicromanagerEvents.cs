using UnityEngine;
using MiraAPI.Events;
using MiraAPI.Events.Vanilla.Player;
using MiraAPI.GameOptions;
using MiraAPI.Utilities;
using TownOfUs.Utilities;
using Reactor.Networking.Attributes;
using Reactor.Networking.Rpc;
using Reactor.Utilities;
using Reactor;
using NullsMod.Assets;
using NullsMod.Roles.Crewmate;
using NullsMod.Options.Roles.Crewmate;

namespace NullsMod.Events.Crewmate;
public static class MicromanagerEvents
{
    [RegisterEvent]
    public static void CompleteTaskEvent(CompleteTaskEvent @event)
    {
        if (@event.Player.Data.Role is not MicromanagerRole micromanagerRole)
        {
            return;
        }

        if (!@event.Player.AmOwner || micromanagerRole.Caught)
        {
            return;
        }

        micromanagerRole.CheckTaskRequirements();
        ++micromanagerRole.managedTaskProgression;

        if (micromanagerRole.managedTaskProgression >=
            OptionGroupSingleton<MicromanagerOptions>.Instance.NumTasksPerManagedTask)
        {
            micromanagerRole.managedTaskProgression = 0f;

            micromanagerRole.CompleteRandomCrewTask();
        }
    }

    [MethodRpc(9101)]
    private static void RpcMicromanagerNotifyRequest(PlayerControl sender,string message)
    {
        if (!AmongUsClient.Instance.AmHost || sender == null || string.IsNullOrEmpty(message))
        {
            return;
        }

        var rpc = PluginSingleton<ReactorPlugin>.Instance.CustomRpcManager.List.OfType<MethodRpc>()
        .FirstOrDefault(x => x.Id == 9102);

        if (rpc == null)
        {
            return;
        }

        rpc.UnsafeSend(sender, new object[] { message },
            targetClientId: sender.OwnerId);
    }

    [MethodRpc(9102)]
    private static void RpcMicromanagerNotifyTarget(
        PlayerControl sender,
        string message)
    {
        if (sender == null || sender.OwnerId != AmongUsClient.Instance.ClientId ||
            string.IsNullOrEmpty(message))
        {
            return;
        }

        var notif = Helpers.CreateAndShowNotification(message, Color.white, new Vector3(0f, 1f, -20f),
            spr: NullsIcons.Micromanager.LoadAsset());

        notif?.AdjustNotification();
    }

    public static void SendMicromanagerNotif(PlayerControl target,string message)
    {
        RpcMicromanagerNotifyRequest(target, message);
    }

    public static MethodRpc? GetMicromanagerNotifRpc()
    {
        return PluginSingleton<ReactorPlugin>.Instance.CustomRpcManager.List.OfType<MethodRpc>().FirstOrDefault(x => x.Id == 9101);
    }    
}