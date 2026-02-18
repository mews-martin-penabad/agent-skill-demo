using FluentAssertions;
using Moq;
using SkillsWorkshop.Application.DTOs;
using SkillsWorkshop.Application.Interfaces;
using SkillsWorkshop.Application.Services;
using SkillsWorkshop.Domain;

namespace SkillsWorkshop.Tests;

public class CardPaymentServiceTests
{
    private readonly Mock<ICardPaymentRepository> _repoMock;
    private readonly CardPaymentService _sut;

    public CardPaymentServiceTests()
    {
        _repoMock = new Mock<ICardPaymentRepository>();
        _sut = new CardPaymentService(_repoMock.Object);
    }

    // -------------------------------------------------------------------------
    // GetAllAsync
    // -------------------------------------------------------------------------

    [Fact]
    public async Task GetAllAsync_ReturnsMappedDtos_ForAllPayments()
    {
        var payments = new List<CardPayment>
        {
            new() { Id = Guid.NewGuid(), CardHolderName = "Alice", Last4Digits = "1234", Amount = 100m, Currency = "USD", Status = PaymentStatus.Pending },
            new() { Id = Guid.NewGuid(), CardHolderName = "Bob",   Last4Digits = "5678", Amount = 200m, Currency = "EUR", Status = PaymentStatus.Authorised }
        };
        _repoMock.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>())).ReturnsAsync(payments);

        var result = await _sut.GetAllAsync();

        result.Should().HaveCount(2)
            .And.AllBeOfType<CardPaymentDto>();
    }

    [Fact]
    public async Task GetAllAsync_ReturnsEmpty_WhenNoPaymentsExist()
    {
        _repoMock.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>())).ReturnsAsync([]);

        var result = await _sut.GetAllAsync();

        result.Should().BeEmpty();
    }

    // -------------------------------------------------------------------------
    // GetByIdAsync
    // -------------------------------------------------------------------------

    [Fact]
    public async Task GetByIdAsync_ReturnsDto_WhenPaymentExists()
    {
        var id = Guid.NewGuid();
        var payment = new CardPayment { Id = id, CardHolderName = "Alice", Last4Digits = "1234", Amount = 50m, Currency = "USD", Status = PaymentStatus.Pending };
        _repoMock.Setup(r => r.GetByIdAsync(id, It.IsAny<CancellationToken>())).ReturnsAsync(payment);

        var result = await _sut.GetByIdAsync(id);

        result.Should().NotBeNull();
        result!.Id.Should().Be(id);
        result.CardHolderName.Should().Be("Alice");
        result.Status.Should().Be("Pending");
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsNull_WhenPaymentDoesNotExist()
    {
        var id = Guid.NewGuid();
        _repoMock.Setup(r => r.GetByIdAsync(id, It.IsAny<CancellationToken>())).ReturnsAsync((CardPayment?)null);

        var result = await _sut.GetByIdAsync(id);

        result.Should().BeNull();
    }

    // -------------------------------------------------------------------------
    // CreateAsync
    // -------------------------------------------------------------------------

    [Fact]
    public async Task CreateAsync_ReturnsMappedDto_WithPendingStatus()
    {
        var dto = new CreateCardPaymentDto { CardHolderName = "Alice", Last4Digits = "1234", Amount = 99.99m, Currency = "USD" };
        _repoMock
            .Setup(r => r.CreateAsync(It.IsAny<CardPayment>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((CardPayment p, CancellationToken _) => p);

        var result = await _sut.CreateAsync(dto);

        result.Should().NotBeNull();
        result.Id.Should().NotBe(Guid.Empty);
        result.CardHolderName.Should().Be("Alice");
        result.Last4Digits.Should().Be("1234");
        result.Amount.Should().Be(99.99m);
        result.Currency.Should().Be("USD");
        result.Status.Should().Be("Pending");
    }

    // -------------------------------------------------------------------------
    // UpdateAsync
    // -------------------------------------------------------------------------

    [Fact]
    public async Task UpdateAsync_ReturnsUpdatedDto_WhenPaymentExists()
    {
        var id = Guid.NewGuid();
        var dto = new UpdateCardPaymentDto { Status = "Authorised" };
        var updated = new CardPayment { Id = id, CardHolderName = "Alice", Last4Digits = "1234", Amount = 50m, Currency = "USD", Status = PaymentStatus.Authorised, ProcessedAt = DateTime.UtcNow };
        _repoMock
            .Setup(r => r.UpdateAsync(id, It.IsAny<CardPayment>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(updated);

        var result = await _sut.UpdateAsync(id, dto);

        result.Should().NotBeNull();
        result!.Status.Should().Be("Authorised");
        result.ProcessedAt.Should().NotBeNull();
    }

    [Fact]
    public async Task UpdateAsync_ReturnsNull_WhenPaymentDoesNotExist()
    {
        var id = Guid.NewGuid();
        var dto = new UpdateCardPaymentDto { Status = "Captured" };
        _repoMock
            .Setup(r => r.UpdateAsync(id, It.IsAny<CardPayment>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((CardPayment?)null);

        var result = await _sut.UpdateAsync(id, dto);

        result.Should().BeNull();
    }

    [Fact]
    public async Task UpdateAsync_ThrowsArgumentException_WhenStatusIsInvalid()
    {
        var id = Guid.NewGuid();
        var dto = new UpdateCardPaymentDto { Status = "NotAStatus" };

        var act = async () => await _sut.UpdateAsync(id, dto);

        await act.Should().ThrowAsync<ArgumentException>()
            .WithMessage("*NotAStatus*");
    }

    [Fact]
    public async Task UpdateAsync_DoesNotCallRepository_WhenStatusIsInvalid()
    {
        var id = Guid.NewGuid();
        var dto = new UpdateCardPaymentDto { Status = "NotAStatus" };

        await Assert.ThrowsAsync<ArgumentException>(() => _sut.UpdateAsync(id, dto));

        _repoMock.Verify(r => r.UpdateAsync(It.IsAny<Guid>(), It.IsAny<CardPayment>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    // -------------------------------------------------------------------------
    // DeleteAsync
    // -------------------------------------------------------------------------

    [Fact]
    public async Task DeleteAsync_ReturnsTrue_WhenPaymentDeleted()
    {
        var id = Guid.NewGuid();
        _repoMock.Setup(r => r.DeleteAsync(id, It.IsAny<CancellationToken>())).ReturnsAsync(true);

        var result = await _sut.DeleteAsync(id);

        result.Should().BeTrue();
    }

    [Fact]
    public async Task DeleteAsync_ReturnsFalse_WhenPaymentDoesNotExist()
    {
        var id = Guid.NewGuid();
        _repoMock.Setup(r => r.DeleteAsync(id, It.IsAny<CancellationToken>())).ReturnsAsync(false);

        var result = await _sut.DeleteAsync(id);

        result.Should().BeFalse();
    }
}
