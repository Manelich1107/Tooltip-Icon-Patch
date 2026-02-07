using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;

namespace TooltipIconPatch
{
	public sealed class SotsVoidCostTooltipGlobalItem : GlobalItem
	{
		private const string SotsModName = "SOTS";

		public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
		{
			if (!IsSotsVoidItem(item))
				return;

			int index = tooltips.FindIndex(line => line.Mod == "Terraria" && line.Name == "UseMana");
			if (index == -1)
				return;

			if (!ModLoader.TryGetMod(SotsModName, out Mod sotsMod))
				return;

			TooltipLine oldLine = tooltips[index];
			TooltipLine newLine = new TooltipLine(sotsMod, "VoidCost", oldLine.Text)
			{
				OverrideColor = oldLine.OverrideColor
			};

			tooltips[index] = newLine;
		}

		private static bool IsSotsVoidItem(Item item)
		{
			if (item.ModItem == null)
				return false;

			Type type = item.ModItem.GetType();
			while (type != null)
			{
				if (type.FullName == "SOTS.Void.VoidItem")
					return true;

				type = type.BaseType;
			}

			return false;
		}
	}
}
