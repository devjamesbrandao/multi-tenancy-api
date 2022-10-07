using Microsoft.EntityFrameworkCore;

namespace MultiTenancy.Api.Extensions
{
    public class DbHealthChecker
    {
        public static async Task TestConnection(DbContext context)
        {
            var maxAttemps = 10;

            var delay = 5000;

            for (int i = 0; i < maxAttemps; i++)
            {
                var canConnect = CanConnect(context);

                if (canConnect) return;

                await Task.Delay(delay);
            }

            throw new Exception("Error wating database. Check ConnectionString and ensure database exist");
        }

        private static bool CanConnect(DbContext context)
        {
            try
            {
                context.Database.CanConnect();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}