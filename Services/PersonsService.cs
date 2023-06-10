using Entities;
using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.Enums;
using Services.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class PersonsService : IPersonsService
    {
        private readonly ApplicationDbContext _db;
        private readonly ICountriesService _countriesService;
        public PersonsService(ApplicationDbContext personsDbContext,ICountriesService countriesService ) 
        { 
            _db = personsDbContext;
            _countriesService = countriesService;
            
        }

        private PersonResponse ConvertPersonToPersonResponse(Person person)
        {
            PersonResponse personResponse = person.ToPersonResponse();
            personResponse.Country = _countriesService.GetCountryByCountryId(person.CountryID)?.CountryName;
            return personResponse;

        }

        public PersonResponse AddPerson(PersonAddRequest? personAddRequest)
        {
            //check if personAddRequest is not null
            if(personAddRequest == null)
            {
                throw new ArgumentNullException(nameof(personAddRequest));
            }
            //validate personname 
            ValidationHelper.ModelValidation(personAddRequest);
            Person person = personAddRequest.ToPerson();

            person.PersonID = Guid.NewGuid();
            //_db.Persons.Add(person);
            //_db.SaveChanges();
            _db.sp_InsertPerson(person);
            return person.ToPersonResponse();

        }

        public List<PersonResponse> GetAllPersons()
        {
           return _db.sp_GetAllPersons().ToList().Select(temp=>ConvertPersonToPersonResponse(temp)).ToList();
        }

        public PersonResponse? GetPersonByPersonID(Guid? personID)
        {
            if (personID == null)
                return null;
           Person? person = _db.Persons.FirstOrDefault(temp=> temp.PersonID == personID); 
            if(person==null) return null;
            return ConvertPersonToPersonResponse(person);
        }

        public List<PersonResponse> GetFilteredPersons(string searchBy, string? searchString)
        {
            List<PersonResponse> allPersons = GetAllPersons();
            List<PersonResponse> matchingPersons = allPersons;
            if(string.IsNullOrEmpty(searchBy) || string.IsNullOrEmpty(searchString))
                return matchingPersons;
            switch(searchBy)
            {
                case nameof(PersonResponse.PersonName):
                    matchingPersons = allPersons.Where(temp=>
                    (!string.IsNullOrEmpty(temp.PersonName)?
                    temp.PersonName.Contains(searchString,StringComparison.OrdinalIgnoreCase):true)).ToList();
                    break;
                case nameof(PersonResponse.Email):
                    matchingPersons = allPersons.Where(temp =>
                    (!string.IsNullOrEmpty(temp.Email) ?
                    temp.Email.Contains(searchString, StringComparison.OrdinalIgnoreCase) : true)).ToList();
                    break;
                case nameof(PersonResponse.DateOfBirth):
                    matchingPersons = allPersons.Where(temp =>
                    (temp.DateOfBirth != null) ?
                    temp.DateOfBirth.Value.ToString("dd MMMM yyyy").Contains(searchString, StringComparison.OrdinalIgnoreCase) : true).ToList();
                    break;

                case nameof(PersonResponse.Gender):
                    matchingPersons = allPersons.Where(temp =>
                    (!string.IsNullOrEmpty(temp.Gender) ?
                    temp.Gender.Contains(searchString, StringComparison.OrdinalIgnoreCase) : true)).ToList();
                    break;

                case nameof(PersonResponse.CountryID):
                    matchingPersons = allPersons.Where(temp =>
                    (!string.IsNullOrEmpty(temp.Country) ?
                    temp.Country.Contains(searchString, StringComparison.OrdinalIgnoreCase) : true)).ToList();
                    break;

                case nameof(PersonResponse.Address):
                    matchingPersons = allPersons.Where(temp =>
                    (!string.IsNullOrEmpty(temp.Address) ?
                    temp.Address.Contains(searchString, StringComparison.OrdinalIgnoreCase) : true)).ToList();
                    break;

                default: matchingPersons = allPersons; break;
            }
            return matchingPersons;
           

        }

        public List<PersonResponse> GetSortedPersons(List<PersonResponse> allPersons, string sortBy, SortOrderOptions sortOrder)
        {
            if(string.IsNullOrEmpty(sortBy))
            {
                return allPersons;
            }
            List<PersonResponse> sortedPersons = (sortBy, sortOrder) switch
            {
                (nameof(PersonResponse.PersonName), SortOrderOptions.ASC) => allPersons.OrderBy(temp => temp.PersonName, StringComparer.OrdinalIgnoreCase).ToList(),

                (nameof(PersonResponse.PersonName), SortOrderOptions.DESC) => allPersons.OrderByDescending(temp => temp.PersonName, StringComparer.OrdinalIgnoreCase).ToList(),

                (nameof(PersonResponse.Email), SortOrderOptions.ASC) => allPersons.OrderBy(temp => temp.Email, StringComparer.OrdinalIgnoreCase).ToList(),

                (nameof(PersonResponse.Email), SortOrderOptions.DESC) => allPersons.OrderByDescending(temp => temp.Email, StringComparer.OrdinalIgnoreCase).ToList(),

                (nameof(PersonResponse.DateOfBirth), SortOrderOptions.ASC) => allPersons.OrderBy(temp => temp.DateOfBirth).ToList(),

                (nameof(PersonResponse.DateOfBirth), SortOrderOptions.DESC) => allPersons.OrderByDescending(temp => temp.DateOfBirth).ToList(),

                (nameof(PersonResponse.Age), SortOrderOptions.ASC) => allPersons.OrderBy(temp => temp.Age).ToList(),

                (nameof(PersonResponse.Age), SortOrderOptions.DESC) => allPersons.OrderByDescending(temp => temp.Age).ToList(),

                (nameof(PersonResponse.Gender), SortOrderOptions.ASC) => allPersons.OrderBy(temp => temp.Gender, StringComparer.OrdinalIgnoreCase).ToList(),

                (nameof(PersonResponse.Gender), SortOrderOptions.DESC) => allPersons.OrderByDescending(temp => temp.Gender, StringComparer.OrdinalIgnoreCase).ToList(),

                (nameof(PersonResponse.Country), SortOrderOptions.ASC) => allPersons.OrderBy(temp => temp.Country, StringComparer.OrdinalIgnoreCase).ToList(),

                (nameof(PersonResponse.Country), SortOrderOptions.DESC) => allPersons.OrderByDescending(temp => temp.Country, StringComparer.OrdinalIgnoreCase).ToList(),

                (nameof(PersonResponse.Address), SortOrderOptions.ASC) => allPersons.OrderBy(temp => temp.Address, StringComparer.OrdinalIgnoreCase).ToList(),

                (nameof(PersonResponse.Address), SortOrderOptions.DESC) => allPersons.OrderByDescending(temp => temp.Address, StringComparer.OrdinalIgnoreCase).ToList(),

                (nameof(PersonResponse.RecieveNewsLetters), SortOrderOptions.ASC) => allPersons.OrderBy(temp => temp.RecieveNewsLetters).ToList(),

                (nameof(PersonResponse.RecieveNewsLetters), SortOrderOptions.DESC) => allPersons.OrderByDescending(temp => temp.RecieveNewsLetters).ToList(),

                _ => allPersons
            };

            return sortedPersons;
        }

        public PersonResponse UpdatePerson(PersonUpdateRequest? personUpdateRequest)
        {
            if(personUpdateRequest == null)
            {
                throw new ArgumentNullException(nameof(personUpdateRequest));
            }
            ValidationHelper.ModelValidation(personUpdateRequest);
            Person? person = _db.Persons.FirstOrDefault(x => x.PersonID == personUpdateRequest.PersonID);
            if (person == null)
            {
                throw new ArgumentException("given personid not exists");
            }
            person.PersonName = personUpdateRequest.PersonName; 
            person.Email = personUpdateRequest.Email;
            person.DateOfBirth = personUpdateRequest.DateOfBirth;
            person.Address = personUpdateRequest.Address;
            person.Gender = personUpdateRequest.Gender.ToString();
            person.RecieveNewsLetters = personUpdateRequest.RecieveNewsLetters;
            person.CountryID = personUpdateRequest?.CountryID;
            _db.SaveChanges();
            return ConvertPersonToPersonResponse(person);
        }

        public bool DeletePerson(Guid? personID)
        {
            if(personID== null)
            {
                throw new ArgumentNullException(nameof(personID));
            }
            Person person = _db.Persons.FirstOrDefault(x=>x.PersonID == personID);
            if (person == null)
            {
                return false;
            }
            _db.Persons.Remove(_db.Persons.First(temp=>temp.PersonID==personID));
            _db.SaveChanges();
            return true;
        }
    }
}
