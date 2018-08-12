using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Azure.DocumentDB.Testing
{
    public static class EnumerableQueryMockFactory
    {
        public static EnumerableQueryMock<T> Create<T>(IEnumerable<T> collection, bool bypasssExpressions = true)
            where T : class
        {
            return new EnumerableQueryMock<T>(new EnumerableQuery<T>(collection), bypasssExpressions);
        }
    }
}