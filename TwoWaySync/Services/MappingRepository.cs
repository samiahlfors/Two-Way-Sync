using System;
using Dapper;
using Npgsql;
using TwoWaySync.Models;

namespace TwoWaySync.Services
{
    public class MappingRepository
    {
        private string _connectionString;

        public MappingRepository(string connectionString)
        {
            _connectionString = connectionString;
        }
        
        public EntityMapping GetEntityByLocalId(string entityType, Guid localId)
        {
            using (var conn = new NpgsqlConnection(_connectionString))
            {
                var sql = @"select entity_type as EntityType,
                            local_id as LocalId
                            from entity_mapping where entity_type = @entityType and local_id = @localId";
                return conn.QueryFirstOrDefault<EntityMapping>(sql, new {entityType, localId});
            }
        }
    }
}