using System;

namespace ORM.Core
{
    public interface IQueryTracker
    {
        void AddQuery(Action action);

        void ExecuteQueries();
    }
}
