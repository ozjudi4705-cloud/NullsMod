using MiraAPI.Events;
using MiraAPI.Events.Vanilla.Gameplay;
using MiraAPI.Hud;
using MiraAPI.Modifiers;
using MiraAPI.Utilities;
using TownOfUs.Buttons;
using TownOfUs.Utilities;
using TownOfUs;
using UnityEngine;
using NullsMod.Assets;
using NullsMod.Modifiers.Game.Crewmate;
using NullsMod.Modifiers.Hidden;

namespace NullsMod.Events.Modifiers;

public static class ShackledEvents
{
    [RegisterEvent]
    public static void ShackledDeathEvent(AfterMurderEvent @event)
    {
        var source = @event.Source;
        var target = @event.Target;

        if (!target.HasModifier<ShackledModifier>() ||
            !source.AmOwner ||
            MeetingHud.Instance)
        {
            return;
        }

        source.RpcAddModifier<ShackledDragModifier>(target.PlayerId);

        var text = "<player> was <modifier>, forcing you to drag their body!"
            .Replace("<player>", target.Data.PlayerName)
            .Replace(
                "<modifier>",
                $"{NullsColors.Shackled.ToTextColor()}Shackled</color>");

        var notif = Helpers.CreateAndShowNotification(
            $"<b>{text}</b>",
            Color.white,
            new Vector3(0f, 1f, -20f),
            spr: NullsIcons.Shackled.LoadAsset());

        notif?.AdjustNotification();
    }
}