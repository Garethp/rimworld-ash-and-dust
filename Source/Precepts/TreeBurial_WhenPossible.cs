using RimWorld;
using Verse;

namespace AshAndDust.Precepts
{
    public class TreeBurial_WhenPossible: TreeBurial, INotify_NonMemberDied
    {
        public void Notify_NonMemberDied(Pawn pawn)
        {
            this.ideo.Notify_MemberDied(pawn);
        }
    }
}