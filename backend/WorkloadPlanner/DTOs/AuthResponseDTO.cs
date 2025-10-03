namespace WorkloadPlanner.DTOs
{
    public class AuthResponseDTO
    {
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Token { get; set; }
        public DateTime ExpiresAt { get; set; }
    }
}
