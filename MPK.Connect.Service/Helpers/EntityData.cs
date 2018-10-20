namespace MPK.Connect.Service.Helpers
{
    public class EntityData
    {
        private readonly string[] _entityStrings;

        public EntityData(string[] entityStrings)
        {
            _entityStrings = entityStrings;
        }

        public string this[int index]
        {
            get
            {
                if (index > _entityStrings.Length || index < 0)
                {
                    return null;
                }
                return _entityStrings[index];
            }
            set => _entityStrings[index] = value;
        }
    }
}