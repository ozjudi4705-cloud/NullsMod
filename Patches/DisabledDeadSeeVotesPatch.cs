using HarmonyLib;
using TownOfUs.Patches.Options;

namespace NullsMod.Patches;

[HarmonyPatch(typeof(DeadSeeVoteColorsPatch), nameof(DeadSeeVoteColorsPatch.Prefix))]
public static class DisableDeadSeeVotesPatch
{
    [HarmonyPrefix]
    public static bool Prefix()
    {
        return false;
    }
}