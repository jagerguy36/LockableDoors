using LockableDoors.DefOf;
using LockableDoors.Enums;
using LockableDoors.Extensions;
using LockableDoors.Patches;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;
using Verse.AI;
using Verse.Sound;

namespace LockableDoors
{
    public class JobDriver_ToggleLock : JobDriver
    {
        protected override IEnumerable<Toil> MakeNewToils()
        {
            this.FailOnDespawnedOrNull(TargetIndex.A);
            this.FailOn(() => base.Map.designationManager.DesignationOn(base.TargetThingA, AddedDefOf.Locks_DesignatorFlick) == null);
            yield return Toils_Reserve.Reserve(TargetIndex.A, 1, -1, null);
            yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch);
            yield return Toils_General.Wait(15).FailOnCannotTouch(TargetIndex.A, PathEndMode.Touch);
            Toil toil = ToilMaker.MakeToil("MakeNewToils");
            toil.initAction = delegate
            {
                Pawn actor = toil.actor;
                Building_Door door = (Building_Door)actor.CurJob.targetA.Thing;

                bool wantedLocked = door.WantedLocked();
                Exceptions wantedState = door.WantedExceptions();
                door.IsLocked() = wantedLocked;
                door.LockExceptions() = wantedState;

                SoundDefOf.FlickSwitch.PlayOneShot(new TargetInfo(door.Position, door.Map, false));
                DoorsPatches.InvalidateReachability(door);
                door.Map.designationManager.DesignationOn(door, AddedDefOf.Locks_DesignatorFlick)?,Delete();
                door.Map.mapDrawer.MapMeshDirty(door.Position, DefOf.LDMapMeshFlagDefOf.DoorLocks);
            };

            toil.defaultCompleteMode = ToilCompleteMode.Instant;
            yield return toil;
        }

        public override bool TryMakePreToilReservations(bool forced)
        {
            return true;
        }
    }
}
