using FluentAssertions;
using Moq;
using SimpleExample.Application.DTOs;
using SimpleExample.Application.Interfaces;
using SimpleExample.Application.Services;
using SimpleExample.Domain.Entities;

namespace SimpleExample.Tests.Application;

public class UserServiceTests
{
    private readonly Mock<IUserRepository> _mockRepository;
    private readonly UserService _service;

    public UserServiceTests()
    {
        _mockRepository = new Mock<IUserRepository>();
        _service = new UserService(_mockRepository.Object);
    }

    [Fact]
    public async Task CreateAsync_WithValidData_ShouldCreateUser()
    {
        // Arrange
        CreateUserDto dto = new CreateUserDto
        {
            FirstName = "Matti",
            LastName = "Meikäläinen",
            Email = "matti@example.com"
        };

        // Mock: Email ei ole käytössä
        _mockRepository
            .Setup(x => x.GetByEmailAsync(dto.Email))
            .ReturnsAsync((User?)null);

        _mockRepository
            .Setup(x => x.AddAsync(It.IsAny<User>()))
            .ReturnsAsync((User u) => u);

        // Act
        UserDto result = await _service.CreateAsync(dto);

        // Assert
        result.Should().NotBeNull();
        result.FirstName.Should().Be("Matti");
        result.LastName.Should().Be("Meikäläinen");
        result.Email.Should().Be("matti@example.com");

        // Varmista että AddAsync kutsuttiin kerran
        _mockRepository.Verify(x => x.AddAsync(It.IsAny<User>()), Times.Once);
    }

    [Fact]
    public async Task CreateAsync_WithDuplicateEmail_ShouldThrowInvalidOperationException()
    {
        // Arrange
        CreateUserDto dto = new CreateUserDto
        {
            FirstName = "Matti",
            LastName = "Meikäläinen",
            Email = "existing@example.com"
        };

        User existingUser = new User("Maija", "Virtanen", "existing@example.com");

        // Mock: Email on jo käytössä!
        _mockRepository
            .Setup(x => x.GetByEmailAsync(dto.Email))
            .ReturnsAsync(existingUser);

        // Act
        Func<Task> act = async () => await _service.CreateAsync(dto);

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("*jo olemassa*");

        // Varmista että AddAsync EI kutsuttu
        _mockRepository.Verify(x => x.AddAsync(It.IsAny<User>()), Times.Never);
    }

    // TEHTÄVÄ: Kirjoita itse testit seuraaville:
    // 1. GetByIdAsync - löytyy
    [Fact]
    public async Task FindUserByIdAsync_WhenUserExists_ShouldReturnUser()
    {
        // Arrange
        Guid id = Guid.NewGuid();
        User user = new User("Matti", "Meikäläinen", "matti@example.com");

        _mockRepository.Setup(x => x.GetByIdAsync(id)).ReturnsAsync(user);

        // Act
        UserDto? result = await _service.GetByIdAsync(id);

        // Assert
        result.Should().NotBeNull();
        result.FirstName.Should().Be(user.FirstName);
        result.LastName.Should().Be(user.LastName);
        result.Email.Should().Be(user.Email);

        _mockRepository.Verify(x => x.GetByIdAsync(id), Times.Once);
    }

    // 2. GetByIdAsync - ei löydy
    [Fact]
    public async Task FindUserByIdAsync_WhenUserNotFound_ShouldReturnNull()
    {
        // Arrange
        Guid id = Guid.NewGuid();

        _mockRepository.Setup(x => x.GetByIdAsync(id)).ReturnsAsync((User?)null);

        // Act
        UserDto? result = await _service.GetByIdAsync(id);

        // Assert
        result.Should().BeNull();
        _mockRepository.Verify(x => x.GetByIdAsync(id), Times.Once);
    }

    // 3. GetAllAsync - palauttaa listan
    [Fact]
    public async Task GellAllAsync_ShouldReturnListOfUsers()
    {
        // Arrange
        List<User> users = new()
        {
             new User("Matti", "Meikäläinen", "matti@example.com"),
            new User("Maija", "Virtanen", "maija@example.com")
        };

        _mockRepository.Setup(x => x.GetAllAsync()).ReturnsAsync(users);

        // Act
        var result = await _service.GetAllAsync();

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(2);
        result.Select(x => x.Email).Should().Contain(new[] { "matti@example.com", "maija@example.com" });

        _mockRepository.Verify(x => x.GetAllAsync(), Times.Once);
    }

    // 4. UpdateAsync - onnistuu
    [Fact]
    public async Task UpdateAsync_WhenUserExists_ShouldUpdateUser()
    {
        // Arrange
        Guid id = Guid.NewGuid();
        User existingUser = new User("Matti", "Meikäläinen", "matti@example.com");
        UpdateUserDto dto = new UpdateUserDto
        {
            FirstName = "Matti",
            LastName = "Meikäläinen",
            Email = "matti@example.com"
        };

        _mockRepository.Setup(x => x.GetByIdAsync(id)).ReturnsAsync(existingUser);

        // If your service checks duplicate email on update:
        _mockRepository.Setup(x => x.GetByEmailAsync(dto.Email)).ReturnsAsync((User?)null);

        _mockRepository
            .Setup(x => x.UpdateAsync(It.IsAny<User>()))
            .ReturnsAsync((User u) => u);

        // Act
        UserDto? result = await _service.UpdateAsync(id, dto);

        // Assert
        result.Should().NotBeNull();
        result.FirstName.Should().Be(dto.FirstName);
        result.LastName.Should().Be(dto.LastName);
        result.Email.Should().Be(dto.Email);
        _mockRepository.Verify(x => x.GetByIdAsync(id), Times.Once);
        _mockRepository.Verify(x => x.UpdateAsync(It.IsAny<User>()), Times.Once);
    }

    // 5. UpdateAsync - käyttäjää ei löydy
    [Fact]
    public async Task UpdateAsync_WhenUserNotFound_ShouldReturnNull()
    {
        // Arrange
        Guid id = Guid.NewGuid();
        UpdateUserDto dto = new UpdateUserDto
        {
            FirstName = "Matti",
            LastName = "Meikäläinen",
            Email = "matti@example.com"
        };

        _mockRepository.Setup(x => x.GetByIdAsync(id)).ReturnsAsync((User?)null);

        // Act
        UserDto? result = await _service.UpdateAsync(id, dto);

        // Assert
        result.Should().BeNull();
        _mockRepository.Verify(x => x.GetByIdAsync(id), Times.Once);
        _mockRepository.Verify(x => x.UpdateAsync(It.IsAny<User>()), Times.Never);
    }

    // 6. DeleteAsync - onnistuu
    [Fact]
    public async Task DeleteAsync_WhenUserExists_ShouldDeleteUser()
    {
        // Arrange
        Guid id = Guid.NewGuid();
        User existingUser = new User("Matti", "Meikäläinen", "matti@example.com"
            );

        _mockRepository.Setup(x => x.ExistsAsync(id)).ReturnsAsync(true);
        _mockRepository.Setup(x => x.DeleteAsync(id)).Returns((Task.CompletedTask));

        // Act
        bool result = await _service.DeleteAsync(id);

        // Assert
        result.Should().BeTrue();
        _mockRepository.Verify(x => x.ExistsAsync(id), Times.Once);
        _mockRepository.Verify(x => x.DeleteAsync(id), Times.Once);
    }

    // 7. DeleteAsync - käyttäjää ei löydy
    [Fact]
    public async Task DeleteAsync_WhenUserNotFound_ShouldReturnFalse()
    {
        // Arrange
        Guid id = Guid.NewGuid();
        _mockRepository.Setup(x => x.ExistsAsync(id)).ReturnsAsync(false);

        // Act
        bool result = await _service.DeleteAsync(id);

        // Assert
        result.Should().BeFalse();
        _mockRepository.Verify(x => x.ExistsAsync(id), Times.Once);
        _mockRepository.Verify(x => x.DeleteAsync(id), Times.Never);
    }
}