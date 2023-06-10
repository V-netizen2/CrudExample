using Entities;
using ServiceContracts;
using ServiceContracts.DTO;
using System.Net.Http.Headers;

namespace Services
{
    public class CountriesService : ICountriesService
    {
        private readonly ApplicationDbContext _db;
        public CountriesService( ApplicationDbContext personsDbContext)
        {
            _db = personsDbContext;
            
        }
        #region AddCountry
        public CountryResponse AddCountry(CountryAddRequest? countryAddRequest)
        {
            if(countryAddRequest == null)
            {
                throw new ArgumentNullException(nameof(countryAddRequest));
            }
            if(countryAddRequest.CountryName == null)
            {
                throw new ArgumentException(nameof(countryAddRequest.CountryName));
            }
            if(_db.Countries.Count(temp=>temp.CountryName== countryAddRequest.CountryName) > 0)
            {
                throw new ArgumentException("country already exists");
            }
            Country country = countryAddRequest.ToCountry();
            
            country.CountryID = Guid.NewGuid();
            _db.Countries.Add(country);
            _db.SaveChanges();  
            return country.ToCountryResponse();
        }
        #endregion 

        public List<CountryResponse> GetCountries()
        {
            return _db.Countries.Select(country=>country.ToCountryResponse()).ToList();
        }

        public CountryResponse? GetCountryByCountryId(Guid? countryID)
        {
            {
                if (countryID == null)
                    return null;

                Country? country_response_from_list = _db.Countries.FirstOrDefault(temp => temp.CountryID == countryID);

                if (country_response_from_list == null)
                    return null;

                return country_response_from_list.ToCountryResponse();
            }
        }
    }
}