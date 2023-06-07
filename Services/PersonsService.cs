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
        private readonly IList<Person> _person;
        private readonly ICountriesService _countriesService;
        public PersonsService( bool initialize=true) 
        { 
            _person = new List<Person>();
            _countriesService = new CountriesService();
            if (initialize)
            {
                _person.Add(new Person() { PersonID = Guid.Parse("8082ED0C-396D-4162-AD1D-29A13F929824"), PersonName = "Aguste", Email = "aleddy0@booking.com", DateOfBirth = DateTime.Parse("1993-01-02"), Gender = "Male", Address = "0858 Novick Terrace", RecieveNewsLetters = false, CountryID = Guid.Parse("14E9C342-FB8A-4AFE-903B-635BDB915814") });

                _person.Add(new Person() { PersonID = Guid.Parse("06D15BAD-52F4-498E-B478-ACAD847ABFAA"), PersonName = "Jasmina", Email = "jsyddie1@miibeian.gov.cn", DateOfBirth = DateTime.Parse("1991-06-24"), Gender = "Female", Address = "0742 Fieldstone Lane", RecieveNewsLetters = true, CountryID = Guid.Parse("14E9C342-FB8A-4AFE-903B-635BDB915814") });

                _person.Add(new Person() { PersonID = Guid.Parse("D3EA677A-0F5B-41EA-8FEF-EA2FC41900FD"), PersonName = "Kendall", Email = "khaquard2@arstechnica.com", DateOfBirth = DateTime.Parse("1993-08-13"), Gender = "Male", Address = "7050 Pawling Alley", RecieveNewsLetters = false, CountryID = Guid.Parse("8A1CFF79-DAA6-4D97-A06F-21BF9EFE8305") });

                _person.Add(new Person() { PersonID = Guid.Parse("89452EDB-BF8C-4283-9BA4-8259FD4A7A76"), PersonName = "Kilian", Email = "kaizikowitz3@joomla.org", DateOfBirth = DateTime.Parse("1991-06-17"), Gender = "Male", Address = "233 Buhler Junction", RecieveNewsLetters = true, CountryID = Guid.Parse("8A1CFF79-DAA6-4D97-A06F-21BF9EFE8305") });

                _person.Add(new Person() { PersonID = Guid.Parse("F5BD5979-1DC1-432C-B1F1-DB5BCCB0E56D"), PersonName = "Dulcinea", Email = "dbus4@pbs.org", DateOfBirth = DateTime.Parse("1996-09-02"), Gender = "Female", Address = "56 Sundown Point", RecieveNewsLetters = false, CountryID = Guid.Parse("E122021C-E5CB-4DF2-A80C-D2D27BCE9A37") });

                _person.Add(new Person() { PersonID = Guid.Parse("A795E22D-FAED-42F0-B134-F3B89B8683E5"), PersonName = "Corabelle", Email = "cadams5@t-online.de", DateOfBirth = DateTime.Parse("1993-10-23"), Gender = "Female", Address = "4489 Hazelcrest Place", RecieveNewsLetters = false, CountryID = Guid.Parse("E122021C-E5CB-4DF2-A80C-D2D27BCE9A37") });

                _person.Add(new Person() { PersonID = Guid.Parse("3C12D8E8-3C1C-4F57-B6A4-C8CAAC893D7A"), PersonName = "Faydra", Email = "fbischof6@boston.com", DateOfBirth = DateTime.Parse("1996-02-14"), Gender = "Female", Address = "2010 Farragut Pass", RecieveNewsLetters = true, CountryID = Guid.Parse("F885B612-A1A0-499A-AFB2-9A2009446396") });

                _person.Add(new Person() { PersonID = Guid.Parse("7B75097B-BFF2-459F-8EA8-63742BBD7AFB"), PersonName = "Oby", Email = "oclutheram7@foxnews.com", DateOfBirth = DateTime.Parse("1992-05-31"), Gender = "Male", Address = "2 Fallview Plaza", RecieveNewsLetters = false, CountryID = Guid.Parse("F885B612-A1A0-499A-AFB2-9A2009446396") });

                _person.Add(new Person() { PersonID = Guid.Parse("6717C42D-16EC-4F15-80D8-4C7413E250CB"), PersonName = "Seumas", Email = "ssimonitto8@biglobe.ne.jp", DateOfBirth = DateTime.Parse("1999-02-02"), Gender = "Male", Address = "76779 Norway Maple Crossing", RecieveNewsLetters = false, CountryID = Guid.Parse("DF7C89CE-3341-4246-84AE-E01AB7BA476E") });

                _person.Add(new Person() { PersonID = Guid.Parse("6E789C86-C8A6-4F18-821C-2ABDB2E95982"), PersonName = "Freemon", Email = "faugustin9@vimeo.com", DateOfBirth = DateTime.Parse("1996-04-27"), Gender = "Male", Address = "8754 Becker Street", RecieveNewsLetters = false, CountryID = Guid.Parse("DF7C89CE-3341-4246-84AE-E01AB7BA476E") });

            }
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
            _person.Add(person);
            return ConvertPersonToPersonResponse(person);
           
        }

        public List<PersonResponse> GetAllPersons()
        {
           return _person.Select(temp=>ConvertPersonToPersonResponse(temp)).ToList();
        }

        public PersonResponse? GetPersonByPersonID(Guid? personID)
        {
            if (personID == null)
                return null;
           Person? person = _person.FirstOrDefault(temp=> temp.PersonID == personID); 
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
            Person? person = _person.Where(x => x.PersonID == personUpdateRequest.PersonID).FirstOrDefault();
            if (person == null)
            {
                throw new ArgumentException("given personid not exists");
            }
           
            person.Address = personUpdateRequest.Address;
            person.Gender = personUpdateRequest.Gender.ToString();
            person.RecieveNewsLetters = personUpdateRequest.RecieveNewsLetters;
            person.CountryID = personUpdateRequest?.CountryID;
            return ConvertPersonToPersonResponse(person);
        }

        public bool DeletePerson(Guid? personID)
        {
            if(personID== null)
            {
                throw new ArgumentNullException(nameof(personID));
            }
            Person person = _person.FirstOrDefault(x=>x.PersonID == personID);
            if (person == null)
            {
                return false;
            }
            _person.Remove(person);
            return true;
        }
    }
}
