using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OneSkyBlock.Commands;

internal class Place : ModCommand
{
    public override CommandType Type => CommandType.World;

    public override string Command => "p";

    public override void Action(CommandCaller caller, string input, string[] args)
    {
        var x = int.Parse(args[0]);
        var y = int.Parse(args[1]);

        WorldGen.Place3x2(x, y, (ushort)TileID.LihzahrdAltar);

        if (!WorldGen.TileEmpty(x, y))
        {
            Main.NewText($"{x} {y} {Main.tile[x, y].TileType}");
        }
    }

    public override bool IsLoadingEnabled(Mod mod)
    {
        return false;
    }
}
