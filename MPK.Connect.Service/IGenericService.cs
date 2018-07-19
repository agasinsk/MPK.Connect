namespace MPK.Connect.Service
{
    public interface IGenericService<T> where T : class
    {
        int ReadFromFile(string filePath);
    }
}