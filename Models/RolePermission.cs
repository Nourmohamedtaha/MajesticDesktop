namespace MajesticDesktop.Models
{
    public class RolePermission
    {
        public int RoleId { get; set; }
        public string ScreenName { get; set; }
        public bool CanAccess { get; set; }
    }
}