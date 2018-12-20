namespace MPK.Connect.Service.Helpers
{
    public static class StringExtensions
    {
        public static EntityData ToEntityData(this string dataString, char splitCharacter)
        {
            var data = dataString.Split(splitCharacter);
            return new EntityData(data);
        }

        public static EntityData ToEntityData(this string dataString)
        {
            var data = dataString.Split(',');
            return new EntityData(data);
        }

        public static string TrimToLower(this string source)
        {
            return source.Trim().ToLower();
        }
    }
}