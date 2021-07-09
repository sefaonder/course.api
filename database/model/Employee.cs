using System;
using course.helper;

namespace course.database.model
{
    public class Employee
    {
        public Guid Id { get; set; }
        public bool IsDeleted { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public DateTime? BirthDate { get; set; }
        public Gender Gender { get; set; }
        public double Salary { get; set; }
    }
}