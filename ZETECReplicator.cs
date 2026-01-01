/* * Copyright (c) 2025 ZETECdev (https://github.com/ZETECdev)
 * Licensed under the MIT License.
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * The above copyright notice and this permission notice shall be included in all
 * copies or substantial portions of the Software.
 */

#region Using declarations
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Xml.Serialization;
using NinjaTrader.Cbi;
using NinjaTrader.Gui;
using NinjaTrader.Gui.Chart;
using NinjaTrader.NinjaScript;
using NinjaTrader.Data;
using NinjaTrader.NinjaScript.Indicators;
using NinjaTrader.NinjaScript.Strategies;
#endregion

namespace NinjaTrader.NinjaScript.Strategies
{
    public class ZETECReplicator : Strategy
    {
        private Account masterAccount;
        private List<Account> slaveAccounts = new List<Account>();
		
		// MAIN ACCOUNT
        private string masterAccountName = "Earn2Trade"; 
		
		// SLAVE ACCOUNTS
        private string[] slaveAccountNames = { "Bulenox" };

        protected override void OnStateChange()
        {
            if (State == State.SetDefaults)
            {
                Description = "ZETECReplicator";
                Name = "ZETECReplicator";
                Calculate = Calculate.OnPriceChange;
                IsOverlay = true;
                IsRealtimeErrorHandlingIgnore = true; 
            }
            else if (State == State.Startup)
            {
                lock (Account.All)
                {
                    masterAccount = Account.All.FirstOrDefault(a => a.Name == masterAccountName);
                    
                    foreach (string name in slaveAccountNames)
                    {
                        var acc = Account.All.FirstOrDefault(a => a.Name == name);
                        if (acc != null) slaveAccounts.Add(acc);
                        else Print("Slave not found: " + name);
                    }
                }

                if (masterAccount == null)
                {
                    Print("Master Account not found.");
                }
                else
                {
                    masterAccount.OrderUpdate += OnMasterOrderUpdate;
                }
            }
        }

        private void OnMasterOrderUpdate(object sender, OrderEventArgs e)
        {
            if (e.Order.Name.StartsWith("ZETEC_")) return;

            if (e.OrderState == OrderState.Submitted)
            {
                foreach (Account acc in slaveAccounts)
                {
                    acc.CreateOrder(e.Order.Instrument, e.Order.OrderAction, e.Order.OrderType, 
                        e.Order.TimeInForce, e.Order.Quantity, e.Order.LimitPrice, e.Order.StopPrice, 
                        "", "ZETEC_" + e.Order.Name, null);
                }
            }

            if (e.OrderState == OrderState.Cancelled)
            {
                foreach (Account acc in slaveAccounts)
                {
                    lock (acc.Orders)
                    {
                        var targetOrder = acc.Orders.FirstOrDefault(o => 
                            o.Instrument == e.Order.Instrument && 
                            o.Name == "ZETEC_" + e.Order.Name &&
                            (o.OrderState == OrderState.Working || o.OrderState == OrderState.Accepted));

                        if (targetOrder != null)
                        {
                            acc.CancelOrder(targetOrder);
                        }
                    }
                }
            }
        }

        protected override void OnTerminated()
        {
            if (masterAccount != null)
                masterAccount.OrderUpdate -= OnMasterOrderUpdate;
        }
    }
}