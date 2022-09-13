namespace OfficeManager.Domain.Entities
{
    public class RefreshToken : BaseAuditableEntity
    {
        public int UserId { get; set; }
        public string Code { get; set; }
        public DateTime Expiration { get; set; }
    }
}
