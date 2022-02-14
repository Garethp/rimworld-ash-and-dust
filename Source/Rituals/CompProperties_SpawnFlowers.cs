using System.Collections.Generic;
using RimWorld;
using Verse;

namespace AshAndDust.Rituals
{

    public class CompProperties_SpawnFlowers : RitualOutcomeComp
    {
        public List<ThingDef> plantsToNotOverwrite;

        public List<ThingDef> baseDecorativePlants;

        public List<ThingDef> enemyDecorativePlants;

        public List<ThingDef> rareRewardPlants;

        public int plantSpawnRadius;

        public int massBurialRadius;

        private SpawnFlowersData data;

        public override RitualOutcomeComp_Data MakeData()
        {
            if (data == null)
            {
                data = new SpawnFlowersData(plantsToNotOverwrite, baseDecorativePlants, enemyDecorativePlants, rareRewardPlants, plantSpawnRadius, massBurialRadius);
            }

            return data;
        }

        public override bool Applies(LordJob_Ritual ritual)
        {
            return true;
        }
    }

    public class SpawnFlowersData : RitualOutcomeComp_Data
    {
        public List<ThingDef> plantsToNotOverwrite;

        public List<ThingDef> baseDecorativePlants;

        public List<ThingDef> enemyDecorativePlants;

        public List<ThingDef> rareRewardPlants;

        public int plantSpawnRadius;

        public int massBurialRadius;

        public SpawnFlowersData(List<ThingDef> plantsToNotOverwrite, List<ThingDef> baseDecorativePlants, List<ThingDef> enemyDecorativePlants, List<ThingDef> rareRewardPlants, int plantSpawnRadius, int massBurialRadius)
        {
            this.plantsToNotOverwrite = plantsToNotOverwrite;
            this.baseDecorativePlants = baseDecorativePlants;
            this.enemyDecorativePlants = enemyDecorativePlants;
            this.rareRewardPlants = rareRewardPlants;
            this.plantSpawnRadius = plantSpawnRadius;
            this.massBurialRadius = massBurialRadius;
        }
    }
}