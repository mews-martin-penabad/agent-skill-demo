using FluentAssertions;
using Moq;
using SkillsWorkshop.Application.DTOs;
using SkillsWorkshop.Application.Interfaces;
using SkillsWorkshop.Application.Services;
using SkillsWorkshop.Domain;

namespace SkillsWorkshop.Tests;

public class ApplePayServiceTests
{
    private readonly Mock<IApplePayRepository> _repoMock;
    private readonly ApplePayService _sut;

    public ApplePayServiceTests()
    {
        _repoMock = new Mock<IApplePayRepository>();
        _sut = new ApplePayService(_repoMock.Object);
    }

    // -------------------------------------------------------------------------
    // GetAllAsync
    // -------------------------------------------------------------------------

    [Fact]
    public async Task GetAllAsync_ReturnsMappedDtos_ForAllTransactions()
    {
        var transactions = new List<ApplePay>
        {
            new() { Id = Guid.NewGuid(), CardHolderName = "Alice", DeviceAccountNumberSuffix = "1234", TransactionId = "txn_1", Amount = 100m, Currency = "USD", Status = ApplePayStatus.Pending },
            new() { Id = Guid.NewGuid(), CardHolderName = "Bob",   DeviceAccountNumberSuffix = "5678", TransactionId = "txn_2", Amount = 200m, Currency = "EUR", Status = ApplePayStatus.Authorised }
        };
        _repoMock.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>())).ReturnsAsync(transactions);

        var result = await _sut.GetAllAsync();

        result.Should().HaveCount(2)
            .And.AllBeOfType<ApplePayDto>();
    }

    [Fact]
    public async Task GetAllAsync_ReturnsEmpty_WhenNoTransactionsExist()
    {
        _repoMock.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>())).ReturnsAsync([]);

        var result = await _sut.GetAllAsync();

        result.Should().BeEmpty();
    }

    // -------------------------------------------------------------------------
    // GetByIdAsync
    // -------------------------------------------------------------------------

    [Fact]
    public async Task GetByIdAsync_ReturnsDto_WhenTransactionExists()
    {
        var id = Guid.NewGuid();
        var transaction = new ApplePay { Id = id, CardHolderName = "Alice", DeviceAccountNumberSuffix = "1234", TransactionId = "txn_1", Amount = 50m, Currency = "USD", Status = ApplePayStatus.Pending };
        _repoMock.Setup(r => r.GetByIdAsync(id, It.IsAny<CancellationToken>())).ReturnsAsync(transaction);

        var result = await _sut.GetByIdAsync(id);

        result.Should().NotBeNull();
        result!.Id.Should().Be(id);
        result.CardHolderName.Should().Be("Alice");
        result.Status.Should().Be("Pending");
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsNull_WhenTransactionDoesNotExist()
    {
        var id = Guid.NewGuid();
        _repoMock.Setup(r => r.GetByIdAsync(id, It.IsAny<CancellationToken>())).ReturnsAsync((ApplePay?)null);

        var result = await _sut.GetByIdAsync(id);

        result.Should().BeNull();
    }

    // -------------------------------------------------------------------------
    // CreateAsync
    // -------------------------------------------------------------------------

    [Fact]
    public async Task CreateAsync_ReturnsMappedDto_WithPendingStatus()
    {
        var dto = new CreateApplePayDto { CardHolderName = "Alice", DeviceAccountNumberSuffix = "1234", TransactionId = "txn_1", Amount = 99.99m, Currency = "USD" };
        _repoMock
            .Setup(r => r.CreateAsync(It.IsAny<ApplePay>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((ApplePay t, CancellationToken _) => t);

        var result = await _sut.CreateAsync(dto);

        result.Should().NotBeNull();
        result.Id.Should().NotBe(Guid.Empty);
        result.CardHolderName.Should().Be("Alice");
        result.DeviceAccountNumberSuffix.Should().Be("1234");
        result.TransactionId.Should().Be("txn_1");
        result.Amount.Should().Be(99.99m);
        result.Currency.Should().Be("USD");
        result.Status.Should().Be("Pending");
    }

    // -------------------------------------------------------------------------
    // UpdateAsync
    // -------------------------------------------------------------------------

    [Fact]
    public async Task UpdateAsync_ReturnsUpdatedDto_WhenTransactionExists()
    {
        var id = Guid.NewGuid();
        var dto = new UpdateApplePayDto { Status = "Authorised" };
        var updated = new ApplePay { Id = id, CardHolderName = "Alice", DeviceAccountNumberSuffix = "1234", TransactionId = "txn_1", Amount = 50m, Currency = "USD", Status = ApplePayStatus.Authorised, ProcessedAt = DateTime.UtcNow };
        _repoMock
            .Setup(r => r.UpdateAsync(id, It.IsAny<ApplePay>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(updated);

        var result = await _sut.UpdateAsync(id, dto);

        result.Should().NotBeNull();
        result!.Status.Should().Be("Authorised");
        result.ProcessedAt.Should().NotBeNull();
    }

    [Fact]
    public async Task UpdateAsync_ReturnsNull_WhenTransactionDoesNotExist()
    {
        var id = Guid.NewGuid();
        var dto = new UpdateApplePayDto { Status = "Captured" };
        _repoMock
            .Setup(r => r.UpdateAsync(id, It.IsAny<ApplePay>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((ApplePay?)null);

        var result = await _sut.UpdateAsync(id, dto);

        result.Should().BeNull();
    }

    [Fact]
    public async Task UpdateAsync_ThrowsArgumentException_WhenStatusIsInvalid()
    {
        var id = Guid.NewGuid();
        var dto = new UpdateApplePayDto { Status = "NotAStatus" };

        var act = async () => await _sut.UpdateAsync(id, dto);

        await act.Should().ThrowAsync<ArgumentException>()
            .WithMessage("*NotAStatus*");
    }

    [Fact]
    public async Task UpdateAsync_DoesNotCallRepository_WhenStatusIsInvalid()
    {
        var id = Guid.NewGuid();
        var dto = new UpdateApplePayDto { Status = "NotAStatus" };

        await Assert.ThrowsAsync<ArgumentException>(() => _sut.UpdateAsync(id, dto));

        _repoMock.Verify(r => r.UpdateAsync(It.IsAny<Guid>(), It.IsAny<ApplePay>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    // -------------------------------------------------------------------------
    // DeleteAsync
    // -------------------------------------------------------------------------

    [Fact]
    public async Task DeleteAsync_ReturnsTrue_WhenTransactionDeleted()
    {
        var id = Guid.NewGuid();
        _repoMock.Setup(r => r.DeleteAsync(id, It.IsAny<CancellationToken>())).ReturnsAsync(true);

        var result = await _sut.DeleteAsync(id);

        result.Should().BeTrue();
    }

    [Fact]
    public async Task DeleteAsync_ReturnsFalse_WhenTransactionDoesNotExist()
    {
        var id = Guid.NewGuid();
        _repoMock.Setup(r => r.DeleteAsync(id, It.IsAny<CancellationToken>())).ReturnsAsync(false);

        var result = await _sut.DeleteAsync(id);

        result.Should().BeFalse();
    }
}
