\set ON_ERROR_STOP on
BEGIN;

WITH products(id, name, variety, category, sort, price, old_price, available, pieces) AS (
    VALUES
        ('p_demo_apple', 'Яблоки', 'Голден', 'c_demo_fruit', 10, 105, 120, true, 5),
        ('p_demo_pear', 'Груши', 'Конференция', 'c_demo_fruit', 20, 180, NULL, true, 6),
        ('p_demo_persimmon', 'Хурма', 'Королёк', 'c_demo_fruit', 30, 160, 195, true, 5),
        ('p_demo_grape', 'Виноград', 'Кишмиш', 'c_demo_berry', 40, 220, NULL, true, NULL),
        ('p_demo_pomegranate', 'Гранаты', NULL, 'c_demo_fruit', 50, 240, NULL, true, 3),
        ('p_demo_blueberry', 'Голубика', NULL, 'c_demo_berry', 60, 840, NULL, false, NULL)
), catalog AS (
    SELECT jsonb_build_object(
        'categories', '[
          {"id":"c_demo_fruit","name":"Фрукты","type":"products","parentId":null,"imageUrl":null,"sortOrder":1},
          {"id":"c_demo_berry","name":"Ягоды и виноград","type":"products","parentId":null,"imageUrl":null,"sortOrder":2},
          {"id":"c_demo_sets","name":"Наборы","type":"sets","parentId":null,"imageUrl":null,"sortOrder":3}
        ]'::jsonb,
        'products', jsonb_agg(jsonb_build_object(
            'id', id, 'name', name, 'variety', variety, 'categoryIds', jsonb_build_array(category),
            'sortOrder', sort, 'imageUrl', NULL, 'unit', 'kg', 'unitTitle', 'кг', 'price', price,
            'oldPrice', old_price, 'step', 500, 'max', 10000, 'approxPiecesPerKg', pieces,
            'weight', NULL, 'season', available, 'badges', '[]'::jsonb, 'available', available,
            'closedOn', '[]'::jsonb, 'boxProductId', NULL, 'searchWords', '[]'::jsonb,
            'description', NULL) ORDER BY sort),
        'sets', '[
          {"id":"s_demo_weekend","name":"Фрукты на выходные","purpose":"Яблоки, груши и виноград","imageUrl":null,
           "items":[{"productId":"p_demo_apple","quantity":1000},{"productId":"p_demo_pear","quantity":1000},
                    {"productId":"p_demo_grape","quantity":500}]},
          {"id":"s_demo_autumn","name":"Осенний набор","purpose":"Хурма и гранаты","imageUrl":null,
           "items":[{"productId":"p_demo_persimmon","quantity":1000},{"productId":"p_demo_pomegranate","quantity":500}]}
        ]'::jsonb) AS payload
    FROM products
)
UPDATE storefront_content SET payload = catalog.payload FROM catalog
WHERE kind = 'singleton' AND id = 'catalog' AND (
    storefront_content.payload->'products' = '[]'::jsonb
    OR storefront_content.payload = catalog.payload
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
            {"id":"w_day","from":"12:00","to":"16:00"},
            {"id":"w_evening","from":"17:00","to":"21:00"}
        ]'::jsonb) ORDER BY day) FROM delivery))
WHERE kind = 'singleton' AND id = 'schedule' AND payload->'days' = '[]'::jsonb;

INSERT INTO storefront_content (kind, id, published_at, payload)
VALUES ('news', 'nw_demo_persimmon', now(), jsonb_build_object(
    'id', 'nw_demo_persimmon', 'type', 'promo', 'title', 'Хурма Королёк со скидкой',
    'subtitle', '160 сом за килограмм', 'imageUrl', NULL,
    'publishedAt', to_char(now() AT TIME ZONE 'Asia/Bishkek', 'YYYY-MM-DD"T"HH24:MI:SS') || '+06:00',
    'blocks', '[{"type":"p","text":"Хурма Королёк в демонстрационной витрине Fruit."}]'::jsonb,
    'action', '{"type":"product","id":"p_demo_persimmon"}'::jsonb))
ON CONFLICT (kind, id) DO NOTHING;

COMMIT;
