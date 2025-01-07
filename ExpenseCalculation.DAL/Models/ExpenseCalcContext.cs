using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;

namespace ExpenseCalculation.DAL.Models
{
    public partial class ExpenseCalcContext : DbContext
    {
        public ExpenseCalcContext()
        {
        }

        public ExpenseCalcContext(DbContextOptions<ExpenseCalcContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Member> Members { get; set; }
        public virtual DbSet<PaymentDetail> PaymentDetails { get; set; }
        public virtual DbSet<TripGroup> TripGroups { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var builder = new ConfigurationBuilder()
           .SetBasePath(Directory.GetCurrentDirectory())
           .AddJsonFile("appsettings.json");
            var config = builder.Build();
            var connectionString = config.GetConnectionString("ExpenseCalcConnectionString");

            if (!optionsBuilder.IsConfigured)
            {
                //#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer(connectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>(entity =>
            {
                entity.ToTable("Category");

                entity.Property(e => e.CategoryId)
                    .ValueGeneratedNever()
                    .HasColumnName("Category_id");

                entity.Property(e => e.CategoryName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("Category_name");
            });

            modelBuilder.Entity<Member>(entity =>
            {
                entity.Property(e => e.MemberId)
                    .ValueGeneratedNever()
                    .HasColumnName("member_id");

                entity.Property(e => e.GroupId).HasColumnName("group_id");

                entity.Property(e => e.MemberName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("member_name");
            });

            modelBuilder.Entity<PaymentDetail>(entity =>
            {
                entity.HasKey(e => e.PaymentId)
                    .HasName("PK__PaymentD__ED1FC9EA8712CB45");

                entity.Property(e => e.PaymentId).HasColumnName("payment_id");

                entity.Property(e => e.Amount)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("amount");

                entity.Property(e => e.CategoryId).HasColumnName("Category_id");

                entity.Property(e => e.CategoryName)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("Category_name");

                entity.Property(e => e.GroupId).HasColumnName("group_id");

                entity.Property(e => e.GroupName)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("group_name");

                entity.Property(e => e.MemberId).HasColumnName("member_id");

                entity.Property(e => e.MemberName)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("member_name");

                entity.Property(e => e.Notes)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("notes");

                entity.Property(e => e.PaymentDate)
                    .HasColumnType("date")
                    .HasColumnName("payment_date");

                entity.Property(e => e.PaymentType)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("payment_type");
            });

            modelBuilder.Entity<TripGroup>(entity =>
            {
                entity.HasKey(e => e.GroupId)
                    .HasName("PK__TripGrou__D57795A0D7557155");

                entity.ToTable("TripGroup");

                entity.Property(e => e.GroupId)
                    .ValueGeneratedNever()
                    .HasColumnName("group_id");

                entity.Property(e => e.GroupName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("group_name");

                entity.Property(e => e.GroupShare)
                    .HasColumnType("decimal(18, 0)")
                    .HasColumnName("group_share")
                    .HasDefaultValueSql("((0))");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
