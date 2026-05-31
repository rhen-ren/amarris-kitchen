namespace amarris_kitchen_backend.DTOs
{
    public class CreateAdminDTO
    {
        public string AdminName { get; set; }
        public string Email { get; set; }
        public string Role { get; set;  }
        public string Password { get; set; }
    }
}
