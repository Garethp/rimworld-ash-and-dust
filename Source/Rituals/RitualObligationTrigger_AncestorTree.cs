using RimWorld;
using Verse;

namespace AshAndDust.Rituals
{
    public class RitualObligationTrigger_AncestorTree : RitualObligationTrigger
    {
        public override void Notify_MemberDied(Pawn pawn)
        {
            if (!pawn.Faction.IsPlayer && !pawn.Corpse.Map.ParentFaction.IsPlayer) return;
            
            this.ritual.AddObligation(new RitualObligation(this.ritual, (TargetInfo) (Thing) pawn.Corpse)
            {
                showAlert = !pawn.IsSlave,
                causeDelayThought = !pawn.IsSlave
            });
        }
    }
}