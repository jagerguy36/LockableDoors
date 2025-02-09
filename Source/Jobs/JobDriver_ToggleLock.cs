using LockableDoors.Enums;
using LockableDoors.Extensions;
using LockableDoors.Patches;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Verse;
using Verse.AI;
using Verse.Sound;

namespace LockableDoors
{
    public class JobDriver_ToggleLock : JobDriver
    {
        [DebuggerHidden]
        protected override IEnumerable<Toil> MakeNewToils()
        {
            this.FailOn(delegate
            {
                Designation designation = Map.designationManager.DesignationOn(TargetThingA, ToggleJobUtility.DesDef);
                return designation == null;
            });
            yield return Toils_Reserve.Reserve(TargetIndex.A, 1, -1, null);
            yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch);
            yield return Toils_General.Wait(15).FailOnCannotTouch(TargetIndex.A, PathEndMode.Touch);
            Toil toil = new Toil();
            toil.initAction = delegate
            {
                Pawn actor = toil.actor;
                Building_Door door = (Building_Door)actor.CurJob.targetA.Thing;


                Exceptions wantedState = door.WantedExceptions();
                Exceptions currentState = door.LockExceptions();
                bool wantedLocked = door.WantedLocked();
                bool currentLocked = door.IsLocked();

                if (currentState != wantedState)
                {
                    door.LockExceptions() = wantedState;
                }
                if (currentLocked != wantedLocked)
                {
                    door.IsLocked() = wantedLocked;
                }

                SoundDefOf.FlickSwitch.PlayOneShot(new TargetInfo(door.Position, door.Map, false));
                DoorsPatches.InvalidateReachability(door);
                Designation designation = Map.designationManager.DesignationOn(door, ToggleJobUtility.DesDef);
                if (designation != null)
                {
                    designation.Delete();
                }
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
