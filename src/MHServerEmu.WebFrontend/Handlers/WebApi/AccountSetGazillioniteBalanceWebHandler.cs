using System.Globalization;
using System.Net;
using System.Text.Json;
using MHServerEmu.Core.Network;
using MHServerEmu.Core.Network.Web;
using MHServerEmu.DatabaseAccess;
using MHServerEmu.WebFrontend.Models;
using MHServerEmu.WebFrontend.Network;

namespace MHServerEmu.WebFrontend.Handlers.WebApi
{
    internal class AccountSetGazillioniteBalanceWebHandler(bool localDashboard = false) : WebHandler
    {
        public override WebApiAccessType Access => localDashboard
            ? WebApiAccessType.None
            : WebApiAccessType.GazillioniteManagement;

        protected override async Task Get(WebRequestContext context)
        {
            if (!localDashboard)
            {
                await base.Get(context);
                return;
            }

            if (AuthorizeLocalDashboard(context) == false)
                return;

            await context.SendJsonAsync(IDBManager.Instance.GetAccountIdentities());
        }

        protected override async Task Post(WebRequestContext context)
        {
            if (localDashboard && AuthorizeLocalDashboard(context) == false)
                return;

            BalanceRequest request;
            try
            {
                request = await context.ReadJsonAsync<BalanceRequest>();
            }
            catch (JsonException)
            {
                await context.SendJsonAsync(new AccountOperationResponse(AccountOperationResponse.GenericFailure));
                return;
            }

            string email = request.Email?.Trim();
            if (string.IsNullOrWhiteSpace(email) || email.Length > 320
                || long.TryParse(request.Balance, NumberStyles.None, CultureInfo.InvariantCulture, out long balance) == false)
            {
                await context.SendJsonAsync(new AccountOperationResponse(AccountOperationResponse.GenericFailure));
                return;
            }

            var response = await GameServiceTaskManager.Instance.DoAccountOperationAsync(
                AccountOperation.SetGazillioniteBalance, email, gazillioniteBalance: balance);
            await context.SendJsonAsync(new AccountOperationResponse(response.ResultCode));
        }

        private static bool AuthorizeLocalDashboard(WebRequestContext context)
        {
            string origin = context.GetHeader("Origin");
            // Browser requests from another origin cannot set this header without preflight.
            if (context.IsDirectLoopback
                && context.GetHeader("X-MHServerEmu-Dashboard") == "1"
                && (origin == null || context.HasSameOrigin(origin)))
                return true;

            context.StatusCode = (int)HttpStatusCode.Forbidden;
            return false;
        }

        private readonly struct BalanceRequest
        {
            public string Email { get; init; }
            public string Balance { get; init; }
        }
    }
}
