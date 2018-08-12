using Microsoft.Azure.Documents;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;

namespace Azure.DocumentDB.Testing
{
    public static class DocumentClientExceptionFactory
    {
        public static DocumentClientException Create(Error error, HttpStatusCode httpStatusCode)
        {
            var type = typeof(DocumentClientException);

            var documentClientExceptionInstance = type.Assembly.CreateInstance(type.FullName,
                false, BindingFlags.Instance | BindingFlags.NonPublic, null,
                new object[] { error, default(HttpResponseHeaders), httpStatusCode }, null, null);

            return (DocumentClientException)documentClientExceptionInstance;
        }
    }
}