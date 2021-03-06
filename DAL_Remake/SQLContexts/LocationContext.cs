using DAL_Remake.Interfaces;
using Mono.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Data;
using UnityEngine;


namespace DAL_Remake.SQLContexts
{
    public class LocationContext : ILocationContext
    {
        private SqliteConnection connection;
        private readonly string connectionString = @"Data Source =" + @Application.dataPath + @"\DBProftaak.db;Version=3; ";

        public LocationContext()
        {
            connection = new SqliteConnection(connectionString);
        }

        public object[] GetGymleader(int locationID)
        {
            object[] data;
            string query = "select c.name, c.id, c.gender, c.money, cl.posX, cl.posY, gl.defeated " +
                           "from characterlocation cl, character c, gymleader gl " +
                           "where cl.characterID = c.ID " +
                           "and c.ID = gl.ID " +
                           "and cl.locationID = @LocationID";

            using (SqliteDataAdapter adapter = new SqliteDataAdapter(query, connection))
            {
                adapter.SelectCommand.Parameters.AddWithValue("@LocationID", locationID);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);
                data = dataTable.Rows[0].ItemArray;
            }
            return data;
        }

        public object[] GetNurse(int locationID)
        {
            object[] data;
            string query = "select c.name, c.id, c.gender, c.money, cl.posX, cl.posY " +
                           "from characterlocation cl, character c, nurse n " +
                           "where cl.characterID = c.ID " +
                           "and c.ID = n.ID " +
                           "and cl.locationID = @LocationID";

            using (SqliteDataAdapter adapter = new SqliteDataAdapter(query, connection))
            {
                adapter.SelectCommand.Parameters.AddWithValue("@LocationID", locationID);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);
                data = dataTable.Rows[0].ItemArray;
            }
            return data;
        }

        public object[] GetShopkeeper(int locationID)
        {
            object[] data;
            string query = "select c.name, c.id, c.gender, c.money, cl.posX, cl.posY " +
                           "from characterlocation cl, character c, shopkeeper sk " +
                           "where cl.characterID = c.ID " +
                           "and c.ID = sk.ID " +
                           "and cl.locationID = @LocationID";

            using (SqliteDataAdapter adapter = new SqliteDataAdapter(query, connection))
            {
                adapter.SelectCommand.Parameters.AddWithValue("@LocationID", locationID);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);
                data = dataTable.Rows[0].ItemArray;
            }
            return data;
        }

        public List<object[]> GetOpponents(int locationID)
        {
            List<object[]> data = new List<object[]>();
            string query = "select c.name, c.id, c.gender, c.money, cl.posX, cl.posY, op.defeated " +
                           "from characterlocation cl, character c, opponent op " +
                           "where cl.characterID = c.ID " +
                           "and c.ID = op.ID " +
                           "and cl.locationID = @LocationID";

            using (SqliteDataAdapter adapter = new SqliteDataAdapter(query, connection))
            {
                adapter.SelectCommand.Parameters.AddWithValue("@LocationID", locationID);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);

                foreach (DataRow dataRow in dataTable.Rows)
                {
                    data.Add(dataRow.ItemArray);
                }
            }
            return data;
        }

        public List<object[]> GetBystanders(int locationID)
        {
            List<object[]> data = new List<object[]>();
            string query = "select c.name, c.id, c.gender, c.money, cl.posX, cl.posY " +
                           "from characterlocation cl, character c, bystander by " +
                           "where cl.characterID = c.ID " +
                           "and c.ID = by.ID " +
                           "and cl.locationID = @LocationID";

            using (SqliteDataAdapter adapter = new SqliteDataAdapter(query, connection))
            {
                adapter.SelectCommand.Parameters.AddWithValue("@LocationID", locationID);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);

                foreach (DataRow dataRow in dataTable.Rows)
                {
                    data.Add(dataRow.ItemArray);
                }
            }
            return data;
        }


        public List<object[]> GetPassages(int locationID)
        {
            List<object[]> data = new List<object[]>();
            string query = "select id, fromx, fromy, tox, toy, tolocationid " +
                                    "from passage " +
                                    "where fromlocationid = @locationID";

            using (SqliteDataAdapter adapter = new SqliteDataAdapter(query, connection))
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

        public List<object[]> GetEncounterablePokemon(int locationID)
        {
            var data = new List<object[]>();
            var query = "select * from PokedexPokemon as p " +
                "inner join PokemonLocation as PL on pl.PokedexPokemonID = p.ID " +
                "inner join Location as l on l.ID = pl.AreaID " +
                "where l.id = @LocationID";

            using (SqliteDataAdapter adapter = new SqliteDataAdapter(query, connection))
            {
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }
                adapter.SelectCommand.Parameters.AddWithValue("@LocationID", locationID);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);

                foreach (DataRow dataRow in dataTable.Rows)
                {
                    data.Add(dataRow.ItemArray);
                }
            }
            return data;
        }

        public List<object[]> GetPokedexPokemonMoves(int pokemonID, int lvl)
        {
            List<object[]> data = new List<object[]>();
            string query = "select pm.id, pdm.name, pdm.maxpp, pdm.accuracy, pdm.description, pdm.basepower " +
                           "FROM PokemonMove pm " +
                           "INNER JOIN PokedexMove pdm " +
                           "ON pm.pokedexmoveid = pdm.id " +
                           "WHERE pm.PokedexPokemonID = @pokemonID AND pm.minlvl <= @lvl " +
                           "ORDER BY Minlvl ASC";

            using (SqliteDataAdapter adapter = new SqliteDataAdapter(query, connection))
            {
                adapter.SelectCommand.Parameters.AddWithValue("@PokemonID", pokemonID);
                adapter.SelectCommand.Parameters.AddWithValue("@lvl", lvl);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);

                foreach (DataRow dataRow in dataTable.Rows)
                {
                    data.Add(dataRow.ItemArray);
                }
            }
            return data;
        }

        public List<object[]> GetPokemonMoves(int pokemonID)
        {
            List<object[]> data = new List<object[]>();
            string query = "select m.ID, pdm.name, m.currentPP, pdm.maxPP, pdm.accuracy, pdm.description, pdm.basePower, pm.minlvl " +
                                    "from move m, pokemonMove pm, pokedexMove pdm " +
                                    "where m.pmid = pm.ID " +
                                    "and pm.ID = pdm.ID " +
                                    "and m.pokemonID = @PokemonID";

            using (SqliteDataAdapter adapter = new SqliteDataAdapter(query, connection))
            {
                adapter.SelectCommand.Parameters.AddWithValue("@PokemonID", pokemonID);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);

                foreach (DataRow dataRow in dataTable.Rows)
                {
                    data.Add(dataRow.ItemArray);
                }
            }
            return data;
        }

        public object[] GetPokemonType(int pokemonID)
        {
            object[] data;
            string query = "SELECT t.ID, t.Name FROM type t INNER JOIN PokedexPokemon pp ON t.id = pp.typeID WHERE pp.ID = @PokemonID";

            using (SqliteDataAdapter adapter = new SqliteDataAdapter(query, connection))
            {
                adapter.SelectCommand.Parameters.AddWithValue("@PokemonID", pokemonID);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);
                data = dataTable.Rows[0].ItemArray;
            }
            return data;
        }

        public object[] GetMoveType(int moveID)
        {
            object[] data;
            string query = "select t.ID, t.Name " +
                           "from type t, pokedexmove pm" +
                           "where t.id = pm.typeID " +
                           "and pm.ID = @MoveID";

            using (SqliteDataAdapter adapter = new SqliteDataAdapter(query, connection))
            {
                adapter.SelectCommand.Parameters.AddWithValue("@MoveID", moveID);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);
                data = dataTable.Rows[0].ItemArray;
            }
            return data;
        }

        public object[] GetEncounterChance(int locationID)
        {
            object[] data;

            string query = "select encounterchance from area where ID = @locationID";
            using (SqliteDataAdapter adapter = new SqliteDataAdapter(query, connection))
            {
                adapter.SelectCommand.Parameters.AddWithValue("@locationID", locationID);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);
                data = dataTable.Rows[0].ItemArray;
            }
            return data;
        }

        public List<object[]> GetInventory(int characterID)
        {
            List<object[]> data = new List<object[]>();
            string query = "select i.id, i.name, i.cost, i.description, p.Quantity " +
                                    "from item i, possesion p " +
                                    "where i.ID = p.ID " +
                                    "and p.CharacterID = @CharacterID";

            using (SqliteDataAdapter adapter = new SqliteDataAdapter(query, connection))
            {
                adapter.SelectCommand.Parameters.AddWithValue("@CharacterID", characterID);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);

                foreach (DataRow dataRow in dataTable.Rows)
                {
                    data.Add(dataRow.ItemArray);
                }
            }
            return data;
        }

        public List<object[]> GetRevives(int characterID)
        {
            List<object[]> data = new List<object[]>();
            string query = "select i.ID, i.Name, i.Cost, i.Description, r.Percentage, p.quantity " +
                                    "from Possesion p, Item i, Consumable c, Revive r " +
                                    "where p.CharacterID = @CharacterID " +
                                    "and p.ItemID = i.ID " +
                                    "and i.ID = c.id " +
                                    "and c.id = r.id";

            using (SqliteDataAdapter adapter = new SqliteDataAdapter(query, connection))
            {
                adapter.SelectCommand.Parameters.AddWithValue("@CharacterID", characterID);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);

                foreach (DataRow dataRow in dataTable.Rows)
                {
                    data.Add(dataRow.ItemArray);
                }
            }
            return data;
        }

        public List<object[]> GetPotions(int characterID)
        {
            List<object[]> data = new List<object[]>();
            string query = "select i.ID, i.Name, i.Cost, i.Description, h.HealAmount, p.quantity " +
                                    "from Possesion p, Item i, Consumable c, HealthPotion h " +
                                    "where p.CharacterID = @CharacterID " +
                                    "and p.ItemID = i.ID " +
                                    "and i.ID = c.id " +
                                    "and c.id = h.id";

            using (SqliteDataAdapter adapter = new SqliteDataAdapter(query, connection))
            {
                adapter.SelectCommand.Parameters.AddWithValue("@CharacterID", characterID);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);

                foreach (DataRow dataRow in dataTable.Rows)
                {
                    data.Add(dataRow.ItemArray);
                }
            }
            return data;
        }

        public List<object[]> GetPokeballs(int characterID)
        {
            List<object[]> data = new List<object[]>();
            string query = "select i.ID, i.Name, i.Cost, i.Description, pb.CatchRate, p.quantity " +
                                    "from Possesion p, Item i, Consumable c, Pokeball pb " +
                                    "where p.CharacterID = @CharacterID " +
                                    "and p.ItemID = i.ID " +
                                    "and i.ID = c.id " +
                                    "and c.id = pb.id";

            using (SqliteDataAdapter adapter = new SqliteDataAdapter(query, connection))
            {
                adapter.SelectCommand.Parameters.AddWithValue("@CharacterID", characterID);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);

                foreach (DataRow dataRow in dataTable.Rows)
                {
                    data.Add(dataRow.ItemArray);
                }
            }
            return data;
        }

        public object[] GetCurrentLocation(int locationID)
        {
            object[] data;
            string query = "SELECT *"+
                            "FROM location "+
                            "LEFT JOIN Area ON location.id = area.id "+
                            "LEFT JOIN Building ON location.id = building.id "+
                            "LEFT JOIN ROUTE ON Area.ID = Route.AreaID "+
                            "WHERE Location.ID = @LocationID";
            using (SqliteDataAdapter adapter = new SqliteDataAdapter(query, connection))
            {
                adapter.SelectCommand.Parameters.AddWithValue("@LocationID", locationID);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);
                data = dataTable.Rows[0].ItemArray;
            }
            return data;
        }

        public List<object[]> GetPokemonFromOpponent(int characterID)
        {
            throw new NotImplementedException();
        }

        public object[] GetPassageByLocationAndCoords(int locationID, int x, int y)
        {
            object[] data;
            string query = "SELECT passage.id, fromx, fromy, tox, toy, tolocationid " +
                                    "FROM location JOIN passage ON location.id=passage.fromID" +
                                    "WHERE location.id = @locationID" +
                                    "AND fromx = @X AND fromy = @Y";

            using (SqliteDataAdapter adapter = new SqliteDataAdapter(query, connection))
            {
                adapter.SelectCommand.Parameters.AddWithValue("@locationID", locationID);
                adapter.SelectCommand.Parameters.AddWithValue("@X", x);
                adapter.SelectCommand.Parameters.AddWithValue("@Y", y);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);

                data = dataTable.Rows[0].ItemArray;
            }
            return data;
        }
    }
}
