using Entities;
using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.Enums;
using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace CrudTests
{
    public class PersonsServiceTests
    {
        private readonly IPersonsService _personService;
        private readonly ICountriesService _countriesService;
        private readonly ITestOutputHelper _testOutputHelper;

        public PersonsServiceTests(ITestOutputHelper testOutputHelper)
        {
            _personService = new PersonsService();
            _countriesService = new CountriesService();
            _testOutputHelper = testOutputHelper;
        }
        #region AddPerson
        [Fact]
        public void AddPerson_NullPerson()
        {
            PersonAddRequest? request = null;
            Assert.Throws<ArgumentNullException>(() =>
            {
                _personService.AddPerson(request);

            });
        }
        [Fact]
        public void AddPerson_PersonNameNull()
        {
            PersonAddRequest request = new PersonAddRequest();
            request.PersonName = null;
            Assert.Throws<ArgumentException>(() =>
            {
                _personService.AddPerson(request);
            });
        }
        [Fact]
        public void AddPerson_ProperPersonDetails()
        {
            //Arrange
            PersonAddRequest? personAddRequest = new PersonAddRequest() { PersonName = "Person name...", Email = "person@example.com", Address = "sample address", CountryID = Guid.NewGuid(), Gender = GenderOptions.Male, DateOfBirth = DateTime.Parse("2000-01-01"), RecieveNewsLetters = true };

            //Act
            PersonResponse person_response_from_add = _personService.AddPerson(personAddRequest);

            List<PersonResponse> persons_list = _personService.GetAllPersons();

            //Assert
            Assert.True(person_response_from_add.PersonID != Guid.Empty);

            Assert.Contains(person_response_from_add, persons_list);
        }
        #endregion
        #region GetPerson
        [Fact]
        public void GetPersonByPersonID_NullPersonID()
        {
            Guid? personID = null;
            PersonResponse? person_response_from_get = _personService.GetPersonByPersonID(personID);
            Assert.Null(person_response_from_get);


        }
        [Fact]
        public void GetPersonByPersonID_WithPersonID()
        {
            //Arange
            CountryAddRequest country_request = new CountryAddRequest() { CountryName = "Canada" };
            CountryResponse country_response = _countriesService.AddCountry(country_request);

            PersonAddRequest person_request = new PersonAddRequest() { PersonName = "person name...", Email = "email@sample.com", Address = "address", CountryID = country_response.CountryID, DateOfBirth = DateTime.Parse("2000-01-01"), Gender = GenderOptions.Male, RecieveNewsLetters = false };

            PersonResponse person_response_from_add = _personService.AddPerson(person_request);

            PersonResponse? person_response_from_get = _personService.GetPersonByPersonID(person_response_from_add.PersonID);

            //Assert
            Assert.Equal(person_response_from_add, person_response_from_get);
        }
        #endregion
        #region GetAllPersons
        [Fact]
        public void GetAllPersons_EmptyList()
        {
            List<PersonResponse> persons_from_get = _personService.GetAllPersons();
            Assert.Empty(persons_from_get);
        }
        [Fact]
        public void GetAllPersons_AddFewPersons()
        {
            //Arrange
            CountryAddRequest country_request_1 = new CountryAddRequest() { CountryName = "USA" };
            CountryAddRequest country_request_2 = new CountryAddRequest() { CountryName = "India" };

            CountryResponse country_response_1 = _countriesService.AddCountry(country_request_1);
            CountryResponse country_response_2 = _countriesService.AddCountry(country_request_2);

            PersonAddRequest person_request_1 = new PersonAddRequest() { PersonName = "Smith", Email = "smith@example.com", Gender = GenderOptions.Male, Address = "address of smith", CountryID = country_response_1.CountryID, DateOfBirth = DateTime.Parse("2002-05-06"), RecieveNewsLetters = true };

            PersonAddRequest person_request_2 = new PersonAddRequest() { PersonName = "Mary", Email = "mary@example.com", Gender = GenderOptions.Female, Address = "address of mary", CountryID = country_response_2.CountryID, DateOfBirth = DateTime.Parse("2000-02-02"), RecieveNewsLetters = false };

            PersonAddRequest person_request_3 = new PersonAddRequest() { PersonName = "Rahman", Email = "rahman@example.com", Gender = GenderOptions.Male, Address = "address of rahman", CountryID = country_response_2.CountryID, DateOfBirth = DateTime.Parse("1999-03-03"), RecieveNewsLetters = true };

            List<PersonAddRequest> person_requests = new List<PersonAddRequest>() { person_request_1, person_request_2, person_request_3 };

            List<PersonResponse> person_response_list_from_add = new List<PersonResponse>();

            foreach (PersonAddRequest person_request in person_requests)
            {
                PersonResponse person_response = _personService.AddPerson(person_request);
                person_response_list_from_add.Add(person_response);
            }
            //print person_response_list_from_add
            _testOutputHelper.WriteLine("Expected");
            foreach(PersonResponse person_response_from_add in person_response_list_from_add)
            {
                _testOutputHelper.WriteLine(person_response_from_add.ToString());
            }

            //Act
            List<PersonResponse> persons_list_from_get = _personService.GetAllPersons();
            //print person_list_from_get
            _testOutputHelper.WriteLine("Actual");
            foreach (PersonResponse person_response_from_get in persons_list_from_get)
            {
                _testOutputHelper.WriteLine(person_response_from_get.ToString());
            }

            //Assert
            foreach (PersonResponse person_response_from_add in person_response_list_from_add)
            {
                Assert.Contains(person_response_from_add, persons_list_from_get);
            }
        }
        #endregion

        #region GetFilteredPersons
        [Fact]
        public void GetFilteredPersons_EmptySearchText()
        {
            //Arrange
            CountryAddRequest country_request_1 = new CountryAddRequest() { CountryName = "USA" };
            CountryAddRequest country_request_2 = new CountryAddRequest() { CountryName = "India" };

            CountryResponse country_response_1 = _countriesService.AddCountry(country_request_1);
            CountryResponse country_response_2 = _countriesService.AddCountry(country_request_2);

            PersonAddRequest person_request_1 = new PersonAddRequest() { PersonName = "Smith", Email = "smith@example.com", Gender = GenderOptions.Male, Address = "address of smith", CountryID = country_response_1.CountryID, DateOfBirth = DateTime.Parse("2002-05-06"), RecieveNewsLetters = true };

            PersonAddRequest person_request_2 = new PersonAddRequest() { PersonName = "Mary", Email = "mary@example.com", Gender = GenderOptions.Female, Address = "address of mary", CountryID = country_response_2.CountryID, DateOfBirth = DateTime.Parse("2000-02-02"), RecieveNewsLetters = false };

            PersonAddRequest person_request_3 = new PersonAddRequest() { PersonName = "Rahman", Email = "rahman@example.com", Gender = GenderOptions.Male, Address = "address of rahman", CountryID = country_response_2.CountryID, DateOfBirth = DateTime.Parse("1999-03-03"), RecieveNewsLetters = true };

            List<PersonAddRequest> person_requests = new List<PersonAddRequest>() { person_request_1, person_request_2, person_request_3 };

            List<PersonResponse> person_response_list_from_add = new List<PersonResponse>();

            foreach (PersonAddRequest person_request in person_requests)
            {
                PersonResponse person_response = _personService.AddPerson(person_request);
                person_response_list_from_add.Add(person_response);
            }
            //print person_response_list_from_add
            _testOutputHelper.WriteLine("Expected");
            foreach (PersonResponse person_response_from_add in person_response_list_from_add)
            {
                _testOutputHelper.WriteLine(person_response_from_add.ToString());
            }

            //Act
            List<PersonResponse> persons_list_from_search = _personService.GetFilteredPersons(nameof(Person.PersonName),"");
            //print person_list_from_get
            _testOutputHelper.WriteLine("Actual");
            foreach (PersonResponse person_response_from_get in persons_list_from_search)
            {
                _testOutputHelper.WriteLine(person_response_from_get.ToString());
            }

            //Assert
            foreach (PersonResponse person_response_from_add in person_response_list_from_add)
            {
                Assert.Contains(person_response_from_add, persons_list_from_search);
            }
        }

        [Fact]
        public void GetFilteredPersons_SearchByPersonName()
        {
            //Arrange
            CountryAddRequest country_request_1 = new CountryAddRequest() { CountryName = "USA" };
            CountryAddRequest country_request_2 = new CountryAddRequest() { CountryName = "India" };

            CountryResponse country_response_1 = _countriesService.AddCountry(country_request_1);
            CountryResponse country_response_2 = _countriesService.AddCountry(country_request_2);

            PersonAddRequest person_request_1 = new PersonAddRequest() { PersonName = "Smith", Email = "smith@example.com", Gender = GenderOptions.Male, Address = "address of smith", CountryID = country_response_1.CountryID, DateOfBirth = DateTime.Parse("2002-05-06"), RecieveNewsLetters = true };

            PersonAddRequest person_request_2 = new PersonAddRequest() { PersonName = "Mary", Email = "mary@example.com", Gender = GenderOptions.Female, Address = "address of mary", CountryID = country_response_2.CountryID, DateOfBirth = DateTime.Parse("2000-02-02"), RecieveNewsLetters = false };

            PersonAddRequest person_request_3 = new PersonAddRequest() { PersonName = "Rahman", Email = "rahman@example.com", Gender = GenderOptions.Male, Address = "address of rahman", CountryID = country_response_2.CountryID, DateOfBirth = DateTime.Parse("1999-03-03"), RecieveNewsLetters = true };

            List<PersonAddRequest> person_requests = new List<PersonAddRequest>() { person_request_1, person_request_2, person_request_3 };

            List<PersonResponse> person_response_list_from_add = new List<PersonResponse>();

            foreach (PersonAddRequest person_request in person_requests)
            {
                PersonResponse person_response = _personService.AddPerson(person_request);
                person_response_list_from_add.Add(person_response);
            }
            //print person_response_list_from_add
            _testOutputHelper.WriteLine("Expected");
            foreach (PersonResponse person_response_from_add in person_response_list_from_add)
            {
                _testOutputHelper.WriteLine(person_response_from_add.ToString());
            }

            //Act
            List<PersonResponse> persons_list_from_search = _personService.GetFilteredPersons(nameof(Person.PersonName), "ma");
            //print person_list_from_get
            _testOutputHelper.WriteLine("Actual");
            foreach (PersonResponse person_response_from_get in persons_list_from_search)
            {
                _testOutputHelper.WriteLine(person_response_from_get.ToString());
            }

            //Assert
            foreach (PersonResponse person_response_from_add in person_response_list_from_add)
            {
                if (person_response_from_add.PersonName != null)
                {
                    if (person_response_from_add.PersonName.Contains("ma", StringComparison.OrdinalIgnoreCase))
                    {
                        Assert.Contains(person_response_from_add, persons_list_from_search);
                    }

                }
               
                
            }
        }
        #endregion
        #region GetSortedPersons
        [Fact]
        public void GetSortedPersons()
        {
            //Arrange
            CountryAddRequest country_request_1 = new CountryAddRequest() { CountryName = "USA" };
            CountryAddRequest country_request_2 = new CountryAddRequest() { CountryName = "India" };

            CountryResponse country_response_1 = _countriesService.AddCountry(country_request_1);
            CountryResponse country_response_2 = _countriesService.AddCountry(country_request_2);

            PersonAddRequest person_request_1 = new PersonAddRequest() { PersonName = "Smith", Email = "smith@example.com", Gender = GenderOptions.Male, Address = "address of smith", CountryID = country_response_1.CountryID, DateOfBirth = DateTime.Parse("2002-05-06"), RecieveNewsLetters = true };

            PersonAddRequest person_request_2 = new PersonAddRequest() { PersonName = "Mary", Email = "mary@example.com", Gender = GenderOptions.Female, Address = "address of mary", CountryID = country_response_2.CountryID, DateOfBirth = DateTime.Parse("2000-02-02"), RecieveNewsLetters = false };

            PersonAddRequest person_request_3 = new PersonAddRequest() { PersonName = "Rahman", Email = "rahman@example.com", Gender = GenderOptions.Male, Address = "address of rahman", CountryID = country_response_2.CountryID, DateOfBirth = DateTime.Parse("1999-03-03"), RecieveNewsLetters = true };

            List<PersonAddRequest> person_requests = new List<PersonAddRequest>() { person_request_1, person_request_2, person_request_3 };

            List<PersonResponse> person_response_list_from_add = new List<PersonResponse>();

            foreach (PersonAddRequest person_request in person_requests)
            {
                PersonResponse person_response = _personService.AddPerson(person_request);
                person_response_list_from_add.Add(person_response);
            }
            //print person_response_list_from_add
            _testOutputHelper.WriteLine("Expected");
            foreach (PersonResponse person_response_from_add in person_response_list_from_add)
            {
                _testOutputHelper.WriteLine(person_response_from_add.ToString());
            }

            //Act
            List<PersonResponse> allPersons = _personService.GetAllPersons();
            List<PersonResponse> persons_list_from_sort = _personService.GetSortedPersons(allPersons,nameof(Person.PersonName), SortOrderOptions.DESC);
            //print person_list_from_get
            _testOutputHelper.WriteLine("Actual");
            foreach (PersonResponse person_response_from_get in persons_list_from_sort)
            {
                _testOutputHelper.WriteLine(person_response_from_get.ToString());
            }
            person_response_list_from_add = person_response_list_from_add.OrderByDescending(tem => tem.PersonName).ToList();

            //Assert
            for (int i = 0; i < person_response_list_from_add.Count; i++)
            {
                Assert.Equal(person_response_list_from_add[i], persons_list_from_sort[i]);
            }
        }
        #endregion
        #region UpdatePerson
        [Fact]
        public void UpdatePerson_NullPerson()
        {
            PersonUpdateRequest? personUpdateRequest = null;
            Assert.Throws<ArgumentNullException>(()=>_personService.UpdatePerson(personUpdateRequest));
        }
        [Fact]
        public void UpdatePerson_InValidPersonID()
        {
            PersonUpdateRequest? personUpdateRequest = new PersonUpdateRequest { PersonID = Guid.NewGuid()};
            Assert.Throws<ArgumentException>(() => _personService.UpdatePerson(personUpdateRequest));
        }
        [Fact]
        public void UpdatePerson_PersonNameIsNull()
        {
            CountryAddRequest country_add_request = new CountryAddRequest() { CountryName = "uk" };
            CountryResponse country_response_from_add = _countriesService.AddCountry(country_add_request);
            PersonAddRequest person_add_request = new PersonAddRequest() { PersonName = "john", CountryID = country_response_from_add.CountryID };
            PersonResponse person_response_from_add = _personService.AddPerson(person_add_request);
            PersonUpdateRequest person_update_request = person_response_from_add.ToPersonUpdateRequest();
            person_update_request.PersonName = null;

            Assert.Throws<ArgumentException>(() =>
            {
                _personService.UpdatePerson(person_update_request);
            });
        }
        [Fact]
        public void UpdatePerson_PersonFullDetails()
        {
            CountryAddRequest country_add_request = new CountryAddRequest() { CountryName = "uk" };
            CountryResponse country_response_from_add = _countriesService.AddCountry(country_add_request);
            PersonAddRequest person_add_request = new PersonAddRequest() { PersonName = "John", CountryID = country_response_from_add.CountryID, Address = "Abc road", DateOfBirth = DateTime.Parse("2000-01-01"), Email = "abc@example.com", Gender = GenderOptions.Male, RecieveNewsLetters = true };
            PersonResponse person_response_from_add = _personService.AddPerson(person_add_request);
            PersonUpdateRequest person_update_request = person_response_from_add.ToPersonUpdateRequest();
            // person_update_request.PersonName = null;

            PersonResponse person_response_from_update = _personService.UpdatePerson(person_update_request);
            PersonResponse person_response_from_get = _personService.GetPersonByPersonID(person_response_from_update.PersonID);
            Assert.Equal(person_response_from_get, person_response_from_update);
           
        }





        #endregion
        #region DeletePerson
        [Fact]
        public void DeletePerson_validPersonID()
        {

            CountryAddRequest country_add_request = new CountryAddRequest() { CountryName = "rs" };
            CountryResponse  country_response_from_add = _countriesService.AddCountry(country_add_request);
            PersonAddRequest person_add_request = new PersonAddRequest() { PersonName = "idi", CountryID = country_response_from_add.CountryID, Address = "jfjf", Gender = GenderOptions.Male, RecieveNewsLetters = false, Email = "al@gmai.com" };
            PersonResponse person_response_from_add = _personService.AddPerson(person_add_request);

            bool isDeleted = _personService.DeletePerson(person_response_from_add.PersonID);
            Assert.True(isDeleted);
        }
        [Fact]
        public void DeletePerson_InvalidPersonID()
        {

            

            bool isDeleted = _personService.DeletePerson(Guid.NewGuid());
            Assert.False(isDeleted);
        }
        #endregion
    }

}

