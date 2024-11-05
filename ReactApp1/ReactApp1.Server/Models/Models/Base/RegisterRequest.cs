namespace ReactApp1.Server.Models
{
    public class RegisterRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string Username { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public int EstablishmentId { get; set; }

        public string Establishment { get; set; }

        public int AddressId { get; set; }

        public string Phone { get; set; }
    }
}