using Terraria.ModLoader;
using Terraria.ID;
using Terraria.ObjectData;
using Terraria;
using Terraria.Localization;

namespace OneSkyBlock.Items
{
	public class CraftableDemonAltar : ModItem
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault(Language.GetTextValue("Mods.OneSkyBlock.CraftableAltar.DisplayName"));
			Tooltip.SetDefault(Language.GetTextValue("Mods.OneSkyBlock.CraftableAltar.Tooltip"));
			TileObjectData.newTile.FullCopyFrom(TileID.DemonAltar);
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
			CreateRecipe()
				.AddIngredient(ItemID.EbonstoneBlock, 15)
				.AddIngredient(ItemID.RottenChunk, 10)
				.AddIngredient(ItemID.Deathweed, 5)
				.AddIngredient(ItemID.BattlePotion, 1)
				.AddIngredient(ItemID.ThornsPotion, 1)
				.AddTile(TileID.WorkBenches)
				.Register();

			CreateRecipe()
				.AddIngredient(ItemID.CrimstoneBlock, 15)
				.AddIngredient(ItemID.Vertebrae, 10)
				.AddIngredient(ItemID.Deathweed, 5)
				.AddIngredient(ItemID.BattlePotion, 1)
				.AddIngredient(ItemID.ThornsPotion, 1)
				.AddTile(TileID.WorkBenches)
				.Register();
			//Recipe recipe = CreateRecipe(ItemID.Zenith);
			//recipe.AddIngredient(ItemID.Wood, 6);
			//recipe.AddTile(TileID.WorkBenches);
			//recipe.Register();

		}
	}
}