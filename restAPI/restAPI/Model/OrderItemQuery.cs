using MySql.Data.MySqlClient;
using OrderApi;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;

namespace restAPI.Model
{
    public class OrderItemQuery
    {
        public AppDb Db { get; }

        public OrderItemQuery(AppDb db)
        {
            Db = db;
        }

        public async Task<OrderItem> FindOneAsync(int id)
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"SELECT `id`, `url` FROM `order_item` WHERE `id` = @id";
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@id",
                DbType = DbType.Int32,
                Value = id,
            });
            var result = await ReadAllAsync(await cmd.ExecuteReaderAsync());
            return result.Count > 0 ? result[0] : null;
        }

        public async Task<List<OrderItem>> LatestOrdersAsync()
        {
            using var cmd = Db.Connection.CreateCommand();
            //cmd.CommandText = @"SELECT `id`, `url` FROM `order_item` ORDER BY `id`";
            cmd.CommandText = @"SELECT o.id, oi.url FROM `orders_order_items` AS ors 
                                INNER JOIN orders 
                                As o ON 
                                ors.orders_id = o.id 
                                INNER JOIN order_item 
                                As oi ON 
                                ors.order_items_id = oi.id";
            return await ReadAllAsync(await cmd.ExecuteReaderAsync());
        }

        public async Task DeleteAllAsync()
        {
            using var txn = await Db.Connection.BeginTransactionAsync();
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"DELETE FROM `order_item`";
            await cmd.ExecuteNonQueryAsync();
            await txn.CommitAsync();
        }

        private async Task<List<OrderItem>> ReadAllAsync(DbDataReader reader)
        {
            var orders = new List<OrderItem>();
            using (reader)
            {
                while (await reader.ReadAsync())
                {
                    var order = new OrderItem(Db)
                    {
                        id = reader.GetInt32(0),
                        url = reader.GetString(1),
                    };
                    orders.Add(order);
                }
            }
            return orders;
        }
    }
}
