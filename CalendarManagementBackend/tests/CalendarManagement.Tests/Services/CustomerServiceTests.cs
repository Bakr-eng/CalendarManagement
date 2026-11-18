using CalendarManagement.Application.DTOs;
using CalendarManagement.Application.Interfaces;
using CalendarManagement.Application.Services;
using CalendarManagement.Domain.Entities;
using Moq;

namespace CalendarManagement.Tests.Services;

public class CustomerServiceTests
{
    private readonly Mock<ICustomerRepository> _repositoryMock;
    private readonly CustomerService _service;

    public CustomerServiceTests()
    {
        // Körs före varje test – varje test får en ny, tom mock
        _repositoryMock = new Mock<ICustomerRepository>();
        _service = new CustomerService(_repositoryMock.Object);
    }

    // ---------- GetAllAsync ----------

    [Fact]
    public async Task GetAllAsync_WhenCustomersExist_ReturnsAllAsDtos()
    {
        // Arrange
        var customers = new List<Customer>
        {
            new() { Id = 1, Name = "Alfa AB", ContactPerson = "Anna", Email = "anna@alfa.se", Phone = "070-111" },
            new() { Id = 2, Name = "Beta AB" }
        };

        _repositoryMock
            .Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(customers);

        // Act
        var result = await _service.GetAllAsync();

        // Assert
        Assert.Equal(2, result.Count);

        Assert.Equal(1, result[0].Id);// kontrollerar att Id är korrekt
        Assert.Equal("Alfa AB", result[0].Name); 
        Assert.Equal("Anna", result[0].ContactPerson);
        Assert.Equal("anna@alfa.se", result[0].Email);
        Assert.Equal("070-111", result[0].Phone);

        Assert.Equal("Beta AB", result[1].Name); 
        Assert.Null(result[1].ContactPerson);
    }

    [Fact]
    public async Task GetAllAsync_WhenNoCustomers_ReturnsEmptyList()
    {
        // Arrange
        _repositoryMock 
            .Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<Customer>());

        // Act
        var result = await _service.GetAllAsync();

        // Assert
        Assert.Empty(result);
    }

    // ---------- GetByIdAsync ----------

    [Fact]
    public async Task GetByIdAsync_WhenCustomerExists_ReturnsDto()
    {
        // Arrange
        var customer = new Customer { Id = 5, Name = "Gamma AB", Email = "info@gamma.se" };

        _repositoryMock
            .Setup(r => r.GetByIdAsync(5, It.IsAny<CancellationToken>()))
            .ReturnsAsync(customer);

        // Act
        var result = await _service.GetByIdAsync(5);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(5, result.Id);
        Assert.Equal("Gamma AB", result.Name);
        Assert.Equal("info@gamma.se", result.Email);
    }

    [Fact]
    public async Task GetByIdAsync_WhenCustomerDoesNotExist_ReturnsNull()
    {
        // Arrange
        _repositoryMock
            .Setup(r => r.GetByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Customer?)null);

        // Act
        var result = await _service.GetByIdAsync(999);

        // Assert
        Assert.Null(result);
    }

    // ---------- CreateAsync ----------

    [Fact]
    public async Task CreateAsync_WithValidRequest_SavesCustomerAndReturnsDtoWithId()
    {
        // Arrange
        var request = new CreateCustomerRequest("Delta AB", "Bertil", "bertil@delta.se", "070-222");
        Customer? savedCustomer = null;

        _repositoryMock
            .Setup(r => r.AddAsync(It.IsAny<Customer>(), It.IsAny<CancellationToken>()))
            .Callback<Customer, CancellationToken>((customer, _) =>
            {
                customer.Id = 10;          // låtsas att databasen gav kunden ett Id
                savedCustomer = customer;  // spara undan för att kunna kontrollera den
            })
            .Returns(Task.CompletedTask);

        // Act
        var result = await _service.CreateAsync(request);

        // Assert – det som returneras
        Assert.Equal(10, result.Id);
        Assert.Equal("Delta AB", result.Name);
        Assert.Equal("Bertil", result.ContactPerson);
        Assert.Equal("bertil@delta.se", result.Email);
        Assert.Equal("070-222", result.Phone);

        // Assert – det som skickades till repositoryn
        Assert.NotNull(savedCustomer);
        Assert.Equal("Delta AB", savedCustomer.Name);

        // Assert – AddAsync anropades exakt en gång
        _repositoryMock.Verify(
            r => r.AddAsync(It.IsAny<Customer>(), It.IsAny<CancellationToken>()),
            Times.Once);
    }

   

    [Fact]
    public async Task CreateAsync_WhenOptionalFieldsAreNull_KeepsThemNull()
    {
        // Arrange
        var request = new CreateCustomerRequest("Zeta AB", null, null, null);
        Customer? savedCustomer = null;

        _repositoryMock
            .Setup(r => r.AddAsync(It.IsAny<Customer>(), It.IsAny<CancellationToken>()))
            .Callback<Customer, CancellationToken>((customer, _) => savedCustomer = customer)
            .Returns(Task.CompletedTask);

        // Act
        await _service.CreateAsync(request);

        // Assert---
        Assert.NotNull(savedCustomer);
        Assert.Null(savedCustomer.ContactPerson);
        Assert.Null(savedCustomer.Email);
        Assert.Null(savedCustomer.Phone);
    }

}