using System;
using System.Collections.Generic;
    public class Account
{
    public string Name { get; set; } = "";
    public decimal Balance { get; set; }
    public string Pin { get; set; } = "";
    public string Passphrase { get; set; } = "";
    public List<string> TransactionHistory { get; set; } = new List<string>();
    public decimal DailyLimit { get; set; }
    public decimal RemainingDailyLimit { get; set; }
    public DateTime LastWithdrawalDate { get; set; }
    public int IBAN { get; set; }
}