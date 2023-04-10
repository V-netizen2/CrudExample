using Entities;
using ServiceContracts;
using ServiceContracts.DTO;
using System.Net.Http.Headers;

namespace Services
{
    public class CountriesService : ICountriesService
    {
        private readonly IList<Country> _countries;
        public CountriesService()
        {
            _countries = new List<Country>();
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
            if(_countries.Where(temp=>temp.CountryName== countryAddRequest.CountryName).Count() > 0)
            {
                throw new ArgumentException("country already exists");
            }
            Country country = countryAddRequest.ToCountry();
            
            country.CountryID = Guid.NewGuid();
            _countries.Add(country);
            return country.ToCountryResponse();
        }
        #endregion 

        public List<CountryResponse> GetCountries()
        {
            return _countries.Select(country=>country.ToCountryResponse()).ToList();
        }

        public CountryResponse? GetCountryByCountryId(Guid? countryID)
        {
            {
                if (countryID == null)
                    return null;

                Country? country_response_from_list = _countries.FirstOrDefault(temp => temp.CountryID == countryID);

                if (country_response_from_list == null)
                    return null;

                return country_response_from_list.ToCountryResponse();
            }
        }
    }
}