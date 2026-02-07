using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

namespace TooltipIconPatch
{
	public sealed class SotsPrefixTooltipGlobalItem : GlobalItem
	{
		private const string SotsModName = "SOTS";
		private const int SampleValue = 12345;
		private static readonly Dictionary<string, Regex> RegexCache = new Dictionary<string, Regex>();

		public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
		{
			if (!ModLoader.TryGetMod(SotsModName, out Mod sotsMod))
				return;

			Regex maxVoidRegex = GetRegex("Mods.SOTS.Prefixes.Effects.MaxVoid");
			Regex voidRegenRegex = GetRegex("Mods.SOTS.Prefixes.Effects.RegVoid");
			Regex voidCostRegex = GetRegex("Mods.SOTS.Prefixes.Effects.CosVoid");

			for (int i = 0; i < tooltips.Count; i++)
			{
				TooltipLine line = tooltips[i];
				if (line.Name != "PrefixAwakened")
					continue;

				string newName = null;
				if (maxVoidRegex.IsMatch(line.Text))
					newName = "PrefixMaxVoid";
				else if (voidRegenRegex.IsMatch(line.Text))
					newName = "PrefixVoidRegen";
				else if (voidCostRegex.IsMatch(line.Text))
					newName = "PrefixVoidCost";
				else
					newName = TryClassifyByKeyword(line.Text);

				if (newName == null)
					continue;

				bool isModifierBad = line.IsModifierBad;
				if (!line.IsModifier && newName == "PrefixVoidCost")
					isModifierBad = GetIsBadFromSign(line.Text);

				TooltipLine newLine = new TooltipLine(sotsMod, newName, line.Text)
				{
					IsModifier = line.IsModifier,
					IsModifierBad = isModifierBad,
					OverrideColor = line.OverrideColor
				};

				tooltips[i] = newLine;
			}
		}

		private static Regex BuildRegex(string localizationKey)
		{
			string sampleText = Language.GetTextValue(localizationKey, SampleValue);
			string escaped = Regex.Escape(sampleText);
			string sampleNumber = SampleValue.ToString(CultureInfo.InvariantCulture);
			string escapedNumber = Regex.Escape(sampleNumber);

			escaped = escaped.Replace("\\+" + escapedNumber, "[-+]?\\d+[\\d,\\.]*");
			escaped = escaped.Replace(escapedNumber, "[-+]?\\d+[\\d,\\.]*");

			return new Regex("^" + escaped + "$");
		}

		private static Regex GetRegex(string localizationKey)
		{
			string culture = Language.ActiveCulture.Name;
			string cacheKey = culture + "|" + localizationKey;
			if (!RegexCache.TryGetValue(cacheKey, out Regex regex))
			{
				regex = BuildRegex(localizationKey);
				RegexCache[cacheKey] = regex;
			}
			return regex;
		}

		private static string TryClassifyByKeyword(string text)
		{
			string lower = text.ToLowerInvariant();
			if (lower.Contains("void cost") || text.Contains("虚空消耗"))
				return "PrefixVoidCost";
			if (lower.Contains("max void") || text.Contains("最大虚空"))
				return "PrefixMaxVoid";
			if (lower.Contains("void gain") || text.Contains("虚空恢复") || text.Contains("虚空回复"))
				return "PrefixVoidRegen";
			return null;
		}

		private static bool GetIsBadFromSign(string text)
		{
			for (int i = 0; i < text.Length; i++)
			{
				char c = text[i];
				if (c == '+')
					return true;
				if (c == '-')
					return false;
			}
			return false;
		}
	}
}
