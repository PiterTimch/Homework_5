using Newtonsoft.Json;
using ServerNovaPost.Models.Area;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerNovaPost.Models.Department
{
    public class DepartmentPostModel
    {
        public string ApiKey { get; set; } = string.Empty;
        public string ModelName { get; set; } = "Address";
        public string CalledMethod { get; set; } = "getWarehouses";
        public MethodProperties? MethodProperties { get; set; }
    }

    public class MethodProperties
    {
        public string CityRef { get; set; }
    }
}
