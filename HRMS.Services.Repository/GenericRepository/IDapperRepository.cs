using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Services.Repository.GenericRepository
{
    public interface IDapperRepository<TEntity>: IDisposable
    {
        DbConnection GetDbconnection();
        T Get<T>(string sp, TEntity entity);
        List<T> GetAll<T>(string sp, TEntity entity);
        T Execute<T>(string sp,TEntity entity);
      
    }
}
