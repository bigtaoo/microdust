using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace ET.Server
{
    [EntitySystemOf(typeof(MicroDustDatabaseComponent))]
    [FriendOf(typeof(MicroDustDatabaseComponent))]
    public static partial class MicroDustDatabaseSystem
    {
        [EntitySystem]
        private static void Awake(this MicroDustDatabaseComponent self, string dbConnection, string dbName)
        {
            self.mongoClient = new MongoClient(dbConnection);
            self.database = self.mongoClient.GetDatabase(dbName);
        }

        public static async ETTask SaveWithComponents<T>(this MicroDustDatabaseComponent self, T entity, string collection) where T : Entity
        {
            Fiber fiber = self.Fiber();
            if (entity == null)
            {
                Log.Error($"save entity is null: {typeof(T).FullName}");
                return;
            }

            var dbObject = new MicroDustDBObject
            {
                Self = entity,
            };
            if (entity is MicroDustPlayerComponent unit)
            {
                dbObject.PlayerId = unit.UserId;
            }
            foreach (var component in entity.Components.Values)
            {
                dbObject.Entities.Add(component);
            }

            using (await self.Root().GetComponent<CoroutineLockComponent>().Wait(CoroutineLockType.DB, entity.Id % MicroDustDatabaseComponent.TaskCount))
            {
                await self.GetCollection(collection).ReplaceOneAsync(d => d.Id == entity.Id, dbObject, new ReplaceOptions { IsUpsert = true });
            }
        }

        public static async ETTask<T> QueryWithComponents<T>(this MicroDustDatabaseComponent self, Scene scene, Expression<Func<MicroDustDBObject, bool>> filter, string collection) where T : Entity
        {
            List<MicroDustDBObject> result;
            using (await self.Root().GetComponent<CoroutineLockComponent>().Wait(CoroutineLockType.DB, RandomGenerator.RandInt64() % DBComponent.TaskCount))
            {
                var cursor = await self.GetCollection<MicroDustDBObject>(collection).FindAsync(filter);
                result = await cursor.ToListAsync();
            }
            Log.Debug($"Database: query result type: {result.GetType()}");
            if (result.FirstOrDefault() is not MicroDustDBObject obj)
            {
                Log.Debug("Database, query with components is not MicroDustDBObject");
                return null;
            }

            var r = obj.Self as T;
            scene.RemoveComponent(r.GetType());
            scene.AddComponent(r);
            foreach (var c in obj.Entities)
            {
                Log.Debug($"Database, add component: {c.ToJson()}");
                r.AddComponent(c);
            }
            Log.Debug($"Database, query result: {r.ToJson()}");
            return r;
        }

        public static async ETTask<List<T>> Query<T>(this MicroDustDatabaseComponent self, Expression<Func<T, bool>> filter, string collection = null)
        where T : Entity
        {
            using (await self.Root().GetComponent<CoroutineLockComponent>().Wait(CoroutineLockType.DB, RandomGenerator.RandInt64() % DBComponent.TaskCount))
            {
                IAsyncCursor<T> cursor = await self.GetCollection<T>(collection).FindAsync(filter);

                return await cursor.ToListAsync();
            }
        }

        public static async ETTask Save<T>(this MicroDustDatabaseComponent self, T entity, string collection = null) where T : Entity
        {
            if (entity == null)
            {
                Log.Error($"save entity is null: {typeof(T).FullName}");

                return;
            }

            if (collection == null)
            {
                collection = entity.GetType().FullName;
            }

            using (await self.Root().GetComponent<CoroutineLockComponent>().Wait(CoroutineLockType.DB, entity.Id % DBComponent.TaskCount))
            {
                await self.GetCollection(collection).ReplaceOneAsync(d => d.Id == entity.Id, entity, new ReplaceOptions { IsUpsert = true });
            }
        }

        public static async ETTask Save<T>(this MicroDustDatabaseComponent self, long taskId, T entity, string collection = null) where T : Entity
        {
            if (entity == null)
            {
                Log.Error($"save entity is null: {typeof(T).FullName}");

                return;
            }

            if (collection == null)
            {
                collection = entity.GetType().FullName;
            }

            using (await self.Root().GetComponent<CoroutineLockComponent>().Wait(CoroutineLockType.DB, taskId % DBComponent.TaskCount))
            {
                await self.GetCollection(collection).ReplaceOneAsync(d => d.Id == entity.Id, entity, new ReplaceOptions { IsUpsert = true });
            }
        }

        private static IMongoCollection<Entity> GetCollection(this MicroDustDatabaseComponent self, string name)
        {
            return self.database.GetCollection<Entity>(name);
        }

        private static IMongoCollection<T> GetCollection<T>(this MicroDustDatabaseComponent self, string collection = null)
        {
            return self.database.GetCollection<T>(collection ?? typeof(T).FullName);
        }
    }
}