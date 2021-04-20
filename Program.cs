using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Colorful;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PokemonApp.Models;
using Console = Colorful.Console;

namespace PokemonApp
{
    public static class Program
    {
       
        static void Main(string[] args)
        {
            Figlet figlet = new Figlet();
            Console.WriteLine(figlet.ToAscii("Pokemon"), Color.FromArgb(67, 144, 198));
            string baseUrl = "https://pokeapi.co/api/v2/pokemon/";
            GetPokemonAsync(baseUrl);
            
            String pokeName = "";
            while (true)
            {
                Console.WriteLine("Type pokeList to get the full list of Pokemons.");
                Console.Write("Enter pokemon name or enter pokeout to close the app: ");
                pokeName = Console.ReadLine();
                if (pokeName.ToLower() == "pokeout")
                {
                    Environment.Exit(0);
                }
                else
                {
                    int value;
                    if (int.TryParse(pokeName, out value) || string.IsNullOrWhiteSpace(pokeName))
                    {
                        Console.WriteLine("You enterd numerical/empty value please enter name only!");
                    }
                    else if (pokeName.ToLower() == "pokelist")
                    {
                        GetPokemonAsync(baseUrl);
                    }
                    else if (!pokemons.ContainsKey(pokeName))
                    {
                        Console.WriteLine("Pokemon name does not exist please enter correct pokemon name or enter pokeout to close the app: ");
                        
                    }
                    else
                    {
                        Pokemon pokeObj = pokemons[pokeName];
                        GetPokemonByName(pokeObj.Name, pokeObj.Url);
                        Console.WriteLine();
                    }
                }

            }
            
            
        }

        public static Dictionary<string, Pokemon> pokemons = new Dictionary<string, Pokemon>();

        #region Active Methods

        /// <summary>
        /// get a full list of pokemons
        /// 
        /// </summary>
        /// <param name="baseUrl"></param>
        public static async void GetPokemonAsync(string baseUrl)
        {
            

            #region Getting all the api data
            try
            {
                using (var client = new HttpClient())
                {
                    using (HttpResponseMessage respo = client.GetAsync(baseUrl).Result)
                    {

                        using (HttpContent content = respo.Content)
                        {
                            var data = await content.ReadAsStringAsync();
                            if (data != null)
                            {
                                if(pokemons.Count > 0)
                                {
                                    pokemons.Clear();
                                }
                                var dataObj = JObject.Parse(data)["results"];
                                var dataObj2 = JsonConvert.DeserializeObject<poke>(data);

                                foreach (var item in dataObj)
                                {
                                    Pokemon pokemon = new Pokemon();
                                    pokemon.Name = item["name"].ToString();
                                    pokemon.Url = item["url"].ToString();

                                    pokemons.Add(pokemon.Name, pokemon);
                                }
                                Console.WriteLine("------Pokemon List------");
                                int i = 1;
                                foreach (var item in pokemons)
                                {
                                    Console.WriteLine("{0,3} - " + item.Value.Name, i);
                                    i++;
                                }
                            }
                            else
                            {
                                Console.WriteLine("NO Data----------?");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                Console.WriteLine("OMG we got an Exception ------------------");
                Console.WriteLine(ex);
            }
            #endregion
        }

        /// <summary>
        /// get the pokemon object by name
        /// 
        /// </summary>
        /// <param name="pokeName"></param>
        /// <param name="baseUrl"></param>
        public static async void GetPokemonByName(string pokeName, string baseUrl)
        {

            #region Getting all the api data
            try
            {

                using (var client = new HttpClient())
                {
                    using (HttpResponseMessage respo = client.GetAsync(baseUrl).Result)
                    {

                        using (HttpContent content = respo.Content)
                        {
                            var data = await content.ReadAsStringAsync();
                            if (data != null)
                            {
                                var dataObj = JObject.Parse(data)["abilities"];
                                
                                
                                poke poke = JsonConvert.DeserializeObject<poke>(data);
                               var abilities =  poke.abilities.Where(a => a != null).ToList();
                               Console.WriteLine("{0} - Abilities are: ",pokeName);
                                foreach (var item in abilities)
                                {
                                    Console.WriteLine(" * {0,3}",item.ability.name);
                                    GetPokemonAbility(pokeName, item.ability.url);
                                }

                            }
                            else
                            {
                                Console.WriteLine("NO Data----------?");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                Console.WriteLine("OMG we got an Exception ------------------");
                Console.WriteLine(ex);
            }
            #endregion

        }

        /// <summary>
        /// get the pokemon abilities 
        /// 
        /// </summary>
        /// <param name="pokeName"></param>
        /// <param name="baseUrl"></param>
        public static async void GetPokemonAbility(string pokeName, string baseUrl)
        {

            #region Getting all the api data
            try
            {

                using (var client = new HttpClient())
                {
                    using (HttpResponseMessage respo = client.GetAsync(baseUrl).Result)
                    {

                        using (HttpContent content = respo.Content)
                        {
                            var data = await content.ReadAsStringAsync();
                            if (data != null)
                            {
                                var dataObj = JObject.Parse(data);
                                var enDataObj = dataObj.Properties().ToList();
                               
                                var pokemonEffects = JsonConvert.DeserializeObject<PokemonAbility.Ability>(dataObj.ToString());

                                foreach (var item in pokemonEffects.effect_entries)
                                {
                                    if(item.language.name== "en")
                                    {
                                        Console.WriteLine("       - {0,-10} ", item.effect);
                                    }
                                }
                                
                            }
                            else
                            {
                                Console.WriteLine("NO Data----------?");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                Console.WriteLine("OMG we got an Exception ------------------");
                Console.WriteLine(ex);
            }
            #endregion

        }
        
        #endregion
    }
}
