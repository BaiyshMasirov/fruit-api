using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

namespace fruit_api.Data;

[DbContext(typeof(FruitDb))]
public sealed class FruitDbSnapshot : ModelSnapshot
{
    protected override void BuildModel(Microsoft.EntityFrameworkCore.ModelBuilder model)
    {
        model.HasAnnotation("Relational:MaxIdentifierLength", 63);
        NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(model);
        model.Entity("fruit_api.Domain.StorefrontContent", entity =>
        {
            entity.Property<string>("Kind").HasColumnName("kind").HasColumnType("text");
            entity.Property<string>("Id").HasColumnName("id").HasColumnType("text");
            entity.Property<System.Text.Json.JsonDocument>("Payload").IsRequired()
                .HasColumnName("payload").HasColumnType("jsonb");
            entity.Property<DateTimeOffset?>("PublishedAt").HasColumnName("published_at")
                .HasColumnType("timestamp with time zone");
            entity.HasKey("Kind", "Id");
            entity.ToTable("storefront_content");
        });
    }
}
