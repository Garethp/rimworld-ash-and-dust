using RimWorld;
using Verse;

namespace AshAndDust.Rituals
{
    public class RitualTargetFilter_WateryGraveWithCorpse: RitualObligationTargetWorker_GraveWithTarget
    {
        public RitualTargetFilter_WateryGraveWithCorpse()
        {
        }

        public RitualTargetFilter_WateryGraveWithCorpse(RitualObligationTargetFilterDef def) : base(def)
        {
        }

        protected override RitualTargetUseReport CanUseTargetInternal(TargetInfo target, RitualObligation obligation)
        {
            if (!target.HasThing) return false;
            if (target.Thing is not Building_Sarcophagus grave) return false;
            if (grave.Corpse != obligation.targetA.Thing) return false;

            return target.Cell.GetTerrain(target.Map).IsWater;
        }
    }
}