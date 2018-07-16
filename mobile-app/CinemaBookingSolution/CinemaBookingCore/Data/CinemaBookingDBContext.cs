using CinemaBookingCore.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaBookingCore.Data
{
    public partial class CinemaBookingDBContext : DbContext
    {
        public CinemaBookingDBContext(DbContextOptions<CinemaBookingDBContext> options) : base(options)
        {
        }

        public virtual DbSet<AdminAccount> AdminAccount { get; set; }
        
        public virtual DbSet<BookingTicket> BookingTicket { get; set; }
        public virtual DbSet<Cinema> Cinema { get; set; }
        public virtual DbSet<CinemaManager> CinemaManager { get; set; }
        public virtual DbSet<Customer> Customer { get; set; }
        public virtual DbSet<DigitalType> DigitalType { get; set; }
        public virtual DbSet<Film> Film { get; set; }
        public virtual DbSet<GroupCinema> GroupCinema { get; set; }
        public virtual DbSet<MovieSchedule> MovieSchedule { get; set; }
        public virtual DbSet<PartnerAccount> PartnerAccount { get; set; }
        public virtual DbSet<Promotion> Promotion { get; set; }
        public virtual DbSet<Room> Room { get; set; }
        public virtual DbSet<Seat> Seat { get; set; }
        public virtual DbSet<ShowTime> ShowTime { get; set; }
        public virtual DbSet<Ticket> Ticket { get; set; }
        public virtual DbSet<TypeOfSeat> TypeOfSeat { get; set; }
        public virtual DbSet<UserAccount> UserAccount { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(@"Server=DESKTOP-D8NCIMI\SQLEXPRESS;Database=CinemaBookingDBTest;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AdminAccount>(entity =>
            {
                entity.HasKey(e => e.AdminId);

                entity.Property(e => e.AdminId)
                    .HasColumnName("adminId")
                    .HasMaxLength(255)
                    .ValueGeneratedNever();

                entity.Property(e => e.AdminPassword)
                    .HasColumnName("adminPassword")
                    .HasMaxLength(255);

                entity.Property(e => e.Email)
                    .HasColumnName("email")
                    .HasMaxLength(255);

                entity.Property(e => e.Phone)
                    .HasColumnName("phone")
                    .HasMaxLength(255);
            });
            
            modelBuilder.Entity<BookingTicket>(entity =>
            {
                entity.HasKey(e => e.BookingId);

                entity.Property(e => e.BookingId).HasColumnName("bookingId");

                entity.Property(e => e.BookingDate)
                    .HasColumnName("bookingDate")
                    .HasColumnType("datetime");

                entity.Property(e => e.CustomerId).HasColumnName("customerId");

                entity.Property(e => e.PaymentMethodId).HasColumnName("paymentMethodId");

                entity.Property(e => e.Quantity).HasColumnName("quantity");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.BookingTickets)
                    .HasForeignKey(d => d.CustomerId)
                    .HasConstraintName("FKBookingTicketCustomer001");
            });

            modelBuilder.Entity<Cinema>(entity =>
            {
                entity.Property(e => e.CinemaId).HasColumnName("cinemaId");

                entity.Property(e => e.CinemaAddress)
                    .HasColumnName("cinemaAddress")
                    .HasMaxLength(255);

                entity.Property(e => e.Email)
                    .HasColumnName("email")
                    .HasMaxLength(200);

                entity.Property(e => e.GroupId).HasColumnName("groupId");

                entity.Property(e => e.Introduction)
                    .HasColumnName("introduction")
                    .HasMaxLength(1000);

                entity.Property(e => e.OpenTime)
                    .HasColumnName("openTime")
                    .HasMaxLength(200);

                entity.Property(e => e.Phone)
                    .HasColumnName("phone")
                    .HasMaxLength(200);

                entity.Property(e => e.ProfilePicture)
                    .HasColumnName("profilePicture")
                    .HasMaxLength(255);

                entity.HasOne(d => d.GroupCinema)
                    .WithMany(p => p.Cinemas)
                    .HasForeignKey(d => d.GroupId)
                    .HasConstraintName("FKCinemaGroupCinema001");
            });

            modelBuilder.Entity<CinemaManager>(entity =>
            {
                entity.HasKey(e => e.ManagerId);

                entity.Property(e => e.ManagerId)
                    .HasColumnName("managerId")
                    .HasMaxLength(255)
                    .ValueGeneratedNever();

                entity.Property(e => e.CinemaId).HasColumnName("cinemaId");

                entity.Property(e => e.Email)
                    .HasColumnName("email")
                    .HasMaxLength(255);

                entity.Property(e => e.ManagerPassword)
                    .HasColumnName("managerPassword")
                    .HasMaxLength(255);

                entity.Property(e => e.Phone)
                    .HasColumnName("phone")
                    .HasMaxLength(255);

                entity.HasOne(d => d.Cinema)
                    .WithMany(p => p.CinemaManagers)
                    .HasForeignKey(d => d.CinemaId)
                    .HasConstraintName("FKCinemaManagerCinema001");
            });

            modelBuilder.Entity<Customer>(entity =>
            {
                entity.Property(e => e.CustomerId).HasColumnName("customerId");

                entity.Property(e => e.Email)
                    .HasColumnName("email")
                    .HasMaxLength(255);

                entity.Property(e => e.Phone)
                    .HasColumnName("phone")
                    .HasMaxLength(255);

                entity.Property(e => e.UserId)
                    .HasColumnName("userId")
                    .HasMaxLength(255);
            });

            modelBuilder.Entity<DigitalType>(entity =>
            {
                entity.HasKey(e => e.DigTypeId);

                entity.Property(e => e.DigTypeId).HasColumnName("digTypeId");

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasMaxLength(255);
            });

            modelBuilder.Entity<Film>(entity =>
            {
                entity.Property(e => e.FilmId).HasColumnName("filmId");

                entity.Property(e => e.ActorList)
                    .HasColumnName("actorList")
                    .HasMaxLength(255);

                entity.Property(e => e.AdditionPicture)
                    .HasColumnName("additionPicture")
                    .HasMaxLength(1000);

                entity.Property(e => e.Author)
                    .HasColumnName("author")
                    .HasMaxLength(255);

                entity.Property(e => e.Countries)
                    .HasColumnName("countries")
                    .HasMaxLength(255);

                entity.Property(e => e.DateRelease)
                    .HasColumnName("dateRelease")
                    .HasColumnType("datetime");

                entity.Property(e => e.DigTypeId)
                    .HasColumnName("digTypeId")
                    .HasMaxLength(50);

                entity.Property(e => e.FilmContent)
                    .HasColumnName("filmContent")
                    .HasMaxLength(1000);

                entity.Property(e => e.FilmLength).HasColumnName("filmLength");

                entity.Property(e => e.FilmStatus).HasColumnName("filmStatus");

                entity.Property(e => e.Imdb).HasColumnName("imdb");

                entity.Property(e => e.MovieGenre)
                    .HasColumnName("movieGenre")
                    .HasMaxLength(50);

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasMaxLength(255);

                entity.Property(e => e.PosterPicture)
                    .HasColumnName("posterPicture")
                    .HasMaxLength(255);

                entity.Property(e => e.Restricted).HasColumnName("restricted");

                entity.Property(e => e.TrailerLink)
                    .HasColumnName("trailerLink")
                    .HasMaxLength(500);
            });

            modelBuilder.Entity<GroupCinema>(entity =>
            {
                entity.HasKey(e => e.GroupId);

                entity.Property(e => e.LogoImg)
                    .HasColumnName("logoImg")
                    .HasMaxLength(50);

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasMaxLength(255);
            });

            modelBuilder.Entity<MovieSchedule>(entity =>
            {
                entity.HasKey(e => e.ScheduleId);

                entity.Property(e => e.ScheduleId).HasColumnName("scheduleId");

                entity.Property(e => e.FilmId).HasColumnName("filmId");

                entity.Property(e => e.RoomId).HasColumnName("roomId");

                entity.Property(e => e.TimeId).HasColumnName("timeId");

                entity.HasOne(d => d.Film)
                    .WithMany(p => p.MovieSchedules)
                    .HasForeignKey(d => d.FilmId)
                    .HasConstraintName("FKMovieScheduleFilm001");

                entity.HasOne(d => d.Room)
                    .WithMany(p => p.MovieSchedules)
                    .HasForeignKey(d => d.RoomId)
                    .HasConstraintName("FKMovieScheduleRoom001");

                entity.HasOne(d => d.ShowTime)
                    .WithMany(p => p.MovieSchedules)
                    .HasForeignKey(d => d.TimeId)
                    .HasConstraintName("FKMovieScheduleShowTime001");
            });

            modelBuilder.Entity<PartnerAccount>(entity =>
            {
                entity.HasKey(e => e.PartnerId);

                entity.Property(e => e.PartnerId)
                    .HasColumnName("partnerId")
                    .HasMaxLength(255)
                    .ValueGeneratedNever();

                entity.Property(e => e.Email)
                    .HasColumnName("email")
                    .HasMaxLength(255);

                entity.Property(e => e.GroupOfCinemaId).HasColumnName("groupOfCinemaId");

                entity.Property(e => e.PartnerPassword)
                    .HasColumnName("partnerPassword")
                    .HasMaxLength(255);

                entity.Property(e => e.Phone)
                    .HasColumnName("phone")
                    .HasMaxLength(255);

                entity.HasOne(d => d.GroupCinema)
                    .WithMany(p => p.PartnerAccounts)
                    .HasForeignKey(d => d.GroupOfCinemaId)
                    .HasConstraintName("FKPartnerAccountGroupCinema001");
            });

            modelBuilder.Entity<Promotion>(entity =>
            {
                entity.Property(e => e.PromotionId).HasColumnName("promotionId");

                entity.Property(e => e.CinemaId).HasColumnName("cinemaId");

                entity.Property(e => e.UrlDocument)
                    .HasColumnName("urlDocument")
                    .HasMaxLength(255);

                entity.HasOne(d => d.Cinema)
                    .WithMany(p => p.Promotions)
                    .HasForeignKey(d => d.CinemaId)
                    .HasConstraintName("FKPromotionCinema001");
            });

            modelBuilder.Entity<Room>(entity =>
            {
                entity.HasKey(e => e.RoomId);

                entity.Property(e => e.RoomId).HasColumnName("roomId");

                entity.Property(e => e.Capacity).HasColumnName("capacity");

                entity.Property(e => e.CinemaId).HasColumnName("cinemaId");

                entity.Property(e => e.DigTypeId).HasColumnName("digTypeId");

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasMaxLength(15);

                entity.HasOne(d => d.Cinema)
                    .WithMany(p => p.Rooms)
                    .HasForeignKey(d => d.CinemaId)
                    .HasConstraintName("FKRoomCinema001");

                entity.HasOne(d => d.DigitalType)
                    .WithMany(p => p.Rooms)
                    .HasForeignKey(d => d.DigTypeId)
                    .HasConstraintName("FKRoomDigitalType001");
            });

            modelBuilder.Entity<Seat>(entity =>
            {
                entity.HasKey(e => e.SeatId);

                entity.Property(e => e.SeatId).HasColumnName("seatId");

                entity.Property(e => e.Px).HasColumnName("px");

                entity.Property(e => e.Py).HasColumnName("py");

                entity.Property(e => e.RoomId).HasColumnName("roomId");

                entity.Property(e => e.TypeSeatId).HasColumnName("typeSeatId");

                entity.HasOne(d => d.Room)
                    .WithMany(p => p.Seats)
                    .HasForeignKey(d => d.RoomId)
                    .HasConstraintName("FKSeatRoom001");

                
            });

            modelBuilder.Entity<ShowTime>(entity =>
            {
                entity.HasKey(e => e.TimeId);

                entity.Property(e => e.TimeId).HasColumnName("timeId");
                entity.Property(e => e.StartTimeDouble).HasColumnName("startTimeDouble");

                entity.Property(e => e.EndTime)
                    .HasColumnName("endTime")
                    .HasColumnType("datetime");

                entity.Property(e => e.StartTime)
                    .HasColumnName("startTime")
                    .HasColumnType("datetime");
            });

            modelBuilder.Entity<Ticket>(entity =>
            {
                entity.HasKey(e => e.TicketId);

                entity.Property(e => e.TicketId).HasColumnName("ticketId");

                entity.Property(e => e.PaymentCode)
                    .HasColumnName("paymentCode")
                    .HasMaxLength(50);

                entity.Property(e => e.Price).HasColumnName("price");

                entity.Property(e => e.QrCode).HasColumnName("qrCode");

                entity.Property(e => e.ScheduleId).HasColumnName("scheduleId");

                entity.Property(e => e.SeatId).HasColumnName("seatId");

                entity.Property(e => e.BookingId).HasColumnName("bookingId");

                entity.Property(e => e.TicketStatus)
                    .HasColumnName("ticketStatus")
                    .HasMaxLength(20);

                entity.HasOne(d => d.MovieSchedule)
                    .WithMany(p => p.Tickets)
                    .HasForeignKey(d => d.ScheduleId)
                    .HasConstraintName("FKTicketMovieSchedule001");

                entity.HasOne(d => d.Seat)
                    .WithMany(p => p.Tickets)
                    .HasForeignKey(d => d.SeatId)
                    .HasConstraintName("FKTicketSeat001");

                entity.HasOne(d => d.BookingTicket)
                    .WithMany(p => p.Tickets)
                    .HasForeignKey(d => d.BookingId)
                    .HasConstraintName("FKTicketBookingTicket001");
            });

            modelBuilder.Entity<TypeOfSeat>(entity =>
            {
                entity.HasKey(e => e.TypeSeatId);

                entity.Property(e => e.TypeSeatId).HasColumnName("typeSeatId");

                entity.Property(e => e.Capacity).HasColumnName("capacity");

                entity.Property(e => e.GroupId).HasColumnName("groupId");

                entity.Property(e => e.Price).HasColumnName("price");

                entity.Property(e => e.TypeName).HasColumnName("typeName");

                entity.HasOne(d => d.GroupCinema)
                    .WithMany(p => p.TypeOfSeats)
                    .HasForeignKey(d => d.GroupId)
                    .HasConstraintName("FKTypeOfSeatGroupCinema001");
            });

            modelBuilder.Entity<UserAccount>(entity =>
            {
                entity.HasKey(e => e.UserId);

                entity.Property(e => e.UserId)
                    .HasColumnName("userId")
                    .HasMaxLength(255)
                    .ValueGeneratedNever();

                entity.Property(e => e.Email)
                    .HasColumnName("email")
                    .HasMaxLength(255);

                entity.Property(e => e.Phone)
                    .HasColumnName("phone")
                    .HasMaxLength(255);

                entity.Property(e => e.UserPassword)
                    .HasColumnName("userPassword")
                    .HasMaxLength(255);
            });

        }
    }
}
