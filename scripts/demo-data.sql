\set ON_ERROR_STOP on
-- Catalog fixture from backend_handoff_2026-10-21.zip, api_model/model.py::seed_catalog.
BEGIN;
UPDATE storefront_content SET payload = $catalog$
{
  "categories": [
    {
      "id": "c_fruit",
      "name": "Фрукты",
      "type": "products",
      "parentId": null,
      "imageUrl": "https://…",
      "sortOrder": 1
    },
    {
      "id": "c_veg",
      "name": "Овощи",
      "type": "products",
      "parentId": null,
      "imageUrl": "https://…",
      "sortOrder": 2
    },
    {
      "id": "c_sets",
      "name": "Наборы",
      "type": "sets",
      "parentId": null,
      "imageUrl": "https://…",
      "sortOrder": 3
    }
  ],
  "products": [
    {
      "id": "p_apple_gold",
      "name": "Яблоки",
      "variety": "Голден",
      "categoryIds": [
        "c_fruit"
      ],
      "sortOrder": 10,
      "imageUrl": "https://img.example/p_apple_gold_v1.jpg",
      "unit": "kg",
      "unitTitle": "кг",
      "price": 105,
      "oldPrice": 120,
      "step": 500,
      "max": 10000,
      "approxPiecesPerKg": 5,
      "weight": null,
      "season": false,
      "badges": [],
      "available": true,
      "closedOn": [],
      "boxProductId": "p_apple_box",
      "searchWords": [
        "яблоки"
      ]
    },
    {
      "id": "p_apple_box",
      "name": "Яблоки",
      "variety": "Голден",
      "categoryIds": [
        "c_fruit"
      ],
      "sortOrder": 10,
      "imageUrl": "https://img.example/p_apple_box_v1.jpg",
      "unit": "box",
      "unitTitle": "ящик",
      "price": 900,
      "oldPrice": null,
      "step": 1,
      "max": 5,
      "approxPiecesPerKg": null,
      "weight": {
        "type": "approx",
        "grams": 10000
      },
      "season": false,
      "badges": [],
      "available": true,
      "closedOn": [],
      "boxProductId": null,
      "searchWords": [
        "яблоки"
      ]
    },
    {
      "id": "p_carrot",
      "name": "Морковь",
      "variety": null,
      "categoryIds": [
        "c_veg"
      ],
      "sortOrder": 10,
      "imageUrl": "https://img.example/p_carrot_v1.jpg",
      "unit": "kg",
      "unitTitle": "кг",
      "price": 37,
      "oldPrice": null,
      "step": 500,
      "max": 10000,
      "approxPiecesPerKg": null,
      "weight": null,
      "season": false,
      "badges": [],
      "available": true,
      "closedOn": [],
      "boxProductId": null,
      "searchWords": [
        "морковь"
      ]
    },
    {
      "id": "p_raspberry",
      "name": "Малина",
      "variety": null,
      "categoryIds": [
        "c_fruit"
      ],
      "sortOrder": 10,
      "imageUrl": "https://img.example/p_raspberry_v1.jpg",
      "unit": "tray",
      "unitTitle": "лоток",
      "price": 160,
      "oldPrice": null,
      "step": 1,
      "max": 20,
      "approxPiecesPerKg": null,
      "weight": {
        "type": "approx",
        "grams": 250
      },
      "season": false,
      "badges": [],
      "available": true,
      "closedOn": [],
      "boxProductId": null,
      "searchWords": [
        "малина"
      ]
    },
    {
      "id": "p_strawberry",
      "name": "Клубника",
      "variety": null,
      "categoryIds": [
        "c_fruit"
      ],
      "sortOrder": 10,
      "imageUrl": "https://img.example/p_strawberry_v1.jpg",
      "unit": "tray",
      "unitTitle": "лоток",
      "price": 200,
      "oldPrice": null,
      "step": 1,
      "max": 20,
      "approxPiecesPerKg": null,
      "weight": {
        "type": "approx",
        "grams": 250
      },
      "season": false,
      "badges": [],
      "available": true,
      "closedOn": [],
      "boxProductId": null,
      "searchWords": [
        "клубника"
      ]
    },
    {
      "id": "p_melon",
      "name": "Дыня",
      "variety": null,
      "categoryIds": [
        "c_fruit"
      ],
      "sortOrder": 10,
      "imageUrl": "https://img.example/p_melon_v1.jpg",
      "unit": "piece",
      "unitTitle": "шт",
      "price": 180,
      "oldPrice": null,
      "step": 1,
      "max": 5,
      "approxPiecesPerKg": null,
      "weight": {
        "type": "min",
        "grams": 3000
      },
      "season": false,
      "badges": [],
      "available": true,
      "closedOn": [],
      "boxProductId": null,
      "searchWords": [
        "дыня"
      ]
    },
    {
      "id": "p_melon_torpeda",
      "name": "Дыня",
      "variety": "Торпеда",
      "categoryIds": [
        "c_fruit"
      ],
      "sortOrder": 10,
      "imageUrl": "https://img.example/p_melon_torpeda_v1.jpg",
      "unit": "piece",
      "unitTitle": "шт",
      "price": 150,
      "oldPrice": null,
      "step": 1,
      "max": 5,
      "approxPiecesPerKg": null,
      "weight": {
        "type": "min",
        "grams": 2500
      },
      "season": false,
      "badges": [],
      "available": true,
      "closedOn": [],
      "boxProductId": null,
      "searchWords": [
        "дыня"
      ]
    },
    {
      "id": "p_potato",
      "name": "Картофель",
      "variety": null,
      "categoryIds": [
        "c_veg"
      ],
      "sortOrder": 10,
      "imageUrl": "https://img.example/p_potato_v1.jpg",
      "unit": "kg",
      "unitTitle": "кг",
      "price": 38,
      "oldPrice": null,
      "step": 500,
      "max": 20000,
      "approxPiecesPerKg": null,
      "weight": null,
      "season": false,
      "badges": [],
      "available": true,
      "closedOn": [],
      "boxProductId": "p_potato_bag",
      "searchWords": [
        "картофель"
      ]
    },
    {
      "id": "p_potato_bag",
      "name": "Картофель",
      "variety": null,
      "categoryIds": [
        "c_veg"
      ],
      "sortOrder": 10,
      "imageUrl": "https://img.example/p_potato_bag_v1.jpg",
      "unit": "bag",
      "unitTitle": "мешок",
      "price": 320,
      "oldPrice": null,
      "step": 1,
      "max": 5,
      "approxPiecesPerKg": null,
      "weight": {
        "type": "approx",
        "grams": 10000
      },
      "season": false,
      "badges": [],
      "available": true,
      "closedOn": [],
      "boxProductId": null,
      "searchWords": [
        "картофель"
      ]
    },
    {
      "id": "p_plum",
      "name": "Слива",
      "variety": null,
      "categoryIds": [
        "c_fruit"
      ],
      "sortOrder": 10,
      "imageUrl": "https://img.example/p_plum_v1.jpg",
      "unit": "kg",
      "unitTitle": "кг",
      "price": 105,
      "oldPrice": null,
      "step": 500,
      "max": 10000,
      "approxPiecesPerKg": null,
      "weight": null,
      "season": false,
      "badges": [],
      "available": true,
      "closedOn": [],
      "boxProductId": null,
      "searchWords": [
        "слива"
      ]
    },
    {
      "id": "p_alycha",
      "name": "Алыча",
      "variety": null,
      "categoryIds": [
        "c_fruit"
      ],
      "sortOrder": 10,
      "imageUrl": "https://img.example/p_alycha_v1.jpg",
      "unit": "kg",
      "unitTitle": "кг",
      "price": 90,
      "oldPrice": null,
      "step": 500,
      "max": 10000,
      "approxPiecesPerKg": null,
      "weight": null,
      "season": false,
      "badges": [],
      "available": true,
      "closedOn": [],
      "boxProductId": null,
      "searchWords": [
        "алыча"
      ]
    },
    {
      "id": "p_beet",
      "name": "Свёкла",
      "variety": null,
      "categoryIds": [
        "c_veg"
      ],
      "sortOrder": 10,
      "imageUrl": "https://img.example/p_beet_v1.jpg",
      "unit": "kg",
      "unitTitle": "кг",
      "price": 40,
      "oldPrice": null,
      "step": 500,
      "max": 10000,
      "approxPiecesPerKg": null,
      "weight": null,
      "season": false,
      "badges": [],
      "available": true,
      "closedOn": [],
      "boxProductId": null,
      "searchWords": [
        "свёкла"
      ]
    },
    {
      "id": "p_cabbage",
      "name": "Капуста",
      "variety": null,
      "categoryIds": [
        "c_veg"
      ],
      "sortOrder": 10,
      "imageUrl": "https://img.example/p_cabbage_v1.jpg",
      "unit": "piece",
      "unitTitle": "кочан",
      "price": 60,
      "oldPrice": null,
      "step": 1,
      "max": 10,
      "approxPiecesPerKg": null,
      "weight": {
        "type": "approx",
        "grams": 2000
      },
      "season": false,
      "badges": [],
      "available": true,
      "closedOn": [],
      "boxProductId": null,
      "searchWords": [
        "капуста"
      ]
    },
    {
      "id": "p_dill",
      "name": "Укроп",
      "variety": null,
      "categoryIds": [
        "c_greens"
      ],
      "sortOrder": 10,
      "imageUrl": "https://img.example/p_dill_v1.jpg",
      "unit": "bunch",
      "unitTitle": "пучок",
      "price": 30,
      "oldPrice": null,
      "step": 1,
      "max": 10,
      "approxPiecesPerKg": null,
      "weight": null,
      "season": false,
      "badges": [],
      "available": true,
      "closedOn": [],
      "boxProductId": null,
      "searchWords": [
        "укроп"
      ]
    },
    {
      "id": "p_honey",
      "name": "Мёд",
      "variety": null,
      "categoryIds": [
        "c_other"
      ],
      "sortOrder": 10,
      "imageUrl": "https://img.example/p_honey_v1.jpg",
      "unit": "piece",
      "unitTitle": "шт",
      "price": 100,
      "oldPrice": null,
      "step": 1,
      "max": 100,
      "approxPiecesPerKg": null,
      "weight": null,
      "season": false,
      "badges": [],
      "available": true,
      "closedOn": [],
      "boxProductId": null,
      "searchWords": [
        "мёд"
      ]
    },
    {
      "id": "p_tomato",
      "name": "Помидоры",
      "variety": "розовые",
      "categoryIds": [
        "c_veg"
      ],
      "sortOrder": 10,
      "imageUrl": "https://img.example/p_tomato_v1.jpg",
      "unit": "kg",
      "unitTitle": "кг",
      "price": 120,
      "oldPrice": null,
      "step": 500,
      "max": 10000,
      "approxPiecesPerKg": null,
      "weight": null,
      "season": false,
      "badges": [],
      "available": true,
      "closedOn": [],
      "boxProductId": null,
      "searchWords": [
        "помидоры"
      ]
    },
    {
      "id": "p_grape",
      "name": "Виноград",
      "variety": null,
      "categoryIds": [
        "c_fruit"
      ],
      "sortOrder": 10,
      "imageUrl": "https://img.example/p_grape_v1.jpg",
      "unit": "kg",
      "unitTitle": "кг",
      "price": 150,
      "oldPrice": null,
      "step": 500,
      "max": 10000,
      "approxPiecesPerKg": null,
      "weight": null,
      "season": false,
      "badges": [],
      "available": true,
      "closedOn": [],
      "boxProductId": null,
      "searchWords": [
        "виноград"
      ]
    },
    {
      "id": "p_pear",
      "name": "Груши",
      "variety": null,
      "categoryIds": [
        "c_fruit"
      ],
      "sortOrder": 10,
      "imageUrl": "https://img.example/p_pear_v1.jpg",
      "unit": "kg",
      "unitTitle": "кг",
      "price": 110,
      "oldPrice": null,
      "step": 500,
      "max": 10000,
      "approxPiecesPerKg": null,
      "weight": null,
      "season": false,
      "badges": [],
      "available": true,
      "closedOn": [],
      "boxProductId": null,
      "searchWords": [
        "груши"
      ]
    },
    {
      "id": "p_onion",
      "name": "Лук",
      "variety": null,
      "categoryIds": [
        "c_veg"
      ],
      "sortOrder": 10,
      "imageUrl": "https://img.example/p_onion_v1.jpg",
      "unit": "kg",
      "unitTitle": "кг",
      "price": 30,
      "oldPrice": null,
      "step": 500,
      "max": 10000,
      "approxPiecesPerKg": null,
      "weight": null,
      "season": false,
      "badges": [],
      "available": true,
      "closedOn": [],
      "boxProductId": null,
      "searchWords": [
        "лук"
      ]
    }
  ],
  "sets": [
    {
      "id": "s_borsch",
      "name": "Для борща",
      "purpose": "…",
      "imageUrl": "https://img.example/s_borsch.jpg",
      "items": [
        {
          "productId": "p_beet",
          "quantity": 500
        },
        {
          "productId": "p_cabbage",
          "quantity": 1
        },
        {
          "productId": "p_dill",
          "quantity": 1
        }
      ]
    },
    {
      "id": "s_apples",
      "name": "Яблочный",
      "purpose": "…",
      "imageUrl": "https://img.example/s_apples.jpg",
      "items": [
        {
          "productId": "p_apple_gold",
          "quantity": 2000
        }
      ]
    },
    {
      "id": "s_plov",
      "name": "Для плова",
      "purpose": null,
      "imageUrl": "https://img.example/s_plov.jpg",
      "items": [
        {
          "productId": "p_carrot",
          "quantity": 1000
        },
        {
          "productId": "p_onion",
          "quantity": 1000
        }
      ]
    }
  ]
}
$catalog$::jsonb
WHERE kind = 'singleton' AND id = 'catalog' AND (
    payload->'products' = '[]'::jsonb
    OR payload #>> '{products,0,id}' = 'p_demo_apple'
);

WITH delivery AS (
    SELECT (now() AT TIME ZONE 'Asia/Bishkek')::date + offset_days AS day
    FROM (VALUES (2), (4), (7)) AS offsets(offset_days)
)
UPDATE storefront_content SET payload = jsonb_build_object(
    'closedDates', '[]'::jsonb,
    'days', (SELECT jsonb_agg(jsonb_build_object(
        'date', day, 'opensAt', NULL,
        'acceptUntil', to_char(day - 1, 'YYYY-MM-DD') || 'T20:00:00+06:00',
        'windows', '[
            {"id":"w12","from":"12:00","to":"16:00"},
            {"id":"w17","from":"17:00","to":"21:00"}
        ]'::jsonb) ORDER BY day) FROM delivery))
WHERE kind = 'singleton' AND id = 'schedule' AND (
    payload->'days' = '[]'::jsonb
    OR payload #>> '{days,0,windows,0,id}' = 'w_day'
);


-- The reference model has no news fixture.
DELETE FROM storefront_content WHERE kind = 'news' AND id = 'nw_demo_persimmon';
COMMIT;
