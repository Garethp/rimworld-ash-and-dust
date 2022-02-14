using RimWorld;
using Verse;

namespace AshAndDust.Plants
{
    public class CompGiveThoughtToRelationsOnDestroy: ThingComp
    {
        private CompProperties_GiveThoughtToRelationsOnDestroy Props => (CompProperties_GiveThoughtToRelationsOnDestroy) this.props;

        public override void PostDestroy(DestroyMode mode, Map previousMap)
        {
            if (parent is not Plants_AncestorTree tree) return;

            var ancestor = tree.Ancestor;

            if (previousMap == null)
                return;
            
            if (!this.Props.message.NullOrEmpty())
                Messages.Message(this.Props.message, (LookTargets) new TargetInfo(this.parent.Position, previousMap), MessageTypeDefOf.NegativeEvent);

            Log.Message("Tree destroyed: " + ancestor.Name);

            foreach (var otherPawn in previousMap.mapPawns.AllPawns)
            {
                if (otherPawn.Faction != ancestor.Faction || otherPawn.Ideo == null) continue;

                var precept = otherPawn.Ideo.GetPrecept(Defs.TreeBurial_Colonists) ??
                              otherPawn.Ideo.GetPrecept(Defs.TreeBurial_WhenPossible) ??
                              otherPawn.Ideo.GetPrecept(Defs.TreeBurial_Required);

                if (precept == null) continue;
                
                var mostImportantRelation = otherPawn.GetMostImportantRelation(ancestor);
                if (mostImportantRelation != null && mostImportantRelation.familyByBloodRelation)
                {
                    otherPawn.needs?.mood?.thoughts.memories.TryGainMemory(this.Props.closeFamilyThought, ancestor, precept);
                } else if (mostImportantRelation != null && mostImportantRelation == PawnRelationDefOf.Spouse)
                {
                    otherPawn.needs?.mood?.thoughts.memories.TryGainMemory(this.Props.spouseThought, ancestor, precept);
                } else if (mostImportantRelation != null && mostImportantRelation == PawnRelationDefOf.Lover)
                {
                    otherPawn.needs?.mood?.thoughts.memories.TryGainMemory(this.Props.loverThought, ancestor, precept);
                } else if (otherPawn.relations.OpinionOf(ancestor) >= 80)
                {
                    otherPawn.needs?.mood?.thoughts.memories.TryGainMemory(this.Props.bestFriendThought, ancestor, precept);
                } else if (otherPawn.relations.OpinionOf(ancestor) >= 50)
                {
                    otherPawn.needs?.mood?.thoughts.memories.TryGainMemory(this.Props.friendThought, ancestor, precept);
                }
                else
                {
                    otherPawn.needs?.mood?.thoughts.memories.TryGainMemory(this.Props.colonistThought, ancestor, precept);
                }
            }
            
            foreach (var otherPawn in ancestor.relations.RelatedPawns)
            {
                
                // foreach (var relation in otherPawn.GetRelations(Ancestor))
                // {
                //     PawnRelationDefOf
                //     Log.Message("Relationship with: " + otherPawn.Name);
                //     Log.Message("Relationship type: " + relation.defName);
                //     Log.Message("Was blood related?: " + relation.familyByBloodRelation);
                //     Log.Message("Importance: " + relation.importance);
                //     Log.Message("Opinion: " + relation.opinionOffset);
                //     Log.Message(" ");
                // }
                // if (relation.def.familyByBloodRelation)
                // {
                    // relation.otherPawn.needs?.mood?.thoughts.memories.TryGainMemory(this.Props.closeFamilyThought, Ancestor);
                // }
                

            }

            foreach (var pawn in previousMap.mapPawns.AllPawns) 
            {
                Log.Message(pawn.Name + "'s opinion of Ancestor: " + pawn.relations.OpinionOf(ancestor));
                // Log.Message(pawn);
            }
            
            // foreach (var relation in Ancestor.relations.GetDirectRelation())
            // {
            //     if (relation.def.familyByBloodRelation)
            //     {
            //         relation.otherPawn.needs?.mood?.thoughts.memories.TryGainMemory(this.Props.closeFamilyThought, Ancestor);
            //     }
            //     
            //     Log.Message("Relationship with: " + relation.otherPawn.Name);
            //     Log.Message("Relationship type: " + relation.def.defName);
            //     Log.Message("Was blood related?: " + relation.def.familyByBloodRelation);
            //     Log.Message("Importance: " + relation.def.importance);
            //     Log.Message("Opinion: " + relation.def.opinionOffset);
            //     Log.Message(" ");
            // }
            
            // pawn.needs?.mood?.thoughts.memories.TryGainMemory(this.Props.thought);
            
            tree.Corpse.Destroy();
        }
    }
}