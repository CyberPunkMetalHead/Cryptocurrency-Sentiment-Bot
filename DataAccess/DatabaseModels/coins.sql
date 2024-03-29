﻿
-- Drop the table if it exists
DROP TABLE IF EXISTS coins;


-- Create the 'coins' table
CREATE TABLE coins (
    name VARCHAR(255) NOT NULL,
    symbol VARCHAR(10) NOT NULL,
    PRIMARY KEY (symbol)
);


INSERT INTO coins (name, symbol)
VALUES 
    ('Bitcoin', 'BTC'),
    ('Ethereum', 'ETH'),
    ('Tether USDt', 'USDT'),
    ('BNB', 'BNB'),
    ('XRP', 'XRP'),
    ('USDC', 'USDC'),
    ('Solana', 'SOL'),
    ('Cardano', 'ADA'),
    ('Dogecoin', 'DOGE'),
    ('TRON', 'TRX'),
    ('Toncoin', 'TON'),
    ('Dai', 'DAI'),
    ('Polygon', 'MATIC'),
    ('Polkadot', 'DOT'),
    ('Litecoin', 'LTC'),
    ('Wrapped Bitcoin', 'WBTC'),
    ('Bitcoin Cash', 'BCH'),
    ('Chainlink', 'LINK'),
    ('Shiba Inu', 'SHIB'),
    ('Avalanche', 'AVAX'),
    ('UNUS SED LEO', 'LEO'),
    ('TrueUSD', 'TUSD'),
    ('Stellar', 'XLM'),
    ('Monero', 'XMR'),
    ('OKB', 'OKB'),
    ('Cosmos', 'ATOM'),
    ('Uniswap', 'UNI'),
    ('Ethereum Classic', 'ETC'),
    ('BUSD', 'BUSD'),
    ('Hedera', 'HBAR'),
    ('Filecoin', 'FIL'),
    ('Maker', 'MKR'),
    ('Lido DAO', 'LDO'),
    ('Internet Computer', 'ICP'),
    ('Mantle', 'MANTLE'),
    ('Cronos', 'CRO'),
    ('Aptos', 'APTOS'),
    ('VeChain', 'VET'),
    ('Optimism', 'OPT'),
    ('Arbitrum', 'ARB'),
    ('NEAR Protocol', 'NEAR'),
    ('Quant', 'QNT'),
    ('Aave', 'AAVE'),
    ('Algorand', 'ALGO'),
    ('The Graph', 'GRT'),
    ('Stacks', 'STX'),
    ('USDD', 'USDD'),
    ('XDC Network', 'XDC'),
    ('Bitcoin SV', 'BSV'),
    ('Immutable', 'IMX'),
    ('Render', 'RNDR'),
    ('Injective', 'INJ'),
    ('Tezos', 'XTZ'),
    ('Bitget Token', 'BGB'),
    ('Axie Infinity', 'AXS'),
    ('MultiversX', 'MULT'),
    ('EOS', 'EOS'),
    ('Theta Network', 'THETA'),
    ('The Sandbox', 'SAND'),
    ('THORChain', 'RUNE'),
    ('Decentraland', 'MANA'),
    ('Synthetix', 'SNX'),
    ('Fantom', 'FTM'),
    ('Kava', 'KAVA'),
    ('Neo', 'NEO'),
    ('eCash', 'XEC'),
    ('Pax Dollar', 'USDP'),
    ('PAX Gold', 'PAXG'),
    ('Flow', 'FLOW'),
    ('Tether Gold', 'XAUT'),
    ('Chiliz', 'CHZ'),
    ('Zcash', 'ZEC'),
    ('KuCoin Token', 'KCS'),
    ('Ocean Protocol', 'OCEAN'),
    ('IOTA', 'MIOTA'),
    ('Conflux', 'CFX'),
    ('Curve DAO Token', 'CRV'),
    ('ApeCoin', 'APE'),
    ('Rocket Pool', 'RPL'),
    ('Frax Share', 'FXS'),
    ('Mina', 'MINA'),
    ('Sui', 'SUI'),
    ('Trust Wallet Token', 'TWT'),
    ('Huobi Token', 'HT'),
    ('Klaytn', 'KLAY'),
    ('dYdX', 'DYDX'),
    ('Gala', 'GALA'),
    ('Casper', 'CSPR'),
    ('BitTorrent(New)', 'BTT'),
    ('GMX', 'GMX'),
    ('GateToken', 'GT'),
    ('Compound', 'COMP'),
    ('Terra Classic', 'LUNA'),
    ('WOO Network', 'WOO'),
    ('APENFT', 'NFT'),
    ('Dash', 'DASH'),
    ('Nexo', 'NEXO'),
    ('Pepe', 'PEPE'),
    ('Oasis Network', 'ROSE'),
    ('Zilliqa', 'ZIL'),
    ('1inch Network', '1INCH'),
    ('Arweave', 'AR'),
    ('PancakeSwap', 'CAKE'),
    ('Flare', 'FLR'),
    ('SafePal', 'SFP'),
    ('Basic Attention Token', 'BAT'),
    ('Gnosis', 'GNO'),
    ('Astar', 'ASTAR'),
    ('Qtum', 'QTUM'),
    ('Loopring', 'LRC'),
    ('NEM', 'XEM'),
    ('Convex Finance', 'CVX'),
    ('Ethereum Name Service', 'ENS'),
    ('Bitcoin Gold', 'BTG'),
    ('Celo', 'CELO'),
    ('SingularityNET', 'AGIX'),
    ('MX TOKEN', 'MX'),
    ('Enjin Coin', 'ENJ'),
    ('aelf', 'ELF'),
    ('Worldcoin', 'WDC'),
    ('Mask Network', 'MASK'),
    ('Gemini Dollar', 'GUSD'),
    ('Helium', 'HNT'),
    ('Sei', 'SEI'),
    ('Theta Fuel', 'TFUEL'),
    ('Akash Network', 'AKT'),
    ('JUST', 'JST'),
    ('Decred', 'DCR'),
    ('Loom Network', 'LOOM'),
    ('Chia', 'XCH'),
    ('Ankr', 'ANKR'),
    ('Aragon', 'ANT'),
    ('Ravencoin', 'RVN'),
    ('Bone ShibaSwap', 'BONE'),
    ('Fetch.ai', 'FET'),
    ('STEPN', 'SPN'),
    ('Golem', 'GLM'),
    ('Storj', 'STORJ'),
    ('Threshold', 'THG'),
    ('tomiNet', 'TOMI'),
    ('Livepeer', 'LPT'),
    ('yearn.finance', 'YFI'),
    ('Terra', 'LUNA'),
    ('Holo', 'HOT'),
    ('FLOKI', 'FLOKI'),
    ('Waves', 'WAVES'),
    ('Blur', 'BLUR'),
    ('Balancer', 'BAL'),
    ('ICON', 'ICX'),
    ('Siacoin', 'SC'),
    ('Audius', 'AUDIO'),
    ('JasmyCoin', 'JASMY'),
    ('IoTeX', 'IOTX'),
    ('Solar', 'SOLAR'),
    ('Biconomy', 'BICO'),
    ('ssv.network', 'SSV'),
    ('0x Protocol', 'ZRX'),
    ('Kusama', 'KSM'),
    ('Moonbeam', 'GLMR'),
    ('Hive', 'HIVE'),
    ('Band Protocol', 'BAND'),
    ('Osmosis', 'OSMO'),
    ('Ontology', 'ONT'),
    ('Merit Circle', 'MC'),
    ('WAX', 'WAXP'),
    ('Illuvium', 'ILV'),
    ('Axelar', 'AXL'),
    ('EthereumPoW', 'ETHP'),
    ('MAGIC', 'MAGIC'),
    ('TomoChain', 'TOMO'),
    ('Tellor', 'TRB'),
    ('SushiSwap', 'SUSHI'),
    ('IOST', 'IOST'),
    ('Harmony', 'ONE'),
    ('STP', 'STPT'),
    ('Kyber Network Crystal v2', 'KNC'),
    ('Nervos Network', 'CKB'),
    ('TerraClassicUSD', 'THT'),
    ('Horizen', 'ZEN'),
    ('Kadena', 'KDA'),
    ('BORA', 'BORA'),
    ('Liquity', 'LQTY'),
    ('SKALE', 'SKL'),
    ('Flux', 'FLUX'),
    ('DigiByte', 'DGB'),
    ('Centrifuge', 'CFG'),
    ('Galxe', 'GALXE'),
    ('UMA', 'UMA'),
    ('Alchemy Pay', 'ACH'),
    ('MobileCoin', 'MOB'),
    ('Celer Network', 'CELR'),
    ('Lisk', 'LSK'),
    ('Cartesi', 'CTSI'),
    ('DAO Maker', 'DAO'),
    ('Pundi X (New)', 'PUNDIX'),
    ('Status', 'SNT'),
    ('Stargate Finance', 'STGT'),
    ('Reserve Rights', 'RSR'),
    ('API3', 'API3'),
    ('Ribbon Finance', 'RBN'),
    ('Lido Staked ETH', 'stETH'),
    ('Wrapped TRON', 'WTRX'),
    ('Wrapped Kava', 'WKAVA'),
    ('Wrapped HBAR', 'WHBAR'),
    ('Bitcoin BEP2', 'BTCB'),
    ('Kaspa', 'KAS'),
    ('Wrapped EOS', 'WEOS'),
    ('Frax', 'FRAX'),
    ('TNC Coin', 'TNC'),
    ('Trexcoin', 'TREX'),
    ('HEX', 'HEX'),
    ('UnlimitedIP', 'UIP'),
    ('Wrapped BNB', 'WBNB'),
    ('Radix', 'XRD'),
    ('Rollbit Coin', 'RLB'),
    ('First Digital USD', 'FDUSD'),
    ('FTX Token', 'FTT'),
    ('WEMIX', 'WEMIX'),
    ('USDJ', 'USDJ'),
    ('Huobi BTC', 'HBTC'),
    ('Liquity USD', 'LUSD'),
    ('DeFiChain', 'DFI'),
    ('Aleph Zero', 'AZERO'),
    ('Edgecoin', 'EDGT'),
    ('SwissBorg', 'CHSB'),
    ('Beldex', 'BDX'),
    ('Wrapped Beacon ETH', 'WBETH'),
    ('LUKSO', 'LYX'),
    ('Dora Factory', 'DORA'),
    ('Pendle', 'PENDLE'),
    ('Fasttoken', 'FTN'),
    ('Baby Doge Coin', 'BabyDoge'),
    ('FINSCHIA', 'FNSA'),
    ('Tribe', 'TRIBE'),
    ('STASIS EURO', 'EURS'),
    ('Symbol', 'XYM'),
    ('Polymath', 'POLY'),
    ('Ronin', 'RON'),
    ('PayPal USD', 'PYUSD'),
    ('USDX [Kava]', 'USDX'),
    ('Gains Network', 'GNS'),
    ('LUKSO (Old)', 'LYXe'),
    ('Ark', 'ARK'),
    ('Decimal', 'DEL'),
    ('Rootstock Smart Bitcoin', 'RBTC'),
    ('Keep Network', 'KEEP'),
    ('OriginTrail', 'TRAC'),
    ('Polymesh', 'POLYX'),
    ('JOE', 'JOE'),
    ('Nano', 'XNO'),
    ('Coin98', 'C98'),
    ('PegNet', 'PEG'),
    ('PlayDapp', 'PLA'),
    ('LiteCoin Ultra', 'LTCU'),
    ('Stratis', 'STRAX'),
    ('Metal DAO', 'MTL'),
    ('Global Currency Reserve', 'GCR'),
    ('inSure DeFi', 'SURE'),
    ('Echelon Prime', 'PRIME'),
    ('Open Campus', 'EDU'),
    ('Vulcan Forged PYR', 'PYR'),
    ('dKargo', 'DKA'),
    ('DeXe', 'DEXE'),
    ('Ontology Gas', 'ONG'),
    ('Decentralized Social', 'DESO'),
    ('VeThor Token', 'VTHO'),
    ('Covalent', 'CQT'),
    ('MiL.k', 'MLK'),
    ('Venus', 'XVS'),
    ('Netrum', 'NTR'),
    ('Radiant Capital', 'RDNT'),
    ('Numeraire', 'NMR'),
    ('Niobium Coin', 'NBC'),
    ('Steem', 'STEEM'),
    ('Everscale', 'EVER'),
    ('Powerledger', 'POWR'),
    ('SPACE ID', 'ID'),
    ('Civic', 'CVC'),
    ('Cannation', 'CNNC'),
    ('VVS Finance', 'VVS'),
    ('IQ', 'IQ'),
    ('MVL', 'MVL'),
    ('Origin Protocol', 'OGN'),
    ('Statter Network', 'STT'),
    ('Prom', 'PROM'),
    ('Ordinals', 'ORDI'),
    ('iExec RLC', 'RLC'),
    ('Dogelon Mars', 'ELON'),
    ('Wrapped NXM', 'WNXM'),
    ('Orbs', 'ORBS'),
    ('StormX', 'STMX'),
    ('Vega Protocol', 'VEGA'),
    ('Hashflow', 'HFT'),
    ('Yield Guild Games', 'YGG'),
    ('Telcoin', 'TEL'),
    ('Rootstock Infrastructure Framework', 'RIF'),
    ('ATOR Protocol', 'ATOR'),
    ('Amp', 'AMP'),
    ('Mainframe', 'MFT'),
    ('Radworks', 'RAD'),
    ('Ardor', 'ARDR'),
    ('Chromia', 'CHR'),
    ('DODO', 'DODO'),
    ('Marlin', 'POND'),
    ('Core', 'CORE'),
    ('Dynex', 'DNX'),
    ('OMG Network', 'OMG'),
    ('Sweat Economy', 'SWEAT'),
    ('NKN', 'NKN'),
    ('Hifi Finance', 'HIFI'),
    ('Request', 'REQ'),
    ('UniBot', 'UNIB')
ON CONFLICT (symbol) DO NOTHING;


-- Drop the table if it exists
DROP TABLE IF EXISTS coin_sentiments;


CREATE TABLE coin_sentiments (
    id SERIAL PRIMARY KEY,
    symbol TEXT REFERENCES coins(symbol),
    date DATE NOT NULL,
    sentiment_value DOUBLE PRECISION,
    UNIQUE (symbol, date)
);

