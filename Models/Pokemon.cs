using System;
using System.Collections.Generic;
using System.Text;

namespace PokemoneApp.Models
{
    public class Pokemon
    {
        public string Name { get; set; }
        public string Url { get; set; }
        public Pokemon()
        {

        }     
        /// <summary>
        ///  pass a name and Url
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="url"></param>
        public Pokemon(string name, string url)
        {
            this.Name = name;
        }

        public Pokemon(string name)
        {
            Name = name;
        }
    }
}
