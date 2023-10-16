using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace 登录
{
    
        public class MongoDBBase
        {
            private readonly IMongoDatabase _database = null;
            public MongoDBBase(string connectionString, string databaseName)
            {
                var client = new MongoClient(connectionString);
                if (client != null)
                {
                    _database = client.GetDatabase(databaseName);
                }
            }

            #region SELECT
            /// <summary>
            /// 根据查询条件，获取数据
            /// </summary>
            /// <typeparam name="T"></typeparam>
            /// <param name="id"></param>
            /// <returns></returns>
            public List<T> GetList<T>(Expression<Func<T, bool>> conditions = null)
            {
                var collection = _database.GetCollection<T>(typeof(T).Name);
                if (conditions != null)
                {
                    return collection.Find(conditions).ToList();
                }
                return collection.Find(_ => true).ToList();
            }
            #endregion

            #region INSERT/// <summary>
            /// 插入多条数据，数据用list表示
            /// </summary>
            /// <typeparam name="T"></typeparam>
            /// <param name="list"></param>
            /// <returns></returns>
            public List<T> InsertMany<T>(List<T> list)
            {
                var collection = _database.GetCollection<T>(typeof(T).Name);
                collection.InsertMany(list);
                return list;
            }

       
        #endregion
    }
    
}
