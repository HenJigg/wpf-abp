using CommunityToolkit.Mvvm.ComponentModel; 

namespace AppFramework.Shared.Models
{
    [INotifyPropertyChanged]
    public partial class EntityObject 
    {
        public int Id { get; set; }

        public EntityObject()
        { }

        public EntityObject(int id) => Id = id;
    }
}