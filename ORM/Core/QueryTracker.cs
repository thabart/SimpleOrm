using System;
using System.Collections.Generic;

namespace ORM.Core
{
    public class QueryTracker : IQueryTracker
    {
        private readonly List<Action> _queries;

        public QueryTracker()
        {
            _queries = new List<Action>();
        }

        public void AddQuery(Action action)
        {
            _queries.Add(action);
        }

        public void ExecuteQueries()
        {
            foreach(var query in _queries)
            {
                query();
            }

            _queries.Clear();
        }
    }
}
