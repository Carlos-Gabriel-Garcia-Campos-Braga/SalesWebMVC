﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace SalesWebMVC.Models
{
    public class Seller
    {
        public int Id { get; set; }
        public string Name { get; set; }
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Display(Name = "Base Salary")]
        [DisplayFormat(DataFormatString = "{0:F2}")]
        public double BaseSalary { get; set; }
        [Display(Name = "Birth Date")]
        [DataType(DataType.Date)]
        //[DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime BirthDate { get; set; }
        public Department Department { get; set; }
        public int DepartmentId { get; set; }
        public ICollection<SalesRecord> SalesRecord { get; set; } = new List<SalesRecord>();

        public Seller() { }

        public Seller(int id, string name, string email, double baseSalary, DateTime birthDate, Department department)
        {
            Id = id;
            Name = name;
            Email = email;
            BaseSalary = baseSalary;
            BirthDate = birthDate;
            Department = department;
        }

        public void AddSale(SalesRecord SaleRecord)
        {
            SalesRecord.Add(SaleRecord);
        }

        public void RemoveSale(SalesRecord SaleRecord)
        {
            SalesRecord.Remove(SaleRecord);
        }

        public double TotalSales(DateTime Initial, DateTime Final)
        {
            return SalesRecord.
                Where(sr => sr.Date >= Initial && sr.Date <= Final).
                Sum(sr => sr.Amount);
        }
    }
}
