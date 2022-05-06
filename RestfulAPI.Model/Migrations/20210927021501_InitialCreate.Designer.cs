﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using RestfulAPI.Model;

namespace RestfulAPI.Model.Migrations
{
    [DbContext(typeof(MyDbContext))]
    [Migration("20210927021501_InitialCreate")]
    partial class InitialCreate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.6-servicing-10079")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("NetCoreWebApi.Model.Models.TbUser", b =>
                {
                    b.Property<string>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("userId")
                        .HasMaxLength(50);

                    b.Property<DateTime>("CreateTime")
                        .HasColumnName("createTime");

                    b.Property<string>("Email")
                        .HasColumnName("email")
                        .HasMaxLength(120);

                    b.Property<string>("UserName")
                        .HasColumnName("userName")
                        .HasMaxLength(100);

                    b.HasKey("UserId");

                    b.ToTable("tb_User");
                });
#pragma warning restore 612, 618
        }
    }
}
