using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using RimWorld;
using Verse;

namespace AshAndDust.HarmonyPatches
{
    [HarmonyPatch(typeof(IdeoManager), "Notify_PawnKilled")]
    public class Patch_IdeoManager_NotifyPawnKilled
    {
        public static void Postfix(Pawn pawn)
        {
            if (pawn?.RaceProps?.Humanlike != true) return;
            if (pawn.Faction == Faction.OfPlayer) return;
            if (pawn.Corpse?.Map?.ParentFaction?.IsPlayer is null or false) return;
            
            var ideos = Faction.OfPlayer.ideos.AllIdeos.ToList();
            foreach (var ideo in ideos)
            {
                var preceptFound = false;
                foreach (var precept in ideo.PreceptsListForReading)
                {
                    if (precept.def == Defs.TreeBurial_Required || precept.def == Defs.TreeBurial_WhenPossible)
                    {
                        preceptFound = true;
                        break;
                    }
                }

                if (preceptFound)
                {
                    ideo.Notify_MemberDied(pawn);
                }
            }
        }
    }

    [HarmonyPatch(typeof(Corpse), "Destroy")]
    public class Patch_Corpse_PostCorpseDestroy
    {
        public static bool Prefix(Corpse __instance)
        {
            if (__instance?.InnerPawn == null || __instance?.Map == null) return true;
            if (!__instance.InnerPawn.RaceProps.Humanlike) return true;

            if (__instance.Bugged) return true;
            var deadPawn = __instance.InnerPawn;

            if (deadPawn == null) return true;

            var mapPawns = __instance.Map.mapPawns;
            
            mapPawns.AllPawns.ForEach(mapPawn =>
            {
                if (mapPawn.Ideo == null) return;

                var precept = mapPawn.Ideo.GetPrecept(Defs.TreeBurial_Required);
                if (mapPawn.Faction != deadPawn.Faction && precept != null)
                {
                    mapPawn.needs?.mood?.thoughts.memories.TryGainMemory(Defs.TreeBurial_EnemyCorpseDestroyed, deadPawn,
                        precept);
                }

                precept ??= mapPawn.Ideo.GetPrecept(Defs.TreeBurial_WhenPossible) ??
                            mapPawn.Ideo.GetPrecept(Defs.TreeBurial_Colonists);
                if (mapPawn.Faction == deadPawn.Faction && precept != null)
                {
                    mapPawn.needs?.mood?.thoughts.memories.TryGainMemory(Defs.TreeBurial_ColonistCorpseDestroyed,
                        deadPawn, precept);
                }
            });

            return true;
        }
    }
}