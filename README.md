# Barco.Librairie

Barco.Librairie is a C# class library that provides core functionalities for a banking application. It is built following Domain-Driven Design (DDD) principles to ensure a clear separation of concerns and a model rich in business logic, closely aligned to the business domain.

## Project Structure

The project is organized into layers, following DDD best practices:

*   **Barco.Librairie/Domain**: This layer contains the core business logic, entities, value objects, and domain events. It represents the heart of the banking domain.
    *   `AggregateRoots`: Contains aggregate root entities like `Account`.
    *   `Entities`: Contains other domain entities like `Transaction`.
    *   `ValueObjects`: Contains value objects that describe attributes of domain objects (e.g., `Money`, `AccountId`).
    *   `Events`: Defines domain events that occur within the system (e.g., `AccountOpened`, `MoneyDeposited`).
*   **Barco.Librairie/Application**: This layer orchestrates the domain logic. It contains application services that handle use cases and coordinate tasks.
    *   `Services`: Includes services like `AccountService` which expose the library's capabilities.
    *   `Events`: Contains handlers for domain events.
*   **Barco.Librairie/Infrastructure**: This layer deals with external concerns like data persistence (though not fully implemented in the provided code), event dispatching, etc.
    *   `DomainEvents`: Contains implementations for dispatching domain events, like `DomainEventDispatcher`.

## Key Features

*   **Account Management**:
    *   Open new bank accounts with an initial deposit.
    *   Support for `Active` account status.
*   **Transaction Processing**:
    *   Deposit funds into an account.
    *   Withdraw funds from an account, with checks for sufficient balance and active account status.
*   **Domain Events**:
    *   `AccountOpened`: Triggered when a new account is created.
    *   `MoneyDeposited`: Triggered when a deposit is made.
    *   `MoneyWithdrawn`: Triggered when a withdrawal is made.
*   **Domain-Driven Design**:
    *   Clear separation of concerns between domain, application, and infrastructure layers.
    *   Use of Entities, Value Objects, Aggregates, and Domain Events.
*   **Result Pattern**:
    *   Uses `FluentResults` for robust error handling and clear communication of operation outcomes.

## Basic Usage (Conceptual)

This library provides services to interact with bank accounts. Here's a conceptual overview of how a client application might use it:

```csharp
// This is conceptual example code.
// Actual implementation would require setting up dependency injection for AccountService,
// IAccountRepository, IUnitOfWork, and other dependencies.

// Assuming AccountService instance is available
var accountService = new AccountService(/* dependencies */);
var customerId = new CustomerId(Guid.NewGuid());
var initialDeposit = new Money(100.00m, Currency.USD); // Assuming Currency is an enum or value object

// 1. Open a new account
var openAccountResult = await accountService.OpenAccountAsync(customerId, initialDeposit);

if (openAccountResult.IsSuccess)
{
    AccountId newAccountId = openAccountResult.Value;
    Console.WriteLine($"Account opened successfully: {newAccountId.Value}");

    // 2. Deposit money
    var depositAmount = new Money(50.00m, Currency.USD);
    var depositResult = await accountService.DepositAsync(newAccountId, depositAmount);

    if (depositResult.IsSuccess)
    {
        Console.WriteLine($"Deposited {depositAmount.Amount} successfully.");
        // The account balance would now be 150.00 USD
        // A MoneyDeposited event would have been raised.
    }
    else
    {
        Console.WriteLine($"Deposit failed: {string.Join(", ", depositResult.Errors.Select(e => e.Message))}");
    }

    // 3. Withdraw money (conceptual, WithdrawAsync not shown in AccountService but present in Account)
    // var account = await accountRepository.GetByIdAsync(newAccountId); // Need to fetch account
    // var withdrawAmount = new Money(30.00m, Currency.USD);
    // var withdrawResult = account.Withdraw(withdrawAmount); // Direct call on domain entity
    // if (withdrawResult.IsSuccess)
    // {
    //    await unitOfWork.SaveChangesAsync();
    //    Console.WriteLine($"Withdrew {withdrawAmount.Amount} successfully.");
    // }
    // else
    // {
    //    Console.WriteLine($"Withdrawal failed: {string.Join(", ", withdrawResult.Errors.Select(e => e.Message))}");
    // }
}
else
{
    Console.WriteLine($"Failed to open account: {string.Join(", ", openAccountResult.Errors.Select(e => e.Message))}");
}
```

**Note**: The `AccountService` in the provided code currently only has `OpenAccountAsync` and `DepositAsync`. A `WithdrawAsync` method would be a natural addition, along with methods to query account balances and transaction history. The `Withdraw` method currently exists on the `Account` domain entity.

## Potential Future Enhancements

*   **Withdrawal Service Method**: Add a `WithdrawAsync` method to the `AccountService`.
*   **Funds Transfer**: Implement functionality to transfer funds between accounts.
*   **Account Queries**: Add methods to `AccountService` to query account balance, status, and transaction history.
*   **Account Statements**: Generate account statements for a given period.
*   **More Account Types**: Introduce different types of accounts (e.g., Savings, Checking) with potentially different rules.
*   **Currency Conversion**: Add support for multiple currencies and currency conversion.
*   **Interest Calculation**: Implement logic for interest accrual on account balances.
*   **User Authentication & Authorization**: Integrate mechanisms for securing access to account operations.
*   **Data Persistence**: Fully implement `IAccountRepository` and `IUnitOfWork` with a chosen data store (e.g., Entity Framework Core with SQL Server, PostgreSQL, or a NoSQL database).
*   **API Layer**: Expose the library's functionality via a RESTful or gRPC API.
*   **Comprehensive Unit and Integration Tests**: Expand test coverage.

## Building the Project

This project is a .NET class library. To build it:

1.  Ensure you have the .NET SDK installed (the project targets .NET 8.0, as seen in `Barco.Librairie.csproj` and `obj` folder structure, though the `.csproj` content wasn't explicitly read, this is a strong inference).
2.  Clone the repository.
3.  Navigate to the root directory of the solution (where `Barco.Librairie.sln` is located).
4.  Run the following command in your terminal:

    ```bash
    dotnet build
    ```

This will compile the library. As a class library, it's meant to be consumed by other applications (e.g., a web API, a console application, or a desktop application).

## Contributing

Contributions are welcome! If you'd like to contribute to this project, please follow these general steps:

1.  Fork the repository.
2.  Create a new branch for your feature or bug fix (`git checkout -b feature/your-feature-name` or `bugfix/issue-description`).
3.  Make your changes, adhering to the existing coding style and design principles.
4.  Add unit tests for any new functionality or bug fixes.
5.  Ensure all tests pass (`dotnet test`).
6.  Commit your changes with a clear and descriptive commit message.
7.  Push your branch to your forked repository.
8.  Create a pull request to the main repository's `main` or `develop` branch (please specify which branch is primary if known, otherwise default to `main`).

Please ensure your code is well-documented and that your pull request clearly describes the changes you've made.
