using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerNovaPost.Models.Area
{
    public class AreaPostModel
    {
        public string ApiKey { get; set; } = string.Empty;
        public string ModelName { get; set; } = "Address";
        public string CalledMethod { get; set; } = "getAreas";

        public MethodProperties? MethodProperties { get; set; }
    }
}
