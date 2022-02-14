using AshAndDust.Buildings;
using RimWorld;
using Verse;

namespace AshAndDust.Rituals
{
    public class TreeGraveWithTargetFilter: RitualObligationTargetWorker_GraveWithTarget
    {
        public TreeGraveWithTargetFilter()
        {
        }

        public TreeGraveWithTargetFilter(RitualObligationTargetFilterDef def) : base(def)
        {
        }
        
        protected override RitualTargetUseReport CanUseTargetInternal(TargetInfo target, RitualObligation obligation)
        {
            return (RitualTargetUseReport) (target.HasThing && target.Thing is Building_TreeGrave thing && thing.Corpse == obligation.targetA.Thing);
        }
    }
}