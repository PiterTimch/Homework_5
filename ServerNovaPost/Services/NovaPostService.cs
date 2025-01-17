using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using ServerNovaPost.Data.Entities;
using ServerNovaPost.Models.Area;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServerNovaPost.Data;
using Microsoft.EntityFrameworkCore;
using ServerNovaPost.Constants;
using ServerNovaPost.Models.City;
using ServerNovaPost.Models.Department;

namespace ServerNovaPost.Services
{
    public class NovaPostService
    {
        private readonly HttpClient _httpClient;
        private readonly string _url;
        private readonly NovaPostDbContext _context;

        public NovaPostService()
        {
            _httpClient = new HttpClient();
            _url = "https://api.novaposhta.ua/v2.0/json/";
            _context = new NovaPostDbContext();
            _context.Database.Migrate();
        }

        public void SeedAreas()
        {
            if (!_context.Areas.Any())
            {
                var modelRequest = new AreaPostModel
                {
                    ApiKey = AppDatabase.NovaPostKey,
                    ModelName = "Address",
                    CalledMethod = "getAreas",
                    MethodProperties = new Models.Area.MethodProperties() { }
                };

                string json = JsonConvert.SerializeObject(modelRequest, new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver(),
                    Formatting = Formatting.Indented
                });
                HttpContent context = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response = _httpClient.PostAsync(_url, context).Result;
                if (response.IsSuccessStatusCode)
                {
                    string jsonResp = response.Content.ReadAsStringAsync().Result;
                    var result = JsonConvert.DeserializeObject<AreaResponse>(jsonResp);
                    if (result != null && result.Data != null && result.Success)
                    {
                        foreach (var item in result.Data)
                        {
                            var entity = new AreaEntity
                            {
                                Ref = item.Ref,
                                AreasCenter = item.AreasCenter,
                                Description = item.Description,
                                Cities = SeedCities(item.Ref)
                            };
                            _context.Areas.Add(entity);
                            _context.SaveChanges();
                        }
                    }
                }
            }
        }

        private List<CityEntity> SeedCities(string areaRef)
        {
            var cityPostModel = new CityPostModel
            {
                ApiKey = AppDatabase.NovaPostKey,
                MethodProperties = new Models.Area.MethodProperties() { }
            };

            string json = JsonConvert.SerializeObject(cityPostModel, new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                Formatting = Formatting.Indented
            });

            HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");
            HttpResponseMessage response = _httpClient.PostAsync(_url, content).Result;

            var cityEntities = new List<CityEntity>();

            if (response.IsSuccessStatusCode)
            {
                string jsonResp = response.Content.ReadAsStringAsync().Result;
                var cityResult = JsonConvert.DeserializeObject<CityResponse>(jsonResp);
                if (cityResult != null && cityResult.Data != null && cityResult.Success)
                {
                    foreach (var cityItem in cityResult.Data.Where(c => c.Area == areaRef))
                    {
                        var cityEntity = new CityEntity
                        {
                            Ref = cityItem.Ref,
                            Description = cityItem.Description,
                            TypeDescription = cityItem.SettlementTypeDescription,
                            Departments = SeedDepartments(cityItem.Ref)
                        };
                        cityEntities.Add(cityEntity);
                        _context.SaveChanges();
                    }
                }
            }

            return cityEntities;
        }

        private List<DepartmentEntity> SeedDepartments(string cityRef)
        {
            var departmentPostModel = new DepartmentPostModel
            {
                ApiKey = AppDatabase.NovaPostKey,
                MethodProperties = new Models.Department.MethodProperties { CityRef = cityRef }
            };

            string json = JsonConvert.SerializeObject(departmentPostModel, new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                Formatting = Formatting.Indented
            });

            HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");
            HttpResponseMessage response = _httpClient.PostAsync(_url, content).Result;

            var departmentEntities = new List<DepartmentEntity>();

            if (response.IsSuccessStatusCode)
            {
                string jsonResp = response.Content.ReadAsStringAsync().Result;
                var departmentResult = JsonConvert.DeserializeObject<DepartmentResponse>(jsonResp);

                if (departmentResult != null && departmentResult.Data != null && departmentResult.Success)
                {
                    foreach (var departmentItem in departmentResult.Data)
                    {
                        var departmentEntity = new DepartmentEntity
                        {
                            Ref = departmentItem.Ref,
                            Description = departmentItem.Description,
                            Address = departmentItem.ShortAddress,
                            Phone = departmentItem.Phone
                        };
                        departmentEntities.Add(departmentEntity);
                        _context.Departments.Add(departmentEntity);
                    }
                    _context.SaveChanges();
                }
            }

            return departmentEntities;
        }

    }
}
