﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CinemaTicket
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class CinemaBookingDBEntities : DbContext
    {
        public CinemaBookingDBEntities()
            : base("name=CinemaBookingDBEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<AdminAccount> AdminAccounts { get; set; }
        public virtual DbSet<BankAccount> BankAccounts { get; set; }
        public virtual DbSet<BankList> BankLists { get; set; }
        public virtual DbSet<BookingDetail> BookingDetails { get; set; }
        public virtual DbSet<BookingTicket> BookingTickets { get; set; }
        public virtual DbSet<Cinema> Cinemas { get; set; }
        public virtual DbSet<CinemaManager> CinemaManagers { get; set; }
        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<DigitalType> DigitalTypes { get; set; }
        public virtual DbSet<Film> Films { get; set; }
        public virtual DbSet<GroupCinema> GroupCinemas { get; set; }
        public virtual DbSet<MovieSchedule> MovieSchedules { get; set; }
        public virtual DbSet<News> News { get; set; }
        public virtual DbSet<PartnerAccount> PartnerAccounts { get; set; }
        public virtual DbSet<Promotion> Promotions { get; set; }
        public virtual DbSet<Room> Rooms { get; set; }
        public virtual DbSet<Seat> Seats { get; set; }
        public virtual DbSet<ShowTime> ShowTimes { get; set; }
        public virtual DbSet<Ticket> Tickets { get; set; }
        public virtual DbSet<TypeOfSeat> TypeOfSeats { get; set; }
        public virtual DbSet<UserAccount> UserAccounts { get; set; }
    }
}
