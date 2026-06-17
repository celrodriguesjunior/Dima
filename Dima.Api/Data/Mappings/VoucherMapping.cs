using Dima.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dima.Api.Data.Mappings;

public class VoucherMapping : IEntityTypeConfiguration<Voucher>
{
    public void Configure(EntityTypeBuilder<Voucher> builder)
    {
        builder.ToTable("Voucher");

        builder.HasKey(v => v.Id);

        builder.Property(v => v.Number)
            .IsRequired()
            .HasColumnType("CHAR")
            .HasMaxLength(8);


        builder.Property(v => v.Title)
            .IsRequired()
            .HasColumnType("NVARCHAR")
            .HasMaxLength(80);

        builder.Property(v => v.Description)
            .IsRequired(false)
            .HasColumnType("NVARCHAR")
            .HasMaxLength(255);

        builder.Property(v => v.Amount)
            .IsRequired(true)
            .HasColumnType("DECIMAL");

        builder.Property(v => v.IsActive)
            .IsRequired(true)
            .HasColumnType("BIT");
    }
}
