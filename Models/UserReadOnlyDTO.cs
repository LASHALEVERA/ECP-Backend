namespace ECPAPI.Models
{
    public class UserReadOnlyDTO
    {
        public int Id { get; set; }
        public string UserNameOrEmail { get; set; }
        public bool IsActive { get; set; }
        public int UserTypeId { get; set; }
    }
}
