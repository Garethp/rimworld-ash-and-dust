using AshAndDust.Plants;
using HarmonyLib;
using RimWorld;
using Verse;

namespace AshAndDust.HarmonyPatches
{
    [HarmonyPatch(typeof(JobDriver_Meditate), "MeditationTick")]
    public class PatchJobDriver_Meditate
    {
        public static void Prefix(JobDriver_Meditate __instance)
        {
            if (__instance.Focus != null && __instance.Focus.Thing is IMeditationSideEffect focusThing)
            {
                focusThing.MeditationTick(__instance.pawn);   
            }
        }
    }
}