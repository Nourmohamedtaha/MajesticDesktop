namespace MajesticDesktop.Models
{
    public class User
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public int RoleId { get; set; }
        public string RoleName { get; set; } = string.Empty;
        public bool IsActive { get; set; }

        // null = سوبر أدمن (كل الفروع)، رقم = أدمن مربوط بفرع محدد
        public int? LocationId { get; set; }
        public string BranchName { get; set; } = string.Empty;
    }
}