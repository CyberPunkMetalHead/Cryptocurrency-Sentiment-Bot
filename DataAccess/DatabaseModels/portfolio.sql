
-- Drop the table if it exists
DROP TABLE IF EXISTS portfolio;


CREATE TABLE portfolio (
    id serial PRIMARY KEY,
    order_id TEXT REFERENCES orders(order_id),
    pnl DECIMAL(18, 8)
);
