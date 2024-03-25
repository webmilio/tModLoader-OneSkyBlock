using Terraria.ModLoader;
using Terraria.ID;
using Terraria.ObjectData;
using Terraria;
using Terraria.Localization;
using System.Collections.Generic;

namespace OneSkyBlock.Items
{
	public class CraftableDemonAltar : ModItem
	{
        public override void SetStaticDefaults()
        {
			//Tooltip.SetDefault("Will not drop anything if you break it!");//Language.GetTextValue("Mods.OneSkyBlock.CraftableAltar.Tooltip"));
		}
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
			TooltipLine tooltip = new(Mod, "OneSkyBlock: DemonAltar", Language.GetTextValue("Mods.OneSkyBlock.CraftableAltar.Tooltip"));
            tooltips.Add(tooltip);
        }
        public override void SetDefaults()
		{
			TileObjectData.newTile.FullCopyFrom(TileID.DemonAltar);
			Item.SetNameOverride(Language.GetTextValue("Mods.OneSkyBlock.CraftableAltar.DisplayName"));
            Item.maxStack = 99;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.consumable = true;
			Item.value = Item.buyPrice(0,1,0,0);
			Item.rare = ItemRarityID.Blue;
			Item.createTile = TileID.DemonAltar; //ModContent.TileType<CraftableDemonAltar>();	
			Item.placeStyle = 0;
		}

        public override void AddRecipes()
        {
            // Demon alter recipe
			Recipe.Create(TileID.DemonAltar)
				.AddIngredient(ItemID.EbonstoneBlock, 15)
				.AddIngredient(ItemID.RottenChunk, 10)
				.AddIngredient(ItemID.Deathweed, 5)
				.AddIngredient(ItemID.BattlePotion, 1)
				.AddIngredient(ItemID.ThornsPotion, 1)
				.AddTile(TileID.WorkBenches)
				.Register();

			Recipe.Create(TileID.DemonAltar)
				.AddIngredient(ItemID.CrimstoneBlock, 15)
				.AddIngredient(ItemID.Vertebrae, 10)
				.AddIngredient(ItemID.Deathweed, 5)
				.AddIngredient(ItemID.BattlePotion, 1)
				.AddIngredient(ItemID.ThornsPotion, 1)
				.AddTile(TileID.WorkBenches)
				.Register();
        }
	}
}