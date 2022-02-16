using RimWorld;
using Verse;

namespace AshAndDust.Buildings
{
    public class ShuttleCasket: Building_Grave
    {
        private void Launch()
        {
            var newThing = SkyfallerMaker.MakeSkyfaller(DefDatabase<ThingDef>.GetNamed("ShuttleCasketSkyfaller"));
            GenSpawn.Spawn(newThing, Position, Map);
            
            Destroy();
        }

        public void Yeet()
        {
            Launch();
        }
    }
}