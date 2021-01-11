using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WizLib_Model.Models;

namespace WizLib_DataAccess.FluentConfig
{
    public class FluentBookConfig : IEntityTypeConfiguration<Fluent_Book>
    {
        public void Configure(EntityTypeBuilder<Fluent_Book> modelBuilder)
        {
            // Book
            modelBuilder.HasKey(b => b.Book_Id);
            modelBuilder.Property(b => b.ISBN).IsRequired().HasMaxLength(15);
            modelBuilder.Property(b => b.Title).IsRequired();
            modelBuilder.Property(b => b.Price).IsRequired();

            // One to one between Book and Book detail
            modelBuilder.HasOne(b => b.Fluent_BookDetail)
                .WithOne(z => z.Fluent_Book)
                .HasForeignKey<Fluent_Book>("BookDetail_Id");

            // One to Many between Book and the Publisher
            modelBuilder.HasOne(b => b.Fluent_Publisher)
                .WithMany(z => z.Fluent_Books)
                .HasForeignKey(b => b.Publisher_Id);
        }
    }
}
