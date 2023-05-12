using RepoDb;
using BankCustomerSystem.Services.Finance.Data.CustomModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankCustomerSystem.Services.Finance.Data.Contracts
{
    public interface IGenericRepository<T, TDbConnection>
    {
#nullable disable
        int Delete(object id, IDbTransaction txn = null);
        int DeleteAll(IEnumerable<T> entities, IDbTransaction txn = null);
        Task<int> DeleteAllAsync(IEnumerable<T> entities, IDbTransaction txn = null);
        Task<int> DeleteAsync(object id, IDbTransaction txn = null);
        int DeleteByCondition(QueryField[] where, IDbTransaction txn = null);
        Task<int> DeleteByConditionAsync(QueryField[] where, IDbTransaction txn = null);
        List<T> ExecuteStoredProcedure<TResponse>(object parameters, string procedureName, IDbTransaction txn = null);
        bool Exists(QueryField[] where);
        IEnumerable<T> GetAll(RepoDbQueryProperties properties, string languageCode);
        Task<IEnumerable<T>> GetAllAsync(RepoDbQueryProperties properties, string languageCode);
        IEnumerable<T> GetByCondition(RepoDbQueryProperties properties, string languageCode, IDbTransaction txn = null);
        Task<IEnumerable<T>> GetByConditionAsync(RepoDbQueryProperties properties, string languageCode, IDbTransaction txn = null);
        T GetById(object id, string languageCode, IDbTransaction txn = null);
        Task<T> GetByIdAsync(object id, string languageCode, IDbTransaction txn = null);
        IDbConnection GetConnection();
        long GetCount(QueryField[] where);
        IEnumerable<T> GetFields(QueryField[] where, Field[] fields);
        TReturn Insert<TReturn>(T entity, Guid? userId, IDbTransaction txn = null);
        int InsertAll(IEnumerable<T> entities, Guid? userId, IDbTransaction txn = null);
        Task<int> InsertAllAsync(IEnumerable<T> entities, Guid? userId, IDbTransaction txn = null);
        Task<TReturn> InsertAsync<TReturn>(T entity, Guid? userId, IDbTransaction txn = null);
        int Update(T entity, List<Field> fields, Guid? userId, IDbTransaction txn = null);
        int UpdateAll(IEnumerable<T> entities, List<Field> fields, Guid? userId, IDbTransaction txn = null);
        Task<int> UpdateAllAsync(IEnumerable<T> entities, List<Field> fields, Guid? userId, IDbTransaction txn = null);
        Task<int> UpdateAsync(T entity, List<Field> fields, Guid? userId, IDbTransaction txn = null);
        int UpdateByCondition(T entity, List<Field> fields, QueryField[] where, Guid? userId, IDbTransaction txn = null);
        Task<int> UpdateByConditionAsync(T entity, List<Field> fields, QueryField[] where, Guid? userId, IDbTransaction txn = null);
    }
}
