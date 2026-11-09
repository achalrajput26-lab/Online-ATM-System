# 🏦 ATM System

A **C# console-based ATM simulation** that demonstrates core banking operations through an interactive terminal experience.

The application supports **multiple user accounts, secure authentication, deposits, withdrawals, money transfers, transaction tracking, daily limits, and persistent data storage**.

## 💳 Features

### 🔐 Authentication & Account Management

* Multiple user account support
* PIN-based authentication
* Passphrase-based PIN recovery
* Limited login attempts
* PIN change functionality
* Account balance management

### 💰 Banking Operations

* Deposit money
* Withdraw money
* Quick withdrawal amount selection
* Custom withdrawal amounts
* Daily withdrawal limit
* Remaining daily limit tracking
* BSMV calculation on applicable transactions
* IBAN-based money transfers
* Transfers between personal accounts
* Transaction history

### 📊 Transaction Management

* Records account transactions
* Displays transaction history
* Tracks deposits, withdrawals, and transfers
* Updates account balances after each transaction
* Prevents transactions that exceed available balance or daily limits

### 💾 Data Persistence

Account and transaction data are stored in **JSON**, allowing user information and account balances to persist between application sessions.

The project uses **System.Text.Json** for serialization and deserialization and **LINQ** for account and collection operations.

### 🛡️ Input Validation

The application includes validation for:

* PIN and passphrase input
* Account selection
* Withdrawal amounts
* Transfer amounts
* IBAN input
* Insufficient balance
* Daily withdrawal limits
* Invalid menu selections

## 🔧 Built With

**C# · .NET · LINQ · System.Text.Json · JSON · File I/O · OOP**

## ▶️ Run

```bash
dotnet run
```

> This project is an educational ATM simulation and is not intended for real financial use.

## 👩‍💻 Author

**Achal Rajput**
