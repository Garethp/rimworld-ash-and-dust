using RimWorld;
using Verse;

namespace AshAndDust.Precepts
{
    public class TreeBurial_Required: TreeBurial_WhenPossible, INotify_NonMemberCorpseDestroyed
    {
        public void Notify_NonMemberCorpseDestroyed(Pawn pawn)
        {
            Log.Message("Non member corpse destroyed");
        }
    }
}