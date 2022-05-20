using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace OscVrcMaui
{
  public static class Serealization
    {
        public static string SerializeToString(this object obj)
        {
            var JsonSerializer = JsonConvert.SerializeObject((obj), Formatting.Indented);

            return JsonSerializer;
        }

        public static T DeserializeString<T>(this string sourceString)
        {

            T JsonDesel = JsonConvert.DeserializeObject<T>(sourceString);
            return JsonDesel;
        }

    }
}
