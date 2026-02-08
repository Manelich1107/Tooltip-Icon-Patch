using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;

namespace TooltipIconPatch
{
	public sealed class SotsVoidCostTooltipGlobalItem : GlobalItem
	{
		private const string SotsModName = "SOTS";
		private const string SotsBardHealerModName = "SOTSBardHealer";
		private const string SotsBardHealerVoidHybridInterface = "SOTSBardHealer.VoidHybrid";

		public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
		{
			if (!TryGetVoidCostMod(item, out Mod targetMod))
				return;

			int index = tooltips.FindIndex(line => line.Mod == "Terraria" && line.Name == "UseMana");
			if (index == -1)
				return;

			TooltipLine oldLine = tooltips[index];
			TooltipLine newLine = new TooltipLine(targetMod, "VoidCost", oldLine.Text)
			{
				OverrideColor = oldLine.OverrideColor
			};

			tooltips[index] = newLine;
		}

		private static bool TryGetVoidCostMod(Item item, out Mod targetMod)
		{
			targetMod = null;
			if (item.ModItem == null)
				return false;

			Type type = item.ModItem.GetType();
			while (type != null)
			{
				if (type.FullName == "SOTS.Void.VoidItem")
					return ModLoader.TryGetMod(SotsModName, out targetMod);

				type = type.BaseType;
			}

			return IsSotsBardHealerVoidHybrid(item, out targetMod);
		}

		private static bool IsSotsBardHealerVoidHybrid(Item item, out Mod targetMod)
		{
			targetMod = null;
			if (!ModLoader.TryGetMod(SotsBardHealerModName, out Mod sotsBardHealerMod))
				return false;

			Type itemType = item.ModItem?.GetType();
			if (itemType == null)
				return false;

			foreach (Type iface in itemType.GetInterfaces())
			{
				if (iface.FullName == SotsBardHealerVoidHybridInterface)
				{
					targetMod = sotsBardHealerMod;
					return true;
				}
			}

			return false;
		}
	}
}
