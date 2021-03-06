// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ParkingAPI.Data;

namespace ParkingAPI.Migrations
{
    [DbContext(typeof(ParkingSpaceContext))]
    partial class ParkingSpaceContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.6-servicing-10079");

            modelBuilder.Entity("ParkingAPI.Models.ParkingSpace", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime?>("LastOccupation");

                    b.Property<DateTime?>("LastVacancy");

                    b.Property<bool>("ParkingSapceActive");

                    b.Property<int>("ParkingStatus");

                    b.HasKey("Id");

                    b.ToTable("Parking");
                });
#pragma warning restore 612, 618
        }
    }
}
