﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using MyFinanceTracker.Api.Data;

#nullable disable

namespace MyFinanceTracker.Api.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("MyFinanceTracker.Api.Models.Account", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("AccountName")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<decimal>("Balance")
                        .HasColumnType("decimal(65,30)");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Account");
                });

            modelBuilder.Entity("MyFinanceTracker.Api.Models.BankAccount", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("AccountName")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("AccountNumber")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("AccountType")
                        .IsRequired()
                        .HasMaxLength(13)
                        .HasColumnType("varchar(13)");

                    b.Property<decimal>("Balance")
                        .HasColumnType("decimal(65,30)");

                    b.Property<string>("BankName")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Accounts");

                    b.HasDiscriminator<string>("AccountType").HasValue("BankAccount");

                    b.UseTphMappingStrategy();
                });

            modelBuilder.Entity("MyFinanceTracker.Api.Models.Budget", b =>
                {
                    b.Property<int>("BudgetId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("BudgetName")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<DateTime>("EndDate")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("BudgetId");

                    b.HasIndex("UserId");

                    b.ToTable("Budgets", (string)null);
                });

            modelBuilder.Entity("MyFinanceTracker.Api.Models.BudgetCategory", b =>
                {
                    b.Property<int>("BudgetCategoryId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<decimal>("AllocatedAmount")
                        .HasColumnType("decimal(65,30)");

                    b.Property<int>("BudgetId")
                        .HasColumnType("int");

                    b.Property<int>("CategoryId")
                        .HasColumnType("int");

                    b.Property<decimal>("SpentAmount")
                        .HasColumnType("decimal(65,30)");

                    b.HasKey("BudgetCategoryId");

                    b.HasIndex("BudgetId");

                    b.HasIndex("CategoryId");

                    b.ToTable("BudgetCategory");
                });

            modelBuilder.Entity("MyFinanceTracker.Api.Models.Category", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int?>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("MyFinanceTracker.Api.Models.Tag", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("Tags");
                });

            modelBuilder.Entity("MyFinanceTracker.Api.Models.Transaction", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<decimal>("Amount")
                        .HasColumnType("decimal(65,30)");

                    b.Property<int?>("BankAccountId")
                        .HasColumnType("int");

                    b.Property<int>("CategoryId")
                        .HasColumnType("int");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Description")
                        .HasColumnType("longtext");

                    b.Property<bool>("IsCleared")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("IsDeposit")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("Memo")
                        .HasColumnType("longtext");

                    b.Property<int?>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("BankAccountId");

                    b.HasIndex("CategoryId");

                    b.HasIndex("UserId");

                    b.ToTable("Transactions");
                });

            modelBuilder.Entity("MyFinanceTracker.Api.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("FirstName")
                        .HasColumnType("longtext");

                    b.Property<string>("GoogleId")
                        .HasColumnType("longtext");

                    b.Property<string>("LastName")
                        .HasColumnType("longtext");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("TagTransaction", b =>
                {
                    b.Property<int>("TagsId")
                        .HasColumnType("int");

                    b.Property<int>("TransactionsId")
                        .HasColumnType("int");

                    b.HasKey("TagsId", "TransactionsId");

                    b.HasIndex("TransactionsId");

                    b.ToTable("TagTransaction");
                });

            modelBuilder.Entity("MyFinanceTracker.Api.Models.CheckingAccount", b =>
                {
                    b.HasBaseType("MyFinanceTracker.Api.Models.BankAccount");

                    b.ToTable("Accounts");

                    b.HasDiscriminator().HasValue("Checking");
                });

            modelBuilder.Entity("MyFinanceTracker.Api.Models.SavingsAccount", b =>
                {
                    b.HasBaseType("MyFinanceTracker.Api.Models.BankAccount");

                    b.Property<decimal>("InterestRate")
                        .HasColumnType("decimal(65,30)");

                    b.ToTable("Accounts");

                    b.HasDiscriminator().HasValue("Savings");
                });

            modelBuilder.Entity("MyFinanceTracker.Api.Models.Account", b =>
                {
                    b.HasOne("MyFinanceTracker.Api.Models.User", "User")
                        .WithMany("Accounts")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("MyFinanceTracker.Api.Models.BankAccount", b =>
                {
                    b.HasOne("MyFinanceTracker.Api.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("MyFinanceTracker.Api.Models.Budget", b =>
                {
                    b.HasOne("MyFinanceTracker.Api.Models.User", "User")
                        .WithMany("Budgets")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("MyFinanceTracker.Api.Models.BudgetCategory", b =>
                {
                    b.HasOne("MyFinanceTracker.Api.Models.Budget", "Budget")
                        .WithMany("BudgetCategories")
                        .HasForeignKey("BudgetId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MyFinanceTracker.Api.Models.Category", "Category")
                        .WithMany()
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Budget");

                    b.Navigation("Category");
                });

            modelBuilder.Entity("MyFinanceTracker.Api.Models.Category", b =>
                {
                    b.HasOne("MyFinanceTracker.Api.Models.User", null)
                        .WithMany("Categories")
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("MyFinanceTracker.Api.Models.Transaction", b =>
                {
                    b.HasOne("MyFinanceTracker.Api.Models.BankAccount", "BankAccount")
                        .WithMany()
                        .HasForeignKey("BankAccountId");

                    b.HasOne("MyFinanceTracker.Api.Models.Category", "Category")
                        .WithMany("Transactions")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MyFinanceTracker.Api.Models.User", null)
                        .WithMany("Transactions")
                        .HasForeignKey("UserId");

                    b.Navigation("BankAccount");

                    b.Navigation("Category");
                });

            modelBuilder.Entity("TagTransaction", b =>
                {
                    b.HasOne("MyFinanceTracker.Api.Models.Tag", null)
                        .WithMany()
                        .HasForeignKey("TagsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MyFinanceTracker.Api.Models.Transaction", null)
                        .WithMany()
                        .HasForeignKey("TransactionsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("MyFinanceTracker.Api.Models.Budget", b =>
                {
                    b.Navigation("BudgetCategories");
                });

            modelBuilder.Entity("MyFinanceTracker.Api.Models.Category", b =>
                {
                    b.Navigation("Transactions");
                });

            modelBuilder.Entity("MyFinanceTracker.Api.Models.User", b =>
                {
                    b.Navigation("Accounts");

                    b.Navigation("Budgets");

                    b.Navigation("Categories");

                    b.Navigation("Transactions");
                });
#pragma warning restore 612, 618
        }
    }
}
