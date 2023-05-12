using BankCustomerSystem.Services.Core.Data.Contracts;
using BankCustomerSystem.Services.Core.Data.CustomModels;
using BankCustomerSystem.Services.Core.Data.Models.DbModels;
using RepoDb;
using System.Data;
using System.Reflection;


namespace BankCustomerSystem.Services.Core.Data.Repositories
{
    public abstract class GenericRepository<T, TDbConnection> : IGenericRepository<T, TDbConnection>
           where T : GenericModel
           where TDbConnection : IDbConnection
    {
#nullable disable
        #region Properties

        /// <summary>
        /// Gets the connection string.
        /// </summary>
        /// <value>
        /// The connection string.
        /// </value>
        public string ConnectionString { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="GenericRepository{T, TDbConnection}"/> class.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        public GenericRepository(string connectionString)
        {
            ConnectionString = connectionString;
        }

        #endregion

        #region Generic Methods

        /// <summary>
        /// Gets the connection.
        /// </summary>
        /// <returns>The connection object.</returns>
        public IDbConnection GetConnection()
        {
            TDbConnection connection = Activator.CreateInstance<TDbConnection>();
            connection.ConnectionString = ConnectionString;
            return connection;
        }

        /// <summary>
        /// Checks if the entity exists
        /// </summary>
        /// <param name="where">The where conditions.</param>
        /// <returns>bool result.</returns>
        public bool Exists(QueryField[] where)
        {
            bool exist;
            using (IDbConnection connection = GetConnection())
            {
                exist = connection.Exists<T>(where);
                CloseConnection(connection);
            }
            return exist;
        }

        /// <summary>
        /// Closes and disposes the connection.
        /// </summary>
        /// <param name="connection">The connection to close.</param>
        public void CloseConnection(IDbConnection connection)
        {
            connection.Close();
            connection.Dispose();
        }

        /// <summary>
        /// Gets the count.
        /// </summary>
        /// <param name="where">The where conditions.</param>
        /// <returns>The count</returns>
        public long GetCount(QueryField[] where)
        {
            long count = 0;
            using (IDbConnection connection = GetConnection())
            {
                count = connection.Count<T>(where);
                CloseConnection(connection);
            }
            return count;
        }

        public List<T> ExecuteStoredProcedure<TResponse>(object parameters, string procedureName, IDbTransaction txn = null)
        {
            IDbConnection connection = txn == null ? GetConnection() : txn.Connection;
            IEnumerable<T> result;
            result = connection.ExecuteQuery<T>(procedureName, parameters, CommandType.StoredProcedure, null, null);
            if (txn == null)
                CloseConnection(connection);

            return result != null ? result.ToList() : new List<T>();
        }

        #endregion

        #region BasicMethods

        /// <summary>
        /// Gets the entity by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="languageCode">The language code.</param>
        /// <returns>The entity.</returns>
        public T GetById(object id, string languageCode, IDbTransaction txn = null)
        {
            T entity = null;
            IDbConnection connection = txn == null ? GetConnection() : txn.Connection;

            entity = connection.Query<T>(id, transaction: txn).FirstOrDefault();

            if (txn == null)
                CloseConnection(connection);

            return entity;
        }

        /// <summary>
        /// Gets the entities by condition (skip, take, where, orderBy).
        /// </summary>
        /// <param name="properties">The grid properties.</param>
        /// <param name="languageCode">The language code.</param>
        /// <returns>The entities.</returns>
        /// <remarks>
        /// Send page size = 0 to get all records.
        /// Send null languageCode for default language.
        /// </remarks>
        public IEnumerable<T> GetByCondition(RepoDbQueryProperties properties, string languageCode, IDbTransaction txn = null)
        {
            IEnumerable<T> entities = null;
            IDbConnection connection = txn == null ? GetConnection() : txn.Connection;

            if (properties.Take == 0)
            {
                entities = connection.Query<T>(properties.Where, orderBy: properties.OrderBy, transaction: txn);
            }
            else
            {
                entities = connection.Query<T>(properties.Where, orderBy: properties.OrderBy, transaction: txn)
                                     .Skip(properties.Skip)
                                     .Take(properties.Take);
            }

            if (txn == null)
                CloseConnection(connection);

            return entities;
        }

        /// <summary>
        /// Gets all entities (skip, take, orderBy).
        /// </summary>
        /// <param name="properties">The grid properties.</param>
        /// <param name="languageCode">The language code.</param>
        /// <returns>The entities.</returns>
        /// <remarks>
        /// Send page size = 0 to get all records.
        /// Send null languageCode for default language.
        /// </remarks>
        public IEnumerable<T> GetAll(RepoDbQueryProperties properties, string languageCode)
        {
            IEnumerable<T> entities = null;
            if (properties == null)
            {
                properties = new RepoDbQueryProperties();
            }
            using (IDbConnection connection = GetConnection())
            {
                if (properties.Take == 0)
                {
                    entities = connection.QueryAll<T>(orderBy: properties.OrderBy);
                }
                else
                {
                    entities = connection.QueryAll<T>(orderBy: properties.OrderBy)
                                         .Skip(properties.Skip)
                                         .Take(properties.Take);
                }

                CloseConnection(connection);
            }
            return entities;
        }

        /// <summary>
        /// Gets specific entity fileds.
        /// </summary>
        /// <param name="where">The where condition.</param>
        /// <param name="fields">The fields to return.</param>
        /// <returns>The entities.</returns>
        public IEnumerable<T> GetFields(QueryField[] where, Field[] fields)
        {
            IEnumerable<T> entities = null;
            using (IDbConnection connection = GetConnection())
            {
                entities = connection.Query<T>(where: where, fields: fields);
                CloseConnection(connection);
            }
            return entities;
        }

        /// <summary>
        /// Inserts the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="userId">The user identifier.</param>
        /// <param name="txn">The transaction.</param>
        /// <returns>The entity identifier if inserted.</returns>
        public TReturn Insert<TReturn>(T entity, Guid? userId, IDbTransaction txn = null)
        {
            if (userId != null)
            {
                if (entity.GetType().GetProperty("CreatedBy") != null)
                    entity.GetType().GetProperty("CreatedBy").SetValue(entity, userId.Value);
                if (entity.GetType().GetProperty("CreatedDate") != null)
                    entity.GetType().GetProperty("CreatedDate").SetValue(entity, DateTime.UtcNow);
                if (entity.GetType().GetProperty("Guid") != null)
                    entity.GetType().GetProperty("Guid").SetValue(entity, Guid.NewGuid());
            }
            IDbConnection connection = txn == null ? GetConnection() : txn.Connection;
            TReturn result = connection.Insert<T, TReturn>(entity, transaction: txn);
            if (txn == null)
                CloseConnection(connection);

            return result;
        }

        /// <summary>
        /// Inserts entities.
        /// </summary>
        /// <param name="entities">The entities.</param>
        /// <param name="userId">The user identifier.</param>
        /// <param name="txn">The transaction.</param>
        /// <returns>The number of inserted entities.</returns>
        public int InsertAll(IEnumerable<T> entities, Guid? userId, IDbTransaction txn = null)
        {
            if (userId != null)
            {
                DateTime utcNow = DateTime.UtcNow;
                entities.All(x =>
                {
                    if (x.GetType().GetProperty("CreatedBy") != null)
                        x.GetType().GetProperty("CreatedBy").SetValue(x, userId.Value);
                    if (x.GetType().GetProperty("CreatedDate") != null)
                        x.GetType().GetProperty("CreatedDate").SetValue(x, utcNow);
                    if (x.GetType().GetProperty("Guid") != null)
                        x.GetType().GetProperty("Guid").SetValue(x, Guid.NewGuid());
                    return true;
                });
            }
            IDbConnection connection = txn == null ? GetConnection() : txn.Connection;
            int result = connection.InsertAll(entities, transaction: txn);
            if (txn == null)
                CloseConnection(connection);

            return result;
        }

        /// <summary>
        /// Updates the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="fields">The fields.</param>
        /// <param name="userId">The user identifier.</param>
        /// <param name="txn">The transaction.</param>
        /// <returns>The number of updated entities.</returns>
        /// <remarks>
        /// fields define the entity properties to update
        /// the entity is selected by identifier for update
        /// </remarks>
        public int Update(T entity, List<Field> fields, Guid? userId, IDbTransaction txn = null)
        {
            if (userId != null)
            {
                if (entity.GetType().GetProperty("ModifiedBy") != null)
                {
                    entity.GetType().GetProperty("ModifiedBy").SetValue(entity, userId);
                    fields.Add(new Field("ModifiedBy"));
                }
                if (entity.GetType().GetProperty("ModifiedDate") != null)
                {
                    entity.GetType().GetProperty("ModifiedDate").SetValue(entity, DateTime.UtcNow);
                    fields.Add(new Field("ModifiedDate"));
                }
            }
            IDbConnection connection = txn == null ? GetConnection() : txn.Connection;
            int result = connection.Update(entity: entity, fields: fields, transaction: txn);
            if (txn == null)
                CloseConnection(connection);

            return result;
        }

        /// <summary>
        /// Updates the entities by condition.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="fields">The fields.</param>
        /// <param name="where">The where condition.</param>
        /// <param name="userId">The user identifier.</param>
        /// <param name="txn">The transaction.</param>
        /// <returns>The number of entities updated.</returns>
        /// <remarks>
        /// fields define the entity properties to update
        /// where defines the condition to select for update
        /// entity contains the field values to update (other fields and identifier can be sent null)
        /// </remarks>
        public int UpdateByCondition(T entity, List<Field> fields, QueryField[] where, Guid? userId, IDbTransaction txn = null)
        {
            if (userId != null)
            {
                if (entity.GetType().GetProperty("ModifiedBy") != null)
                {
                    entity.GetType().GetProperty("ModifiedBy").SetValue(entity, userId);
                    fields.Add(new Field("ModifiedBy"));
                }
                if (entity.GetType().GetProperty("ModifiedDate") != null)
                {
                    entity.GetType().GetProperty("ModifiedDate").SetValue(entity, DateTime.UtcNow);
                    fields.Add(new Field("ModifiedDate"));
                }
            }
            IDbConnection connection = txn == null ? GetConnection() : txn.Connection;
            int result = connection.Update(entity, where, fields, transaction: txn);
            if (txn == null)
                CloseConnection(connection);

            return result;
        }

        /// <summary>
        /// Updates the specified entities.
        /// </summary>
        /// <param name="entities">The entities.</param>
        /// <param name="fields">The fields.</param>
        /// <param name="userId">The user identifier.</param>
        /// <param name="txn">The transaction.</param>
        /// <returns>The number of updated entities.</returns>
        /// <remarks>
        /// fields define the entity properties to update
        /// the entities are selected by identifier for update
        /// </remarks>
        public int UpdateAll(IEnumerable<T> entities, List<Field> fields, Guid? userId, IDbTransaction txn = null)
        {
            if (userId != null)
            {
                DateTime utcNow = DateTime.UtcNow;
                PropertyInfo modifiedBy = entities.FirstOrDefault().GetType().GetProperty("ModifiedBy");
                PropertyInfo modifiedDate = entities.FirstOrDefault().GetType().GetProperty("ModifiedDate");
                if (modifiedBy != null)
                    fields.Add(new Field("ModifiedBy"));
                if (modifiedDate != null)
                    fields.Add(new Field("ModifiedDate"));

                entities.All(x =>
                {
                    if (modifiedBy != null)
                        x.GetType().GetProperty("ModifiedBy").SetValue(x, userId);
                    if (modifiedDate != null)
                        x.GetType().GetProperty("ModifiedDate").SetValue(x, utcNow);
                    return true;
                });
            }
            IDbConnection connection = txn == null ? GetConnection() : txn.Connection;
            int result = connection.UpdateAll(entities: entities, fields: fields, transaction: txn);
            if (txn == null)
                CloseConnection(connection);

            return result;
        }

        /// <summary>
        /// Deletes the specified entity.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="txn">The transaction.</param>
        /// <returns>The number of deleted entities.</returns>
        public int Delete(object id, IDbTransaction txn = null)
        {
            IDbConnection connection = txn == null ? GetConnection() : txn.Connection;
            int result = connection.Delete<T>(id, transaction: txn);
            if (txn == null)
                CloseConnection(connection);

            return result;
        }

        /// <summary>
        /// Deletes all entities.
        /// </summary>
        /// <param name="entities">The entities.</param>
        /// <param name="txn">The transaction.</param>
        /// <returns>The number of deleted entities.</returns>
        public int DeleteAll(IEnumerable<T> entities, IDbTransaction txn = null)
        {
            IDbConnection connection = txn == null ? GetConnection() : txn.Connection;
            int result = connection.DeleteAll(entities, transaction: txn);
            if (txn == null)
                CloseConnection(connection);

            return result;
        }

        /// <summary>
        /// Deletes the specified entities.
        /// </summary>
        /// <param name="where">The where condition.</param>
        /// <param name="txn">The transaction.</param>
        /// <returns>The number of deleted entities.</returns>
        public int DeleteByCondition(QueryField[] where, IDbTransaction txn = null)
        {
            IDbConnection connection = txn == null ? GetConnection() : txn.Connection;
            int result = connection.Delete<T>(where, transaction: txn);
            if (txn == null)
                CloseConnection(connection);

            return result;
        }

        #endregion

        #region Async Methods

        /// <summary>
        /// Gets the entity by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="languageCode">The language code.</param>
        /// <returns>The entity.</returns>
        public async Task<T> GetByIdAsync(object id, string languageCode, IDbTransaction txn = null)
        {
            IDbConnection connection = txn == null ? GetConnection() : txn.Connection;
            IEnumerable<T> queryResult = await connection.QueryAsync<T>(id, transaction: txn);
            T entity = queryResult.FirstOrDefault();

            if (txn == null)
                CloseConnection(connection);

            return entity;
        }

        /// <summary>
        /// Gets the entities by condition (skip, take, where, orderBy).
        /// </summary>
        /// <param name="properties">The grid properties.</param>
        /// <param name="languageCode">The language code.</param>
        /// <returns>The entities.</returns>
        /// <remarks>
        /// Send page size = 0 to get all records.
        /// Send null languageCode for default language.
        /// </remarks>
        public async Task<IEnumerable<T>> GetByConditionAsync(RepoDbQueryProperties properties, string languageCode, IDbTransaction txn = null)
        {
            IEnumerable<T> entities = null;
            IDbConnection connection = txn == null ? GetConnection() : txn.Connection;
            if (properties.Take == 0)
            {
                entities = await connection.QueryAsync<T>(properties.Where, orderBy: properties.OrderBy, transaction: txn);
            }
            else
            {
                entities = (await connection.QueryAsync<T>(properties.Where, orderBy: properties.OrderBy, transaction: txn))
                                            .Skip(properties.Skip)
                                            .Take(properties.Take);
            }

            if (txn == null)
                CloseConnection(connection);

            return entities;
        }

        /// <summary>
        /// Gets all entities (skip, take, orderBy).
        /// </summary>
        /// <param name="properties">The grid properties.</param>
        /// <param name="languageCode">The language code.</param>
        /// <returns>The entities.</returns>
        /// <remarks>
        /// Send page size = 0 to get all records.
        /// Send null languageCode for default language.
        /// </remarks>
        public async Task<IEnumerable<T>> GetAllAsync(RepoDbQueryProperties properties, string languageCode)
        {
            IEnumerable<T> entities = null;
            if (properties == null)
            {
                properties = new RepoDbQueryProperties();
            }
            using (IDbConnection connection = GetConnection())
            {
                if (properties.Take == 0)
                {
                    entities = await connection.QueryAllAsync<T>(orderBy: properties.OrderBy);
                }
                else
                {
                    entities = (await connection.QueryAllAsync<T>(orderBy: properties.OrderBy))
                                                .Skip(properties.Skip)
                                                .Take(properties.Take);
                }
                CloseConnection(connection);
            }
            return entities;
        }

        /// <summary>
        /// Inserts the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="userId">The user identifier.</param>
        /// <param name="txn">The transaction.</param>
        /// <returns>The entity identifier if inserted.</returns>
        public async Task<TReturn> InsertAsync<TReturn>(T entity, Guid? userId, IDbTransaction txn = null)
        {
            if (userId != null)
            {
                if (entity.GetType().GetProperty("CreatedBy") != null)
                    entity.GetType().GetProperty("CreatedBy").SetValue(entity, userId.Value);
                if (entity.GetType().GetProperty("CreatedDate") != null)
                    entity.GetType().GetProperty("CreatedDate").SetValue(entity, DateTime.UtcNow);
                if (entity.GetType().GetProperty("Guid") != null)
                    entity.GetType().GetProperty("Guid").SetValue(entity, Guid.NewGuid());
            }
            IDbConnection connection = txn == null ? GetConnection() : txn.Connection;
            TReturn result = await connection.InsertAsync<T, TReturn>(entity, transaction: txn);
            if (txn == null)
                CloseConnection(connection);

            return result;
        }

        /// <summary>
        /// Inserts entities.
        /// </summary>
        /// <param name="entities">The entities.</param>
        /// <param name="userId">The user identifier.</param>
        /// <param name="txn">The transaction.</param>
        /// <returns>The number of inserted entities.</returns>
        public async Task<int> InsertAllAsync(IEnumerable<T> entities, Guid? userId, IDbTransaction txn = null)
        {
            if (userId != null)
            {
                DateTime utcNow = DateTime.UtcNow;
                entities.All(x =>
                {
                    if (x.GetType().GetProperty("CreatedBy") != null)
                        x.GetType().GetProperty("CreatedBy").SetValue(x, userId.Value);
                    if (x.GetType().GetProperty("CreatedDate") != null)
                        x.GetType().GetProperty("CreatedDate").SetValue(x, utcNow);
                    if (x.GetType().GetProperty("Guid") != null)
                        x.GetType().GetProperty("Guid").SetValue(x, Guid.NewGuid());
                    return true;
                });
            }
            IDbConnection connection = txn == null ? GetConnection() : txn.Connection;
            int result = await connection.InsertAllAsync(entities, transaction: txn);
            if (txn == null)
                CloseConnection(connection);

            return result;
        }

        /// <summary>
        /// Updates the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="fields">The fields.</param>
        /// <param name="userId">The user identifier.</param>
        /// <param name="txn">The transaction.</param>
        /// <returns>The number of updated entities.</returns>
        /// <remarks>
        /// fields define the entity properties to update
        /// the entity is selected by identifier for update
        /// </remarks>
        public async Task<int> UpdateAsync(T entity, List<Field> fields, Guid? userId, IDbTransaction txn = null)
        {
            if (userId != null)
            {
                if (entity.GetType().GetProperty("ModifiedBy") != null)
                {
                    entity.GetType().GetProperty("ModifiedBy").SetValue(entity, userId);
                    fields.Add(new Field("ModifiedBy"));
                }
                if (entity.GetType().GetProperty("ModifiedDate") != null)
                {
                    entity.GetType().GetProperty("ModifiedDate").SetValue(entity, DateTime.UtcNow);
                    fields.Add(new Field("ModifiedDate"));
                }
            }
            IDbConnection connection = txn == null ? GetConnection() : txn.Connection;
            int result = await connection.UpdateAsync(entity: entity, fields: fields, transaction: txn);
            if (txn == null)
                CloseConnection(connection);

            return result;
        }

        /// <summary>
        /// Updates the entities by condition.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="fields">The fields.</param>
        /// <param name="where">The where condition.</param>
        /// <param name="userId">The user identifier.</param>
        /// <param name="txn">The transaction.</param>
        /// <returns>The number of entities updated.</returns>
        /// <remarks>
        /// fields define the entity properties to update
        /// where defines the condition to select for update
        /// entity contains the field values to update (other fields and identifier can be sent null)
        /// </remarks>
        public async Task<int> UpdateByConditionAsync(T entity, List<Field> fields, QueryField[] where, Guid? userId, IDbTransaction txn = null)
        {
            if (userId != null)
            {
                if (entity.GetType().GetProperty("ModifiedBy") != null)
                {
                    entity.GetType().GetProperty("ModifiedBy").SetValue(entity, userId);
                    fields.Add(new Field("ModifiedBy"));
                }
                if (entity.GetType().GetProperty("ModifiedDate") != null)
                {
                    entity.GetType().GetProperty("ModifiedDate").SetValue(entity, DateTime.UtcNow);
                    fields.Add(new Field("ModifiedDate"));
                }
            }
            IDbConnection connection = txn == null ? GetConnection() : txn.Connection;
            int result = await connection.UpdateAsync(entity, where, fields, transaction: txn);
            if (txn == null)
                CloseConnection(connection);

            return result;
        }

        /// <summary>
        /// Updates the specified entities.
        /// </summary>
        /// <param name="entities">The entities.</param>
        /// <param name="fields">The fields.</param>
        /// <param name="userId">The user identifier.</param>
        /// <param name="txn">The transaction.</param>
        /// <returns>The number of updated entities.</returns>
        /// <remarks>
        /// fields define the entity properties to update
        /// the entities are selected by identifier for update
        /// </remarks>
        public async Task<int> UpdateAllAsync(IEnumerable<T> entities, List<Field> fields, Guid? userId, IDbTransaction txn = null)
        {
            if (userId != null)
            {
                DateTime utcNow = DateTime.UtcNow;
                PropertyInfo modifiedBy = entities.FirstOrDefault().GetType().GetProperty("ModifiedBy");
                PropertyInfo modifiedDate = entities.FirstOrDefault().GetType().GetProperty("ModifiedDate");
                if (modifiedBy != null)
                    fields.Add(new Field("ModifiedBy"));
                if (modifiedDate != null)
                    fields.Add(new Field("ModifiedDate"));

                entities.All(x =>
                {
                    if (modifiedBy != null)
                        x.GetType().GetProperty("ModifiedBy").SetValue(x, userId);
                    if (modifiedDate != null)
                        x.GetType().GetProperty("ModifiedDate").SetValue(x, utcNow);
                    return true;
                });
            }
            IDbConnection connection = txn == null ? GetConnection() : txn.Connection;
            int result = await connection.UpdateAllAsync(entities: entities, fields: fields, transaction: txn);
            if (txn == null)
                CloseConnection(connection);

            return result;
        }

        /// <summary>
        /// Deletes the specified entity.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="txn">The transaction.</param>
        /// <returns>The number of deleted entities.</returns>
        public async Task<int> DeleteAsync(object id, IDbTransaction txn = null)
        {
            IDbConnection connection = txn == null ? GetConnection() : txn.Connection;
            int result = await connection.DeleteAsync<T>(id, transaction: txn);
            if (txn == null)
                CloseConnection(connection);

            return result;
        }

        /// <summary>
        /// Deletes all entities.
        /// </summary>
        /// <param name="entities">The entities.</param>
        /// <param name="txn">The transaction.</param>
        /// <returns>The number of deleted entities.</returns>
        public async Task<int> DeleteAllAsync(IEnumerable<T> entities, IDbTransaction txn = null)
        {
            IDbConnection connection = txn == null ? GetConnection() : txn.Connection;
            int result = await connection.DeleteAllAsync(entities, transaction: txn);
            if (txn == null)
                CloseConnection(connection);

            return result;
        }

        /// <summary>
        /// Deletes the specified entities.
        /// </summary>
        /// <param name="where">The where condition.</param>
        /// <param name="txn">The transaction.</param>
        /// <returns>The number of deleted entities.</returns>
        public async Task<int> DeleteByConditionAsync(QueryField[] where, IDbTransaction txn = null)
        {
            IDbConnection connection = txn == null ? GetConnection() : txn.Connection;
            int result = await connection.DeleteAsync<T>(where, transaction: txn);
            if (txn == null)
                CloseConnection(connection);

            return result;
        }

        #endregion

    }
}