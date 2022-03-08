using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace HRMS.API.Helpers
{
    public abstract class GenericResponseModel
    {
        public HttpStatusCode StatusCode { get; set; }
        public string Message { get; set; }
        protected GenericResponseModel(HttpStatusCode statusCode, string messgae)
        {
            StatusCode = statusCode;
            Message = messgae;
        }

    }

    public class ResponseEntityList<TEntity> : GenericResponseModel where TEntity : class
    {
        public List<TEntity> ResponseData { get; set; }
        public ResponseEntityList(HttpStatusCode statusCode, string message, List<TEntity> entities) : base(statusCode, message)
        {
            ResponseData = entities;
        }

        public ResponseEntityList<TEntity> GetResponseEntityList()
        {
            return new ResponseEntityList<TEntity>(StatusCode, Message, ResponseData);
        }
    }

    public class ResponseEntity<TEntity> : GenericResponseModel where TEntity : class
    {
        public TEntity Entity { get; set; }

        public ResponseEntity(HttpStatusCode statusCode, string message, TEntity entity) : base(statusCode, message)
        {
            Entity = entity;
        }

        public ResponseEntity<TEntity> GetResponseEntity()
        {
            return new ResponseEntity<TEntity>(this.StatusCode, this.Message, this.Entity);
        }
    }
}
