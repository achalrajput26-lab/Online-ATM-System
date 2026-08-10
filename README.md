# 🏦 ATM System

A C# console-based ATM simulation designed to model everyday banking operations through an interactive terminal experience.

The application manages user accounts, authentication, transactions, transfers, and persistent account data.

### 💳 Banking Operations

* Create and manage multiple accounts
* Secure login flow with PIN and passphrase recovery
* Deposit and withdraw money
* Daily withdrawal limit management
* IBAN-based transfers between accounts
* Transfer funds between personal accounts
* Review transaction history
* Change account PIN

### 💾 Data & Logic

Account and transaction data are persisted using **JSON**, allowing information to remain available after restarting the application.

The project also uses **LINQ** for account queries and collection management, alongside extensive input validation for user interactions.

### 🔧 Built With

**C# · .NET · LINQ · System.Text.Json · File I/O**

### ▶️ Run

```bash
dotnet run
```

> This is an educational ATM simulation and is not intended for real financial use.

### 👩‍💻 Author

**Hilal Nur**
