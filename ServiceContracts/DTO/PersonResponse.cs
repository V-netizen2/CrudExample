using Entities;
using ServiceContracts.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceContracts.DTO
{
    public class PersonResponse
    {
        public Guid PersonID { get; set; }
        public string? PersonName { get; set; }
        public string? Email { get; set;}
        public DateTime? DateOfBirth { get; set; }
        public string? Gender { get; set; }
        public Guid? CountryID { get; set; }
        public string? Address { get; set; }
        public string? Country { get; set; }
        public bool RecieveNewsLetters { get; set; }
        public double? Age { get; set; }

        public override bool Equals(object? obj)
        {
            if(obj==null)
            {
                return false;
            }
            if(obj.GetType()!= typeof(PersonResponse))
            {
                return false;
            }
            PersonResponse person = (PersonResponse)obj;
            return PersonID == person.PersonID && PersonName == person.PersonName && Email == person.Email && DateOfBirth == person.DateOfBirth && Gender == person.Gender && CountryID == person.CountryID && Address == person.Address && RecieveNewsLetters == person.RecieveNewsLetters;
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        public override string ToString()
        {
            return $"Person ID : {PersonID}, PersonName:{PersonName},Email:{Email},Dateofbirth:{DateOfBirth},Gender:{Gender},Country:{Country},countryId:{CountryID},RecieveNews:{RecieveNewsLetters},address:{Address}";
        }
        public PersonUpdateRequest ToPersonUpdateRequest()
        {
            return new PersonUpdateRequest()
            {
                PersonID = PersonID,
                PersonName = PersonName,
                Email = Email,
                DateOfBirth = DateOfBirth,
                Gender = (GenderOptions)(Enum.Parse(typeof(GenderOptions), Gender, true)),
                Address = Address,
                CountryID = CountryID,
                RecieveNewsLetters = RecieveNewsLetters
            };
        }

    }
    public static class PersonExtensions
    {
        public static PersonResponse ToPersonResponse(this Person person)
        {
            return new PersonResponse()
            {
                PersonID = person.PersonID,
                PersonName = person.PersonName,
                Email = person.Email,
                DateOfBirth = person.DateOfBirth,
                RecieveNewsLetters = person.RecieveNewsLetters,
                Address = person.Address,
                CountryID = person.CountryID,
                Gender = person.Gender,
                Age = (person.DateOfBirth != null) ? Math.Round((DateTime.Now - person.DateOfBirth.Value).TotalDays / 365.25) : null
            };
        }
    }
}
