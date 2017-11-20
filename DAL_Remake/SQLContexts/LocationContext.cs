﻿using DAL_Remake.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL_Remake.SQLContexts
{
    public class LocationContext : ILocationContext
    {
        private SQLiteConnection connection;
        private readonly string connectionString = @"Data Source=Assets/testdb.db;Version=3;";

        public List<object[]> GetCharacters(int locationID)
        {
            List<object[]> data = new List<object[]>();
            string query = "select c.*,cl.posx,cl.posy from Location l" +
                                " inner join CharacterLocation cl on cl.LocationId = l.id" +
                                " inner join Character c on cl.CharacterId = c.id" +
                                " where l.id = @locationID";

            using (SQLiteDataAdapter adapter = new SQLiteDataAdapter(query, connection))
            {
                adapter.SelectCommand.Parameters.AddWithValue("@locationID", locationID);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);

                foreach (DataRow dataRow in dataTable.Rows)
                {
                    data.Add(dataRow.ItemArray);
                }
            }
            return data;
        }

        public List<object[]> GetPassages()
        {
            return new List<object[]>();
        }

        public List<object[]> GetPokemon()
        {
            return new List<object[]>();
        }
    }
}
