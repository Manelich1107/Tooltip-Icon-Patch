using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.ModLoader;

namespace TooltipIconPatch
{
	// Please read https://github.com/tModLoader/tModLoader/wiki/Basic-tModLoader-Modding-Guide#mod-skeleton-contents for more information about the various files in a mod.
	public class TooltipIconPatch : Mod
	{
		private const string TooltipIconModName = "TooltipIcon";
		private const string ThoriumModName = "ThoriumMod";
		private const string ThoriumReworkModName = "ThoriumRework";
		private const string SOTSModName = "SOTS";
		private const string SOTSBardHealerModName = "SOTSBardHealer";
		private const string CalamityModName = "CalamityMod"; 
		private const string RagnarokModName = "RagnarokMod";
		private const string ThrowerUnificationModName = "ThrowerUnification";
		private const string InfernalEclipseAPIModName = "InfernalEclipseAPI";
		private const string ClickerClassModName = "ClickerClass";
		private const string CaptureDiscClassModName = "CaptureDiscClass";
		private const string DBZModPortModName = "DBZMODPORT";
		private const string DemolisherClassModName = "DemolisherClass";
		private const string OrchidModName = "OrchidMod";
		private const string RedemptionModName = "Redemption";

		public override void PostSetupContent()
		{
			if (!ModLoader.TryGetMod(TooltipIconModName, out Mod TooltipIcon))
				return;

			TryAddVanillaClasslessIcon(TooltipIcon, "TooltipIconPatch/Assets/TooltipIcons/ClasslessDamage");

			bool hasThorium = ModLoader.TryGetMod(ThoriumModName, out Mod Thorium);
			bool hasCalamity = ModLoader.TryGetMod(CalamityModName, out Mod CalamityMod);
			bool hasRagnarok = ModLoader.TryGetMod(RagnarokModName, out Mod RagnarokMod);
			bool hasThrowerUnification = ModLoader.TryGetMod(ThrowerUnificationModName, out Mod ThrowerUnification);
			bool useMergedThrowingIcon = hasRagnarok || hasThrowerUnification;

			if (hasThorium)
			{
				RegisterThoriumDamageIcons(TooltipIcon, Thorium);
				RegisterThoriumInspirationCostIcon(TooltipIcon);
				RegisterThoriumEmpowermentDurationIcon(TooltipIcon);
				RegisterThoriumMaxInspirationConsumables(TooltipIcon, Thorium);
			}

			if (ModLoader.TryGetMod(ThoriumReworkModName, out Mod ThoriumRework))
				RegisterThoriumReworkLifeCostPrefixIcon(TooltipIcon);

			if (ModLoader.TryGetMod(SOTSModName, out Mod SOTS))
			{
				RegisterSOTSVoidDamageIcons(TooltipIcon, SOTS);
				RegisterSOTSVoidCostIcon(TooltipIcon);
				RegisterSOTSPrefixIcons(TooltipIcon);
				RegisterSOTSMaxVoidConsumables(TooltipIcon, SOTS);
			}

			if (ModLoader.TryGetMod(SOTSBardHealerModName, out Mod SOTSBardHealer))
			{
				RegisterSOTSBardHealerVoidDamageIcons(TooltipIcon, SOTSBardHealer);
				RegisterSOTSBardHealerVoidCostIcon(TooltipIcon);
			}

			if (useMergedThrowingIcon)
			{
				RegisterMergedThrowingIcons(TooltipIcon, CalamityMod, hasCalamity, hasThorium);

				if (hasCalamity)
				{
					RegisterCalamityRoguePrefixIcons(TooltipIcon);
					RegisterCalamityVanillaPrefixIcons(TooltipIcon);
				}

				if (hasRagnarok)
					RegisterRagnarokMergedThrowingIcon(TooltipIcon, RagnarokMod);

				if (hasThrowerUnification)
					RegisterThrowerUnificationIcon(TooltipIcon, ThrowerUnification);
			}
			else
			{
				if (hasCalamity)
				{
					RegisterCalamityRogueDamageIcons(TooltipIcon, CalamityMod);
					RegisterCalamityRoguePrefixIcons(TooltipIcon);
					RegisterCalamityVanillaPrefixIcons(TooltipIcon);
				}

				if (hasThorium)
					RegisterThoriumThrowingIcon(TooltipIcon);
			}

			if (hasCalamity)
				RegisterCalamityMaxStatsConsumables(TooltipIcon, CalamityMod);

			if (ModLoader.TryGetMod(InfernalEclipseAPIModName, out Mod InfernalEclipseAPI))
				RegisterInfernalEclipseAPIDamageIcons(TooltipIcon, InfernalEclipseAPI);

			if (ModLoader.TryGetMod(ClickerClassModName, out Mod ClickerClassMod))
				RegisterClickerClassDamageIcons(TooltipIcon, ClickerClassMod);

			if (ModLoader.TryGetMod(CaptureDiscClassModName, out Mod CaptureDiscClassMod))
				RegisterCaptureDiscClassDamageIcons(TooltipIcon, CaptureDiscClassMod);

			if (ModLoader.TryGetMod(DBZModPortModName, out Mod DBZModPortMod))
				RegisterDBZModPortDamageIcons(TooltipIcon, DBZModPortMod);

			if (ModLoader.TryGetMod(DemolisherClassModName, out Mod DemolisherClassMod))
				RegisterDemolisherClassDamageIcons(TooltipIcon, DemolisherClassMod);

			if (ModLoader.TryGetMod(OrchidModName, out Mod OrchidMod))
				RegisterOrchidModDamageIcons(TooltipIcon, OrchidMod);

			if (ModLoader.TryGetMod(RedemptionModName, out Mod RedemptionMod))
				RegisterRedemptionDamageIcons(TooltipIcon, RedemptionMod);
		}

		private static void RegisterCalamityMaxStatsConsumables(Mod TooltipIcon, Mod CalamityMod)
		{
			Asset<Texture2D> maxLifeUp = ModContent.Request<Texture2D>(
				"TooltipIcon/Textures/Consumables/MaxLifeUp",
				AssetRequestMode.ImmediateLoad);
			Asset<Texture2D> maxManaUp = ModContent.Request<Texture2D>(
				"TooltipIcon/Textures/Consumables/MaxManaUp",
				AssetRequestMode.ImmediateLoad);
			Asset<Texture2D> maxRageUp = ModContent.Request<Texture2D>(
				"TooltipIconPatch/Assets/TooltipIcons/MaxRageUp",
				AssetRequestMode.ImmediateLoad);
			Asset<Texture2D> maxAdrenalineUp = ModContent.Request<Texture2D>(
				"TooltipIconPatch/Assets/TooltipIcons/MaxAdrenalineUp",
				AssetRequestMode.ImmediateLoad);

			TryAddConsumableIcon(
				TooltipIcon,
				CalamityMod,
				new[] { "SanguineTangerine", "TaintedCloudberry", "MiracleFruit", "SacredStrawberry" },
				maxLifeUp);
			TryAddConsumableIcon(
				TooltipIcon,
				CalamityMod,
				new[] { "EnchantedStarfish", "CometShard", "EtherealCore", "PhantomHeart" },
				maxManaUp);
			TryAddConsumableIcon(
				TooltipIcon,
				CalamityMod,
				new[] { "MushroomPlasmaRoot", "InfernalBlood", "RedLightningContainer" },
				maxRageUp);
			TryAddConsumableIcon(
				TooltipIcon,
				CalamityMod,
				new[] { "ElectrolyteGelPack", "StarlightFuelCell", "Ectoheart" },
				maxAdrenalineUp);
		}

		private static void RegisterThoriumMaxInspirationConsumables(Mod TooltipIcon, Mod Thorium)
		{
			Asset<Texture2D> maxInspirationUp = ModContent.Request<Texture2D>(
				"TooltipIconPatch/Assets/TooltipIcons/MaxInspirationUp",
				AssetRequestMode.ImmediateLoad);

			TryAddConsumableIcon(
				TooltipIcon,
				Thorium,
				new[] { "InspirationFragment", "InspirationShard", "InspirationCrystalNew", "InspirationGem" },
				maxInspirationUp);
		}

		private static void RegisterSOTSMaxVoidConsumables(Mod TooltipIcon, Mod SOTS)
		{
			Asset<Texture2D> maxVoidUp = ModContent.Request<Texture2D>(
				"TooltipIconPatch/Assets/TooltipIcons/MaxVoidUp",
				AssetRequestMode.ImmediateLoad);

			TryAddConsumableIcon(
				TooltipIcon,
				SOTS,
				new[] { "ScarletStar", "VioletStar", "VoidenAnkh", "SoulHeart" },
				maxVoidUp);
		}

		private static void RegisterThoriumDamageIcons(Mod TooltipIcon, Mod Thorium)
		{
			const string classlessIconPath = "TooltipIconPatch/Assets/TooltipIcons/ClasslessDamage";
			TryAddDamageClassIcon(
				TooltipIcon,
				Thorium,
				"BardDamage",
				"TooltipIconPatch/Assets/TooltipIcons/ThoriumBard");

			TryAddDamageClassIcon(
				TooltipIcon,
				Thorium,
				"HealerDamage",
				"TooltipIconPatch/Assets/TooltipIcons/ThoriumHealer");

			TryAddDamageClassIcon(
				TooltipIcon,
				Thorium,
				"TrueDamage",
				classlessIconPath);
		}

		private static void RegisterThoriumInspirationCostIcon(Mod TooltipIcon)
		{
			Asset<Texture2D> texture = ModContent.Request<Texture2D>(
				"TooltipIconPatch/Assets/TooltipIcons/ThoriumInspiration",
				AssetRequestMode.ImmediateLoad);
			TooltipIcon.Call("AddNormalIcon", ThoriumModName, "InspirationCostText", texture);
		}

		private static void RegisterThoriumEmpowermentDurationIcon(Mod TooltipIcon)
		{
			Asset<Texture2D> texture = ModContent.Request<Texture2D>(
				"TooltipIconPatch/Assets/TooltipIcons/ThoriumEmpowermentDuration",
				AssetRequestMode.ImmediateLoad);
			TooltipIcon.Call("AddPrefixIcon", ThoriumModName, "PrefixEmpowermentDuration", texture, false);
		}

		private static void RegisterThoriumReworkLifeCostPrefixIcon(Mod TooltipIcon)
		{
			Asset<Texture2D> texture = ModContent.Request<Texture2D>(
				"TooltipIconPatch/Assets/TooltipIcons/ThoriumReworkLifeCost",
				AssetRequestMode.ImmediateLoad);
			TooltipIcon.Call("AddPrefixIcon", ThoriumReworkModName, "RadiantLifeCostModifier", texture, true);
		}

		private static void RegisterSOTSVoidDamageIcons(Mod TooltipIcon, Mod SOTS)
		{
			const string voidIconPath = "TooltipIconPatch/Assets/TooltipIcons/SOTS/";
			TryAddDamageClassIcon(TooltipIcon, SOTS, "VoidGeneric", voidIconPath+ "SotsVoid");
			TryAddDamageClassIcon(TooltipIcon, SOTS, "VoidMelee", voidIconPath+ "VoidMelee");
			TryAddDamageClassIcon(TooltipIcon, SOTS, "VoidRanged", voidIconPath+ "VoidRanged");
			TryAddDamageClassIcon(TooltipIcon, SOTS, "VoidMagic", voidIconPath+ "VoidMagic");
			TryAddDamageClassIcon(TooltipIcon, SOTS, "VoidSummon", voidIconPath+ "VoidSummon");
		}

		private static void RegisterSOTSVoidCostIcon(Mod TooltipIcon)
		{
			Asset<Texture2D> texture = ModContent.Request<Texture2D>(
				"TooltipIconPatch/Assets/TooltipIcons/SOTSVoidCost",
				AssetRequestMode.ImmediateLoad);
			TooltipIcon.Call("AddNormalIcon", SOTSModName, "VoidCost", texture);
		}

		private static void RegisterSOTSPrefixIcons(Mod TooltipIcon)
		{
			Asset<Texture2D> maxVoidTexture = ModContent.Request<Texture2D>(
				"TooltipIconPatch/Assets/TooltipIcons/SOTSPrefixMaxVoid",
				AssetRequestMode.ImmediateLoad);
			TooltipIcon.Call("AddPrefixIcon", SOTSModName, "PrefixMaxVoid", maxVoidTexture, false);

			Asset<Texture2D> voidRegenTexture = ModContent.Request<Texture2D>(
				"TooltipIconPatch/Assets/TooltipIcons/SOTSPrefixVoidRegen",
				AssetRequestMode.ImmediateLoad);
			TooltipIcon.Call("AddPrefixIcon", SOTSModName, "PrefixVoidRegen", voidRegenTexture, false);

			Asset<Texture2D> voidCostTexture = ModContent.Request<Texture2D>(
				"TooltipIconPatch/Assets/TooltipIcons/SOTSPrefixVoidCost",
				AssetRequestMode.ImmediateLoad);
			TooltipIcon.Call("AddPrefixIcon", SOTSModName, "PrefixVoidCost", voidCostTexture, true);
		}

		private static void RegisterSOTSBardHealerVoidDamageIcons(Mod TooltipIcon, Mod SOTSBardHealer)
		{
			TryAddDamageClassIcon(TooltipIcon, SOTSBardHealer, "VoidRadiant", "TooltipIconPatch/Assets/TooltipIcons/SOTSBardHealerVoidRadiant");
			TryAddDamageClassIcon(TooltipIcon, SOTSBardHealer, "VoidSymphonic", "TooltipIconPatch/Assets/TooltipIcons/SOTSBardHealerVoidSymphonic");
			TryAddDamageClassIcon(TooltipIcon, SOTSBardHealer, "VoidThrowing", "TooltipIconPatch/Assets/TooltipIcons/SOTSBardHealerVoidThrowing");
		}

		private static void RegisterSOTSBardHealerVoidCostIcon(Mod TooltipIcon)
		{
			Asset<Texture2D> texture = ModContent.Request<Texture2D>(
				"TooltipIconPatch/Assets/TooltipIcons/SOTSVoidCost",
				AssetRequestMode.ImmediateLoad);
			TooltipIcon.Call("AddNormalIcon", SOTSBardHealerModName, "VoidCost", texture);
		}

		private static void RegisterCalamityRogueDamageIcons(Mod TooltipIcon, Mod CalamityMod)
		{
			const string throwingIconPath = "TooltipIconPatch/Assets/TooltipIcons/RogueThrowing";
			const string classlessIconPath = "TooltipIconPatch/Assets/TooltipIcons/ClasslessDamage";
			TryAddDamageClassIcon(TooltipIcon, CalamityMod, "RogueDamageClass", throwingIconPath);
			TryAddDamageClassIcon(TooltipIcon, CalamityMod, "StealthDamageClass", throwingIconPath);
			TryAddDamageClassIcon(TooltipIcon, CalamityMod, "AverageDamageClass", classlessIconPath);
		}

		private static void RegisterCalamityRoguePrefixIcons(Mod TooltipIcon)
		{
			Asset<Texture2D> stealthStrikeTexture = ModContent.Request<Texture2D>(
				"TooltipIconPatch/Assets/TooltipIcons/CalamityStealthStrike",
				AssetRequestMode.ImmediateLoad);
			TooltipIcon.Call("AddPrefixIcon", CalamityModName, "PrefixStealthDamageBoost", stealthStrikeTexture, false);
			TooltipIcon.Call("AddPrefixIcon", CalamityModName, "CalamityMod:PrefixStealthDamage", stealthStrikeTexture, false);

			Asset<Texture2D> stealthGenTexture = ModContent.Request<Texture2D>(
				"TooltipIconPatch/Assets/TooltipIcons/CalamityStealthGen",
				AssetRequestMode.ImmediateLoad);
			TooltipIcon.Call("AddPrefixIcon", CalamityModName, "PrefixStealthGenBoost", stealthGenTexture, false);
			TooltipIcon.Call("AddPrefixIcon", CalamityModName, "CalamityMod:PrefixAccStealthGen", stealthGenTexture, false);
		}

		private static void RegisterCalamityVanillaPrefixIcons(Mod TooltipIcon)
		{
			Asset<Texture2D> damageReductionTexture = ModContent.Request<Texture2D>(
				"TooltipIconPatch/Assets/TooltipIcons/CalamityPrefixDamageReduction",
				AssetRequestMode.ImmediateLoad);
			TooltipIcon.Call("AddPrefixIcon", CalamityModName, "PrefixAccDamageReduction", damageReductionTexture, false);

			Asset<Texture2D> luckTexture = ModContent.Request<Texture2D>(
				"TooltipIconPatch/Assets/TooltipIcons/CalamityPrefixLuck",
				AssetRequestMode.ImmediateLoad);
			TooltipIcon.Call("AddPrefixIcon", CalamityModName, "PrefixAccLuck", luckTexture, false);

			Asset<Texture2D> maxLifeTexture = ModContent.Request<Texture2D>(
				"TooltipIcon/Textures/Consumables/MaxLifeUp",
				AssetRequestMode.ImmediateLoad);
			TooltipIcon.Call("AddPrefixIcon", CalamityModName, "CalamityMod:PrefixMaxLifeBoost", maxLifeTexture, false);

			Asset<Texture2D> lifeRegenTexture = ModContent.Request<Texture2D>(
				"TooltipIconPatch/Assets/TooltipIcons/CalamityPrefixLifeRegen",
				AssetRequestMode.ImmediateLoad);
			TooltipIcon.Call("AddPrefixIcon", CalamityModName, "CalamityMod:PrefixLifeRegenBoost", lifeRegenTexture, false);

			Asset<Texture2D> arcaneMagicDamageTexture = ModContent.Request<Texture2D>(
				"TooltipIconPatch/Assets/TooltipIcons/CalamityPrefixArcaneMagicDamage",
				AssetRequestMode.ImmediateLoad);
			TooltipIcon.Call("AddPrefixIcon", CalamityModName, "PrefixAccArcaneMagicDamage", arcaneMagicDamageTexture, false);

			Asset<Texture2D> arcaneManaCostTexture = ModContent.Request<Texture2D>(
				"TooltipIconPatch/Assets/TooltipIcons/CalamityPrefixArcaneManaCost",
				AssetRequestMode.ImmediateLoad);
			TooltipIcon.Call("AddPrefixIcon", CalamityModName, "PrefixAccArcaneManaCost", arcaneManaCostTexture, true);
		}

		private static void RegisterMergedThrowingIcons(Mod TooltipIcon, Mod CalamityMod, bool hasCalamity, bool hasThorium)
		{
			const string mergedThrowingIconPath = "TooltipIconPatch/Assets/TooltipIcons/MergedThrowing";
			const string classlessIconPath = "TooltipIconPatch/Assets/TooltipIcons/ClasslessDamage";

			if (hasThorium)
				TryAddVanillaThrowingIcon(TooltipIcon, mergedThrowingIconPath);

			if (hasCalamity)
			{
				TryAddDamageClassIcon(TooltipIcon, CalamityMod, "RogueDamageClass", mergedThrowingIconPath);
				TryAddDamageClassIcon(TooltipIcon, CalamityMod, "StealthDamageClass", mergedThrowingIconPath);
				TryAddDamageClassIcon(TooltipIcon, CalamityMod, "AverageDamageClass", classlessIconPath);
			}
		}

		private static void RegisterRagnarokMergedThrowingIcon(Mod TooltipIcon, Mod RagnarokMod)
		{
			TryAddDamageClassIcon(TooltipIcon, RagnarokMod, "ThoriumRogueClass", "TooltipIconPatch/Assets/TooltipIcons/MergedThrowing");
		}

		private static void RegisterThrowerUnificationIcon(Mod TooltipIcon, Mod ThrowerUnification)
		{
			TryAddDamageClassIcon(TooltipIcon, ThrowerUnification, "UnitedModdedThrower", "TooltipIconPatch/Assets/TooltipIcons/MergedThrowing");
		}

		private static void RegisterInfernalEclipseAPIDamageIcons(Mod TooltipIcon, Mod InfernalEclipseAPI)
		{
			TryAddDamageClassIcon(TooltipIcon, InfernalEclipseAPI, "VoidRogue", "TooltipIconPatch/Assets/TooltipIcons/InfernalVoidRogue");
			TryAddDamageClassIcon(TooltipIcon, InfernalEclipseAPI, "MeleeWhip", "TooltipIconPatch/Assets/TooltipIcons/InfernalMeleeWhip");
			TryAddDamageClassIcon(TooltipIcon, InfernalEclipseAPI, "MythicMelee", "TooltipIconPatch/Assets/TooltipIcons/InfernalMythicMelee");
			TryAddDamageClassIcon(TooltipIcon, InfernalEclipseAPI, "MythicRanged", "TooltipIconPatch/Assets/TooltipIcons/InfernalMythicRanged");
			TryAddDamageClassIcon(TooltipIcon, InfernalEclipseAPI, "MythicMagic", "TooltipIconPatch/Assets/TooltipIcons/InfernalMythicMagic");
			TryAddDamageClassIcon(TooltipIcon, InfernalEclipseAPI, "MythicSummon", "TooltipIconPatch/Assets/TooltipIcons/InfernalMythicSummon");
			TryAddDamageClassIcon(TooltipIcon, InfernalEclipseAPI, "LegendaryMelee", "TooltipIconPatch/Assets/TooltipIcons/InfernalLegendaryMelee");
			TryAddDamageClassIcon(TooltipIcon, InfernalEclipseAPI, "LegendaryRanged", "TooltipIconPatch/Assets/TooltipIcons/InfernalLegendaryRanged");
			TryAddDamageClassIcon(TooltipIcon, InfernalEclipseAPI, "LegendaryMagic", "TooltipIconPatch/Assets/TooltipIcons/InfernalLegendaryMagic");
		}

		private static void RegisterClickerClassDamageIcons(Mod TooltipIcon, Mod ClickerClassMod)
		{
			TryAddDamageClassIcon(TooltipIcon, ClickerClassMod, "ClickerDamage", "TooltipIconPatch/Assets/TooltipIcons/Clicker");
		}

		private static void RegisterCaptureDiscClassDamageIcons(Mod TooltipIcon, Mod CaptureDiscClassMod)
		{
			TryAddDamageClassIcon(TooltipIcon, CaptureDiscClassMod, "CaptureDamage", "TooltipIconPatch/Assets/TooltipIcons/CaptureDiscCapture");
			TryAddDamageClassIcon(TooltipIcon, CaptureDiscClassMod, "TrueCaptureDamage", "TooltipIconPatch/Assets/TooltipIcons/CaptureDiscTrueCapture");
		}

		private static void RegisterDBZModPortDamageIcons(Mod TooltipIcon, Mod DBZModPortMod)
		{
			TryAddDamageClassIcon(TooltipIcon, DBZModPortMod, "KiDamageType", "TooltipIconPatch/Assets/TooltipIcons/DBZKi");
		}

		private static void RegisterDemolisherClassDamageIcons(Mod TooltipIcon, Mod DemolisherClassMod)
		{
			TryAddDamageClassIcon(TooltipIcon, DemolisherClassMod, "DemolisherDamage", "TooltipIconPatch/Assets/TooltipIcons/Demolisher");
		}

		private static void RegisterOrchidModDamageIcons(Mod TooltipIcon, Mod OrchidMod)
		{
			TryAddDamageClassIcon(TooltipIcon, OrchidMod, "AlchemistDamageClass", "TooltipIconPatch/Assets/TooltipIcons/OrchidAlchemist");
			TryAddDamageClassIcon(TooltipIcon, OrchidMod, "DancerDamageClass", "TooltipIconPatch/Assets/TooltipIcons/OrchidDancer");
			TryAddDamageClassIcon(TooltipIcon, OrchidMod, "GamblerDamageClass", "TooltipIconPatch/Assets/TooltipIcons/OrchidGambler");
			TryAddDamageClassIcon(TooltipIcon, OrchidMod, "GamblerChipDamageClass", "TooltipIconPatch/Assets/TooltipIcons/OrchidGamblerChip");
			TryAddDamageClassIcon(TooltipIcon, OrchidMod, "GuardianDamageClass", "TooltipIconPatch/Assets/TooltipIcons/OrchidGuardian");
			TryAddDamageClassIcon(TooltipIcon, OrchidMod, "ShamanDamageClass", "TooltipIconPatch/Assets/TooltipIcons/OrchidShaman");
			TryAddDamageClassIcon(TooltipIcon, OrchidMod, "ShapeshifterDamageClass", "TooltipIconPatch/Assets/TooltipIcons/OrchidShapeshifter");
		}

		private static void RegisterRedemptionDamageIcons(Mod TooltipIcon, Mod RedemptionMod)
		{
			TryAddDamageClassIcon(TooltipIcon, RedemptionMod, "RitualistClass", "TooltipIconPatch/Assets/TooltipIcons/RedemptionRitualist");
		}

		private static void TryAddDamageClassIcon(Mod TooltipIcon, Mod ownerMod, string damageClassName, string texturePath)
		{
			if (!ownerMod.TryFind(damageClassName, out DamageClass damageClass))
				return;

			Asset<Texture2D> texture = ModContent.Request<Texture2D>(texturePath, AssetRequestMode.ImmediateLoad);
			TooltipIcon.Call("AddDamageClassIcon", damageClass, texture, null, null, null);
		}

		private static void TryAddVanillaThrowingIcon(Mod TooltipIcon, string texturePath)
		{
			Asset<Texture2D> texture = ModContent.Request<Texture2D>(texturePath, AssetRequestMode.ImmediateLoad);
			TooltipIcon.Call("AddDamageClassIcon", DamageClass.Throwing, texture, null, null, null);
		}

		private static void TryAddConsumableIcon(Mod TooltipIcon, Mod ownerMod, string[] itemNames, Asset<Texture2D> texture)
		{
			HashSet<int> itemTypes = new HashSet<int>();
			foreach (string itemName in itemNames)
			{
				if (ownerMod.TryFind(itemName, out ModItem modItem))
					itemTypes.Add(modItem.Type);
			}

			if (itemTypes.Count == 0)
				return;

			TooltipIcon.Call(
				"AddConsumableIcon",
				(Func<Item, DrawableTooltipLine, bool>)((item, line) => itemTypes.Contains(item.type)),
				texture);
		}
        private static void TryAddVanillaClasslessIcon(Mod TooltipIcon, string texturePath)
        {
            Asset<Texture2D> texture = ModContent.Request<Texture2D>(texturePath, AssetRequestMode.ImmediateLoad);
            TooltipIcon.Call("AddDamageClassIcon", DamageClass.Default, texture, null, null, null);
        }

        private static void RegisterThoriumThrowingIcon(Mod TooltipIcon)
		{
			TryAddVanillaThrowingIcon(TooltipIcon, "TooltipIconPatch/Assets/TooltipIcons/ThoriumThrowing");
		}
	}
}
