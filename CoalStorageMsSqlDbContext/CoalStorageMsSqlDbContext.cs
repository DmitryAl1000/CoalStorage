﻿using Microsoft.EntityFrameworkCore;
using Domain;
using Application;

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
        public DbSet<SlotHistory> SlotsHistory { get; set; } = null!;
        public DbSet<CargoHistory> CargoHistory { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //Для таблици пикетов (Slot)
            modelBuilder.Entity<Slot>(b => b.HasKey(e => e.SlotId));

            modelBuilder.Entity<Slot>().Property(x => x.SlotName).IsRequired(true);
            modelBuilder.Entity<Slot>().Property(x => x.AreaName).IsRequired(true);
            modelBuilder.Entity<Slot>().Property(x => x.StorageId).IsRequired(true);

            //Для таблици зон Area
            modelBuilder.Entity<Area>(b => b.HasKey(e => e.AreaId));
            modelBuilder.Entity<Area>().Property(x => x.AreaName).IsRequired(true);
            modelBuilder.Entity<Area>().Property(x => x.CargoOnArea).IsRequired(true);

            // Истории состяния зон на складе 

            modelBuilder.Entity<SlotHistory>(b => b.HasKey(e => e.DateTime));
            modelBuilder.Entity<SlotHistory>().Property(x => x.DateTime).IsRequired(true);
            modelBuilder.Entity<SlotHistory>().Property(x => x.SlotName).IsRequired(true);
            modelBuilder.Entity<SlotHistory>().Property(x => x.NewAreaName).IsRequired(true);

            // История имененеия грузов на складе

            modelBuilder.Entity<CargoHistory>(b => b.HasKey(e => e.DateTime));
            // modelBuilder.Entity<CargoHistory>().Property(u => u.DateTime).HasDefaultValueSql("DATETIME('now')");
            modelBuilder.Entity<CargoHistory>().Property(x => x.DateTime).IsRequired(true);

            modelBuilder.Entity<CargoHistory>().Property(x => x.AreaName).IsRequired(true);
            modelBuilder.Entity<CargoHistory>().Property(x => x.CargoOnArea).IsRequired(true);
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

                // Добавляем в историю стартовые данные

                SaveChanges();

                FillFirstContactSlotHistory();
                FillFirstContactCargoHistory();
                _firstContact = false;
            }
        }

        private void FillFirstContactSlotHistory()
        {
            if (_firstContact)
            {
                SlotsHistory.AddRange(SlotHistoryLogic.GetFirstFreezeSlotHistory(Slots));
                SaveChanges();
            }
        }

        private void FillFirstContactCargoHistory()
        {
            if (_firstContact)
            {
                CargoHistory.AddRange(CargoHistoryLogic.GetFirstFreezeCargoHistory(Areas));
                SaveChanges();
            }
        }

    }
}
