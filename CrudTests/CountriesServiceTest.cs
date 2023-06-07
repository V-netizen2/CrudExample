using ServiceContracts;
using ServiceContracts.DTO;
using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace CrudTests
{
    public class CountriesServiceTest
    {
        private readonly ICountriesService _countriesService;

        public CountriesServiceTest()
        {
            _countriesService = new CountriesService(false);
        }
        #region AddCountry
        [Fact]
        public void AddCountry_NullCountry()
        {
            CountryAddRequest? request= null;
            Assert.Throws<ArgumentNullException>(() =>
            {
                _countriesService.AddCountry(request);
            });
        }
        [Fact]
        public void AddCountry_CountryNameIsNull()
        {
            CountryAddRequest? request = new CountryAddRequest()
            {
                CountryName = null
            };
            Assert.Throws<ArgumentException>(() =>
            {
                _countriesService.AddCountry(request);
            });
        }
        [Fact]
        public void AddCountry_DuplicateCountryName()
        {
            CountryAddRequest? request1 = new CountryAddRequest()
            {
                CountryName = "USA"
            };
            CountryAddRequest? request2 = new CountryAddRequest()
            {
                CountryName = "USA"
            };

            Assert.Throws<ArgumentException>(() =>
            {
                _countriesService.AddCountry(request1);
                _countriesService.AddCountry(request2);
            });
        }
        [Fact]
        public void AddCountry_ProperCountryDetails()
        {
            CountryAddRequest? request1 = new CountryAddRequest()
            {
                CountryName = "USA"
            };
            CountryResponse response = _countriesService.AddCountry(request1);
            List<CountryResponse> countries_from_getallcountries = _countriesService.GetCountries();
           
            Assert.True(response.CountryID != Guid.Empty);
            Assert.Contains(response, countries_from_getallcountries);

           
        }
        #endregion

        #region GetAllCountry

        [Fact]
        public void GetAllCountries_EmptyList()
        {
            List<CountryResponse> actual_country_response_list = _countriesService.GetCountries();
            Assert.Empty(actual_country_response_list);
        }
        [Fact]
        public void GetAllCountry_AddFewCountries()
        {
            List<CountryAddRequest> country_request_list = new List<CountryAddRequest>();
            CountryAddRequest request1 = new CountryAddRequest()
            {
                CountryName = "USA"
            };
            CountryAddRequest request2 = new CountryAddRequest() { CountryName = "UK" };
            country_request_list.Add(request1);
            country_request_list.Add(request2);
            List<CountryResponse> countries_list_from_add_country = new
                List<CountryResponse>();
            foreach(var country_request in country_request_list)
            {
                countries_list_from_add_country.Add(_countriesService.AddCountry(country_request));
            }

            List<CountryResponse> actualCountryResponseList = _countriesService.GetCountries();
            //read each country from countries_list_from_country 
            foreach(CountryResponse expected_country in countries_list_from_add_country)
            {
                Assert.Contains(expected_country, actualCountryResponseList);
            }


        }
        #endregion

        [Fact]
        public void GetCountryByCountryId_NullCountryId()
        {
            
            Guid countryid = Guid.Empty;

            CountryResponse? countryResponse = _countriesService.GetCountryByCountryId(countryid);
            Assert.Null(countryResponse);
        }
        [Fact]
        public void GetCountryByCountryId_ValidCountryId()
        {
            //arrange
            CountryAddRequest? country_add_request = new CountryAddRequest() { CountryName = "China" };
            CountryResponse response = _countriesService.AddCountry(country_add_request);
            //act
            CountryResponse response1 = _countriesService.GetCountryByCountryId(response.CountryID);
            //Assert
            Assert.Equal(response, response1);
           
        }
        
        


    }
}
