using HarmonyLib;
using RimWorld;
using RimWorld.Planet;
using Verse;

namespace AshAndDust.HarmonyPatches
{
    [HarmonyPatch(typeof(CompMeditationFocus), "CanPawnUse")]
    public class Patch_CompMeditationFocus
    {
        public static bool Prefix(CompMeditationFocus __instance, ref bool __result, Pawn pawn)
        {
            if (__instance.parent is IManualMeditationCanFocus parent)
            {
                __result = parent.CanUse(pawn);
                return false;
            }

            return true;
        }
    }
}