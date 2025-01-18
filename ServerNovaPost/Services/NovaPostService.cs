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

        public async Task SeedAreasAsync()
        {
            if (!_context.Areas.Any())
            {
                var modelRequest = new AreaPostModel
                {
                    ApiKey = AppDatabase.NovaPostKey
                };

                var areas = await GetApiResponseAsync<AreaResponse>(modelRequest);

                if (areas?.Data != null && areas.Success)
                {
                    foreach (var area in areas.Data)
                    {
                        var cities = await SeedCitiesAsync(area.Ref);

                        var areaEntity = new AreaEntity
                        {
                            Ref = area.Ref,
                            AreasCenter = area.AreasCenter,
                            Description = area.Description,
                            Cities = cities
                        };

                        _context.Areas.Add(areaEntity);
                        await _context.SaveChangesAsync();
                    }
                }
            }
        }


        private async Task<List<CityEntity>> SeedCitiesAsync(string areaRef)
        {
            var cityPostModel = new CityPostModel
            {
                ApiKey = AppDatabase.NovaPostKey,
                MethodProperties = new Models.City.MethodProperties { AreaRef = areaRef }
            };

            var cities = await GetApiResponseAsync<CityResponse>(cityPostModel);
            var cityEntities = new List<CityEntity>();

            if (cities?.Data != null && cities.Success)
            {
                foreach (var city in cities.Data)
                {
                    var departments = await SeedDepartmentsAsync(city.Ref);

                    var cityEntity = new CityEntity
                    {
                        Ref = city.Ref,
                        Description = city.Description,
                        TypeDescription = city.SettlementTypeDescription,
                        Departments = departments
                    };

                    _context.Cities.Add(cityEntity);
                    await _context.SaveChangesAsync();
                    cityEntities.Add(cityEntity);
                }
            }

            return cityEntities;
        }

        private async Task<List<DepartmentEntity>> SeedDepartmentsAsync(string cityRef)
        {
            var departmentPostModel = new DepartmentPostModel
            {
                ApiKey = AppDatabase.NovaPostKey,
                MethodProperties = new Models.Department.MethodProperties { CityRef = cityRef }
            };

            var departments = await GetApiResponseAsync<DepartmentResponse>(departmentPostModel);
            var departmentEntities = new List<DepartmentEntity>();

            if (departments?.Data != null && departments.Success)
            {
                foreach (var dep in departments.Data)
                {
                    var departmentEntity = new DepartmentEntity
                    {
                        Ref = dep.Ref,
                        Description = dep.Description,
                        Address = dep.ShortAddress,
                        Phone = dep.Phone
                    };

                    _context.Departments.Add(departmentEntity);
                    await _context.SaveChangesAsync();
                    departmentEntities.Add(departmentEntity);
                }
            }

            return departmentEntities;
        }


        private async Task<T?> GetApiResponseAsync<T>(object model)
        {
            string json = JsonConvert.SerializeObject(model, new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                Formatting = Formatting.Indented
            });

            if (model is CityPostModel)
            {
                json = json.Replace("areaRef", "AreaRef");
            }
            else if (model is DepartmentPostModel)
            {
                json = json.Replace("cityRef", "CityRef");
            }
            try
            {
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync(_url, content);

                if (response.IsSuccessStatusCode)
                {
                    string jsonResp = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<T>(jsonResp);
                }
            }
            catch (Exception)
            {

            }
            

            return default;
        }

    }
}
