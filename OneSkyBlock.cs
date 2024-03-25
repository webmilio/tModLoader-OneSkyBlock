using System;
using System.Threading;
using System.Linq;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.IO;
using Terraria.ModLoader;
using Terraria.Localization;
using Terraria.WorldBuilding;
using Terraria.GameContent.Generation;
using System.Numerics;
using ReLogic.Utilities;
using Terraria.DataStructures;

namespace OneSkyBlock
{
    public class OneSkyBlock : Mod
    {
        internal static OneSkyBlock Instance;
        internal const string ModifyOneSkyBlockConfig_Permission = "ModifyOneSkyBlockConfig";
        internal const string ModifyOneSkyBlockConfig_Display = "Modify OneSkyBlock Config";
        public override void Load()
        {
            Instance = this;
            /*
            cheatSheet = ModLoader.GetMod("CheatSheet");
            herosMod = ModLoader.GetMod("HEROsMod");
            */
        }
        public override void Unload()
        {
            Instance = null;
        }
        public override void PostSetupContent()
        {
            ModLoader.TryGetMod("HEROsMod", out Mod HEROsMod);
            HEROsMod?.Call(
                    "AddPermission",
                    ModifyOneSkyBlockConfig_Permission,
                    ModifyOneSkyBlockConfig_Display
                );
        }
    }
    class WeightedRandomBag<T>
    {
        private struct Entry
        {
            public double accumulatedWeight;
            public T item;
        }

        private readonly List<Entry> entries = new();
        private double accumulatedWeight;
        private readonly Random rand = new();
        public void AddEntry(T item, double weight)
        {
            accumulatedWeight += weight;
            entries.Add(new Entry { item = item, accumulatedWeight = accumulatedWeight });
        }
        public T GetRandom()
        {
            double r = rand.NextDouble() * accumulatedWeight;

            foreach (Entry entry in entries)
            {
                if (entry.accumulatedWeight >= r)
                {
                    return entry.item;
                }
            }
            return default;
        }
    }

    public class OneSkyBlockResetWorld : ModSystem
    {
        public override void ModifyWorldGenTasks(List<GenPass> tasks, ref double totalWeight)
        {
            var config = ModContent.GetInstance<OneSkyBlockConfig>();

            foreach (var genTask in tasks)
            {
                Console.WriteLine("GenTask: {0}", genTask.Name);
            }
            GenPass resetTask = tasks.Find(x => x.Name == "Reset");
            /*
            GenPass dungeonTask = (ModContent.GetInstance<OneSkyBlockConfig>().GenerateDungeon) ? tasks.Find(x => x.Name == "Dungeon") : null;
            GenPass jungleTask1 = (ModContent.GetInstance<OneSkyBlockConfig>().GenerateTemple) ? tasks.Find(x => x.Name == "Jungle Temple") : null;
            GenPass jungleTask2 = (ModContent.GetInstance<OneSkyBlockConfig>().GenerateTemple) ? tasks.Find(x => x.Name == "Lihzahrd Altars") : null;
            GenPass islandTask1 = (ModContent.GetInstance<OneSkyBlockConfig>().GenerateIslands) ? tasks.Find(x => x.Name == "Floating Island") : null;
            GenPass islandTask2 = (ModContent.GetInstance<OneSkyBlockConfig>().GenerateIslands) ? tasks.Find(x => x.Name == "Floating Island Houses") : null;
            */
            GenPass hellTask1 = (config.GenerateUnderworld) ? tasks.Find(x => x.Name == "Underworld") : null;
            GenPass hellTask2 = (config.GenerateUnderworld) ? tasks.Find(x => x.Name == "Hellforge") : null;
            GenPass pyramidTask = (config.GeneratePyramids) ? tasks.Find(x => x.Name == "Pyramids") : null;
            GenPass microTask = (config.GenerateBiomes) ? tasks.Find(x => x.Name == "Micro Biomes") : null;
            //GenPass oceanTask = tasks.Find(x => x.Name == "Oasis");
            tasks.Clear();
            tasks.Add(resetTask);
            /*
            if (ModContent.GetInstance<OneSkyBlockConfig>().GenerateDungeon)
            {
                tasks.Add(dungeonTask);
            }
            if (ModContent.GetInstance<OneSkyBlockConfig>().GenerateIslands)
            {
                tasks.Add(islandTask1);
                tasks.Add(islandTask2);
            }
            */
            if (config.GenerateUnderworld)
            {
                tasks.Add(hellTask1);
                tasks.Add(hellTask2);
            }
            if (config.GeneratePyramids)
            {
                tasks.Add(pyramidTask);
            }
            if (config.GenerateBiomes)
            {
                tasks.Add(microTask);
            }

            int genIndex = tasks.FindIndex(genpass => genpass.Name.Equals("Reset"));
            tasks.Insert(genIndex + 1, new PassLegacy("Erasing World", ResetWorldPass, 1f));
            //tasks.Add(oceanTask);
            //tasks.Insert(genIndex + 1, new PassLegacy("Generating Temple", NewTemplePass, 1f));
        }

        [Flags]
        enum TF : byte
        {
            /// <summary>Cloud</summary>
            C = 1,

            /// <summary>Shimmer</summary>
            S = C << 1,

            /// <summary>Platform</summary>
            P = S << 1
        }

        private void ResetWorldPass(GenerationProgress progress, GameConfiguration config)
        {
            var skyblockConfig = ModContent.GetInstance<OneSkyBlockConfig>();

            progress.Message = Language.GetTextValue("Mods.OneSkyBlock.WorldGen.PrepareChallenge");
            progress.Set(0f);
            Main.spawnTileX = Main.maxTilesX / 2;
            Main.spawnTileY = (Main.maxTilesY / 4) - 30;
            Main.worldSurface = Main.maxTilesY / 4;
            Main.rockLayer = Main.worldSurface + (Main.spawnTileY / 1.5);

            if (skyblockConfig.ShimmerChallenge)
            {
                var tiles = new TF[,]
                {
                    { TF.C, TF.S, TF.S, TF.S | TF.P, TF.S, TF.S, TF.C },
                    { TF.C, TF.S, TF.S,    TF.S,     TF.S, TF.S, TF.C },
                    { TF.C, TF.S, TF.S,    TF.S,     TF.S, TF.S, TF.C },
                    { 0,    TF.C, TF.C,    TF.C,     TF.C, TF.C, 0 },
                };

                var xLength = tiles.GetLength(1);
                var xOffset = xLength / 2;

                for (int y = 0; y < tiles.GetLength(0); y++)
                {
                    for (int x = 0; x < xLength; x++)
                    {
                        int x2 = Main.spawnTileX + x - xOffset;
                        int y2 = Main.spawnTileY + y;

                        var t = tiles[y, x];

                        if (t.HasFlag(TF.C))
                        {
                            WorldGen.PlaceTile(x2, y2, TileID.Cloud);
                        }

                        if (t.HasFlag(TF.S))
                        {
                            WorldGen.PlaceLiquid(x2, y2, (byte)LiquidID.Shimmer, byte.MaxValue);
                        }

                        if (t.HasFlag(TF.P))
                        {
                            WorldGen.PlaceTile(x2, y2, TileID.Platforms);
                        }
                    }
                }
            }
            else
            {
                WorldGen.PlaceTile(Main.spawnTileX, Main.spawnTileY, TileID.Cloud, false, false);
            }
            
            Thread.Sleep(500); // lol
            Random rand = new();
            int dungeonSide = Main.dungeonX < Main.maxTilesX / 2 ? -1 : 1;
            //int next = (int)(Main.GlobalTimeWrappedHourly);

            if (skyblockConfig.GenerateIslands)
            {
                progress.Message = $"{Language.GetTextValue("Mods.OneSkyBlock.WorldGen.GenerateIslands")} (1/2)";
                progress.Set(0.3f);
                var islandSuccess = false;
                var totalAttempts = 0;

                while (!islandSuccess && totalAttempts < 100000)
                {
                    List<int> randomX = new()
                    {
                        (int)(Main.spawnTileX / (1.11 + (rand.NextDouble() / 3))),
                        (int)(Main.spawnTileX / (2 + (rand.NextDouble() / 2))),
                        (int)(Main.spawnTileX * (1.1 + (rand.NextDouble() / 3))),
                        (int)(Main.spawnTileX * (1.5 + (rand.NextDouble() / 2)))
                    };
                    List<int> islandX = randomX.OrderBy(a => Guid.NewGuid()).ToList();

                    int islandY1 = (int)(Main.spawnTileY / (2 + (rand.NextDouble() * 2)));
                    int islandY2 = (int)(Main.spawnTileY / (2 + (rand.NextDouble() * 2)));
                    int islandY3 = (int)(Main.spawnTileY / (2 + (rand.NextDouble() * 2)));
                    int islandY4 = (int)(Main.spawnTileY / (2 + (rand.NextDouble() * 2)));

                    //static IEnumerable<int> AdjacentTiles(int i, int j)
                    //{
                    //    for (int x = i - 100; x <= i + 100; x++)
                    //        for (int y = j - 100; y <= j + 100; y++)
                    //            if (x != i || y != j && Framing.GetTileSafely(x, y).HasTile)
                    //                yield return Framing.GetTileSafely(x, y).TileType;
                    //}
                    foreach (var islandTask in islandX)
                    {
                        Console.WriteLine("Island: {0}", islandTask);
                    }
                    try
                    {
                        WorldGen.CloudIsland(islandX[0], islandY1);
                        //if (AdjacentTiles(islandX[0], islandY1).Contains(TileID.Dirt))
                        //{
                        //    WorldGen.PlaceTile(islandX[0], islandY1, TileID.Grass);
                        //    WorldGen.SpreadGrass(islandX[0], islandY1);
                        //}
                        WorldGen.IslandHouse(islandX[0], islandY1, 2);

                        WorldGen.DesertCloudIsland(islandX[1], islandY2);
                        WorldGen.IslandHouse(islandX[1], islandY2, 3);

                        WorldGen.CloudLake(islandX[2], islandY3);

                        WorldGen.SnowCloudIsland(islandX[3], islandY4);
                        WorldGen.IslandHouse(islandX[3], islandY4, 1);
                        islandSuccess = true;
                        progress.Message = $"{Language.GetTextValue("Mods.OneSkyBlock.WorldGen.GenerateIslands")} (2/2)";
                        Thread.Sleep(250);
                    }
                    catch (Exception)
                    {
                        islandSuccess = false;
                        totalAttempts++;
                        progress.Message = $"{Language.GetTextValue("Mods.OneSkyBlock.WorldGen.GenerateIslands")} (1/2) - {totalAttempts}";
                    }
                }

            }

            if (skyblockConfig.GenerateTemple)
            {
                progress.Message = Language.GetTextValue("Mods.OneSkyBlock.WorldGen.GenerateJungle");
                progress.Set(0.6f);
                //tasks.Add(jungleTask1);
                //tasks.Add(jungleTask2);
                //ModLoader.TryGetMod("CalamityMod", out Mod CalamityMod);

                var x = dungeonSide == 1 ? Main.maxTilesX / 5 : (int)(Main.maxTilesX / 1.25);
                var y = (int)Main.rockLayer;

                if (skyblockConfig.ShimmerChallenge)
                {
                    const int platformLength = 70;
                    const int halfPlatformLength = platformLength / 2;

                    x -= halfPlatformLength;

                    for (int i = x; i < x + platformLength; i++)
                    {
                        WorldGen.PlaceTile(i, y, TileID.LihzahrdBrick);

                        for (int j = y; j >= y - 1; j--)
                        {
                            WorldGen.PlaceWall(i, j - 1, WallID.LihzahrdBrickUnsafe);
                        }
                    }

                    --y;
                    WorldGen.Place3x2(x + halfPlatformLength, y, TileID.LihzahrdAltar);
                }
                else
                {
                    WorldGen.makeTemple(x, y);
                }
            }

            if (skyblockConfig.GenerateDungeon)
            {
                progress.Message = $"{Language.GetTextValue("Mods.OneSkyBlock.WorldGen.GenerateDungeon")} (1/2)";
                progress.Set(0.8f);
                Main.dungeonX = dungeonSide == 1 ? (int)(Main.maxTilesX / 1.25) : Main.maxTilesX / 5;
                Main.dungeonY = Main.spawnTileY + 30;
                WorldGen.PlaceTile(Main.dungeonX, Main.dungeonY, TileID.Stone);
                Thread.Sleep(250);
                
                var dungeonSuccess = false;
                
                while (!dungeonSuccess)
                {
                    try
                    {
                        if (skyblockConfig.ShimmerChallenge)
                        {
                            var variant = Main.rand.Next(3);

                            variant = variant switch
                            {
                                0 => TileID.BlueDungeonBrick,
                                1 => TileID.GreenDungeonBrick,
                                _ => TileID.PinkDungeonBrick,
                            };

                            var wallType = variant switch
                            {
                                TileID.BlueDungeonBrick => Main.rand.NextBool() ? WallID.BlueDungeonSlabUnsafe : WallID.BlueDungeonTileUnsafe,
                                TileID.GreenDungeonBrick => Main.rand.NextBool() ? WallID.GreenDungeonSlabUnsafe : WallID.GreenDungeonTileUnsafe,
                                TileID.PinkDungeonBrick => Main.rand.NextBool() ? WallID.PinkDungeonSlabUnsafe : WallID.PinkDungeonTileUnsafe,
                            };

                            for (int x = Main.dungeonX - 1; x <= Main.dungeonX + 1; x++)
                            {
                                for (int y = Main.dungeonY; y >= Main.dungeonY - 0; y--)
                                {
                                    WorldGen.PlaceTile(x, y, variant);
                                }
                            }

                            NPC.NewNPC(new EntitySource_WorldGen(), Main.dungeonX * 16 + 8, Main.dungeonY * 16, 37);

                            dungeonSuccess = true;
                            progress.Message = $"{Language.GetTextValue("Mods.OneSkyBlock.WorldGen.GenerateDungeon")} (2/2)";
                            Thread.Sleep(250);
                        }
                        else
                        {
                            WorldGen.MakeDungeon(Main.dungeonX, Main.dungeonY);
                            dungeonSuccess = true;
                            progress.Message = $"{Language.GetTextValue("Mods.OneSkyBlock.WorldGen.GenerateDungeon")} (2/2)";
                            Thread.Sleep(250);
                        }
                    }
                    catch (Exception)
                    {
                        Main.dungeonX = rand.Next(Main.maxTilesX);
                        Main.dungeonY = Main.spawnTileY + 30;
                        dungeonSuccess = false;
                        progress.Message = $"{Language.GetTextValue("Mods.OneSkyBlock.WorldGen.GenerateDungeon")} (1/2) - {Main.dungeonX}";
                    }
                }
            }

            Thread.Sleep(250);
            progress.Message = Language.GetTextValue("Mods.OneSkyBlock.WorldGen.FinishLoading");
            progress.Set(1f);
            Thread.Sleep(500);
        }
    }
}