using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

namespace fruit_api.Data;

[DbContext(typeof(FruitDb))]
[Migration("20261103000000_Storefront")]
public sealed class CreateStorefront : Migration
{
    protected override void Up(MigrationBuilder migration) => migration.Sql("""
        CREATE TABLE storefront_content (
            kind text NOT NULL,
            id text NOT NULL,
            payload jsonb NOT NULL CHECK (jsonb_typeof(payload) = 'object'),
            published_at timestamptz,
            PRIMARY KEY (kind, id),
            CHECK (kind <> 'news' OR published_at IS NOT NULL)
        );
        CREATE INDEX ix_storefront_news ON storefront_content (published_at DESC, id)
        WHERE kind = 'news';
        INSERT INTO storefront_content (kind, id, payload) VALUES
          ('singleton', 'catalog', '{"categories":[],"products":[],"sets":[]}'),
          ('singleton', 'schedule', '{"days":[],"closedDates":[]}'),
          ('singleton', 'update', '{"update":"none","version":null,"message":null}'),
          ('singleton', 'config', '{
            "settings":{"minOrder":1000,"deliveryFee":100,"freeDeliveryFrom":2000,"minQr":0,
                        "acceptanceReminderMinutes":120,"paymentPollSeconds":3,"complaintWindowHours":24},
            "support":{"whatsapp":"+996000000000","hours":null},
            "terms":{"offerVersion":"2026-10-01","privacyVersion":"2026-10-01","refundTermsVersion":null},
            "zone":{"polygon":{"type":"MultiPolygon","coordinates":[]}},
            "bank":{"hosts":[],"appSchemes":[],"androidPackages":[]},"snippets":{}
          }');
        """);

    protected override void Down(MigrationBuilder migration) =>
        migration.DropTable("storefront_content");
}
