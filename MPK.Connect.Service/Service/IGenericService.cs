namespace MPK.Connect.Service.Service
{
    public interface IGenericService<T> where T : class
    {
        int ReadFromFile(string filePath);
    }
}