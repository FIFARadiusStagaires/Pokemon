﻿using DAL_Remake.Interfaces;
using DAL_Remake.SQLContexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Classes.Repos
{
    public class PokemonRepository
    {
        private IPokemonContext context;

        public PokemonRepository()
        {
            context = new PokemonContext();
        }

        public List<Move> GetMoves(int pokemonID)
        {
            List<Move> moves = new List<Move>();
            List<object[]> data = context.GetMoves(pokemonID);

            foreach (object[] row in data)
            {
                //TODO Roberto fix die null -> type moet ook worden opgehaald (ook fix query)
                moves.Add(new Move(Convert.ToInt32(data[0]), data[1].ToString(), Convert.ToInt32(data[2]), Convert.ToInt32(data[3]), 
                    Convert.ToInt32(data[4]), data[5].ToString(), Convert.ToBoolean(data[6]), Convert.ToInt32(data[7]), Convert.ToInt32(data[8]), null));
            }

            return moves;
        }

        public Type GetPokemonType(int pokedexPokemonID)
        {
            object[] data = context.GetPokemonType(pokedexPokemonID);
            Type type = new Type(Convert.ToInt32(data[0]), data[1].ToString());
            return type;
        }
        
        public LevelUpXP GetNextLevelUpXp(int level)
        {
            object[] data = context.GetNextLevelUpXp(level);
            LevelUpXP levelUpXp = new LevelUpXP(Convert.ToInt32(data[0]),Convert.ToInt32(data[1]));
            return levelUpXp;
        }

        public void UpdatePokemon(Pokemon pokemon)//TODO Query voor updaten van pokemon
        {
            context.UpdatePokemon(pokemon);
        }

        public Pokemon GetEvolvePokemon(int id)
        {
            return context.GetEvolvePokemon(id);
        }

        internal Pokemon GetLevelUpStats(int id)
        {
            return context.GetLevelUpStats(id);
        }
    }
}
