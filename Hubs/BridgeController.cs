using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using System.Data.SqlClient;
using System.IO;

namespace ScrumTable.Hubs
{
    /*DB Requests*/
    public class BridgeController : Hub
    {
        string Connection = $@"Data Source=(localdb)\MSSQLLocalDB;
                            AttachDBFilename = {Directory.GetCurrentDirectory() + @"\wwwroot\DB\ScrumTableDB.mdf"};
                            Integrated Security=True;
                            ";
        string Executer(string query)
        {
            string result = "Status: ";
            try
            {
                using (SqlConnection connection = new SqlConnection(Connection))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(query, connection);
                    command.ExecuteNonQuery();
                }
                return result += $"Успешно";
            }
            catch (Exception ex)
            {
                return result += $"{ex.Message}";
            }
        }
        public async Task Remove(string itemID)
        {
            string query = $"DELETE FROM ScrumTable WHERE item_id='{itemID}';";
            await Clients.All.SendAsync("Remove", Executer(query));
        }
        public async Task Add(int Col_id, string Title, string Descr, string Color)
        {
            string query = $"INSERT INTO ScrumTable (Col_id, item_title, item_descr, item_color) VALUES ('{Col_id}', N'{Title}', N'{Descr}', '{Color}');";
            await Clients.All.SendAsync("Add", Executer(query));
        }
        public async Task Transfer(int item_id,int col_id)
        {
            string query = $"UPDATE ScrumTable SET Col_id='{col_id}' WHERE item_id='{item_id}';";
            await Clients.All.SendAsync("Transfer", Executer(query));
        }
        public List<ItemInfo> GetData()
        {
            List<ItemInfo> Items_list = new List<ItemInfo>();
            using (SqlConnection connection = new SqlConnection(Connection))
            {
                connection.Open();
                string query = "SELECT * FROM ScrumTable";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ItemInfo item = new ItemInfo();
                            item.Col_id = reader.GetInt32(0);
                            item.Item_id = reader.GetInt32(1);
                            item.Title = reader.GetString(2);
                            try
                            {
                                item.Descr = reader.GetString(3);
                            }
                            catch { }
                            item.Color = reader.GetString(4);
                            Items_list.Add(item);
                        }
                    }
                }
            }
            return Items_list;
        }
    }
}
