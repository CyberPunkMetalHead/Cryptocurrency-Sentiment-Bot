# Sentiment Crypto Trading Bot - Inverse Reddit Sentiment

## Overview

**InverseCC Bot** is a cryptocurrency trading bot that leverages sentiment analysis from Reddit to make trading decisions. The key feature of this bot is its ability to analyze sentiment in an inverted manner. It actively monitors the sentiment of Reddit posts and descriptions related to cryptocurrencies, focusing on /r/CryptoCurrency. The bot identifies coins with negative sentiment and executes buy orders, hence the name "InverseCC Bot."


## How It Works

The bot follows these general steps:

1. **Sentiment Analysis:** Used NLTK and Vader Lexicon to analyze sentiment from Reddit posts and descriptions, specifically focusing on negative sentiment.

2. **Coin Selection:** Identifies cryptocurrencies associated with negative Reddit sentiment because Reddit /r/Cryptocurrency is always a great inverse indicator.

3. **Trade Execution:** Places buy orders for the selected coins.

4. **Monitoring and Feedback:** Continuously monitors sentiment trends and adjusts trading strategies accordingly.

## Features

- **Inverse Sentiment Analysis:** Buys cryptocurrencies with negative sentiment on /r/CryptoCurrency.
- **Automated Trading:** Executes trades based on predefined strategies.
- **Dockerized Deployment:** Easily deploy and manage the bot using Docker containers.

## Getting Started

### Prerequisites

Make sure you have the following installed for running and editing:

- Docker
- WSL (on windows)
- Visual Studio 2022
- Dotnet 7.0+


### Installation

1. Clone the repository
2. Run Docker
3. Be a chad and buy whatever reddit virgins say not to.

## No code solutions
If you're looking for an easy no-code solution for a crypto trading bot - check out [**Aesir Crypto Trading Bot**](https://aesircrypto.com). It's easy to use and has a wide range of features including Copy Trading, Volatilty Trading, in addition to being one of the fastest trading bot platforms on the market.
<br><br>For more Algotraing talk, [**Join the Aesir Discord**](https://discord.gg/4GGezGQhhg)