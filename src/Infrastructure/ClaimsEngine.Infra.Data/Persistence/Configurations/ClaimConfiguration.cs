using System;
using ClaimsEngine.Domain.Aggregates.ClaimAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClaimsEngine.Infra.Data.Persistence.Configurations
{
    internal sealed class ClaimConfiguration : IEntityTypeConfiguration<Claim>
    {
        public void Configure(EntityTypeBuilder<Claim> builder)
        {
            builder.ToTable("Claims");
            builder.HasKey(c => c.Id);

            builder.Property(c => c.RowVersion)
                .IsRowVersion();

            builder.ComplexProperty(x => x.Patient, p =>
            {
                p.Property(p => p.Name)
                .HasColumnName("PatientName")
                .IsRequired()
                .HasMaxLength(50);

                p.Property(p => p.DateOfBirth)
                .HasColumnName("PatientDateOfBirth");

                p.Property(p => p.RelationshipToInsured)
                .HasColumnName("PatientRelationshipToInsured")
                .IsRequired();
            });

            builder.ComplexProperty(x => x.Insured, i =>
            {
                i.Property(i => i.Name)
                .HasColumnName("InsuredName")
                .IsRequired()
                .HasMaxLength(50);

                i.Property(i => i.DateOfBirth)
                .HasColumnName("InsuredDateOfBirth");
            });

            builder.HasMany(c => c.LineItems)
            .WithOne()
            .HasForeignKey("ClaimId")
            .OnDelete(DeleteBehavior.Cascade);

            builder.Metadata.FindNavigation(nameof(Claim.LineItems))!
                .SetPropertyAccessMode(PropertyAccessMode.Field);
        }
    }
}
