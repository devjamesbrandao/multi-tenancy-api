namespace MultiTenancy.Data.DTO
{
    public class Tenants
    {
        public Guid UserId { get; set; }
        public string DbProvider { get; set; }
        public string ConnectionString { get; set; }
    }
}