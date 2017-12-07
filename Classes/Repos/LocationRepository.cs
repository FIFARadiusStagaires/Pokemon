﻿using DAL_Remake.Interfaces;
using DAL_Remake.SQLContexts;
using System;
using System.Collections.Generic;

namespace Classes.Repos
{
    public class LocationRepository
    {
        private ILocationContext context;


        public LocationRepository(ILocationContext context)
        {
            context = new LocationContext();
        }

        public Gymleader GetGymleader(int locationID)
        {
            object[] data = context.GetGymleader(locationID);

            // TODO: implementeer dit
            return null;
        }

        public Nurse GetNurse(int locationID)
        {
            object[] data = context.GetNurse(locationID);
            // TODO: implementeer dit
            return null;
        }

        public ShopKeeper GetShopkeeper(int locationID)
        {
            object[] data = context.GetShopkeeper(locationID);
            // TODO: implementeer dit
            return null;
        }

        public List<Oppenent> GetOpponents(int locationID)
        {
            List<object[]> data = context.GetOpponents(locationID);
            List<Oppenent> oppenents = new List<Oppenent>();

            foreach (object[] row in data)
            {
                // TODO: implementeer dit
            }
            return oppenents;
        }

        public List<Bystander> GetBystanders(int locationID)
        {
            List<object[]> data = context.GetBystanders(locationID);
            List<Bystander> bystanders = new List<Bystander>();

            foreach (object[] row in data)
            {
                // TODO: implementeer dit
            }
            return bystanders;
        }

        public List<Passage> GetPassages(int locationID)
        {
            List<object[]> data = context.GetPassages(locationID);
            List<Passage> passages = new List<Passage>();

            foreach (object[] row in data)
            {
                // TODO: implementeer dit
            }

            return passages;
        }

        public List<Pokemon> GetEncounterablePokemon(int locationID)
        {
            List<object[]> data = context.GetEncounterablePokemon(locationID);
            List<Pokemon> pokemonAtLocation = new List<Pokemon>();

            foreach (object[] row in data)
            {
                object[] typeData = context.GetPokemonType(Convert.ToInt32(row[0]));
                Type type = new Type(Convert.ToInt32(typeData[0]), typeData[1].ToString());
                List<Move> pokemonMoves = GetPokemonMoves(Convert.ToInt32(row[0]));

                pokemonAtLocation.Add(new Pokemon(type, pokemonMoves, Convert.ToInt32(row[0]), row[1].ToString(), Convert.ToBoolean(row[2]),
                    Convert.ToInt32(row[3]), Convert.ToInt32(row[4]), Convert.ToInt32(row[5]), Convert.ToInt32(row[6]),
                    Convert.ToInt32(row[7]), Convert.ToInt32(row[8]), Convert.ToInt32(row[9]), Convert.ToInt32(row[10]),
                    Convert.ToInt32(row[11])));
            }
            return pokemonAtLocation;
        }

        public List<Move> GetPokemonMoves(int pokemonID)
        {
            List<object[]> data = context.GetPokemonMoves(pokemonID);
            List<Move> pokemonMoves = new List<Move>();

            foreach (object[] row in data)
            {
                pokemonMoves.Add(new Move(Convert.ToInt32(row[0]), row[1].ToString(), Convert.ToInt32(row[2]), Convert.ToInt32(row[3]),
                    Convert.ToInt32(row[4]), row[5].ToString(), Convert.ToBoolean(row[6]), Convert.ToInt32(row[7]), Convert.ToInt32(row[8]), GetMoveType(Convert.ToInt32(row[0]))));
            }

            return pokemonMoves;
        }

        public Type GetPokemonType(int pokemonID)
        {
            object[] data = context.GetPokemonType(pokemonID);

            return new Type(Convert.ToInt32(data[0]), data[1].ToString());
        }

        public Type GetMoveType(int moveID)
        {
            object[] data = context.GetMoveType(moveID);

            return new Type(Convert.ToInt32(data[0]), data[1].ToString());
        }

        public List<Item> GetInventory(int characterID)
        {
            List<object[]> data = context.GetInventory(characterID);
            List<Item> inventoryItems = new List<Item>();

            foreach (Revive revive in GetRevives(characterID))
            {
                inventoryItems.Add(revive);
            }
            foreach (Potion potion in GetPotions(characterID))
            {
                inventoryItems.Add(potion);
            }
            foreach (Pokeball pokeball in GetPokeballs(characterID))
            {
                inventoryItems.Add(pokeball);
            }
            return inventoryItems;
        }

        public List<Revive> GetRevives(int characterID)
        {
            List<object[]> data = context.GetRevives(characterID);
            List<Revive> revives = new List<Revive>();

            foreach (object[] row in data)
            {
                for (int i = 0; i < Convert.ToInt32(data[5]); i++)
                {
                    revives.Add(new Revive(Convert.ToInt32(row[0]), row[1].ToString(), Convert.ToInt32(row[2]), row[3].ToString(), Convert.ToInt32(row[4])));
                }

            }
            return revives;
        }

        public List<Potion> GetPotions(int characterID)
        {
            List<object[]> data = context.GetPotions(characterID);
            List<Potion> potions = new List<Potion>();

            foreach (object[] row in data)
            {
                for (int i = 0; i < Convert.ToInt32(data[5]); i++)
                {
                    potions.Add(new Potion(Convert.ToInt32(row[0]), row[1].ToString(), Convert.ToInt32(row[2]), row[3].ToString(), Convert.ToInt32(row[4])));
                }
            }
            return potions;
        }

        public List<Pokeball> GetPokeballs(int characterID)
        {
            List<object[]> data = context.GetPokeballs(characterID);
            List<Pokeball> pokeballs = new List<Pokeball>();

            foreach (object[] row in data)
            {
                for (int i = 0; i < Convert.ToInt32(data[5]); i++)
                {
                    pokeballs.Add(new Pokeball(Convert.ToInt32(row[0]), row[1].ToString(), Convert.ToInt32(row[2]), row[3].ToString(), Convert.ToInt32(row[4])));
                }
            }
            return pokeballs;
        }

        public decimal GetEncounterChance(int locationID)
        {
            object[] data = context.GetEncounterChance(locationID);

            return Convert.ToDecimal(data[0]);
        }

        //TODO: Laura, fix dit, en vraag om database. Query is niet toereikend.
        //public Location GetCurrentLocation(int locationID)
        //{
        //    object[] data = context.GetCurrentLocation(locationID);
        //    Location location = new Location(Convert.ToInt32(data[0]), data[1], data[2], data[3]);
        //      Geen idee, kan niet zien hoe de database in elkaar zit. ik stel voor dat we hier nog even kijken vanavond.
        //      kan iets bedenken met een switch statement, afhankelijk van wat er in het type vak zit, en dat daarmee wordt bepaald wat voor location er wordt teruggegeven.
        //      Zoals het er nu staat, kan ik niks, omdat ik niet in de database kan kijken.
        //    return location;
        //}
    }
}
