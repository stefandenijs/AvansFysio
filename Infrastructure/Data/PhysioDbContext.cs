using System;
using Core.Domain;
using Core.Domain.PatientRecord;
using Core.Domain.People;
using Core.Domain.Space;
using Core.Domain.Time;
using Core.Domain.Treatment;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class PhysioDbContext : DbContext
    {
        public DbSet<Worker> Workers { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Physiotherapist> Physiotherapists { get; set; }
        public DbSet<Intern> Interns { get; set; }
        public DbSet<Availability> Availabilities { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<PatientRecord> PatientRecords { get; set; }
        public DbSet<Treatment> Treatments { get; set; }
        public DbSet<TreatmentPlan> TreatmentPlans { get; set; }
        public DbSet<Room> Rooms { get; set; }

        public PhysioDbContext(DbContextOptions<PhysioDbContext> contextOptions) : base(contextOptions)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Physiotherapist>()
                .HasMany(e => e.HeadPractitioner)
                .WithOne(e => e.HeadPractitioner)
                .HasForeignKey(e => e.HeadPractitionerId)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Physiotherapist>()
                .HasMany(e => e.Supervisor)
                .WithOne(e => e.IntakeSupervisor)
                .HasForeignKey(e => e.IntakeSupervisorId)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Worker>()
                .HasMany(e => e.Intake)
                .WithOne(e => e.IntakeHandler)
                .HasForeignKey(e => e.IntakeHandlerId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Worker>(entity =>
            {
                entity.HasIndex(e => e.Email).IsUnique();
            });

            modelBuilder.Entity<Physiotherapist>().HasData(
                new Physiotherapist
                {
                    Id = 1,
                    Name = "Daniel Marssen",
                    Email = "d.m@docent.avans.nl",
                    PhoneNumber = "0653242635",
                    EmployeeNumber = "302",
                    BigNumber = "25632"
                },
                new Physiotherapist
                {
                    Id = 2,
                    Name = "Mark Frueger",
                    Email = "m.frueger@docent.avans.nl",
                    PhoneNumber = "0664326365",
                    EmployeeNumber = "326",
                    BigNumber = "27042"
                }
            );

            modelBuilder.Entity<Intern>().HasData(
                new Intern
                {
                    Id = 3,
                    Name = "Bidgret van Hengelo",
                    Email = "bam.vheng@student.avans.nl",
                    StudentNumber = "243233"
                },
                new Intern
                {
                    Id = 4,
                    Name = "Rens van Rijen",
                    Email = "r.vrij@student.avans.nl",
                    StudentNumber = "243372"
                },
                new Intern
                {
                    Id = 5,
                    Name = "Magdela Rodriguez",
                    Email = "m.rodriguez@student.avans.nl",
                    StudentNumber = "242331"
                }
            );

            modelBuilder.Entity<Patient>().HasData(
                new Patient
                {
                    Id = 1,
                    Name = "Frank de Boer",
                    Email = "frank@boer.nl",
                    PhoneNumber = "0643434344",
                    Gender = Gender.Man,
                    BirthDate = new DateTime(1978, 6, 20),
                    Role = Role.Docent,
                    IdentificationNumber = "425"
                },
                new Patient
                {
                    Id = 2,
                    Name = "Melissa de Jonge",
                    Email = "Mel@Jonge.com",
                    PhoneNumber = "0643923744",
                    Gender = Gender.Vrouw,
                    BirthDate = new DateTime(1996, 3, 12),
                    Role = Role.Student,
                    IdentificationNumber = "2174234"
                },
                new Patient
                {
                    Id = 3,
                    Name = "Mathieu Demontreux",
                    Email = "mtdemont@orange.fr",
                    PhoneNumber = "0674054252",
                    Gender = Gender.Man,
                    BirthDate = new DateTime(1999, 2, 3),
                    Role = Role.Student,
                    IdentificationNumber = "2125345"
                }
            );

            modelBuilder.Entity<Room>().HasData(
                new Room
                {
                    Id = 1,
                    RoomNumber = 1,
                    RoomType = "Oefenzaal"
                },
                new Room
                {
                    Id = 2,
                    RoomNumber = 2,
                    RoomType = "Oefenzaal"
                },
                new Room
                {
                    Id = 3,
                    RoomNumber = 3,
                    RoomType = "Oefenzaal"
                },
                new Room
                {
                    Id = 4,
                    RoomNumber = 4,
                    RoomType = "Oefenzaal"
                },
                new Room
                {
                    Id = 5,
                    RoomNumber = 1,
                    RoomType = "Behandelruimte"
                },
                new Room
                {
                    Id = 6,
                    RoomNumber = 2,
                    RoomType = "Behandelruimte"
                },
                new Room
                {
                    Id = 7,
                    RoomNumber = 3,
                    RoomType = "Behandelruimte"
                },
                new Room
                {
                    Id = 8,
                    RoomNumber = 4,
                    RoomType = "Behandelruimte"
                }
            );
        }
    }
}
