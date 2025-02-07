namespace AppFramework.Authorization.Permissions.Dto
{
    public class FlatPermissionDto
    {
        public string ParentName { get; set; }
        
        public string Name { get; set; }
        
        public string DisplayName { get; set; }
        
        public string Description { get; set; }
        
        public bool IsGrantedByDefault { get; set; }
    }
}