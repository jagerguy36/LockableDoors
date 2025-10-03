using LockableDoors.DefOf;
using RimWorld;
using System.Collections.Generic;
using System.Diagnostics;
using Verse;
using Verse.AI;

namespace LockableDoors
{
    public class WorkGiver_ToggleLock : WorkGiver_Scanner
    {
        [DebuggerHidden]
        public override IEnumerable<Thing> PotentialWorkThingsGlobal(Pawn pawn)
        {
            List<Designation> desList = pawn.Map.designationManager.designationsByDef[AddedDefOf.Locks_DesignatorFlick];
            for (int i = 0; i < desList.Count; i++)
            {
                yield return desList[i].target.Thing;
            }
        }

        public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
        {
            ThingWithComps door = (ThingWithComps)t;
            return pawn.Map.designationManager.DesignationOn(t, AddedDefOf.Locks_DesignatorFlick) != null && pawn.CanReserveAndReach(t, PathEndMode.Touch, pawn.NormalMaxDanger(), 1, -1, null, forced);
        }

        public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
        {

            ThingWithComps door = (ThingWithComps)t;
            return new Job(AddedDefOf.Locks_JobFlick, t);
        }
    }
}
