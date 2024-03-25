using Microsoft.Xna.Framework;
using Terraria.ModLoader;

namespace OneSkyBlock.Commands;

internal class DebugTeleport : ModCommand
{
    public override CommandType Type => CommandType.World;

    public override string Command => "dt";

    public override void Action(CommandCaller caller, string input, string[] args)
    {
        var x = int.Parse(args[0]) * 16;
        var y = int.Parse(args[1]) * 16;

        caller.Player.Teleport(new Vector2(x, y));
    }

    public override bool IsLoadingEnabled(Mod mod)
    {
        return false;
    }
}
