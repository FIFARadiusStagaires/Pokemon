﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization.Formatters;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UI;
using Classes;
using Classes.Exceptions;
using UnityEngine.EventSystems;
using UnityEngine.Networking.NetworkSystem;
using UnityEngine.SceneManagement;
using CPlayer = Classes.Player;
using CType = Classes.Type;

namespace Assets.Scripts
{
    public class BattleConroller : MonoBehaviour
    {
        [SerializeField] private Sprite[] _attackTypeSprites;
        [SerializeField] private GameObject _mainPanel;
        [SerializeField] private GameObject _infoAttackPanel;
        [SerializeField] private GameObject _pokemonPanel;
        [SerializeField] private GameObject _itemContent;
        [SerializeField] private GameObject _itemButton;
        [SerializeField] private GameObject _textPanel;
        [SerializeField] private GameObject _EnemyPanel;
        [SerializeField] private GameObject _PlayerPanel;

        private bool _PokemonSwitchTurn;
        private Battle _battle;

        public void Start()
        {
            /*var type1 = new CType(1, "Fire");
            var type2 = new CType(2, "Water");
            var type3 = new CType(3, "Grass");

            var move1 = new Move(1, "Fire attack", 15, 15, 100, "A fire attack, duhh", false, 60, 1, type1);
            var move3 = new Move(3, "grass attack", 15, 15, 100, "A grass attack, duhh", false, 60, 1, type3);
            var move4 = new Move(4, "Fire!!!!", 15, 15, 80, "A fire attack, duhh", false, 80, 1, type1);
            var move5 = new Move(5, "Water!!!!", 15, 15, 80, "A water attack, duhh", false, 80, 1, type2);
            var move6 = new Move(6, "grass!!!!", 15, 15, 80, "A grass attack, duhh", false, 80, 1, type3);

            var movelist1 = new List<Move> { move1, move4, move3 };
            var movelist2 = new List<Move> { move5, move6 };

            var wildpokemon = new Pokemon(type3, movelist2, 110, 1, "Cutescumber", false, 10, 100, 100, 5, false, 10, 10, 10, 50, 50);
            var playerpokemon = new Pokemon(type1, movelist1, 15, 4, "Dubbleup", false, 11, 5, 100, 100, false, 15, 11, 9, 50, 10);
            var playerpokemon2 = new Pokemon(type3, movelist2, 2982 , 8, "Pikkie", false, 12, 110, 110, 6, false ,10, 10, 10, 25, 10);
            var playerpokemon3 = new Pokemon(type3, movelist2, 2982 , 8, "DikkieDik", false, 12, 110, 110, 6, true ,10, 10, 10, 25, 10);
            
            var pokeball = new Possesion(5, new Pokeball(5, "Pokeball", 200, "JUST A FCKING POKEBALL BRO", 20));
            var pokeball2 = new Possesion(9, new Pokeball(6, "Greatball", 200, "THIS THING IS FUCKING GREAT!", 40));

            var potion = new Possesion(28, new Potion(1, "fuking potion", 12, "fdsaods", 25));
            var potion2 = new Possesion(2, new Potion(2, "fuking cool potion", 1234, "dojidsads", 40));

            var revive = new Possesion(4, new Revive(4, "fcking revive", 12345678, "uygradfbinjk", 50));
            var itemlist = new List<Possesion>(){ pokeball, pokeball2, potion, potion2, revive };

            var player = new CPlayer("Ayyayayay", 1, "Male", 1000, 5, 5, null, itemlist, new List<Pokemon> { playerpokemon, playerpokemon2, playerpokemon3}, 50, 5, null);
            var battle = new Battle(player, wildpokemon);
            CreateNewBattle(battle);*/
            CreateNewBattle(ApplicationModel.battle);
        }

        public void CreateNewBattle(Battle battle)
        {
            _battle = battle;
            SetPokemonInfo(_PlayerPanel, _battle.PlayerPokemon);
            SetPokemonInfo(_EnemyPanel, _battle.WildPokemon ?? _battle.OpponentPokemon);         
            SetPokemonSprite(_PlayerPanel, "PokemonBack/back" + _battle.PlayerPokemon.PokedexId.ToString("000"));
            if (_battle.WildPokemon == null)
            {
                SetPokemonSprite(_EnemyPanel, "PokemonFront/front" + _battle.OpponentPokemon.PokedexId.ToString("000"));
            }
            else
            {
                SetPokemonSprite(_EnemyPanel, "PokemonFront/front" + _battle.WildPokemon.PokedexId.ToString("000"));
            }
        }

        private void SetPokemonSprite(GameObject panel, string uri)
        {
            panel.transform.Find("Base").transform.Find("Image").gameObject.GetComponent<Image>().sprite =
                Resources.LoadAll<Sprite>(uri)[0];
        }

        private void SetPokemonInfo(GameObject panel, Pokemon pokemon)
        {
            panel.transform.Find("Stats").transform.Find("NameText").gameObject.GetComponent<Text>().text =
                pokemon.Name;
            panel.transform.Find("Stats").transform.Find("LevelText").gameObject.GetComponent<Text>().text =
                "Lv " + pokemon.Level;
            UpdateHpUi(panel, pokemon);
        }

        private void UpdateHpUi(GameObject panel, Pokemon pokemon)
        {
            panel.transform.Find("Stats").transform.Find("HpBar").gameObject.GetComponent<Scrollbar>().size =
                pokemon.CurrentHp / (float) pokemon.MaxHp;
            panel.transform.Find("Stats").transform.Find("HpText").gameObject.GetComponent<Text>().text =
                pokemon.CurrentHp + "/" + pokemon.MaxHp;
            var bar = panel.transform.Find("Stats").transform.Find("HpBar").gameObject.GetComponent<Scrollbar>();
            if (pokemon.CurrentHp/(float)pokemon.MaxHp < 0.1f)
            {
                bar.gameObject.transform.Find("Sliding Area").transform.Find("Handle").gameObject
                    .GetComponent<Image>().color = Color.red;
            }
            else if (pokemon.CurrentHp / (float)pokemon.MaxHp < 0.5f)
            {
                bar.gameObject.transform.Find("Sliding Area").transform.Find("Handle").gameObject
                    .GetComponent<Image>().color = new Color(0.96f, 0.76f, 0.26f);
            }
            else
            {
                bar.gameObject.transform.Find("Sliding Area").transform.Find("Handle").gameObject
                    .GetComponent<Image>().color = Color.green;
            }
        }
        
        public void OnAttackMenuButtonPress(GameObject attackMenu)
        {
            for (var i = 0; i < 4; i++)
            {
                var button = attackMenu.transform.Find("MoveButton" + i).gameObject;
                try
                {
                    var move = _battle.PlayerPokemon.GetMoves()[i];
                    button.GetComponent<Image>().sprite = _attackTypeSprites.First(p => p.name == _battle.PlayerPokemon.GetMoves()[i].GetType().Name);
                    button.transform.Find("Move").gameObject
                        .GetComponent<Text>().text = move.Name;
                    button.transform.Find("PP").gameObject.GetComponent<Text>().text = move.CurrentPP + " / " + move.MaxPP;
                    var percentage = move.CurrentPP / (float)move.MaxPP;
                    if (percentage < 0.5)
                    {
                        button.transform.Find("PP").gameObject.GetComponent<Text>().color = new Color(189f / 255f, 129f / 255f, 0);
                    }
                    else if (percentage < 0.25)
                    {
                        button.transform.Find("PP").gameObject
                            .GetComponent<Text>().color = new Color(193f / 255f, 9f / 255f, 0);
                    }
                    else
                    {
                        button.transform.Find("PP").gameObject.GetComponent<Text>().color = new Color(0, 0, 0);
                    }
                    button.SetActive(true);
                }
                catch (ArgumentOutOfRangeException)
                {
                    Debug.Log("no move bitch!");
                    button.SetActive(false);
                }

            }
        }

        public void HideObject(GameObject gameObject)
        {
            gameObject.SetActive(false);
        }

        public void ShowObject(GameObject gameObject)
        {
            gameObject.SetActive(true);
        }
        
        public void OnBackToMainMenuButtonPress(GameObject currentMenu)
        {
            _mainPanel.SetActive(true);
            currentMenu.SetActive(false);
        }

        public void OnUseMoveButtonPress(int moveNumber)
        { 
            StartCoroutine(AttackTurn(moveNumber));
            EventSystem.current.SetSelectedGameObject(null);
        }

        private IEnumerator SwitchPokemonIfAlive(Pokemon pokemon)
        {
            if (_battle.Player.Pokemons.Exists(p => !p.Fainted))
            {
            
                SetPokemonSwitchTurn(false);
                LoadPokemonMenuInfo();
                _pokemonPanel.SetActive(true);
            }
            else
            {
                yield return EndBattle(pokemon);
            }
            
            yield return null;
        }

        private IEnumerator LowerHp(Pokemon pokemon)
        {
            if (pokemon.Id == _battle.PlayerPokemon.Id)
            {
                yield return LowerPokemonHp(_PlayerPanel, pokemon.CurrentHp, pokemon.MaxHp);
            }
            else
            {
                yield return LowerPokemonHp(_EnemyPanel, pokemon.CurrentHp, pokemon.MaxHp);
            }
        }

        private IEnumerator PokemonFaintedEndBattle(Pokemon pokemon)
        {
            if (!pokemon.Fainted) yield break;
            if (pokemon.Id == _battle.PlayerPokemon.Id)
            {
                yield return PokemonFainted(_PlayerPanel);
            }
            else
            {
                yield return PokemonFainted(_EnemyPanel);
            }
            yield return SwitchPokemonIfAlive(pokemon);
        }

        private IEnumerator AttackTurn(int playerMoveNumber)
        {
            _textPanel.SetActive(true);
            _mainPanel.SetActive(false);

            var first = _battle.FirstAttack(_battle.PlayerPokemon, _battle.WildPokemon ?? _battle.OpponentPokemon);
            var second = _battle.PlayerPokemon;
            var firstMove = _battle.PickRandomMove(_battle.WildPokemon) ??
                            _battle.PickRandomMove(_battle.OpponentPokemon);
            var secondMove = _battle.PlayerPokemon.GetMoves()[playerMoveNumber];

            if (first.Id == _battle.PlayerPokemon.Id)
            {
                second = _battle.WildPokemon ?? _battle.OpponentPokemon;
                secondMove = _battle.PickRandomMove(_battle.WildPokemon) ??
                             _battle.PickRandomMove(_battle.OpponentPokemon); ;
                firstMove = _battle.PlayerPokemon.GetMoves()[playerMoveNumber];
            }

            yield return UseAttack(first, second, firstMove);
            yield return LowerHp(second);
            yield return PokemonFaintedEndBattle(second);
            if (!second.Fainted)
            {
                yield return UseAttack(second, first, secondMove);
                yield return LowerHp(first);
                yield return PokemonFaintedEndBattle(first);

                _textPanel.SetActive(false);
                _infoAttackPanel.transform.parent.gameObject.SetActive(false);
                _mainPanel.SetActive(true);
            }

        }

        public IEnumerator EndBattle(Pokemon pokemon, bool pokemonCatch = false)
        {
            if (!pokemonCatch)
            {
                if (pokemon.Id == _battle.PlayerPokemon.Id)
                {
                    _textPanel.transform.Find("Text").gameObject.GetComponent<Text>().text =
                        pokemon.Name + " has fainted.";
                    yield return WaitForInput();
                    _textPanel.transform.Find("Text").gameObject.GetComponent<Text>().text +=
                        "\nYou lost the battle.";
                }
                else
                {
                    _textPanel.transform.Find("Text").gameObject.GetComponent<Text>().text =
                        pokemon.Name + " has fainted.";
                    yield return WaitForInput();
                    _textPanel.transform.Find("Text").gameObject.GetComponent<Text>().text +=
                        "\nYou won the battle.";
                    yield return WaitForInput();
                }
            }
            
            GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().inBattle = false;
            SceneManager.UnloadSceneAsync("Battle");
        }

        public IEnumerator UseAttack(Pokemon attackPokemon, Pokemon defendPokemon, Move move)
        {
            _textPanel.transform.Find("Text").gameObject.GetComponent<Text>().text =
                attackPokemon.Name + " used " + move.Name;
            yield return WaitForInput();
            var damage = _battle.Attack(attackPokemon, defendPokemon, move);
            _textPanel.transform.Find("Text").gameObject.GetComponent<Text>().text +=
                "\n" + move.Name + " did " + damage + " damage";
            yield return WaitForInput();
        }

        public void OnHighlightButton(int moveNumber)
        {
            _infoAttackPanel.transform.Find("DescriptionText").gameObject.GetComponent<Text>().text = _battle.PlayerPokemon.GetMoves()[moveNumber].Description;
            _infoAttackPanel.transform.Find("PowerText").gameObject.GetComponent<Text>().text = "Power: " + _battle.PlayerPokemon.GetMoves()[moveNumber].BasePower;
            _infoAttackPanel.transform.Find("AccuracyText").gameObject.GetComponent<Text>().text = "Accuracy: " + _battle.PlayerPokemon.GetMoves()[moveNumber].Accuracy;
        }

        public void OnTryFleeButtonPress()
        {
            var text = _textPanel.transform.Find("Text").gameObject.GetComponent<Text>();
            EventSystem.current.SetSelectedGameObject(null);
            StartCoroutine(FleeAnimation(text));
        }

        private IEnumerator WaitForInput()
        {
            _textPanel.transform.Find("ArrowPanel").gameObject.SetActive(false);
            yield return new WaitForSeconds(1f);
            _textPanel.transform.Find("ArrowPanel").gameObject.SetActive(true);
            var pressed = false;
            while (!pressed)
            {
                if (Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.Space))
                {
                    pressed = true;
                }
                yield return null;
            }
            yield return new WaitForSeconds(0.1f);
        }

        private IEnumerator FleeAnimation(Text text)
        {
            _mainPanel.SetActive(false);
            _textPanel.SetActive(true);
            bool flee;
            Exception e = null;
            try
            {
                flee = _battle.Flee(_battle.PlayerPokemon);
                text.text = "You try to run from the battle\n";
            }
            catch (CannotFleeTrainerBattleException ex)
            {
                e = ex;
                flee = false;
                text.text = "You cannot flee from a trainer battle!";
            }
            if (e == null)
            {
                yield return new WaitForSeconds(1.5f);
                text.text += flee ? "You escaped the battle succesfully!" : "But you failed!";
            }
            yield return WaitForInput();
            if (flee)
            {
                yield return new WaitForSeconds(1f);
                GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().inBattle = false;
                SceneManager.UnloadSceneAsync("Battle");
            }
            else
            {
                yield return EnemyMoveEndTurn();
            }

        }
            
        public void LoadPokemonMenuInfo()
        {
            _pokemonPanel.transform.Find("BackButton").gameObject.SetActive(!_battle.PlayerPokemon.Fainted);
            for (var i = 0; i < 6; i++)
            {
                try
                {
                    StartCoroutine(SwitchIconSprites(i, _battle.Player.Pokemons[i].PokedexId));
                    var button = _pokemonPanel.transform.Find("PokemonButton" + i).gameObject;
                    button.SetActive(true);
                    var pokemon = _battle.Player.Pokemons[i];
                    
                    if (pokemon.Fainted)
                    {
                        button.GetComponent<Image>().color = Color.red;
                    }
                    else
                    {
                        button.GetComponent<Image>().color = pokemon.Id == _battle.PlayerPokemon.Id ? Color.blue : new Color(1,1,1,1);
                    }
                

                }
                catch (ArgumentOutOfRangeException)
                {
                    _pokemonPanel.transform.Find("PokemonButton" + i).gameObject.SetActive(false);
                }
            }
        }

        public void SwitchPokemonButton(int pokemonindex)
        {
            //TODO MAKE DIS ENUMERATOR WITH TEXTS IF TIME
            if (_battle.Player.Pokemons[pokemonindex].Fainted)
            {
                return;
            }
            _battle.PlayerPokemon = _battle.Player.Pokemons[pokemonindex];
            SetPokemonInfo(_PlayerPanel, _battle.PlayerPokemon);
            SetPokemonSprite(_PlayerPanel, "PokemonBack/back" + _battle.PlayerPokemon.PokedexId.ToString("000"));
            if (_PokemonSwitchTurn)
            {
                StartCoroutine(EnemyMoveEndTurn());
            }
            else
            {
                //TODO switch pokemon when dead
                _PlayerPanel.transform.Find("Base").transform.Find("Image").transform.localPosition = new Vector3(0, 150, 0);
                _textPanel.SetActive(false);
                _infoAttackPanel.transform.parent.gameObject.SetActive(false);
                _mainPanel.SetActive(true);
            }
        }

        private IEnumerator EnemyMoveEndTurn()
        {
            _textPanel.SetActive(true);
            var pokemon = _battle.WildPokemon ?? _battle.OpponentPokemon;
            var move = _battle.PickRandomMove(_battle.WildPokemon) ??
                       _battle.PickRandomMove(_battle.OpponentPokemon); ;
            yield return UseAttack(pokemon, _battle.PlayerPokemon, move);
            yield return LowerPokemonHp(_PlayerPanel, _battle.PlayerPokemon.CurrentHp, _battle.PlayerPokemon.MaxHp);
            if (_battle.PlayerPokemon.Fainted)
            {
                yield return SwitchPokemonIfAlive(_battle.PlayerPokemon);
            }
            _textPanel.SetActive(false);
            _mainPanel.SetActive(true);
        }

        public void ShowItems(string type)
        {
            var possesionList = new List<Possesion>();
            switch (type)
            {
                case "Pokeball":
                    possesionList = _battle.GetSpecificItem(typeof(Pokeball));
                    break;
                case "Potion":
                    possesionList = _battle.GetSpecificItem(typeof(Potion));
                    break;
                case "Revive":
                    possesionList = _battle.GetSpecificItem(typeof(Revive));
                    break;
                default:
                    return;
            }
            foreach (Transform i in _itemContent.transform)
            {
                Destroy(i.gameObject);
            }
            
            _itemContent.transform.GetComponent<RectTransform>().sizeDelta = new Vector2(0, 300 + 100 * possesionList.Count);
            for (var i = 0; i < possesionList.Count; i++)
            {
                var button = Instantiate(_itemButton, _itemContent.transform);
                var y = -200 - i * 100;
                int x;
                if (i % 2 == 0)
                {
                    x = -400;
                }
                else
                {
                    x = 400;
                }
                button.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(x, y, 0);
                var sprites = Resources.LoadAll<Sprite>("Items/item" + possesionList[i].Item.Id.ToString("000"));
                button.transform.Find("Image").gameObject.GetComponent<Image>().sprite = sprites[0];
                button.transform.Find("Text").gameObject.GetComponent<Text>().text = possesionList[i].Item.Name + "\n (" + possesionList[i].Quantity + "x)";
                var posession = possesionList[i];
                button.GetComponent<Button>().onClick.AddListener(() => { HideObject(button.transform.parent.parent.parent.parent.gameObject);UseItem(posession);});
            }
        }

        private void UseItem(Possesion possesion)
        {
            if (possesion.Item is Potion)
            {
                StartCoroutine(PotionTurn((Potion) possesion.Item));
            } else if (possesion.Item is Pokeball)
            {
                StartCoroutine(PokeballTurn((Pokeball) possesion.Item));
            }
        }

        private IEnumerator PokeballTurn(Pokeball pokeball)
        {
            var succes = _battle.UseItem(pokeball, _battle.WildPokemon);
            _textPanel.SetActive(true);
            if (succes)
            {
                _textPanel.transform.Find("Text").gameObject.GetComponent<Text>().text =
                    "You caught " + _battle.WildPokemon.Name + "!\nI hope you are happy";
                _battle.Player.CatchPokemon(_battle.WildPokemon);
                yield return WaitForInput();
            }
            else
            {
                _textPanel.transform.Find("Text").gameObject.GetComponent<Text>().text =
                    "You failed.\nJust like the rest of your life!";
                yield return WaitForInput();
                yield return EnemyMoveEndTurn();
            }

        }

        private IEnumerator PotionTurn(Potion potion)
        {
            _textPanel.SetActive(true);
            var amount = Mathf.Clamp(potion.HealAmount, 0, _battle.PlayerPokemon.MaxHp - _battle.PlayerPokemon.CurrentHp);
            _textPanel.transform.Find("Text").gameObject.GetComponent<Text>().text = "You used " + potion.Name + ".\nIt heals " + amount + " hp.";
            _battle.UseItem(potion, _battle.PlayerPokemon);
            yield return UpPokemonHp(_PlayerPanel, _battle.PlayerPokemon.CurrentHp, _battle.PlayerPokemon.MaxHp);
            yield return WaitForInput();
            yield return EnemyMoveEndTurn();
        }

        private IEnumerator UpPokemonHp(GameObject panel, int currenthp, int maxhp)
        {
            var newValue = currenthp / (float)maxhp;
            var bar = panel.transform.Find("Stats").transform.Find("HpBar").gameObject.GetComponent<Scrollbar>();
            var text = panel.transform.Find("Stats").transform.Find("HpText").gameObject.GetComponent<Text>();
            while (bar.size < newValue)
            {
                bar.size = bar.size + 0.01f;
                text.text = (int)Mathf.Ceil(maxhp * bar.size) + " / " + maxhp;
                if (bar.size < 0.1f)
                {
                    bar.gameObject.transform.Find("Sliding Area").transform.Find("Handle").gameObject
                        .GetComponent<Image>().color = Color.red;
                }
                else if (bar.size < 0.5f)
                {
                    bar.gameObject.transform.Find("Sliding Area").transform.Find("Handle").gameObject
                        .GetComponent<Image>().color = new Color(0.96f, 0.76f, 0.26f);
                }
                else
                {
                    bar.gameObject.transform.Find("Sliding Area").transform.Find("Handle").gameObject
                        .GetComponent<Image>().color = Color.green;
                }
                yield return new WaitForSeconds(0.05f);
            }
            text.text = currenthp + " / " + maxhp;
        }

        private IEnumerator SwitchIconSprites(int counter, int pokemonId)
        {
            var count = 0;
            var sprites = Resources.LoadAll<Sprite>("PokemonIcons/icon" + pokemonId.ToString("000"));
            yield return sprites;
            while (_pokemonPanel.activeSelf)
            { 
                _pokemonPanel.transform.Find("PokemonButton" + counter).transform.Find("Image").gameObject.GetComponent<Image>().sprite = sprites[count];
                count++;
                count = count % 2;
                yield return new WaitForSeconds(0.25f);
            }
        }

        private IEnumerator LowerPokemonHp(GameObject panel, int currenthp, int maxhp)
        {
            var newValue = currenthp/(float)maxhp;
            var bar = panel.transform.Find("Stats").transform.Find("HpBar").gameObject.GetComponent<Scrollbar>();
            var text = panel.transform.Find("Stats").transform.Find("HpText").gameObject.GetComponent<Text>();
            while (bar.size > newValue)
            {
                bar.size = bar.size - 0.01f;
                text.text = (int)Mathf.Ceil(maxhp * bar.size) + " / " + maxhp;
                if (bar.size < 0.1f)
                {
                    bar.gameObject.transform.Find("Sliding Area").transform.Find("Handle").gameObject
                        .GetComponent<Image>().color = Color.red;
                } else if (bar.size < 0.5f)
                {
                    bar.gameObject.transform.Find("Sliding Area").transform.Find("Handle").gameObject
                        .GetComponent<Image>().color = new Color(0.96f, 0.76f, 0.26f);
                }
                else
                {
                    bar.gameObject.transform.Find("Sliding Area").transform.Find("Handle").gameObject
                        .GetComponent<Image>().color = Color.green;
                }
                yield return new WaitForSeconds(0.05f);
            }
            text.text = currenthp + " / " + maxhp;
        }

        private IEnumerator PokemonFainted(GameObject panel)
        {
            var pokemonTransform = panel.transform.Find("Base").transform.Find("Image").transform;
            var starty = pokemonTransform.localPosition.y;
            while (pokemonTransform.localPosition.y < starty + 100)
            {
                pokemonTransform.localPosition += new Vector3(0, 20, 0);
                yield return new WaitForSeconds(0.001f);
            }
            while (pokemonTransform.localPosition.y > -575)
            {
                pokemonTransform.localPosition += new Vector3(0, -50, 0);
                yield return new WaitForSeconds(0.001f);
            }
        }

        private IEnumerator GetExp(Pokemon defeaded, Pokemon winner)
        {
            var exp = _battle.XpGranted(defeaded, winner);
            exp = _battle.LevelUpCheck(exp, winner);
            
            while (exp > 0)
            {
                //exp full
                //level animation


                exp = _battle.LevelUpCheck(exp, winner);
            }
            yield return null;
        }

        public void SetPokemonSwitchTurn(bool value)
        {
            _PokemonSwitchTurn = value;
        }
    }
}