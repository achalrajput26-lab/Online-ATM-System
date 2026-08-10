using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
    public class ATM
{
    public static Account ?currentUser;
    public static void CreateAnAccount()
    {
        Console.WriteLine("============= Create An Account =============");
        Console.WriteLine();
        
        string name;
        do
        {
            Console.Write("Your Name : ");
            name = Console.ReadLine()!;
            if (string.IsNullOrWhiteSpace(name))
            {
                Console.WriteLine("Please do not leave this blank !!");
            }
            
        }while(string.IsNullOrWhiteSpace(name));
        
        Console.Write("Your date of birth (DD/MM/YYYY) : ");
        DateTime dailyDate;
        while(!DateTime.TryParse(Console.ReadLine(), out dailyDate))
        {
            Console.WriteLine("Invalid date.");
        }

        DateTime nowDate = DateTime.Now;

        if(dailyDate > nowDate.AddYears(-18))
        {
            Console.WriteLine("If you are under 18 , you cannot open an account !!");
            return;
        }

        Console.WriteLine("Note : It must consist solely of numbers and be 6 digits long !!");
        string pin;
        while (true)
        {
            pin = PiN("Create a PIN : ");
        
            if (!Program.accounts.Any(x =>
                                        x.Name.Equals(name, StringComparison.OrdinalIgnoreCase) && 
                                        x.Pin == pin))
            {
                break;
            }
            
            Console.WriteLine("This name and PIN are already used by another account.");
            Console.WriteLine("Please choose a different PIN.");
        }
            

        string passphrase;
        do
        {
            Console.Write("Choose a passphrase in case you forget your PIN : ");
            passphrase = Console.ReadLine()!;
            
            if (string.IsNullOrWhiteSpace(passphrase))
            {
                Console.WriteLine("Please do not leave this blank !!");
            }
            
        }while(string.IsNullOrWhiteSpace(passphrase));
        
        int iban = Program.accounts.Count > 0 ? Program.accounts.Max(x => x.IBAN) + 1 : 100000;

        Account account = new Account();
        account.IBAN = iban;
        account.Name = name;
        account.Pin = pin;
        account.Passphrase = passphrase;
        account.DailyLimit = 20000m;
        account.RemainingDailyLimit = 20000m;
        account.Balance = 0m;
        Program.accounts.Add(account);

        SaveLoad.SaveData();

        Console.WriteLine("Your account has been successfully created 💫");
        return;
    }
    public static bool Login()
    {
        Console.WriteLine("============= Log In =============");
        Console.WriteLine();

        if(Program.accounts.Count() == 0)
        {
            Console.WriteLine("There are currently no users. Please create an account !! ");
            Console.WriteLine("==================================");
            return false;
        }

        string name;
        do
        {
            Console.Write("Your Name : ");
            name = Console.ReadLine()!;
            if (string.IsNullOrWhiteSpace(name))
            {
                Console.WriteLine("Please do not leave this blank !!");
                continue;
            }

            if (!Program.accounts.Exists(x => x.Name.Equals(name, StringComparison.OrdinalIgnoreCase)))
            {
                Console.WriteLine("There is no user with this name !!");
            }
            
        }while(string.IsNullOrWhiteSpace(name) || !Program.accounts.Exists(x => x.Name.Equals(name, StringComparison.OrdinalIgnoreCase)));

        for(int i = 1 ; i <= 3 ; i++)
        {
            string pin = PiN("Your PIN : ");

            var account = Program.accounts.FirstOrDefault(x =>
                                                                x.Name.Equals(name, StringComparison.OrdinalIgnoreCase) &&
                                                                x.Pin == pin);
            
            if(account != null)
            {
                currentUser = account;

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Correct PIN");
                Console.ResetColor();

                return true;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                
                if(3 - i == 0)
                {
                    Console.WriteLine("You have no attempts remaining !!");
                }
                else
                {
                    Console.WriteLine($"Incorrect PIN !! --- You have {3 - i} left ");
                }
                Console.ResetColor();
            } 
        }
        
        Console.WriteLine("You have entered the wrong PIN 3 times");
        
        string passphrase;
        for (int i = 1; i <= 3 ; i++)
        {
            do
            {
                Console.Write("Enter your passphrase : ");
                passphrase = Console.ReadLine()!;

                if (string.IsNullOrWhiteSpace(passphrase))
                {
                    Console.WriteLine("Please do not leave this blank !!");
                }

            }while(string.IsNullOrWhiteSpace(passphrase));

            var account = Program.accounts.FirstOrDefault(x =>
                                                                x.Name.Equals(name, StringComparison.OrdinalIgnoreCase) &&
                                                                x.Passphrase == passphrase);

            if(account != null)
            {
                currentUser = account;

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("⭐ Passphrase verified ⭐");
                Console.ResetColor();

                Console.WriteLine();
                
                while (true)
                {
                    string pin = PiN("Create a new PIN : ");
                    
                    string newpin = PiN("Enter the new PIN again : ");

                    if(newpin != pin)
                    {
                        Console.WriteLine("The new PIN doesn't match; please try again !!");
                        continue;
                    }
                    
                    if(Program.accounts.Any(x =>
                                                x != currentUser &&
                                                x.Name.Equals(currentUser!.Name, StringComparison.OrdinalIgnoreCase) &&
                                                x.Pin == newpin))
                    {
                        Console.WriteLine("This PIN is already used by another account with the same name.");
                        Console.WriteLine("Please choose a different PIN.");
                        continue;
                    }
                    
                    currentUser.Pin = newpin;
                    SaveLoad.SaveData();

                    Console.WriteLine("Your PIN has been successfully changed 💫");
                    Console.WriteLine("=========================================");
                    return true;   
                }
            }
            else
            {
                ForeColor("Red");
                if(3 - i == 0)
                {
                    Console.WriteLine("You have no attempts remaining !!");
                }
                else
                {
                    Console.WriteLine($"Incorrect passphrase !! --- You have {3 - i} left ");
                }
                Console.ResetColor();
            }
        }
        
        Console.WriteLine("You are being redirected to the main menu...");
        return false;
    }
    public static string PiN(string write)
    {
        while (true)
        {
            Console.Write(write);
            string inputPin = Console.ReadLine()!;

            if (string.IsNullOrWhiteSpace(inputPin))
            {
                Console.WriteLine("Please do not leave this blank !!");
                continue;
            }

            if (inputPin.Length == 6 && inputPin.All(char.IsDigit))
            {
                return inputPin;
            }
            else
            {
                Console.WriteLine("Please enter a 6-digits number !!");
            }
        }
    }
    public static void ShowMenu()
    {
        bool end = false;
        while (!end)
        {
            ForeColor("White");
            BackColor("Black");
            Console.WriteLine("========== ATM ==========");
            BackColor("Blue");
            Console.WriteLine($"Your Iban : TR{currentUser!.IBAN}");
            Console.WriteLine("1 - Balance Inquiry");
            Console.WriteLine("2 - Cash Deposit");
            Console.WriteLine("3 - Cash Withdrawal");
            Console.WriteLine("4 - Funds Transfer");
            Console.WriteLine("5 - Mini Statement");
            Console.WriteLine("6 - Pin Change");
            Console.WriteLine("0 - Exit");
            Console.ResetColor();
            Console.WriteLine("=========================");

            int choice;
            Console.Write("Your Choice :");
            while(!int.TryParse(Console.ReadLine(), out choice))
            {
                Console.WriteLine("Please enter a number !!");
                Console.Write("Your choice : ");
            }

            switch (choice)
            {
                case 1:
                    ShowBalance();
                break;

                case 2:
                    Deposit();
                break;

                case 3:
                    Withdrawal();
                break;

                case 4:
                    Transfer();
                break;

                case 5:
                    ShowHistory();
                break;

                case 6:
                    PinChange();
                break;

                case 0:
                    currentUser = null;
                    end = true;
                break;

                default:
                    Console.WriteLine("Please enter a valid number !!");
                break;
            }
        } 
    }
    public static void ShowBalance()
    {
        Console.WriteLine("===============================");
        ForeColor("White");
        BackColor("Black");
        Console.WriteLine("\n BALANCE INFORMATION");
        Console.WriteLine();
        BackColor("Blue");
        Console.WriteLine("Currency Code \n TL");
        Console.WriteLine($"Balance \n {currentUser!.Balance}");
        Console.WriteLine($"Available Balance \n {currentUser!.RemainingDailyLimit}");
        Console.WriteLine("===============================");
        Console.ResetColor();
        Console.ReadKey();
    }
    public static void Deposit()
    {
        Console.WriteLine("===============================");
        ForeColor("White");
        BackColor("Black");
        Console.WriteLine("\n CASH DEPOSIT");
        Console.WriteLine();
        BackColor("Blue");
        Console.WriteLine($"Your Current Balance \n {currentUser!.Balance}");
        Console.WriteLine("⚠️ Note: Our ATM doesn't accept coins !!");

        Console.ResetColor();

        decimal cash;
        
        while (true)
        {
            Console.Write("Enter the amount you wish to deposit : ");
            
            if(!decimal.TryParse(Console.ReadLine(), out cash))
            {
                Console.WriteLine("Please enter a number !!");
                continue;
            }

            if (cash <= 0)
            {
                Console.WriteLine("Please enter only positive numbers !!");
                continue;
            }

            if(cash % 5 != 0)
            {
                Console.WriteLine("Please deposit paper money !!");
                continue;
            }
            break;
        }
        
        Console.WriteLine("===============================================");
        Console.WriteLine($"{cash} TL will be deposited into your account.");
        
        string answer;
        while (true)
        {
            Console.Write("Do you approve ? (Yes / No) : ");
            answer = Console.ReadLine()?.ToUpper() ?? "";

            if (string.IsNullOrWhiteSpace(answer))
            {
                ForeColor("Red");
                Console.WriteLine("Please do not leave it blank !!");
                continue;
            }

            if(answer != "YES" && answer != "NO")
            {
                ForeColor("Red");
                Console.WriteLine("Please write yes or no !!");
                continue;
            }
            ForeColor("White");
            break;
        }

        if (answer == "YES")
        {
            Console.WriteLine("⏳ Please wait, your money is being counted ...");
            Thread.Sleep(2000);
            currentUser.Balance += cash;
            currentUser.TransactionHistory.Add($"{DateTime.Now:dd/MM/yyyy HH:mm} _ Deposit  +{cash} TL");
            SaveLoad.SaveData();

            Console.WriteLine("✅ Your transaction has been successfully completed ! ");
            Console.WriteLine($"New Balance : {currentUser.Balance} TL");
            Console.ResetColor();
            Console.ReadKey();
        }
        else
        {
           Console.WriteLine("===============================================");
           Console.WriteLine("❌ Your transaction has been cancelled."); 
        }
    }
    public static void Withdrawal()
    {
        while (true)
        {
            Console.WriteLine("===============================");
            ForeColor("White");
            BackColor("Black");
            Console.WriteLine("\n CASH WITHDRAWAL");
            Console.WriteLine();
            BackColor("Blue");
            Console.WriteLine($"Daily Limit : {currentUser!.DailyLimit}");
            Console.WriteLine($"Remaining Daily Limit : {currentUser!.RemainingDailyLimit}");
            Console.WriteLine();
            Console.WriteLine("1 - 100 TL");
            Console.WriteLine("2 - 200 TL");
            Console.WriteLine("3 - 500 TL");
            Console.WriteLine("4 - 1000 TL");
            Console.WriteLine("5 - 2000 TL");
            Console.WriteLine("6 - 5000 TL");
            Console.WriteLine("7 - Other");
            Console.WriteLine("0 - Back");
            Console.WriteLine("===============================");
            Console.WriteLine();

            Console.ResetColor();

            int choice;
            Console.Write("Select an amount : ");
            while(!int.TryParse(Console.ReadLine(), out choice))
            {
                Console.WriteLine("Please enter a number !!");
                Console.Write("Select an amount : ");
            }

            switch (choice)
            {
                case 1:
                    if (Calculation(100))
                    return;
                break;

                case 2:
                    if(Calculation(200))
                    return;
                break;

                case 3:
                    if(Calculation(500))
                    return;
                break;

                case 4:
                    if(Calculation(1000))
                    return;
                break;

                case 5:
                    if(Calculation(2000))
                    return;
                break;

                case 6:
                    if(Calculation(5000))
                    return;
                break;

                case 7:
                    Other();
                    return;

                case 0:
                    return;

                default:
                    Console.WriteLine("Please enter a number between 0 and 7 !!");
                break;
            }
        }
    }
    public static bool Calculation(decimal cash)
    {
        if (currentUser!.LastWithdrawalDate != DateTime.Today)
        {
            currentUser.RemainingDailyLimit = currentUser.DailyLimit;
        }

        decimal? totalAmount = CheckDailyLimit(cash);

        if (totalAmount == null)
        {
            return false;
        }
        
        if (currentUser!.Balance < totalAmount.Value)
        {
            ForeColor("Red");
            Console.WriteLine("🚧 Your balance is insufficient !! , please try again.");
            ForeColor("White");
            return false;
        }

        Console.WriteLine("⏳ Your transaction is being processed...");
        Thread.Sleep(2000);
        
        currentUser.Balance -= totalAmount.Value;
        currentUser.TransactionHistory.Add($"{DateTime.Now:dd/MM/yyyy HH:mm} _ Withdrawal  -{cash} TL");

        if(cash <= currentUser.RemainingDailyLimit)
        {  
            currentUser.RemainingDailyLimit -= cash;
        }
        else
        {
            currentUser.RemainingDailyLimit = 0;
        }
            
        currentUser.LastWithdrawalDate = DateTime.Today;
        
        SaveLoad.SaveData();
        
        ForeColor("Green");
        Console.WriteLine("✅ Your transaction has been successfully completed ! ");
        ForeColor("White");
            
        Console.WriteLine($"New Balance : {currentUser.Balance} TL");
        Console.WriteLine($"Remaining Daily Limit : {currentUser.RemainingDailyLimit} TL");
        return true;
    }
    public static decimal? CheckDailyLimit(decimal cash)
    {
        if(cash > currentUser!.RemainingDailyLimit)
        {
            ForeColor("Red");
            Console.WriteLine("⚠️ Your daily withdrawal limit has been exceeded !!");
            ForeColor("White");
            Console.WriteLine();

            Console.WriteLine($"Withdrawal Amount : {cash} TL");
            Console.WriteLine($"Remaining Daily Limit : {currentUser.RemainingDailyLimit} TL");
            
            decimal exceeded = cash - currentUser.RemainingDailyLimit;

            Console.WriteLine($"Exceeded Amount : {exceeded} TL");
            Console.WriteLine();

            return Exceeded(cash, exceeded);
        }
        
        return cash;
    }
    public static decimal? Exceeded(decimal cash ,decimal exceededAmount)
    {
        decimal commission = exceededAmount * 0.029m;
        decimal bsmv = commission * 0.05m;
        decimal extraCharge = bsmv + commission;
        decimal totalAmount = extraCharge + cash;
        
        Console.WriteLine($"Extra Charge : {extraCharge} TL");
        Console.WriteLine($"Total Amount Deducted : {totalAmount} TL");
        Console.WriteLine();
        
        string answer;
        while (true)
        {
            Console.Write("Do you approve this transaction ? (Yes / No):");
            answer = Console.ReadLine()?.ToUpper() ?? "";

            if (string.IsNullOrWhiteSpace(answer))
            {
                ForeColor("Red");
                Console.WriteLine("Please do not leave it blank !!");
                continue;
            }

            if(answer != "YES" && answer != "NO")
            {
                ForeColor("Red");
                Console.WriteLine("Please write yes or no !!");
                continue;
            }
            ForeColor("White");
            break;
        }

        if (answer == "YES")
        {
            return totalAmount;
        }
        else
        {
           Console.WriteLine("❌ Your transaction has been cancelled.");
           return null; 
        }
    }
    public static void Other()
    {
        decimal cash;
        while (true)
        {
            Console.Write("Amount to be withdrawn : ");
            
            if(!decimal.TryParse(Console.ReadLine(), out cash))
            {
                Console.WriteLine("Please enter a number !!");
                continue;
            }

            if (cash <= 0)
            {
                Console.WriteLine("Please enter only positive numbers !!");
                continue;
            }

            if(cash % 100 != 0)
            {
                Console.WriteLine("Multiples of 100 are being drawn; please try again !!");
                continue;
            }

            if (!Calculation(cash))
            {
                continue;
            }
            else
            {
                return;
            }
        }     
    }
    public static void Transfer()
    {
        while (true)
        {
            Console.WriteLine("===============================");
            ForeColor("White");
            BackColor("Black");
            Console.WriteLine("\n FUNDS TRANSFER");
            Console.WriteLine();
            BackColor("Blue");
            Console.WriteLine($"Your Iban : TR{currentUser!.IBAN}");
            Console.WriteLine();
            Console.WriteLine("1 - Transfer to Another Account");
            Console.WriteLine("2 - My Accounts");
            Console.WriteLine("0 - Back");
            Console.WriteLine("===============================");
            Console.WriteLine();

            Console.ResetColor();

            int choice;
            Console.Write("Your choice : ");
            while(!int.TryParse(Console.ReadLine(), out choice))
            {
                Console.WriteLine("Please enter a number !!");
                Console.Write("Your choice : ");
            }

            switch (choice)
            {
                case 1:
                    if (toAnotherAccount())
                    {
                        return;
                    }
                break;
                
                case 2:
                    if (MyAccounts())
                    {
                        return;
                    }
                break;
                
                case 0:
                    return;
            
                default:
                    Console.WriteLine("Please enter a number between 0 and 2 !!");
                break;
            }
        }
        
    }
    public static bool toAnotherAccount()
    {
        Console.WriteLine("\n TRANSFER");
        Console.WriteLine();
        
        int iban;
        while (true)
        {
            Console.Write("Enter your IBAN number : ");

            if(!int.TryParse(Console.ReadLine(), out iban))
            {
                ForeColor("Red");
                Console.WriteLine("Please enter a number");
                ForeColor("White");
                continue;
            }

            if (iban < 100000)
            {
                ForeColor("Red");
                Console.WriteLine("Please enter the correct IBAN !!");
                ForeColor("White");
                continue;
            }

            if (!Program.accounts.Exists(x => x.IBAN == iban))
            {
                ForeColor("Red");
                Console.WriteLine("🚧 IBAN not found, please try again !!");
                ForeColor("White");
                continue;
            }    
            break;
        }

        int amount;
        while (true)
        {
            Console.Write("Enter the Amount: ");

            if(!int.TryParse(Console.ReadLine(), out amount))
            {
                ForeColor("Red");
                Console.WriteLine("Please enter a number");
                ForeColor("White");
                continue;
            }

            if(amount <= 0)
            {
                ForeColor("Red");
                Console.WriteLine("Please enter a valid number");
                ForeColor("White");
                if (transferExit("Do you want to exit this process ? (Yes/ No)"))
                {
                    return false;
                }
                continue;
            }

            if (currentUser!.Balance < amount)
            {
                ForeColor("Red");
                Console.WriteLine("🚧 Your balance is insufficient !! , please try again.");
                ForeColor("White");
                if (transferExit("Do you want to exit this process ? (Yes/ No)"))
                {
                    return false;
                }
                continue;
            }
            break;
        }
        
        var recipient = Program.accounts.Find(x => x.IBAN == iban);

        Console.WriteLine($"Recipient : {recipient!.Name}");
        Console.WriteLine($"IBAN : TR{iban}");

        Console.WriteLine($"Amount: {amount}");
        
        if (!transferExit("Do you approve this transaction ? (Yes / No):"))
        {
            Console.WriteLine("❌ Your transaction has been cancelled.");
            return false;
        }
        else
        {
            Console.WriteLine("⏳ Your transaction is being processed...");
            Thread.Sleep(2000);

            currentUser.Balance -= amount;
            
            currentUser.TransactionHistory.Add($"{DateTime.Now:dd/MM/yyyy HH:mm} _ Transfer to {recipient!.Name}  -{amount} TL");
            recipient.TransactionHistory.Add($"{DateTime.Now:dd/MM/yyyy HH:mm} _ Transfer from {currentUser!.Name}  +{amount} TL");
            SaveLoad.SaveData();

            Console.WriteLine("✅ Your transaction has been successfully completed ! ");
            Console.WriteLine($"New Balance : {currentUser.Balance} TL");
            return true;
        }
    }
    public static bool transferExit(string question)
    {
        string decision;
        do
        {
            Console.Write(question);
            decision = Console.ReadLine()!.ToUpper();

            if (string.IsNullOrWhiteSpace(decision))
            {
                Console.WriteLine("Please do not leave it blank !!");
                continue;
            }

            if(decision != "YES" && decision != "NO")
            {
                Console.WriteLine("Please write Yes or No !!");
            }

        }while(string.IsNullOrWhiteSpace(decision) || (decision != "YES" && decision != "NO"));

        if(decision == "YES")
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public static bool MyAccounts()
    {
        var myAccounts = Program.accounts
                        .Where(x => x.Name == currentUser!.Name)
                        .ToList();
        
        if(myAccounts.Count == 1)
        {
            Console.WriteLine("You have only one account.");
            Console.WriteLine("You cannot transfer money between your accounts.");
            return false;
        }

        Console.WriteLine("\n MY ACCOUNTS");
        Console.WriteLine();

        int toCount = 1;
        
        foreach(var account in myAccounts)
        {
            if(account.IBAN == currentUser!.IBAN)
            {
                Console.WriteLine($"Your Current IBAN : TR{account.IBAN}");
            }
            else
            {
                Console.WriteLine($"{toCount} - IBAN : TR{account.IBAN}");
                toCount++;
            } 
        }
        
        Console.WriteLine();
        int totalcount = toCount-1;
        Console.WriteLine("Which IBAN would you like the money sent to ?");
        
        int choice;
        while (true)
        {
            Console.Write("Your Choice : ");
            
            if(!int.TryParse(Console.ReadLine(), out choice))
            {
                Console.WriteLine("Please enter a number !!");
                continue;
            }

            if(choice < 1 || choice > totalcount)
            {
                Console.WriteLine($"Please enter a number between 1 and {totalcount} !!");
                continue; 
            }
            break;
        }

        var OtherAccounts = myAccounts
                            .Where(x => x.IBAN != currentUser!.IBAN)
                            .ToList();
        
        var selectedAccount = OtherAccounts[choice - 1];
        int selectedIBAN = selectedAccount.IBAN;
        
        int amount;
        while (true)
        {
            Console.Write("Enter the amount to be sent : ");

            if (!int.TryParse(Console.ReadLine(), out amount))
            {
                Console.WriteLine("Please enter a number !!");
                continue;
            }
            
            if (amount <= 0)
            {
                Console.WriteLine("Please enter a positive amount !!");
                continue;
            } 

            if (currentUser!.Balance < amount)
            {
                ForeColor("Red");
                Console.WriteLine("🚧 Your balance is insufficient !! , please try again.");
                ForeColor("White");
                continue;
            }

            break;
        }
        Console.WriteLine($"IBAN to which the money will be sent : {selectedIBAN}");
        Console.WriteLine($"Amount : {amount}");

        if (!transferExit("Do you approve this transaction ? (Yes / No):"))
        {
            Console.WriteLine("❌ Your transaction has been cancelled.");
            return false;
        }
        else
        {
            Console.WriteLine("⏳ Your transaction is being processed...");
            Thread.Sleep(2000);
            currentUser.Balance -= amount;
            selectedAccount.Balance += amount;

            currentUser.TransactionHistory.Add($"{DateTime.Now:dd/MM/yyyy HH:mm} _ Transferred to my other account -{amount} TL");
            selectedAccount.TransactionHistory.Add($"{DateTime.Now:dd/MM/yyyy HH:mm} _ Transferred from my other account +{amount} TL");
            SaveLoad.SaveData();

            Console.WriteLine("✅ Your transaction has been successfully completed ! ");
            Console.WriteLine($"New Balance : {currentUser.Balance} TL");
            return true;
        }
    }
    public static void ShowHistory()
    {
        Console.WriteLine("=============================");
        ForeColor("White");
        BackColor("Black");
        Console.WriteLine("\n TRANSACTION HISTORY");
        BackColor("Blue");
        
        Console.WriteLine();
        if(currentUser!.TransactionHistory.Count == 0)
        {
            Console.WriteLine("There are no transactions yet.");
        }
        else
        {
            foreach(var history in currentUser!.TransactionHistory)
            {
                Console.WriteLine(history);
            }
        }
            
        Console.ResetColor();
        Console.WriteLine("=============================");

        Console.ReadKey();
    }
    public static void PinChange()
    {
        Console.WriteLine("=========================================");
        ForeColor("White");
        BackColor("Black");
        Console.WriteLine("\n  PIN CHANGE");
        Console.WriteLine();
        Console.ResetColor();
        while (true)
        {    
            string pin = PiN("Enter your pin : ");

            if(currentUser!.Pin == pin)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("⭐ Pin verified ⭐");
                Console.ResetColor();

                Console.WriteLine();
                
                while (true)
                {
                    string inputpin = PiN("Create a new PIN : ");
                    
                    string newpin = PiN("Enter the new PIN again : ");
                    
                    if(newpin != inputpin)
                    {
                        Console.WriteLine("The new PIN doesn't match; please try again !!");
                        continue;
                    }
                    
                    if(Program.accounts.Any(x =>
                                                x != currentUser &&
                                                x.Name.Equals(currentUser!.Name, StringComparison.OrdinalIgnoreCase) &&
                                                x.Pin == newpin))
                    {
                        Console.WriteLine("This PIN is already used by another account with the same name.");
                        Console.WriteLine("Please choose a different PIN.");
                        continue;
                    }
                    
                    currentUser.Pin = newpin;
                    SaveLoad.SaveData();

                    Console.WriteLine("Your PIN has been successfully changed 💫");
                    Console.WriteLine("=========================================");
                    return;   
                }
            }
            else
            {
                ForeColor("Red");
                Console.WriteLine("Incorrect PIN !!");
                ForeColor("White");
            }
        }   
    }
    public static void ForeColor(string newcolor)
    {
        if(Enum.TryParse(newcolor, true, out ConsoleColor color))
        Console.ForegroundColor = color;
    }
    public static void BackColor(string newcolor)
    {
        if(Enum.TryParse(newcolor, true, out ConsoleColor color))
        Console.BackgroundColor = color;
    }
}