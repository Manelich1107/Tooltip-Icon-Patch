# Tooltip Icon Patch

本模组通过为其他模组注册额外的 Tooltip 图标来扩展 Tooltip Icon。

## 源码文件说明

### TooltipIconPatch.cs
- 目的: 模组入口，集中注册所有图标。
- 解决的问题: Tooltip Icon 只识别已注册的 (Mod, LineName)，需要把各个外部模组的行绑定到图标。
- 实现方式: 在 PostSetupContent 里通过 Mod.Call("AddNormalIcon"/"AddPrefixIcon"/"AddDamageClassIcon") 注册图标，并按是否加载某模组决定是否注册。

### SotsVoidCostTooltipGlobalItem.cs
- 目的: 为 SOTS 的虚空消耗行显示独立图标。
- 解决的问题: SOTS 把虚空消耗复用到 Terraria 的 UseMana 行，导致只能显示法力图标。
- 实现方式: 在 GlobalItem.ModifyTooltips 中把 UseMana 行替换为 (SOTS, VoidCost)，文本不变，只改标识。

### SotsPrefixTooltipGlobalItem.cs
- 目的: 为 SOTS 的虚空前缀行（最大虚空/虚空恢复/虚空消耗）显示对应图标。
- 解决的问题: SOTS 统一使用 PrefixAwakened 行名，且不同语言文本导致匹配不稳定。
- 实现方式: 根据本地化模板生成正则，并加入中文关键字兜底识别，把行名改成 PrefixMaxVoid/PrefixVoidRegen/PrefixVoidCost，并保留文本与颜色。

### CalamityPrefixTooltipGlobalItem.cs
- 目的: 为灾厄修改的原版前缀额外词条显示图标（护佑的伤害减免、幸运的运气值）。
- 解决的问题: 额外词条被拼接到同一行，Tooltip Icon 只能识别原行名，无法显示新图标。
- 实现方式: 在 GlobalItem.ModifyTooltips 中按换行拆分原行，保留原行文本，并插入新的 (CalamityMod, PrefixAccDamageReduction/PrefixAccLuck) 行。

### ThoriumAddonInspirationTooltipGlobalItem.cs
- 目的: 让所有 Thorium 附属模组的“使用灵感值”行显示 Thorium 图标。
- 解决的问题: 附属模组的行名相同，但 ModName 不同，导致图标匹配失败。
- 实现方式: 将任意 Mod 的 InspirationCostText 行统一改为 (ThoriumMod, InspirationCostText)，文本不变，仅调整标识。

## 待补充占位符图标
- CalamityPrefixDamageReduction，灾厄的护佑添加的“伤害减免”
- CalamityPrefixLuck，灾厄的幸运添加的“幸运值”
- CalamityStealthGen，灾厄的寂静添加的“潜伏值恢复速度”
- CalamityStealthStrike，灾厄的化境添加的“潜伏攻击伤害”
- CaptureDiscCapture，捕猎碟的捕获伤害
- CaptureDiscTrueCapture，捕猎碟的真实捕获伤害（游戏内未找到，何意味）
- DBZKi，龙珠的气伤害
- Demolisher，爆破手的爆破伤害
- OrchidAlchemist，兰花炼金术士的化学伤害
- OrchidDancer，兰花舞者的雅致伤害（未实装）
- OrchidGambler，兰花赌徒的赌博伤害
- OrchidGamblerChip，兰花赌徒的筹码（有这个伤害类型但我一时半会没找到，游戏内的筹码是赌博伤害）
- OrchidGuardian，兰花守卫的抗争伤害
- OrchidShaman，兰花萨满，查无此人
- OrchidShapeshifter，兰花野性者（拟态师）的野性伤害，等我做补充汉化一定改个名字
- RedemptionRitualist，救赎魂仪师的仪式伤害
- SOTSPrefixMaxVoid，暗影奥秘全知添加的“增加最大虚空值”
- SOTSPrefixVoidCost，暗影奥秘全能添加的“减小虚空消耗”
- SOTSPrefixVoidRegen，暗影奥秘缚魂添加的“虚空恢复量”
- SOTSVoidCost，暗影奥秘武器的“消耗虚空值”
- ThoriumEmpowermentDuration，瑟银传颂添加的“咒音增幅时间”
- ThoriumInspiration，瑟银乐师武器的“使用灵感值”
- ThoriumReworkLifeCost，瑟银冥界的冒险添加的“增加生命消耗”
- MaxRageUp，灾厄的暴怒模式永久提升物品
- MaxAdrenalineUp，灾厄的肾上腺素永久提升物品
- MaxVoidUp，暗影奥秘的最大虚空值永久提升物品
- MaxInspirationUp，瑟银的最大灵感值永久提升物品
- CalamityPrefixArcaneManaCost，灾厄的奥秘添加的“减魔力消耗”
- CalamityPrefixArcaneMagicDamage，灾厄的奥秘添加的“魔法伤害”
- CalamityPrefixLifeRegen，灾厄的焕生添加的“生命再生”

## 后续
- 完成贴图
- 对应修改description_workshop.txt