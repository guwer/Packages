using Microsoft.Azure.Documents.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Azure.DocumentDB.Extensions
{
    public static class DocumentQueryExtensions
    {
        public static async Task<List<T>> ToListAsync<T>(this IDocumentQuery<T> queryable)
        {
            var list = new List<T>();
            while (queryable.HasMoreResults)
            {
                var response = await queryable.ExecuteNextAsync<T>();
                list.AddRange(response);
            }
            return list;
        }

        public static async Task<List<T>> ToListAsync<T>(this IQueryable<T> query)
        {
            return await query.AsDocumentQuery().ToListAsync();
        }
    }
}