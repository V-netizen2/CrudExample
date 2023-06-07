using Entities;
using ServiceContracts.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceContracts.DTO
{
    public class PersonAddRequest
    {
        [Required(ErrorMessage ="person name cannot be nul")]
        public string? PersonName { get; set; }
        [Required(ErrorMessage ="email cannot be null")]
        [EmailAddress(ErrorMessage ="email should be valid")]
        [DataType(DataType.EmailAddress)]
        public string? Email { get; set; }
        [DataType(DataType.Date)]
        public DateTime? DateOfBirth { get; set; }

        [Required(ErrorMessage ="Please select a gender")]
        public GenderOptions? Gender { get; set; }
        [Required(ErrorMessage ="please select a country")]
        public Guid? CountryID { get; set; }
        public string? Address { get; set; }
        public bool RecieveNewsLetters { get; set; }

        public Person ToPerson()
        {
            return new Person()
            {
                PersonName = PersonName,
                Email = Email,
                DateOfBirth = DateOfBirth,
                Gender = Gender.ToString(),
                CountryID = CountryID,
                Address = Address,
                RecieveNewsLetters = RecieveNewsLetters


            };
        }

    }
}

