using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace LockableDoors
{
    public static class ToggleJobUtility
    {
        private static DesignationDef designationDef;
        private static JobDef jobDef;

        public static DesignationDef DesDef
        {
            get
            {
                if (designationDef == null)
                {
                    designationDef = DefDatabase<DesignationDef>.GetNamed("Locks_Flick");
                }

                return designationDef;
            }
        }

        public static JobDef JobDef
        {
            get
            {
                if (jobDef == null)
                {
                    jobDef = DefDatabase<JobDef>.GetNamed("Locks_Flick");
                }

                return jobDef;
            }
        }
    }
}
