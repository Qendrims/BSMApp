using Xunit;
using Moq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using BMS.Core.Models;
using BMS.Controllers;
using BMS.Repo.Repositories.Interfaces;

public class UserControllerTests
{
    // Helper method to create a list of fake users
    private List<User> GetFakeUsers()
    {
        return new List<User>
        {
            new User { Id = "1", Email = "user1@example.com" },
            new User { Id = "2", Email = "user2@example.com" }
        };
    }

    // Test for GetAllUsers action
    [Fact]
    public async Task GetAllUsers_ReturnsOkResultWithUsers()
    {
        // Arrange
        var mockUserRepository = new Mock<IUserRepository>();
        var mockPostRepository = new Mock<IPostRepository>();
        var controller = new UserController(mockUserRepository.Object, mockPostRepository.Object);

        var fakeUsers = GetFakeUsers();
        //mockUserRepository.Setup(repo => repo.GetAll()).Returns(fakeUsers);

        // Act
        var result = await controller.GetAllUsers();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var users = Assert.IsAssignableFrom<List<User>>(okResult.Value);
        Assert.Equal(2, users.Count);
    }

    // Test for DeleteUser action when deleting by Id
    [Fact]
    public async Task DeleteUser_WithValidId_ReturnsOkResult()
    {
        // Arrange
        var mockUserRepository = new Mock<IUserRepository>();
        var mockPostRepository = new Mock<IPostRepository>();
        var controller = new UserController(mockUserRepository.Object, mockPostRepository.Object);

        var userIdToDelete = "1";
        var userToDelete = new User { Id = userIdToDelete, Email = "user1@example.com" };

        mockUserRepository.Setup(repo => repo.GetUserById(userIdToDelete)).Returns(userToDelete);

        // Act
        var result = await controller.DeleteUser(id: userIdToDelete);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal("User Deleted", okResult.Value);
        mockUserRepository.Verify(repo => repo.Remove(userToDelete), Times.Once);
        mockUserRepository.Verify(repo => repo.Save(), Times.Once);
    }

    // Test for DeleteUser action when deleting by email
    [Fact]
    public async Task DeleteUser_WithValidEmail_ReturnsOkResult()
    {
        // Arrange
        var mockUserRepository = new Mock<IUserRepository>();
        var mockPostRepository = new Mock<IPostRepository>();
        var controller = new UserController(mockUserRepository.Object, mockPostRepository.Object);

        var userEmailToDelete = "user1@example.com";
        var userToDelete = new User { Id = "1", Email = userEmailToDelete };

        mockUserRepository.Setup(repo => repo.checkEmailExists(userEmailToDelete)).Returns(userToDelete);

        // Act
        var result = await controller.DeleteUser(email: userEmailToDelete);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal("User Deleted", okResult.Value);
        mockUserRepository.Verify(repo => repo.Remove(userToDelete), Times.Once);
        mockUserRepository.Verify(repo => repo.Save(), Times.Once);
    }
}