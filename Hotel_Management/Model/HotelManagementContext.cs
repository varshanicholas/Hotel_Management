using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Hotel_Management.Model;

public partial class HotelManagementContext : DbContext
{
    public HotelManagementContext()
    {
    }

    public HotelManagementContext(DbContextOptions<HotelManagementContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Booking> Bookings { get; set; }

    public virtual DbSet<CurrentBooking> CurrentBookings { get; set; }

    public virtual DbSet<Guest> Guests { get; set; }

    public virtual DbSet<GuestBookingHistory> GuestBookingHistories { get; set; }

    public virtual DbSet<Room> Rooms { get; set; }

    public virtual DbSet<RoomsOverview> RoomsOverviews { get; set; }

    public virtual DbSet<Service> Services { get; set; }

    public virtual DbSet<ServiceRequest> ServiceRequests { get; set; }

//    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
//        => optionsBuilder.UseSqlServer("Data Source =SHAJITH-NICHOLA\\SQLEXPRESS; Initial Catalog = HotelManagement; Integrated Security = True; Trusted_Connection=True;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Booking>(entity =>
        {
            entity.HasKey(e => e.BookingId).HasName("PK__Bookings__73951AEDA56ABDBD");

            entity.Property(e => e.BookingStatus)
                .HasMaxLength(20)
                .HasDefaultValueSql("('Active')");
            entity.Property(e => e.CheckInDate).HasColumnType("date");
            entity.Property(e => e.CheckOutDate).HasColumnType("date");

            entity.HasOne(d => d.Guest).WithMany(p => p.Bookings)
                .HasForeignKey(d => d.GuestId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Bookings__GuestI__2B3F6F97");

            entity.HasOne(d => d.Room).WithMany(p => p.Bookings)
                .HasForeignKey(d => d.RoomId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Bookings__RoomId__2C3393D0");
        });

        modelBuilder.Entity<CurrentBooking>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("CurrentBookings");

            entity.Property(e => e.CheckInDate).HasColumnType("date");
            entity.Property(e => e.GuestName).HasMaxLength(100);
            entity.Property(e => e.RoomNumber).HasMaxLength(10);
        });

        modelBuilder.Entity<Guest>(entity =>
        {
            entity.HasKey(e => e.GuestId).HasName("PK__Guests__0C423C12CE45CBDD");

            entity.HasIndex(e => e.PhoneNumber, "UQ__Guests__85FB4E381688BEF0").IsUnique();

            entity.Property(e => e.Address).HasMaxLength(200);
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.GuestName).HasMaxLength(100);
            entity.Property(e => e.PhoneNumber).HasMaxLength(15);
        });

        modelBuilder.Entity<GuestBookingHistory>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("GuestBookingHistory");

            entity.Property(e => e.CheckInDate).HasColumnType("date");
            entity.Property(e => e.CheckOutDate).HasColumnType("date");
            entity.Property(e => e.GuestName).HasMaxLength(100);
        });

        modelBuilder.Entity<Room>(entity =>
        {
            entity.HasKey(e => e.RoomId).HasName("PK__Rooms__328639394982A442");

            entity.HasIndex(e => e.RoomNumber, "UQ__Rooms__AE10E07AF6589331").IsUnique();

            entity.Property(e => e.AvailabilityStatus)
                .HasMaxLength(20)
                .HasDefaultValueSql("('Available')");
            entity.Property(e => e.PricePerNight).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.RoomNumber).HasMaxLength(10);
            entity.Property(e => e.RoomType).HasMaxLength(50);
        });

        modelBuilder.Entity<RoomsOverview>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("RoomsOverview");

            entity.Property(e => e.AvailabilityStatus).HasMaxLength(20);
            entity.Property(e => e.PricePerNight).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.RoomNumber).HasMaxLength(10);
            entity.Property(e => e.RoomType).HasMaxLength(50);
        });

        modelBuilder.Entity<Service>(entity =>
        {
            entity.HasKey(e => e.ServiceId).HasName("PK__Services__C51BB00A68CB8212");

            entity.Property(e => e.ServiceCost).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.ServiceName).HasMaxLength(100);
        });

        modelBuilder.Entity<ServiceRequest>(entity =>
        {
            entity.HasKey(e => e.RequestId).HasName("PK__ServiceR__33A8517AF26B9F82");

            entity.Property(e => e.RequestDate).HasColumnType("date");

            entity.HasOne(d => d.Booking).WithMany(p => p.ServiceRequests)
                .HasForeignKey(d => d.BookingId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ServiceRe__Booki__31EC6D26");

            entity.HasOne(d => d.Service).WithMany(p => p.ServiceRequests)
                .HasForeignKey(d => d.ServiceId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ServiceRe__Servi__32E0915F");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
