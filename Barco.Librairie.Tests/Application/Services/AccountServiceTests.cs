using Microsoft.VisualStudio.TestTools.UnitTesting;
using Barco.Librairie.Application.Services;
using Barco.Librairie.Domain.ValueObjects;
using Barco.Librairie.Domain.AggregateRoots;
using Barco.Librairie.Infrastructure.Data; // For InMemory types
using FluentAssertions;
using System.Threading.Tasks;
using System;
using FluentResults;

namespace Barco.Librairie.Tests.Application.Services
{
    [TestClass]
    public class AccountServiceTests
    {
        private IAccountRepository _accountRepository;
        private IUnitOfWork _unitOfWork;
        private AccountService _accountService;
        private CustomerId _testCustomerId;
        private Money _initialDeposit;

        [TestInitialize]
        public void TestInitialize()
        {
            _accountRepository = new InMemoryAccountRepository();
            _unitOfWork = new InMemoryUnitOfWork();
            _accountService = new AccountService(_accountRepository, _unitOfWork);

            _testCustomerId = new CustomerId(Guid.NewGuid());
            _initialDeposit = new Money(100m, Currency.EUR); // Assuming Currency.EUR
        }

        private async Task<AccountId> CreateAndOpenTestAccountAsync()
        {
            var openResult = await _accountService.OpenAccountAsync(_testCustomerId, _initialDeposit);
            openResult.IsSuccess.Should().BeTrue();
            return openResult.Value;
        }

        [TestMethod]
        public async Task OpenAccountAsync_ShouldCreateAccount()
        {
            var accountId = await CreateAndOpenTestAccountAsync();
            var account = await _accountRepository.GetByIdAsync(accountId);
            account.Should().NotBeNull();
            account.Balance.Should().Be(_initialDeposit);
            account.OwnerId.Should().Be(_testCustomerId);
            account.Status.Should().Be(AccountStatus.Active);
        }

        [TestMethod]
        public async Task DepositAsync_ValidDeposit_ShouldIncreaseBalance()
        {
            var accountId = await CreateAndOpenTestAccountAsync();
            var depositAmount = new Money(50m, Currency.EUR);

            var depositResult = await _accountService.DepositAsync(accountId, depositAmount);
            depositResult.IsSuccess.Should().BeTrue();
            depositResult.Value.Amount.Should().Be(depositAmount);

            var account = await _accountRepository.GetByIdAsync(accountId);
            account.Balance.Amount.Should().Be(_initialDeposit.Amount + depositAmount.Amount);
        }

        [TestMethod]
        public async Task WithdrawAsync_ValidWithdrawal_ShouldDecreaseBalance()
        {
            var accountId = await CreateAndOpenTestAccountAsync();
            var withdrawalAmount = new Money(30m, Currency.EUR);

            var withdrawResult = await _accountService.WithdrawAsync(accountId, withdrawalAmount);
            withdrawResult.IsSuccess.Should().BeTrue();
            withdrawResult.Value.Amount.Should().Be(withdrawalAmount);

            var account = await _accountRepository.GetByIdAsync(accountId);
            account.Balance.Amount.Should().Be(_initialDeposit.Amount - withdrawalAmount.Amount);
        }

        [TestMethod]
        public async Task WithdrawAsync_InsufficientFunds_ShouldFail()
        {
            var accountId = await CreateAndOpenTestAccountAsync();
            var withdrawalAmount = new Money(200m, Currency.EUR); // More than initial deposit

            var withdrawResult = await _accountService.WithdrawAsync(accountId, withdrawalAmount);
            withdrawResult.IsSuccess.Should().BeFalse();
            withdrawResult.Errors.Should().ContainSingle(e => e.Message == "Insufficient funds");

            var account = await _accountRepository.GetByIdAsync(accountId);
            account.Balance.Should().Be(_initialDeposit); // Balance should not change
        }

        [TestMethod]
        public async Task WithdrawAsync_AccountNotFound_ShouldFail()
        {
            var nonExistentAccountId = new AccountId(Guid.NewGuid());
            var withdrawalAmount = new Money(50m, Currency.EUR);

            var withdrawResult = await _accountService.WithdrawAsync(nonExistentAccountId, withdrawalAmount);
            withdrawResult.IsSuccess.Should().BeFalse();
            withdrawResult.Errors.Should().ContainSingle(e => e.Message == "Account not found");
        }

        [TestMethod]
        public async Task GetAccountBalanceAsync_AccountExists_ShouldReturnBalance()
        {
            var accountId = await CreateAndOpenTestAccountAsync();

            var balanceResult = await _accountService.GetAccountBalanceAsync(accountId);
            balanceResult.IsSuccess.Should().BeTrue();
            balanceResult.Value.Should().Be(_initialDeposit);
        }

        [TestMethod]
        public async Task GetAccountBalanceAsync_AccountNotFound_ShouldFail()
        {
            var nonExistentAccountId = new AccountId(Guid.NewGuid());

            var balanceResult = await _accountService.GetAccountBalanceAsync(nonExistentAccountId);
            balanceResult.IsSuccess.Should().BeFalse();
            balanceResult.Errors.Should().ContainSingle(e => e.Message == "Account not found");
        }

        [TestMethod]
        public async Task GetAccountStatusAsync_AccountExists_ShouldReturnStatus()
        {
            var accountId = await CreateAndOpenTestAccountAsync();

            var statusResult = await _accountService.GetAccountStatusAsync(accountId);
            statusResult.IsSuccess.Should().BeTrue();
            statusResult.Value.Should().Be(AccountStatus.Active);
        }

        [TestMethod]
        public async Task GetAccountStatusAsync_AccountNotFound_ShouldFail()
        {
            var nonExistentAccountId = new AccountId(Guid.NewGuid());

            var statusResult = await _accountService.GetAccountStatusAsync(nonExistentAccountId);
            statusResult.IsSuccess.Should().BeFalse();
            statusResult.Errors.Should().ContainSingle(e => e.Message == "Account not found");
        }
    }
}
