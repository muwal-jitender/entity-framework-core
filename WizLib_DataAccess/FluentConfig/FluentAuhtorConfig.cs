using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using WizLib_Model.Models;

namespace WizLib_DataAccess.FluentConfig
{
   public class FluentAuhtorConfig : IEntityTypeConfiguration<Fluent_Author>
    {
        public void Configure(EntityTypeBuilder<Fluent_Author> modelBuilder)
        {            
            // Author
            modelBuilder.Property(b => b.FirstName).IsRequired();
            modelBuilder.Property(b => b.LastName).IsRequired();
            modelBuilder.Property(b => b.BirthDate).IsRequired(false);
            modelBuilder.Property(b => b.Location).IsRequired(false);
            modelBuilder.Ignore(b => b.FullName);
            modelBuilder.HasKey(b => b.Author_Id);
        }
    }
}
