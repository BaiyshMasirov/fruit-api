using Microsoft.EntityFrameworkCore;
using fruit_api.Domain;

namespace fruit_api.Data;

public sealed class FruitDb(DbContextOptions<FruitDb> options) : DbContext(options)
{
    protected override void OnModelCreating(ModelBuilder model)
    {
        var content = model.Entity<StorefrontContent>();
        content.ToTable("storefront_content");
        content.HasKey(row => new { row.Kind, row.Id });
        content.Property(row => row.Kind).HasColumnName("kind");
        content.Property(row => row.Id).HasColumnName("id");
        content.Property(row => row.Payload).HasColumnName("payload").HasColumnType("jsonb");
        content.Property(row => row.PublishedAt).HasColumnName("published_at");
    }
}


