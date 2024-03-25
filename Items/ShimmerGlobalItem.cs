using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

namespace OneSkyBlock.Items;

internal class ShimmerGlobalItem : GlobalItem
{
    public override void ModifyItemLoot(Item item, ItemLoot itemLoot)
    {
        var config = ModContent.GetInstance<OneSkyBlockConfig>();

        if (!config.ShimmerChallenge)
        {
            return;
        }

        switch (item.type)
        {
            case ItemID.LavaCrate or ItemID.LavaCrateHard:
                var localized = Mod.GetLocalization("Conditions.DownedSkeletron");
                var rule = ItemDropRule.ByCondition(new SimpleItemDropRuleCondition(localized, () => NPC.downedBoss3, ShowItemDropInUI.Always), ItemID.Hellstone, 2, 4, 10);

                itemLoot.Add(rule);
                break;
        }
    }
}
