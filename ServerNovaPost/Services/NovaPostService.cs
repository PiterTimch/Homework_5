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
        public NovaPostService()
        {
            _httpClient = new HttpClient();
            _url = "https://api.novaposhta.ua/v2.0/json/";
            _context = new NovaPostDbContext();
            _context.Database.Migrate();
        }

        private async Task LoadCitiesLocal()
        {
            var cityPostModel = new CityPostModel
            {
                ApiKey = AppDatabase.NovaPostKey
            };

            var cities = await GetApiResponseAsync<CityResponse>(cityPostModel);
            _cityEntities = new List<CityEntity>();

            if (cities?.Data != null && cities.Success)
            {
                foreach (var city in cities.Data)
                {
                    var departments = GetDepartments(city.Ref);

                    var cityEntity = new CityEntity
                    {
                        Ref = city.Ref,
                        Description = city.Description,
                        TypeDescription = city.SettlementTypeDescription,
                        Departments = departments,
                        AreaRef = city.Area
                    };

                    _cityEntities.Add(cityEntity);
                }
            }
        }

        private async Task LoadDepartamentsLocal()
        {
            var departmentPostModel = new DepartmentPostModel
            {
                ApiKey = AppDatabase.NovaPostKey
            };

            var departments = await GetApiResponseAsync<DepartmentResponse>(departmentPostModel);
            _departmentEntities = new List<DepartmentEntity>();

            if (departments?.Data != null && departments.Success)
            {
                foreach (var dep in departments.Data)
                {
                    var departmentEntity = new DepartmentEntity
                    {
                        Ref = dep.Ref,
                        Description = dep.Description,
                        Address = dep.ShortAddress,
                        Phone = dep.Phone,
                        CityRef = dep.CityRef
                    };

                    _departmentEntities.Add(departmentEntity);
                }
            }
        }

        public async Task SeedAreasAsync()
        {
            if (!_context.Areas.Any())
            {
                await LoadDepartamentsLocal();
                await LoadCitiesLocal();

                var modelRequest = new AreaPostModel
                {
                    ApiKey = AppDatabase.NovaPostKey
                };

                var areas = await GetApiResponseAsync<AreaResponse>(modelRequest);

                if (areas?.Data != null && areas.Success)
                {
                    foreach (var area in areas.Data)
                    {
                        var cities = GetCities(area.Ref);

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


        private List<CityEntity> GetCities(string areaRef)
        {
            return _cityEntities.Where(c => c.AreaRef == areaRef).ToList();
        }

        private List<DepartmentEntity> GetDepartments(string cityRef)
        {
            return _departmentEntities.Where(d => d.CityRef == cityRef).ToList();
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


        private readonly HttpClient _httpClient;
        private readonly string _url;
        private readonly NovaPostDbContext _context;
        
        private List<DepartmentEntity> _departmentEntities;
        private List<CityEntity> _cityEntities;
    }
}
