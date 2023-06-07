using Entities;
using ServiceContracts;
using ServiceContracts.DTO;
using System.Net.Http.Headers;

namespace Services
{
    public class CountriesService : ICountriesService
    {
        private readonly List<Country> _countries;
        public CountriesService(bool initialize = true)
        {
            _countries = new List<Country>();
            if(initialize)
            {
                _countries.AddRange(new List<Country>()
                {
                    new Country(){CountryID=Guid.Parse("14E9C342-FB8A-4AFE-903B-635BDB915814"),CountryName="USA"},
                    new Country(){CountryID=Guid.Parse("8A1CFF79-DAA6-4D97-A06F-21BF9EFE8305"),CountryName="Canada"},
                    new Country(){CountryID=Guid.Parse("E122021C-E5CB-4DF2-A80C-D2D27BCE9A37"),CountryName="UK"},
                    new Country(){CountryID=Guid.Parse("F885B612-A1A0-499A-AFB2-9A2009446396"),CountryName="Australia"},
                    new Country(){CountryID=Guid.Parse("DF7C89CE-3341-4246-84AE-E01AB7BA476E"),CountryName="India"}
                }); 
 
                //8A1CFF79-DAA6-4D97-A06F-21BF9EFE8305
                //E122021C-E5CB-4DF2-A80C-D2D27BCE9A37
                //E122021C-E5CB-4DF2-A80C-D2D27BCE9A37
                //F885B612-A1A0-499A-AFB2-9A2009446396
            }
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