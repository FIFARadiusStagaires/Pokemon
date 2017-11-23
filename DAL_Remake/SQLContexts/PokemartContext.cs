using Mono.Data.Sqlite;
using DAL_Remake.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;


namespace DAL_Remake.SQLContexts
{
    public class PokemartContext : IPokemartContext
    {
        private SqliteConnection connection;
        private readonly string connectionString = @"Data Source=Assets/testdb.db;Version=3;";

        public PokemartContext()
        {
            connection = new SqliteConnection(connectionString);
        }

        public List<object[]> GetRevives(int pokemartID)
        {
            List<object[]> data = new List<object[]>();
            string query = "select i.ID, i.Name, i.Cost, i.Description, r.Percentage " +
                                    "from Item i, Consumable c, Consumable_Pokemart cp, Revive r " +
                                    "where i.ID = c.ItemID " +
                                    "and c.ItemID = r.ConsumableID " +
                                    "and c.ItemID = cp.ConsumableID " +
                                    "and p.PokemartID = @PokemartID";
            using (SqliteDataAdapter adapter = new SqliteDataAdapter(query, connection))
            {
                adapter.SelectCommand.Parameters.AddWithValue("@PokemartID", pokemartID);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);

                foreach (DataRow dataRow in dataTable.Rows)
                {
                    data.Add(dataRow.ItemArray);
                }
            }

            return data;
        }

        public List<object[]> GetPotions(int pokemartID)
        {
            List<object[]> data = new List<object[]>();
            string query = "select i.ID, i.Name, i.Cost, i.Description, h.HealAmount " +
                                    "from Item i, Consumable c, Consumable_Pokemart cp, HealthPotion h " +
                                    "where i.ID = c.ItemID " +
                                    "and c.ItemID = r.ConsumableID " +
                                    "and c.ItemID = cp.ConsumableID " +
                                    "and sp.PokemartID = @PokemartID";
            using (SqliteDataAdapter adapter = new SqliteDataAdapter(query, connection))
            {
                adapter.SelectCommand.Parameters.AddWithValue("@PokemartID", pokemartID);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);

                foreach (DataRow dataRow in dataTable.Rows)
                {
                    data.Add(dataRow.ItemArray);
                }
            }

            return data;
        }

        public List<object[]> GetPokeballs(int pokemartID)
        {
            List<object[]> data = new List<object[]>();
            string query = "select i.ID, i.Name, i.Cost, i.Description, pb.CatchRate " +
                                    "from Item i, Consumable c, Consumable_Pokemart cp, Pokeball pb " +
                                    "where i.ID = c.ItemID " +
                                    "and c.ItemID = r.ConsumableID " +
                                    "and c.ItemID = cp.ConsumableID " +
                                    "and sp.PokemartID = @PokemartID";
            using (SqliteDataAdapter adapter = new SqliteDataAdapter(query, connection))
            {
                adapter.SelectCommand.Parameters.AddWithValue("@pokemartID", pokemartID);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);

                foreach (DataRow dataRow in dataTable.Rows)
                {
                    data.Add(dataRow.ItemArray);
                }
            }

            return data;
        }
    }
}