using ServerNovaPost.Models.Area;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerNovaPost.Models.City
{
    public class CityPostModel
    {
        public string ApiKey { get; set; } = string.Empty;
        public string ModelName { get; set; } = "Address";
        public string CalledMethod { get; set; } = "getCities";
        public MethodProperties? MethodProperties { get; set; }
    }
}
