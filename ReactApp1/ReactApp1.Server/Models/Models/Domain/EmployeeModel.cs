using System.ComponentModel.DataAnnotations;
using ReactApp1.Server.Models.Enums;

namespace ReactApp1.Server.Models.Models.Domain
{
    public class EmployeeModel
    {
        /// <summary>
        /// Identifikatorius
        /// </summary>
        public int EmployeeId { get; set; }

        /// <summary>
        /// Imonės identifikatorius
        /// </summary>
        public int EstablishmentId { get; set; }
        
        /// <summary>
        /// Pareigos
        /// </summary>
        public TitleEnum Title { get; set; }

        /// <summary>
        /// Asmens kodas
        /// </summary>
        [StringLength(255)]
        public string? PersonalCode { get; set; }
        
        /// <summary>
        /// Vardas
        /// </summary>
        [StringLength(255)]
        public string? FirstName { get; set; }

        /// <summary>
        /// Pavardė
        /// </summary>
        [StringLength(255)]
        public string? LastName { get; set; }

        /// <summary>
        /// Gimimo data
        /// </summary>
        public DateTime? BirthDate { get; set; }

        /// <summary>
        /// Telefono numeris
        /// </summary>
        [StringLength(255)]
        public string? Phone { get; set; }

        /// <summary>
        /// El. paštas
        /// </summary>
        [StringLength(255)]
        public string? Email { get; set; }

        /// <summary>
        /// Slaptažodžio hash'as
        /// </summary>
        [StringLength(255)]
        public string? PasswordHash { get; set; }

        /// <summary>
        /// Valstybė
        /// </summary>
        [StringLength(255)]
        public string? Country { get; set; }

        /// <summary>
        /// Miestas
        /// </summary>
        [StringLength(255)]
        public string? City { get; set; }

        /// <summary>
        /// Gatvė
        /// </summary>
        [StringLength(255)]
        public string? Street { get; set; }

        /// <summary>
        /// Gatvės numeris
        /// </summary>
        [StringLength(255)]
        public string? StreetNumber { get; set; }

        /// <summary>
        /// Namo numeris
        /// </summary>
        public string? HouseNumber { get; set; }
        
        public void MapUpdate(Employee existingEmployee)
        {
            existingEmployee.Title = (int)this.Title;
            existingEmployee.PersonalCode = this.PersonalCode;
            existingEmployee.FirstName = this.FirstName;
            existingEmployee.LastName = this.LastName;
            existingEmployee.BirthDate = this.BirthDate;
            existingEmployee.Phone = this.Phone;
            existingEmployee.Email = this.Email;
            if (existingEmployee.AddressId != 0)
            {
                existingEmployee.EmployeeAddress.Country = this.Country;
                existingEmployee.EmployeeAddress.City = this.City;
                existingEmployee.EmployeeAddress.Street = this.Street;
                existingEmployee.EmployeeAddress.StreetNumber = this.StreetNumber;
                existingEmployee.EmployeeAddress.HouseNumber = this.HouseNumber;
            }
        }
    }
}