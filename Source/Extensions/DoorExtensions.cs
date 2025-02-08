using LockableDoors.Enums;
using Prepatcher;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LockableDoors.Extensions
{
	public static class DoorExtensions
	{
		/// <summary>
		/// Injected prepatcher locked field on Building_Door object.
		/// </summary>
		[PrepatcherField]
		[DefaultValue(true)]
		public static extern ref bool IsLocked(this Building_Door target);

		/// <summary>
		/// Injected prepatcher Gizmo field on Building_Door object.
		/// </summary>
		[PrepatcherField]
		[DefaultValue(null)]
		public static extern ref Verse.Command_Action ToggleLockGizmo(this Building_Door target);

		/// <summary>
		/// Injected prepatcher Gizmo field on Building_Door object.
		/// </summary>
		[PrepatcherField]
		[DefaultValue(Exceptions.Colonists | Exceptions.Slaves | Exceptions.Pets | Exceptions.Allies | Exceptions.ColonyMechs)]
		public static extern ref Exceptions LockExceptions(this Building_Door target);
	}
}
