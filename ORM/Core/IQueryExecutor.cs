using System.Collections.Generic;

namespace ORM.Core
{
    public interface IQueryExecutor
    {
        object ExecuteText(string sqlScript);

        List<TResult> ExecuteText<TResult>(string sqlScript);
    }
}
