namespace SMM.Models.DTO
{
    public class ChangePasswordRequestDTO
    {
        public string Email { get; set; }
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
    }
}
