using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using UnityEngine;
using Verse;

namespace AshAndDust.Plants
{
    public class Plants_AncestorTree : Plant, IThingHolder, IMeditationSideEffect, IManualMeditationCanFocus
    {
        ThingOwner innerContainer;

        private bool contentsKnown;

        private Graphic styleGraphicInt;

        public Plants_AncestorTree()
        {
            this.innerContainer = (ThingOwner) new ThingOwner<Thing>(this, false);
        }

        public Corpse Corpse
        {
            get
            {
                foreach (var thing in this.innerContainer)
                {
                    if (thing is Corpse corpse)
                        return corpse;
                }

                return (Corpse) null;
            }
        }

        public Pawn Ancestor => this.Corpse.InnerPawn;

        public override void SpawnSetup(Map map, bool respawningAfterLoad)
        {
            base.SpawnSetup(map, respawningAfterLoad);
            if (Faction?.IsPlayer == true)
                contentsKnown = true;
        }

        public ThingOwner GetDirectlyHeldThings() => this.innerContainer;

        public void GetChildHolders(List<IThingHolder> outChildren) =>
            ThingOwnerUtility.AppendThingHoldersFromThings(outChildren, (IList<Thing>) this.GetDirectlyHeldThings());

        public virtual bool Accepts(Thing thing) => this.innerContainer.CanAcceptAnyOf(thing);

        public virtual bool TryAcceptThing(Thing thing, bool allowSpecialEffects = true)
        {
            if (!this.Accepts(thing) || thing is not Corpse corpse)
                return false;
            bool flag;
            if (thing.holdingOwner != null)
            {
                thing.holdingOwner.TryTransferToContainer(thing, this.innerContainer, thing.stackCount);
                flag = true;
            }
            else
                flag = this.innerContainer.TryAdd(thing);

            if (!flag)
                return false;

            if (corpse.InnerPawn.Faction?.IsPlayer == true)
                contentsKnown = true;

            return true;
        }

        public override string Label
        {
            get
            {
                if (!this.contentsKnown) return "Tree of an Unknown Hero";
                return "Tree of the beloved " + Ancestor.Name.ToStringShort;
            }
        }

        public void MeditationTick(Pawn pawn)
        {
            Ancestor.skills.skills
                .FindAll(record => record.passion == Passion.Major || record.passion == Passion.Minor)
                .ForEach(skill =>
                {
                    var skillWeighting = 0.5f;
                    if (skill.passion == Passion.Minor)
                    {
                        skillWeighting *= .5f;
                    }

                    switch (skill.Level)
                    {
                        case > 20:
                        case 20:
                            skillWeighting *= 1f;
                            break;
                        case >= 15:
                            skillWeighting *= .75f;
                            break;
                        case >= 10:
                            skillWeighting *= .5f;
                            break;
                        case >= 5:
                            skillWeighting *= .1f;
                            break;
                        default:
                            skillWeighting *= 0f;
                            break;
                    }

                    pawn.skills.Learn(skill.def, 0.018f * skillWeighting);
                });
        }

        public bool CanUse(Pawn pawn)
        {
            return (pawn.Ideo.HasPrecept(Defs.TreeBurial_Colonists)
                    || pawn.Ideo.HasPrecept(Defs.TreeBurial_WhenPossible)
                    || pawn.Ideo.HasPrecept(Defs.TreeBurial_Required))
                   && Ancestor.IsColonist && pawn.IsColonist;
        }

        public override void ExposeData()
        {
            Scribe_Deep.Look<ThingOwner>(ref this.innerContainer, "innerContainer", (object) this);
            Scribe_Values.Look<bool>(ref this.contentsKnown, "contentsKnown");

            base.ExposeData();
        }

        public override Graphic Graphic
        {
            get
            {
                ThingStyleDef styleDef = this.StyleDef;
                if (styleDef == null || styleDef.Graphic == null)
                    return this.DefaultGraphic;
                if (this.styleGraphicInt == null)
                {
                    this.styleGraphicInt = styleDef.graphicData == null
                        ? styleDef.Graphic
                        : styleDef.graphicData.GraphicColoredFor(this);

                    if (this.styleGraphicInt.MatSingle.shader != ShaderDatabase.CutoutPlant)
                    {
                        this.styleGraphicInt.MatSingle.shader = ShaderDatabase.CutoutPlant;
                        WindManager.Notify_PlantMaterialCreated(this.styleGraphicInt.MatSingle);
                    }
                }

                return this.styleGraphicInt;
            }
        }
    }
}