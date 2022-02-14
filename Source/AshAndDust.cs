using HarmonyLib;
using RimWorld;
using Verse;

namespace AshAndDust
{
    public class Mod: Verse.Mod
    {
        public Mod(ModContentPack content) : base(content)
        {
            new Harmony("Garethp.rimworld.AshAndDust.main").PatchAll();
        }
    }
}