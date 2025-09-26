namespace ECPAPI.Models
{
    public class UserDTO
    {
        public int Id { get; set; }
        public string UserNameOrEmail { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string PhoneNumber { get; set; }
        public bool IsActive { get; set; }
        public int UserTypeId { get; set; }
    }
}
