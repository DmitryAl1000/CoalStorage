using System.Collections.Generic;
using System.Reflection.Emit;
using Microsoft.EntityFrameworkCore;
using Domain;
using Application;
using System.ComponentModel.DataAnnotations.Schema;
using static System.Reflection.Metadata.BlobBuilder;

namespace MsSqlDbContext
{
    public class CoalStorageMsSqlDbContext : DbContext
    {
        private static bool _firstContact = true;
        public CoalStorageMsSqlDbContext(DbContextOptions<CoalStorageMsSqlDbContext> optionsBuilder) : base(optionsBuilder)
        {
            FakeDbConnected();
        }


        public DbSet<Slot> Slots { get; set; } = null!;
        public DbSet<Area> Areas { get; set; } = null!;
        public DbSet<SlotHistory> AreasHistory { get; set; } = null!;
        public DbSet<CargoHistory> CargoHistory { get; set; } = null!;


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=BlackListDrovosekk;Trusted_Connection=True;");
        }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //Для таблици пикетов (Slot)
            modelBuilder.Entity<Slot>(b => b.HasKey(e => e.SlotId));

            modelBuilder.Entity<Slot>().Property(x => x.SlotName).IsRequired(true);
            modelBuilder.Entity<Slot>().Property(x => x.AreaId).IsRequired(true);
            modelBuilder.Entity<Slot>().Property(x => x.StorageId).IsRequired(true);

            //  //modelBuilder.Entity<Slot>()
            //  //    .HasOne(u => u.Area)
            //  //    .WithMany(c => c.SlotList)
            //  //    .HasForeignKey(u => u.AreaId);

            //Для таблици зон Area
            modelBuilder.Entity<Area>(b => b.HasKey(e => e.AreaId));
            modelBuilder.Entity<Area>().Property(x => x.AreaName).IsRequired(true);
            modelBuilder.Entity<Area>().Property(x => x.CargoOnArea).IsRequired(true);

            //  //Истории состяния зон на складе 

            modelBuilder.Entity<SlotHistory>(b => b.HasKey(e => e.DateTime));
            //  modelBuilder.Entity<SlotHistory>().Property(u => u.DateTime).HasDefaultValueSql("DATETIME('now')");
            modelBuilder.Entity<SlotHistory>().Property(x => x.DateTime).IsRequired(true);
            modelBuilder.Entity<SlotHistory>().Property(x => x.SlotId).IsRequired(true);
            modelBuilder.Entity<SlotHistory>().Property(x => x.NewAreaId).IsRequired(true);

            //  //modelBuilder.Entity<SlotHistory>()
            //  //    .HasOne(u => u.Slot)
            //  //    .WithMany(c => c.SlotHistory)
            //  //    .HasForeignKey(u => u.SlotId);

            //  // История имененеия грузов на складе

            modelBuilder.Entity<CargoHistory>(b => b.HasKey(e => e.DateTime));
            //  modelBuilder.Entity<CargoHistory>().Property(u => u.DateTime).HasDefaultValueSql("DATETIME('now')");
            modelBuilder.Entity<CargoHistory>().Property(x => x.DateTime).IsRequired(true);

            modelBuilder.Entity<CargoHistory>().Property(x => x.AreaName).IsRequired(true);
            modelBuilder.Entity<CargoHistory>().Property(x => x.CargoOnArea).IsRequired(true);

            //  //modelBuilder.Entity<CargoHistory>()
            //  //    .HasOne(u => u.Area)
            //  //    .WithMany(c => c.CargoHistory)
            //  //    .HasForeignKey(u => u.AreaId);
        }

        //временно до подключения к базе данных
        private void FakeDbConnected()
        {
            //Временная часть
            //Создаётся пустая локальная база данных, заполняется хардкодом.
            //Имитация подключения к существующей базе данных
            if (_firstContact)
            {
                Database.EnsureDeleted();
                Database.EnsureCreated();

                Slots.AddRange(SlotLogic.SetSlotStartValues());
                Areas.AddRange(AreaLogic.SetAreaStartValues());

                SaveChanges();
                _firstContact = false;
            }
        }
    }
}
