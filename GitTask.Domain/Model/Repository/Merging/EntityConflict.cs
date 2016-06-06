namespace GitTask.Domain.Model.Repository.Merging
{
    public class EntityConflict<T>
    {
        public T AncestorValue { get; set; }
        public T OurValue { get; set; }
        public T TheirValue { get; set; }
    }
}
