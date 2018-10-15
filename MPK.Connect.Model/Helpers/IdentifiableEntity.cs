namespace MPK.Connect.Model.Helpers
{
    public abstract class IdentifiableEntity<T>
    {
        public virtual T Id { get; set; }
    }
}