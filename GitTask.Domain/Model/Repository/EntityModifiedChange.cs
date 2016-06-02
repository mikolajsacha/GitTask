namespace GitTask.Domain.Model.Repository
{
    public class EntityModifiedChange : EntityChange
    {
        public string PropertyName { get; set; }
        public object PropertyValueAfterChange { get; set; }
    }
}
