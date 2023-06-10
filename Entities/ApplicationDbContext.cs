using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class ApplicationDbContext:DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
  {
  }


        public virtual DbSet<Person> Persons { get; set;}
        public virtual DbSet<Country> Countries { get; set;}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Country>().ToTable("Countries");
            modelBuilder.Entity<Person>().ToTable("Persons");

            //seed to countries
            string countriesJson = System.IO.File.ReadAllText("countries.json");
            List<Country> countries = System.Text.Json.JsonSerializer.Deserialize<List<Country>>(countriesJson);
            foreach(Country country in countries)
            {
                modelBuilder.Entity<Country>().HasData(country);
            }

            //seed to persons
            string personsJson = System.IO.File.ReadAllText("persons.json");
            List<Person> persons = System.Text.Json.JsonSerializer.Deserialize<List<Person>>(personsJson);
            foreach (Person person in persons)
            {
                modelBuilder.Entity<Person>().HasData(person);
            }
        }
        public List<Person> sp_GetAllPersons()
        {
           return  Persons.FromSqlRaw("EXECUTE [dbo].[GetAllPersons]").ToList();
        }
        public int sp_InsertPerson(Person person)
        {
            SqlParameter[] parameters = new SqlParameter[] {
        new SqlParameter("@PersonID", person.PersonID),
        new SqlParameter("@PersonName", person.PersonName),
        new SqlParameter("@Email", person.Email),
        new SqlParameter("@DateOfBirth", person.DateOfBirth),
        new SqlParameter("@Gender", person.Gender),
        new SqlParameter("@CountryID", person.CountryID),
        new SqlParameter("@Address", person.Address),
        new SqlParameter("@ReceiveNewsLetters", person.RecieveNewsLetters)
      };

            return Database.ExecuteSqlRaw("EXECUTE [dbo].[InsertPerson] @PersonID, @PersonName, @Email, @DateOfBirth, @Gender, @CountryID, @Address, @RecieveNewsLetters", parameters);
        }
    }
}
