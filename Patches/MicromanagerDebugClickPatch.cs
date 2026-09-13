// using HarmonyLib;
// using MiraAPI.Utilities;
// using TownOfUs;
// using TownOfUs.Roles;
// using TownOfUs.Events.TouEvents;
// using UnityEngine;
// using TownOfUs.Utilities;
// using NullsMod.Roles.Crewmate;
// using NullsMod.Assets;

// namespace NullsMod.Patches;

// [HarmonyPatch]
// public static class MicromanagerDebugClickPatch
// {
//     [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.OnClick))]
//     [HarmonyPostfix]
//     public static void DebugClick(PlayerControl __instance)
//     {
//         if (MeetingHud.Instance)
//         {
//             return;
//         }

//         if (!PlayerControl.LocalPlayer || PlayerControl.LocalPlayer.Data == null)
//         {
//             return;
//         }

//         if (PlayerControl.LocalPlayer.Data.IsDead)
//         {
//             return;
//         }

//         if (!__instance || __instance.Data == null)
//         {
//             return;
//         }

//         if (__instance.Data.Role is not MicromanagerRole micromanager)
//         {
//             return;
//         }

//         var nearGhost = !PhysicsHelpers.AnythingBetween(
//             PlayerControl.LocalPlayer.GetTruePosition(),
//             __instance.GetTruePosition(),
//             Constants.ShipAndObjectsMask,
//             false);

//         var debugClick = Helpers.CreateAndShowNotification($"MICRO CLICK DEBUG: Role={__instance.Data.Role.GetType().Name}, IsGhost={__instance.Data.Role is IGhostRole}, CanBeClicked={micromanager.CanBeClicked}, CanCatch={micromanager.CanCatch()}, GhostActive={micromanager.GhostActive}, Caught={micromanager.Caught}, Setup={micromanager.Setup}, TaskStage={micromanager.TaskStage}, NearGhost={nearGhost},s ColliderEnabled={__instance.Collider != null && __instance.Collider.enabled}, /. Layer={__instance.gameObject.layer}", Color.white, new Vector3(0f, 1f, -20f), spr: NullsIcons.Micromanager.LoadAsset());
//         debugClick.AdjustNotification();
//     }
// }