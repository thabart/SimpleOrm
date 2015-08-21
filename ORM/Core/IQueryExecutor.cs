namespace ORM.Core
{
    public interface IQueryExecutor
    {
        void ExecuteSelect(string sqlScript);
    }
}
