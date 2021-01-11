using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WizLib_Model.Models;

namespace WizLib_DataAccess.FluentConfig
{
    public class FluentBookAuthorConfig : IEntityTypeConfiguration<Fluent_BookAuthor>
    {
        public void Configure(EntityTypeBuilder<Fluent_BookAuthor> modelBuilder)
        {

            // Many to many relation between Book and the Author
            modelBuilder.HasKey(b => new { b.Book_Id, b.Author_Id });
            modelBuilder
                .HasOne(ba => ba.Fluent_Book)
                .WithMany(z => z.Fluent_BookAuthors)
                .HasForeignKey(z => z.Book_Id);
            modelBuilder
                .HasOne(a => a.Fluent_Author)
                .WithMany(z => z.Fluent_BookAuthors)
                .HasForeignKey(z => z.Author_Id);
        }
    }
}
