﻿using System;
using System.Collections.Generic;

namespace Classes
{
    public class Oppenent : Character, IItemUser
    {
        public bool Defeated { get; private set; }

        public void UseItemInBattle(Consumable consumable)
        {
            throw new NotImplementedException();
        }

        public Oppenent(string name, int id, string gender, int money, int posX, int posY, Location currentLocation, List<Possesion> inventory, List<Pokemon> partyPokemon) : base(name, id, gender, money, posX, posY, currentLocation, inventory, partyPokemon)
        {
        }
    }
}
