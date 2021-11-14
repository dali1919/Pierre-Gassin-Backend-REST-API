using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace WebsiteRESTAPI.ResponseHelper
{
    public class APIResponseResult<T> where T : class
    {
        public T Entity { get; private set; }

    public HttpStatusCode Status { get; private set; }

    public Exception Exception { get; private set; }

    public ApiResonse Message { get; set; }


    public APIResponseResult(T entity, HttpStatusCode status)
    {
        Entity = entity;
        Status = status;
    }

    public APIResponseResult(T entity, HttpStatusCode status, Exception exception) : this(entity, status)
    {
        Exception = exception;
    }

    public APIResponseResult(T entity, HttpStatusCode status, ApiResonse msg) : this(entity, status)
    {
        Message = msg;
    }
}
    [Serializable]
    public class UnhandledRepositoryActionStatusException : Exception
    {
        public UnhandledRepositoryActionStatusException() { }
        public UnhandledRepositoryActionStatusException(string message) : base(message) { }
        public UnhandledRepositoryActionStatusException(string message, Exception inner) : base(message, inner) { }
        protected UnhandledRepositoryActionStatusException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
    public class ApiResonse
    {
        public bool success { get; set; }
        public string message { get; set; }
        public object data { get; set; }
    }
}
