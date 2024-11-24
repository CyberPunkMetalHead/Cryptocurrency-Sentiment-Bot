-- Drop the table if it exists
DROP TABLE IF EXISTS statistics;

CREATE TABLE statistics (
    id serial PRIMARY KEY,
    symbol TEXT NOT NULL,
    pnl DECIMAL(18, 8) NOT NULL,
    date DATE NOT NULL,
    CONSTRAINT unique_symbol_date UNIQUE (symbol, date)
);