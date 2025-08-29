using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dima.Api.Data.Mappings.Identity;

public class IdentityUserTokenMapping : IEntityTypeConfiguration<IdentityUserToken<long>>
{
    public void Configure(EntityTypeBuilder<IdentityUserToken<long>> builder)
    {
        builder.ToTable("IdentityUserToken");
        builder.HasKey(ut => new { ut.UserId, ut.LoginProvider, ut.Name });
        builder.Property(x => x.LoginProvider).HasMaxLength(128);
        builder.Property(x => x.Name).HasMaxLength(128);
    }
}
