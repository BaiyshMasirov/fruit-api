using System.Text.Json;
using fruit_api.Application;
using fruit_api.Features;
using Microsoft.AspNetCore.Mvc;

namespace fruit_api.Controllers;

[ApiController]
[Route("api/v1/cart")]
public sealed class CartController(IStorefront storefront) : ControllerBase
{
    [HttpPost("quote")]
    public async Task<IActionResult> Quote(CartInput input, CancellationToken token)
    {
        if (Request.Headers.ContainsKey("Authorization"))
            return Responses.Error(401, "unauthorized", "Требуется вход");

        if (input.Lines is null || input.Lines.Count > 100)
            return Responses.Error(400, "validation", "Проверьте строки корзины");

        using var catalog = JsonDocument.Parse(await storefront.ReadCatalog(token));
        using var configuration = JsonDocument.Parse(await storefront.Read("singleton", "config", token)
            ?? throw new InvalidOperationException("Storefront configuration is missing."));
        using var schedule = JsonDocument.Parse(await storefront.Read("singleton", "schedule", token)
            ?? throw new InvalidOperationException("Delivery schedule is missing."));

        var products = catalog.RootElement.GetProperty("products").EnumerateArray()
            .ToDictionary(product => product.GetProperty("id").GetString()!);
        var date = input.Date?.ToString("yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
        var scheduled = date is not null && schedule.RootElement.GetProperty("days").EnumerateArray()
            .Any(day => day.GetProperty("date").GetString() == date);
        var seen = new HashSet<string>();
        var lines = new List<object>();
        long goods = 0;

        foreach (var line in input.Lines)
        {
            if (line?.ProductId is not { Length: > 0 } id || line.Quantity <= 0 || line.AddedPrice < 0 ||
                !seen.Add(id) || !products.TryGetValue(id, out var product))
                return Responses.Error(400, "validation", "Проверьте товар и количество");

            var unit = product.GetProperty("unit").GetString();
            var step = product.GetProperty("step").GetInt32();
            var max = product.GetProperty("max").GetInt32();
            var price = product.GetProperty("price").GetInt32();
            var quantity = Math.Min(((long)line.Quantity + step - 1) / step, max / step) * step;
            var available = product.GetProperty("available").GetBoolean() &&
                (!scheduled || !product.GetProperty("closedOn").EnumerateArray().Any(day => day.GetString() == date));
            var subtotal = quantity * price;
            var sum = available ? (unit == "kg" ? (subtotal + 999) / 1000 : subtotal) : 0;
            goods = checked(goods + sum);
            lines.Add(new
            {
                product = new
                {
                    productId = id,
                    name = product.GetProperty("name"),
                    variety = product.GetProperty("variety"),
                    imageUrl = product.GetProperty("imageUrl"),
                    unit,
                    unitTitle = product.GetProperty("unitTitle"),
                    weight = product.GetProperty("weight")
                },
                quantity, price, line.AddedPrice, sum,
                status = available ? "ok" : "out_of_stock",
                adjusted = quantity != line.Quantity
            });
        }

        var settings = configuration.RootElement.GetProperty("settings");
        var freeDeliveryMissing = Math.Max(0, settings.GetProperty("freeDeliveryFrom").GetInt32() - goods);
        var delivery = goods == 0 || freeDeliveryMissing == 0 ? 0 : settings.GetProperty("deliveryFee").GetInt32();
        var total = checked(goods + delivery);
        return Responses.Ok(new
        {
            date, mode = new { type = "new" }, lines, goods, delivery, total,
            pointsAvailable = 0, pointsApplied = 0, toPay = total,
            minimum = new { missing = Math.Max(0, settings.GetProperty("minOrder").GetInt32() - goods) },
            freeDelivery = new { missing = freeDeliveryMissing },
            addonOptions = Array.Empty<object>()
        });
    }

    public sealed record CartInput(DateOnly? Date, IReadOnlyList<CartLine?>? Lines);
    public sealed record CartLine(string? ProductId, int Quantity, int AddedPrice);
}
