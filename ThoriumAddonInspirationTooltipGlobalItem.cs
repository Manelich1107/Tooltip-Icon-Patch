using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;

namespace TooltipIconPatch
{
	public sealed class ThoriumAddonInspirationTooltipGlobalItem : GlobalItem
	{
		private const string ThoriumModName = "ThoriumMod";

		public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
		{
			if (!ModLoader.TryGetMod(ThoriumModName, out Mod thoriumMod))
				return;

			for (int i = 0; i < tooltips.Count; i++)
			{
				TooltipLine line = tooltips[i];
				if (line.Name != "InspirationCostText")
					continue;
				if (line.Mod == ThoriumModName)
					continue;

				TooltipLine newLine = new TooltipLine(thoriumMod, "InspirationCostText", line.Text)
				{
					IsModifier = line.IsModifier,
					IsModifierBad = line.IsModifierBad,
					OverrideColor = line.OverrideColor
				};

				tooltips[i] = newLine;
			}
		}
	}
}
