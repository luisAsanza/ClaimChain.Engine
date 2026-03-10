using System.IdentityModel.Tokens.Jwt;
using ClaimsEngine.Infra.Data.Outbox;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClaimsEngine.Infra.Data.Persistence.Configurations
{
	internal sealed class OutboxMessageConfiguration : IEntityTypeConfiguration<OutboxMessage>
	{
		public void Configure(EntityTypeBuilder<OutboxMessage> builder)
		{
			builder.ToTable("OutboxMessages");

			builder.HasKey(o => o.Id);

			builder.Property(o => o.Type)
				.IsRequired()
				.HasMaxLength(255);

			builder.Property(o => o.Content)
				.IsRequired();

			builder.Property(o => o.CreatedAt)
				.IsRequired();

			builder.Property(o => o.ProcessedAt);

			builder.Property(o => o.Error)
				.HasMaxLength(2000);

			builder.HasIndex(o => o.ProcessedAt)
                .HasFilter("[ProcessedAt] IS NULL")
                .HasDatabaseName("IX_OutboxMessages_Unprocessed");
		}
	}
}

