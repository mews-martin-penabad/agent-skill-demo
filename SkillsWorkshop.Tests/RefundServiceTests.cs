using FluentAssertions;
using Moq;
using SkillsWorkshop.Application.DTOs;
using SkillsWorkshop.Application.Interfaces;
using SkillsWorkshop.Application.Services;
using SkillsWorkshop.Domain;

namespace SkillsWorkshop.Tests;

public class RefundServiceTests
{
    private readonly Mock<IRefundRepository> _repoMock;
    private readonly RefundService _sut;

    public RefundServiceTests()
    {
        _repoMock = new Mock<IRefundRepository>();
        _sut = new RefundService(_repoMock.Object);
    }

    // -------------------------------------------------------------------------
    // GetAllAsync
    // -------------------------------------------------------------------------

    [Fact]
    public async Task GetAllAsync_ReturnsMappedDtos_ForAllRefunds()
    {
        var paymentId = Guid.NewGuid();
        var refunds = new List<Refund>
        {
            new() { Id = Guid.NewGuid(), OriginalPaymentId = paymentId, Amount = 50m, Reason = "Duplicate", Status = RefundStatus.Pending },
            new() { Id = Guid.NewGuid(), OriginalPaymentId = paymentId, Amount = 25m, Reason = "Not as described", Status = RefundStatus.Processed }
        };
        _repoMock.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>())).ReturnsAsync(refunds);

        var result = await _sut.GetAllAsync();

        result.Should().HaveCount(2)
            .And.AllBeOfType<RefundDto>();
    }

    [Fact]
    public async Task GetAllAsync_ReturnsEmpty_WhenNoRefundsExist()
    {
        _repoMock.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>())).ReturnsAsync([]);

        var result = await _sut.GetAllAsync();

        result.Should().BeEmpty();
    }

    // -------------------------------------------------------------------------
    // GetByIdAsync
    // -------------------------------------------------------------------------

    [Fact]
    public async Task GetByIdAsync_ReturnsDto_WhenRefundExists()
    {
        var id = Guid.NewGuid();
        var paymentId = Guid.NewGuid();
        var refund = new Refund { Id = id, OriginalPaymentId = paymentId, Amount = 50m, Reason = "Duplicate", Status = RefundStatus.Pending };
        _repoMock.Setup(r => r.GetByIdAsync(id, It.IsAny<CancellationToken>())).ReturnsAsync(refund);

        var result = await _sut.GetByIdAsync(id);

        result.Should().NotBeNull();
        result!.Id.Should().Be(id);
        result.OriginalPaymentId.Should().Be(paymentId);
        result.Status.Should().Be("Pending");
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsNull_WhenRefundDoesNotExist()
    {
        var id = Guid.NewGuid();
        _repoMock.Setup(r => r.GetByIdAsync(id, It.IsAny<CancellationToken>())).ReturnsAsync((Refund?)null);

        var result = await _sut.GetByIdAsync(id);

        result.Should().BeNull();
    }

    // -------------------------------------------------------------------------
    // CreateAsync
    // -------------------------------------------------------------------------

    [Fact]
    public async Task CreateAsync_ReturnsMappedDto_WithPendingStatus()
    {
        var paymentId = Guid.NewGuid();
        var dto = new CreateRefundDto { OriginalPaymentId = paymentId, Amount = 75m, Reason = "Damaged item" };
        _repoMock
            .Setup(r => r.CreateAsync(It.IsAny<Refund>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Refund r, CancellationToken _) => r);

        var result = await _sut.CreateAsync(dto);

        result.Should().NotBeNull();
        result.Id.Should().NotBe(Guid.Empty);
        result.OriginalPaymentId.Should().Be(paymentId);
        result.Amount.Should().Be(75m);
        result.Reason.Should().Be("Damaged item");
        result.Status.Should().Be("Pending");
    }

    // -------------------------------------------------------------------------
    // UpdateAsync
    // -------------------------------------------------------------------------

    [Fact]
    public async Task UpdateAsync_ReturnsUpdatedDto_WhenRefundExists()
    {
        var id = Guid.NewGuid();
        var dto = new UpdateRefundDto { Status = "Processed" };
        var updated = new Refund { Id = id, OriginalPaymentId = Guid.NewGuid(), Amount = 50m, Reason = "Duplicate", Status = RefundStatus.Processed, ProcessedAt = DateTime.UtcNow };
        _repoMock
            .Setup(r => r.UpdateAsync(id, It.IsAny<Refund>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(updated);

        var result = await _sut.UpdateAsync(id, dto);

        result.Should().NotBeNull();
        result!.Status.Should().Be("Processed");
        result.ProcessedAt.Should().NotBeNull();
    }

    [Fact]
    public async Task UpdateAsync_ReturnsNull_WhenRefundDoesNotExist()
    {
        var id = Guid.NewGuid();
        var dto = new UpdateRefundDto { Status = "Rejected" };
        _repoMock
            .Setup(r => r.UpdateAsync(id, It.IsAny<Refund>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Refund?)null);

        var result = await _sut.UpdateAsync(id, dto);

        result.Should().BeNull();
    }

    [Fact]
    public async Task UpdateAsync_ThrowsArgumentException_WhenStatusIsInvalid()
    {
        var id = Guid.NewGuid();
        var dto = new UpdateRefundDto { Status = "NotAStatus" };

        var act = async () => await _sut.UpdateAsync(id, dto);

        await act.Should().ThrowAsync<ArgumentException>()
            .WithMessage("*NotAStatus*");
    }

    [Fact]
    public async Task UpdateAsync_DoesNotCallRepository_WhenStatusIsInvalid()
    {
        var id = Guid.NewGuid();
        var dto = new UpdateRefundDto { Status = "NotAStatus" };

        await Assert.ThrowsAsync<ArgumentException>(() => _sut.UpdateAsync(id, dto));

        _repoMock.Verify(r => r.UpdateAsync(It.IsAny<Guid>(), It.IsAny<Refund>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    // -------------------------------------------------------------------------
    // DeleteAsync
    // -------------------------------------------------------------------------

    [Fact]
    public async Task DeleteAsync_ReturnsTrue_WhenRefundDeleted()
    {
        var id = Guid.NewGuid();
        _repoMock.Setup(r => r.DeleteAsync(id, It.IsAny<CancellationToken>())).ReturnsAsync(true);

        var result = await _sut.DeleteAsync(id);

        result.Should().BeTrue();
    }

    [Fact]
    public async Task DeleteAsync_ReturnsFalse_WhenRefundDoesNotExist()
    {
        var id = Guid.NewGuid();
        _repoMock.Setup(r => r.DeleteAsync(id, It.IsAny<CancellationToken>())).ReturnsAsync(false);

        var result = await _sut.DeleteAsync(id);

        result.Should().BeFalse();
    }
}
