
-- Drop the table if it exists
DROP TABLE IF EXISTS orders;


CREATE TABLE orders (
    id serial PRIMARY KEY,
    order_id TEXT NOT NULL,
    client_order_id TEXT,
    result INT NOT NULL,
    result_code TEXT,
    message TEXT,
    amount DECIMAL NOT NULL,
    amount_filled DECIMAL,
    is_amount_filled_reversed BOOLEAN,
    price DECIMAL,
    average_price DECIMAL,
    order_date TIMESTAMP NOT NULL,
    http_header_date TIMESTAMP,
    completed_date TIMESTAMP,
    market_symbol TEXT NOT NULL,
    is_buy BOOLEAN NOT NULL,
    fees DECIMAL,
    fees_currency TEXT,
    trade_id TEXT,
    trade_date TIMESTAMP,
    update_sequence BIGINT,

    CONSTRAINT unique_order_id UNIQUE (order_id)
);
