namespace ReactApp1.Server.Models.Models.Base
{
    public class NavigationItem
    {
        public string Path { get; set; }
        public string Label { get; set; }
        public bool? IsDropdown { get; set; }
        public List<NavigationItem>? DropdownItems { get; set; }
    }
    
    public class NavigationModel
    {
        public List<NavigationItem> Items { get; set; } = new List<NavigationItem>();
    }
}