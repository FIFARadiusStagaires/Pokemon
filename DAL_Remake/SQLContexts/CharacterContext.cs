﻿using DAL_Remake.Interfaces;
using Mono.Data.Sqlite;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System;

namespace DAL_Remake.SQLContexts
{
    public class CharacterContext : ICharacterContext
    {
        private SqliteConnection connection;
        private readonly string connectionString = @"Data Source=Assets/testdb.db;Version=3;";

        public CharacterContext()
        {
            connection = new SqliteConnection(connectionString);
        }

        public List<object[]> GetRevives(int characterID)
        {
            List<object[]> data = new List<object[]>();
            string query = "select i.ID, i.Name, i.Cost, i.Description, r.Percentage " +
                           "from Posession p, Item i, Consumable c, Revive r " +
                           "where p.CharacterID = @CharacterID " +
                           "and p.ItemID = i.ID " +
                           "and i.ID = c.ItemID " +
                           "and c.ItemID = r.ConsumableID";

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
            string query = "select i.ID, i.Name, i.Cost, i.Description, h.HealAmount " +
                           "from Posession p, Item i, Consumable c, HealthPotion h " +
                           "where p.CharacterID = @CharacterID " +
                           "and p.ItemID = i.ID " +
                           "and i.ID = c.ItemID " +
                           "and c.ItemID = h.ConsumableID";

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
            string query = "select i.ID, i.Name, i.Cost, i.Description, pb.CatchRate " +
                           "from Posession p, Item i, Consumable c, Pokeball pb " +
                           "where p.CharacterID = @CharacterID " +
                           "and p.ItemID = i.ID " +
                           "and i.ID = c.ItemID " +
                           "and c.ItemID = pb.ConsumableID";

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

        public List<object[]> GetBadges(int characterID)
        {
            List<object[]> data = new List<object[]>();
            string query = "select i.ID, i.Name, i.Description " +
                           "from Posession p, Item i, NonConsumable nc, Badge b " +
                           "where p.CharacterID = @CharacterID " +
                           "and p.ItemID = i.ID " +
                           "and i.ID = nc.ItemID " +
                           "and nc.ItemID = b.NCID";

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

        public List<object[]> GetKeyItems(int characterID)
        {
            List<object[]> data = new List<object[]>();
            string query = "select i.ID, i.Name, i.Description, ki.IsUsable " +
                           "from Posession p, Item i, NonConsumable nc, KeyItem ki " +
                           "where p.CharacterID = @CharacterID " +
                           "and p.ItemID = i.ID " +
                           "and i.ID = nc.ItemID " +
                           "and nc.ItemID = ki.NCID";

            using (SqliteDataAdapter adapter = new SqliteDataAdapter(query, connection))
            {
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);

                foreach (DataRow dataRow in dataTable.Rows)
                {
                    data.Add(dataRow.ItemArray);
                }
            }
            return data;
        }

        public List<object[]> GetPokemonFromParty(int characterID)
        {
            List<object[]> data = new List<object[]>();
            string query =
                "select p.id, pp.name, p.inParty, p.level, p.currentHp, p.maxHp, p.xp, p.attack, p.defense, p.speed, pp.evolveLevel, pp.captureRate " +
                "from pokemon p, pokedexpokemon pp " +
                "where p.pokedexpokemonID = pp.ID " +
                "and p.InParty = 1 " +
                "and p.CharacterID = @CharacterID";
            using (SqliteDataAdapter adapter = new SqliteDataAdapter(query, connection))
            {
                adapter.SelectCommand.Parameters.AddWithValue("@characterID", characterID);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);

                foreach (DataRow dataRow in dataTable.Rows)
                {
                    data.Add(dataRow.ItemArray);
                }
            }
            return data;
        }

        public List<object[]> GetDialogues(int characterID)
        {
            List<object[]> data = new List<object[]>();
            string query = "select * " +
                           "from dialogue d, character c " +
                           "where d.id = c.dialogueid " +
                           "and c.ID = @CharacterID";
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

        public List<object[]> GetPokemonMoves(int pokemonID)
        {
            List<object[]> data = new List<object[]>();
            string query =
                "select m.ID, pdm.name, m.currentPP, pdm.maxPP, pdm.accuracy, pdm.description, pdm.hasOverworldEffect, pdm.basePower, pm.minlevel " +
                "from move m, pokemonMoves pm, pokedexMove pdm " +
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
            string query = "select t.ID, t.Name " +
                           "from type t, pokedexpokemon pp, pokemon p " +
                           "where t.id = pp.typeID " +
                           "and pp.ID = p.pokedexpokemonID " +
                           "and p.ID = @PokemonID";

            using (SqliteDataAdapter adapter = new SqliteDataAdapter(query, connection))
            {
                adapter.SelectCommand.Parameters.AddWithValue("@PokemonID", pokemonID);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);
                data = dataTable.Rows[0].ItemArray;
            }
            return data;
        }

        public object[] GetCurrentLocation(int characterID)
        {
            object[] data;
            string query = "select l.ID, l.Name, pc.LastVisited " +
                           "from CharacterLocation cl, Location l, Building b, PokeCenter pc " +
                           "where cl.LocationID = l.ID " +
                           "and l.ID = b.LocationID " +
                           "and b.LocationID = pc.BuildingID " +
                           "and lc.CharacterID = @CharacterID";

            using (SqliteDataAdapter adapter = new SqliteDataAdapter(query, connection))
            {
                adapter.SelectCommand.Parameters.AddWithValue("@CharacterID", characterID);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);
                data = dataTable.Rows[0].ItemArray;
            }
            return data;
        }

        public string GetLocationType(int locationID)
        {
            string locationType = "";
            if (CheckLocationType("select buildingID from pokemart where buildingID = @LocationID", locationID))
            {
                locationType = "pokemart";
            }
            else if (CheckLocationType("select buildingID from gym where buildingID = @LocationID", locationID))
            {
                locationType = "gym";
            }
            else if (CheckLocationType("select buildingID from pokecenter where buildingID = @LocationID", locationID))
            {
                locationType = "pokecenter";
            }
            else if (CheckLocationType("select buildingID from enemyhq where buildingID = @LocationID", locationID))
            {
                locationType = "enemyhq";
            }
            else if (CheckLocationType("select areaID from city where areaID = @LocationID", locationID))
            {
                locationType = "city";
            }
            else if (CheckLocationType("select areaID from cave where areaID = @LocationID", locationID))
            {
                locationType = "cave";
            }
            else if (CheckLocationType("select areaID from route where areaID = @LocationID", locationID))
            {
                locationType = "route";
            }
            return locationType;
        }

        private bool CheckLocationType(string query, int locationID)
        {
            object data;

            using (SqliteDataAdapter adapter = new SqliteDataAdapter(query, connection))
            {
                DataTable dataTable = new DataTable();
                adapter.SelectCommand.Parameters.AddWithValue("@LocationID", locationID);
                adapter.Fill(dataTable);
                data = dataTable.Rows[0].ItemArray[0];
            }

            if (!string.IsNullOrEmpty(data.ToString()))
            {
                return true;
            }
            return false;
        }

        public void InsterIntro(int pokemonID, string CharacterName, string Gender)
        {
            string query = "insert into Character (gender, Name,Money) values (@gender,@name,@money)";
            SqliteCommand insercharactercmd = new SqliteCommand(query, connection);
            insercharactercmd.Parameters.Add(new SqliteParameter("@gender", Gender));
            insercharactercmd.Parameters.Add(new SqliteParameter("@name", CharacterName));
            insercharactercmd.Parameters.Add(new SqliteParameter("@money", 2000));
            SqliteCommand playercmd =
                new SqliteCommand(
                    "insert into Player(PlayerId, wins, losses) values((select id from Character where Name = '@name'), 0, 0))",
                    connection);
            using (connection)
            {
                connection.Open();
                insercharactercmd.ExecuteNonQuery();
                playercmd.ExecuteNonQuery();
            }
        }
    }
}