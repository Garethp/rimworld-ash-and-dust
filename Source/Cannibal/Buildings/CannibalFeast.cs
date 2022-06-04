using RimWorld;
using Verse;

namespace AshAndDust.Cannibal.Buildings
{
    public class CannibalFeast: Building_Grave
    {
        public override void Draw()
        {
            base.Draw();

            if (!HasCorpse) return;
            
            var drawLoc = DrawPos;
            var rotation2 = Rotation;
            if (rotation2 == Rot4.North || rotation2 == Rot4.South)
                drawLoc.z += 0.4f;
            else
                drawLoc.z += 0.2f;

            drawLoc.y += 1;
            
            Corpse.InnerPawn.Drawer.renderer.RenderPawnAt(drawLoc, Rotation, true);
        }
    }
}