using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;

namespace TooltipIconPatch
{
	public sealed class CalamityPrefixTooltipGlobalItem : GlobalItem
	{
		private const string CalamityModName = "CalamityMod";

		public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
		{
			if (!ModLoader.TryGetMod(CalamityModName, out Mod calamityMod))
				return;

			for (int i = 0; i < tooltips.Count; i++)
			{
				TooltipLine line = tooltips[i];
				if (line.Mod != "Terraria")
					continue;

				if (line.Name == "PrefixAccDefense" && TrySplitLine(line.Text, out string mainDefense, out string extraDefense))
				{
					line.Text = mainDefense;
					TooltipLine extraLine = new TooltipLine(calamityMod, "PrefixAccDamageReduction", extraDefense)
					{
						IsModifier = true,
						IsModifierBad = IsNegative(extraDefense)
					};
					tooltips.Insert(i + 1, extraLine);
					i++;
				}
				else if (line.Name == "PrefixAccCritChance" && TrySplitLine(line.Text, out string mainCrit, out string extraCrit))
				{
					line.Text = mainCrit;
					TooltipLine extraLine = new TooltipLine(calamityMod, "PrefixAccLuck", extraCrit)
					{
						IsModifier = true,
						IsModifierBad = IsNegative(extraCrit)
					};
					tooltips.Insert(i + 1, extraLine);
					i++;
				}
				else if (line.Name == "PrefixAccMaxMana" && TrySplitLine(line.Text, out string mainMana, out string extraMana))
				{
					line.Text = mainMana;
					string[] extraParts = extraMana.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
					if (extraParts.Length < 2)
						continue;

					TooltipLine magicDamageLine = new TooltipLine(calamityMod, "PrefixAccArcaneMagicDamage", extraParts[0].Trim())
					{
						IsModifier = true,
						IsModifierBad = IsNegative(extraParts[0])
					};
					TooltipLine manaCostLine = new TooltipLine(calamityMod, "PrefixAccArcaneManaCost", extraParts[1].Trim())
					{
						IsModifier = true,
						IsModifierBad = IsNegativeBenefit(extraParts[1])
					};
					tooltips.Insert(i + 1, magicDamageLine);
					tooltips.Insert(i + 2, manaCostLine);
					i += 2;
				}
			}
		}

		private static bool TrySplitLine(string text, out string mainLine, out string extraLine)
		{
			int splitIndex = text.IndexOf('\n');
			if (splitIndex == -1)
			{
				mainLine = null;
				extraLine = null;
				return false;
			}

			mainLine = text.Substring(0, splitIndex);
			extraLine = text.Substring(splitIndex + 1);
			return !string.IsNullOrWhiteSpace(extraLine);
		}

		private static bool IsNegative(string text)
		{
			return text.TrimStart().StartsWith("-", StringComparison.Ordinal);
		}

		private static bool IsNegativeBenefit(string text)
		{
			return !IsNegative(text);
		}
	}
}
