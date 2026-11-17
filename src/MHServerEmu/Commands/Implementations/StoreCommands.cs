using MHServerEmu.Commands.Attributes;
using MHServerEmu.Core.Config;
using MHServerEmu.Core.Network;
using MHServerEmu.DatabaseAccess.Models;
using MHServerEmu.Games.Entities;
using MHServerEmu.Games.MTXStore;
using MHServerEmu.Games.Network;

namespace MHServerEmu.Commands.Implementations
{
    [CommandGroup("store")]
    [CommandGroupDescription("Commands for interacting with the in-game store.")]
    public class StoreCommands : CommandGroup
    {
        [Command("convertes")]
        [CommandDescription("Converts Eternity Splinters to the equivalent amount of Gs. Defaults to 100 Eternity Splinters if no value is specified.")]
        [CommandUsage("store convertes [amount]")]
        [CommandInvokerType(CommandInvokerType.Client)]
        public string ConvertES(string[] @params, NetClient client)
        {
            const int DefaultESAmount = 100;

            PlayerConnection playerConnection = (PlayerConnection)client;
            Player player = playerConnection.Player;

            int esAmount = DefaultESAmount;
            if (@params.Length > 0)
            {
                if (int.TryParse(@params[0], out esAmount) == false)
                    return $"'{@params[0]}' is not a valid Eternity Splinter amount.";

                // Floor to the nearest step value if needed
                int step = Math.Max(ConfigManager.Instance.GetConfig<MTXStoreConfig>().ESToGazillioniteConversionStep, 1);
                if (step > 1)
                    esAmount = esAmount / step * step;
            }

            int gAmount = player.ConvertEternitySplintersToGazillionite(esAmount);

            if (gAmount == 0)
                return "Failed to convert.";

            return $"Converted {esAmount} Eternity Splinters to {gAmount} Gs.";
        }

        private const string AddGDescription = "立即為目前登入的管理員帳號增加 G 幣。";

        [Command("addg")]
        [CommandDescription(AddGDescription)]
        [CommandUsage("store addg [數量]")]
        [CommandUserLevel(AccountUserLevel.Admin)]
        [CommandInvokerType(CommandInvokerType.Client)]
        public string AddG(string[] @params, NetClient client)
        {
            if (@params.Length != 1 || long.TryParse(@params[0], out long amount) == false || amount <= 0)
                return "請輸入正整數，例如 !store addg 5000。";

            PlayerConnection playerConnection = (PlayerConnection)client;
            Player player = playerConnection.Player;

            if (player.GazillioniteBalance > long.MaxValue - amount)
                return "增加後的 G 幣餘額會超出上限。";

            if (player.AcquireGazillionite(amount) == false)
                return "無法增加 G 幣。";

            return $"已增加 {amount:N0} G，目前餘額為 {player.GazillioniteBalance:N0} G。";
        }

        [Command("加G幣")]
        [CommandDescription(AddGDescription)]
        [CommandUsage("store 加G幣 [數量]")]
        [CommandUserLevel(AccountUserLevel.Admin)]
        [CommandInvokerType(CommandInvokerType.Client)]
        public string AddGTraditionalChinese(string[] @params, NetClient client)
        {
            return AddG(@params, client);
        }
    }
}
