using MPK.Connect.Model.Helpers;

namespace MPK.Connect.Model.Business
{
    public abstract class LocalizableEntity<T> : IdentifiableEntity<T>
    {
        public abstract double GetDistanceTo(LocalizableEntity<T> otherEntity);
    }
}