﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SmartMoveWebApp
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class SmartMoveEntities : DbContext
    {
        public SmartMoveEntities()
            : base("name=SmartMoveEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<CustomerCreditCard> CustomerCreditCards { get; set; }
        public virtual DbSet<CustomerRating> CustomerRatings { get; set; }
        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<Login> Logins { get; set; }
        public virtual DbSet<OrderBid> OrderBids { get; set; }
        public virtual DbSet<OrderImage> OrderImages { get; set; }
        public virtual DbSet<OrderPayment> OrderPayments { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<sysdiagram> sysdiagrams { get; set; }
        public virtual DbSet<TruckOwnerCreditCard> TruckOwnerCreditCards { get; set; }
        public virtual DbSet<TruckOwnerRating> TruckOwnerRatings { get; set; }
        public virtual DbSet<TruckOwner> TruckOwners { get; set; }
        public virtual DbSet<Truck> Trucks { get; set; }
        public virtual DbSet<TruckType> TruckTypes { get; set; }
    }
}