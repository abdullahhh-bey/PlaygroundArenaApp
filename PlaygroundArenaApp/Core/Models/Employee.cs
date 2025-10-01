namespace PlaygroundArenaApp.Core.Models
{
    public class Employee
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty; 
        public decimal Email { get; set; }
        public string Position { get; set; } = string.Empty;
        public int Salary { get; set; }

    }
}
