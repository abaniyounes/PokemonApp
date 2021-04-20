using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PokemoneApp.Models;


namespace PokemoneApp
{
    class Program
    {
       
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            string baseUrl = "https://pokeapi.co/api/v2/pokemon/";
            GetPokemonAsync(baseUrl);
            Console.Write("Enter pokemon name or enter pokeout to close the app: ");
            String pokeName = Console.ReadLine();
            while ((pokeName.Trim().ToLower() != "pokeout") || string.IsNullOrEmpty(pokeName.Trim()))
            {
                
                int value;
                if(int.TryParse(pokeName , out value) || string.IsNullOrWhiteSpace(pokeName))
                {
                    Console.WriteLine("You enterd numerical/empty value please enter name only!");
                    Console.Write("Enter pokemon name or enter pokeout to close the app: ");
                    pokeName = Console.ReadLine();
                }
                else if(!pokemons.ContainsKey(pokeName))
                {
                    Console.Write("Pokemon name does not exist please enter correct pokemon name or enter pokeout to close the app: ");
                    pokeName = Console.ReadLine();
                }
                else
                {
                    break;
                }
            }
            if (pokeName.ToLower() == "pokeout")
            {
                Environment.Exit(0);
            } 
            else
            {

               Pokemon pokeObj = pokemons[pokeName];
               GetPokemonByName(pokeObj.Name, pokeObj.Url);
            }
            
            
        }
        public static Dictionary<string, Pokemon> pokemons = new Dictionary<string, Pokemon>();

        public static async Task<poke> GetPokemonDataAsync(string baseUrl)
        {

            poke pokemonData = new poke();
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
                                pokemonData = JsonConvert.DeserializeObject<poke>(data);


                               
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

            return pokemonData;
        }
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
                               var abilities =  poke.abilities.Where(a => a != null).Select(ab => ab.ability.name).ToList();


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
    }
}
