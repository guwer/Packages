using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Azure.DocumentDB.Testing
{
    public class EnumerableQueryMock<T> : IDocumentQuery<T>, IOrderedQueryable<T>
        where T : class
    {
        private readonly bool bypassExpressions;

        public IQueryable<T> List;
        public Expression Expression => List.Expression;
        public Type ElementType => typeof(T);
        public IQueryProvider Provider => new QueryProviderMock<T>(this, bypassExpressions);
        public bool HasMoreResults { get; private set; } = true;

        public EnumerableQueryMock(EnumerableQuery<T> List, bool bypassExpressions = true)
        {
            this.List = List;
            this.bypassExpressions = bypassExpressions;
        }

        public IEnumerator<T> GetEnumerator()
        {
            return List.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Dispose()
        {
        }

        public Task<FeedResponse<TResult>> ExecuteNextAsync<TResult>(CancellationToken token = new CancellationToken())
        {
            var flags = BindingFlags.NonPublic | BindingFlags.Instance;
            var feed = Activator.CreateInstance(
                typeof(FeedResponse<T>),
                flags,
                null,
                new Object[] { List.Select(j => (T)j), 0, new NameValueCollection(), false, null },
                null) as FeedResponse<T>;
            HasMoreResults = false;

            return Task.FromResult(feed as FeedResponse<TResult>);
        }

        public Task<FeedResponse<dynamic>> ExecuteNextAsync(CancellationToken token = new CancellationToken())
        {
            throw new NotImplementedException();
        }
    }

    internal class QueryProviderMock<T> : IQueryProvider where T : class
    {
        private readonly EnumerableQueryMock<T> mockQuery;
        private readonly bool bypassExpressions;

        public QueryProviderMock(EnumerableQueryMock<T> mockQuery, bool byPassExpressions)
        {
            this.mockQuery = mockQuery;
            this.bypassExpressions = byPassExpressions;
        }

        public IQueryable CreateQuery(Expression expression)
        {
            throw new NotImplementedException();
        }

        public IQueryable<TElement> CreateQuery<TElement>(Expression expression)
        {
            if (!bypassExpressions)
            {
                mockQuery.List = mockQuery.List.Provider.CreateQuery<TElement>(expression) as IQueryable<T>;
            }

            return (IQueryable<TElement>)mockQuery;
        }

        public object Execute(Expression expression)
        {
            throw new NotImplementedException();
        }

        public TResult Execute<TResult>(Expression expression)
        {
            throw new NotImplementedException();
        }
    }
}