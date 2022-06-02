using Model.Users;
using System;
using System.Collections.Generic;

namespace Model
{
    public class People
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName => $"{FirstName} {LastName}";
        public string Affliation { get; set; }
        public string Research_Interest { get; set; }
        public DateTime Joining_Year { get; set; }
        public string Role { get; set; }
        public string Short_Description { get; set; }
        public string Linkedin_id { get; set; }
        public string Personal_Website { get; set; }
        public Gender Gender { get; set; }
        public string Photo { get; set; }
        public string Email { get; set; }
        public DateTime? Created { get; set; }
        public DateTime? Updated { get; set; }
        public DateTime? BirthDate { get; set; }
        public List<News> News { get; set; }
    }
}