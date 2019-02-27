using MPK.Connect.Model.Helpers;

namespace MPK.Connect.Model.Business
{
    public abstract class LocalizableEntity<T> : Identifiable<T>
    {
        public abstract double GetDistanceTo(LocalizableEntity<T> otherEntity);

        public abstract double GetDistanceTo(double latitude, double longitude);
    }
}