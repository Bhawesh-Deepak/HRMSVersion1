using Dapper;
using HRMS.Services.Implementation.SqlConstant;
using HRMS.Services.Repository.GenericRepository;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Reflection;

namespace HRMS.Services.Implementation.GenericImplementation
{
    public class DapperImplementation<TEntity> : IDapperRepository<TEntity>
    {
        private readonly IConfiguration _config;
        private readonly string _connectionString;

        public DapperImplementation(IConfiguration config)
        {
            _config = config;
            _connectionString = _config.GetSection("ConnectionStrings:DefaultConnection").Value;
        }
        public void Dispose()
        {

        }
        public T Execute<T>(string sp, TEntity entity)
        {
            try
            {
                T result;

                using IDbConnection db = new SqlConnection(_connectionString);

                if (db.State == ConnectionState.Closed)
                    db.Open();

                var query = GetQueryString(sp);

                var parms = ConvertObjectToDBParameter<TEntity>(entity);
                result = db.Query<T>(query, parms, commandType: CommandType.StoredProcedure, transaction: null).FirstOrDefault();

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        public T Get<T>(string sp,TEntity entity)
        {
            var parms = ConvertObjectToDBParameter<TEntity>(entity);
            using IDbConnection db = new SqlConnection(_connectionString);
            return db.Query<T>(sp, parms, commandType: CommandType.StoredProcedure).FirstOrDefault();
        }
        public List<T> GetAll<T>(string sp, TEntity entity)
        {
            //var spName= SqlQuery.
            using IDbConnection db = new SqlConnection(_connectionString);
            var parms = ConvertObjectToDBParameter<TEntity>(entity);
            return db.Query<T>(sp, parms, commandType: CommandType.StoredProcedure).ToList();
        }
        public DbConnection GetDbconnection()
        {
            return new SqlConnection(_config.GetConnectionString(_connectionString));
        }
        public DynamicParameters ConvertObjectToDBParameter<T>(T entity)
        {
            Type t = entity.GetType();
            var paramData = new DynamicParameters();
            foreach (var entityProp in t.GetProperties())
            {
                paramData.Add($"@{entityProp.Name}", entityProp.GetValue(entity, null), GetDbType(entityProp.PropertyType));
            }
            return paramData;
        }
        public static DbType GetDbType(Type runtimeType)
        {
            var nonNullableType = Nullable.GetUnderlyingType(runtimeType);

            if (nonNullableType != null)
            {
                runtimeType = nonNullableType;
            }

            var templateValue = (Object)null;
            if (runtimeType.IsClass == false)
            {
                templateValue = Activator.CreateInstance(runtimeType);
            }

            var sqlParamter = new SqlParameter(parameterName: String.Empty, value: templateValue);

            return sqlParamter.DbType;
        }
        public string GetQueryString(string spName)
        {
            string query = string.Empty;

            Type type = typeof(SqlQuery);

            query = type.GetField(spName).GetValue(type).ToString();

            return query;
        }

    }
}
