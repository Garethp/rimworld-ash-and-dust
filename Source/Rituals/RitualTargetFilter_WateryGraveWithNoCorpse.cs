using RimWorld;
using Verse;

namespace AshAndDust.Rituals
{
    public class RitualTargetFilter_WateryGraveWithNoCorpse: RitualObligationTargetWorker_AnyEmptyGrave
    {
        public RitualTargetFilter_WateryGraveWithNoCorpse()
        {
        }

        public RitualTargetFilter_WateryGraveWithNoCorpse(RitualObligationTargetFilterDef def) : base(def)
        {
        }

        protected override RitualTargetUseReport CanUseTargetInternal(TargetInfo target, RitualObligation obligation)
        {
            if (!target.HasThing) return false;
            if (target.Thing is not Building_Sarcophagus grave) return false;
            if (grave.Corpse != null) return false;

            return target.Cell.GetTerrain(target.Map).IsWater;
        }
    }
}