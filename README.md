# ZETECReplicator

A NinjaTrader 8 strategy that automatically replicates orders from a master account to one or more slave accounts in real-time.

## Features

- **Real-time Order Replication**: Automatically copies orders from a master account to slave accounts
- **Multi-Account Support**: Replicate to multiple slave accounts simultaneously
- **Order Synchronization**: Handles both order submissions and cancellations
- **Collision Prevention**: Uses prefixed order names to avoid infinite replication loops
- **Thread-Safe**: Implements proper locking mechanisms for account operations

## How It Works

ZETECReplicator monitors all orders placed on a designated master account. When an order is submitted or cancelled, the strategy automatically replicates that action to all configured slave accounts.

### Supported Order Events

- **Order Submission**: When an order is submitted on the master account, identical orders are created on all slave accounts
- **Order Cancellation**: When an order is cancelled on the master account, corresponding orders are cancelled on slave accounts

## Installation

1. Download the `ZETECReplicator.cs` file
2. Open NinjaTrader 8
3. Go to **Tools > Import > NinjaScript Add-On**
4. Navigate to the downloaded file and import it
5. Compile the strategy: **Tools > Compile**

## Configuration

### Setting Up Accounts

Before using the strategy, you need to configure your master and slave accounts in the code:

1. Open the `ZETECReplicator.cs` file in the NinjaScript Editor
2. Locate lines 40 and 43:

```csharp
// MAIN ACCOUNT
private string masterAccountName = "Earn2Trade"; 

// SLAVE ACCOUNTS
private string[] slaveAccountNames = { "Bulenox" };
```

3. **Change the master account**: Replace `"Earn2Trade"` with your master account name
4. **Change the slave accounts**: Replace `"Bulenox"` with your slave account name(s)
   - For multiple slave accounts, use: `{ "Account1", "Account2", "Account3" }`
   - For a single slave account, use: `{ "AccountName" }`

5. Save the file and recompile: **Tools > Compile**

## Usage

### Activating the Strategy

1. Open a chart in NinjaTrader 8
2. Right-click on the chart and select **Strategies**
3. Click **Add** and select **ZETECReplicator** from the list
4. Click **OK** to apply the strategy to the chart

### Important Notes

- The strategy must be **enabled and running** on a chart for replication to work
- Make sure all account names match exactly (case-sensitive)
- The strategy will print error messages to the Output window if accounts are not found
- Orders replicated to slave accounts will be prefixed with `ZETEC_` to prevent infinite loops

### Verifying It's Working

1. Check the **Output** window (**Tools > Output Window**) for status messages
2. If accounts are not found, you'll see messages like: `"Master Account not found"` or `"Slave not found: AccountName"`
3. Place a test order on your master account and verify it appears on slave accounts

## How to Use in NinjaTrader

### Step-by-Step Activation

1. **Connect your accounts**: Make sure both master and slave accounts are connected in NinjaTrader
2. **Open a chart**: Any instrument, any timeframe
3. **Apply the strategy**:
   - Right-click on the chart → **Strategies**
   - Click **Add**
   - Select **ZETECReplicator**
   - Click **OK**
4. **Verify activation**: Look for the strategy name in the chart's indicator panel
5. **Start trading**: Any order placed on the master account will now be replicated

### Deactivating the Strategy

1. Right-click on the chart
2. Select **Strategies**
3. Select **ZETECReplicator** and click **Remove**
4. Click **OK**

## Troubleshooting

### Orders are not being replicated

- Verify account names are spelled correctly in the code
- Check that all accounts are connected
- Look for error messages in the Output window
- Ensure the strategy is enabled on a chart

### "Account not found" errors

- Double-check the account names in lines 40 and 43
- Account names are case-sensitive
- Make sure accounts are connected before enabling the strategy

## License

This project is licensed under the MIT License - see the LICENSE file for details.

Copyright (c) 2026 ZETECdev

## Disclaimer

**Use at your own risk.** This software is provided "as is" without warranty of any kind. Trading involves risk, and automated order replication can amplify both gains and losses. Always test thoroughly in a simulation environment before using with live accounts.

## Author

Created by [ZETECdev](https://github.com/ZETECdev)

## Contributing

Contributions, issues, and feature requests are welcome! Feel free to check the issues page.
