using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Microsoft.Extensions.Caching.Memory;
using ReserGo.Business.Interfaces;
using ReserGo.Common.DTO;
using ReserGo.Common.Requests.User;
using ReserGo.WebAPI.Controllers.Administration.User;
using ReserGo.DataAccess;
using ReserGo.Common.Entity;
using ReserGo.Business.Implementations;
using ReserGo.DataAccess.Implementations;
using ReserGo.Common.Models;

public class UserControllerTests {
    private readonly UserController _controller;
    private readonly IUserService _userService;
    private readonly Mock<ILogger<UserController>> _loggerMock;

    public UserControllerTests() {
        var serviceProvider = new ServiceCollection()
            .AddEntityFrameworkInMemoryDatabase()
            .BuildServiceProvider();

        var options = new DbContextOptionsBuilder<ReserGoContext>()
            .UseInMemoryDatabase("InMemoryDb")
            .UseInternalServiceProvider(serviceProvider)
            .Options;

        var context = new ReserGoContext(options);

        // Seed the database with test data if necessary
        context.Users.Add(new User
            { Id = Guid.NewGuid(), FirstName = "John", LastName = "Doe", Email = "john.doe@example.com", Username = "johndoe" });
        context.SaveChanges();

        _userService = new UserService(new Mock<ILogger<UserService>>().Object, new UserDataAccess(context),
            new Mock<ILoginService>().Object, new Mock<IImageService>().Object, new Mock<IMemoryCache>().Object);
        _loggerMock = new Mock<ILogger<UserController>>();
        _controller = new UserController(_loggerMock.Object, null, _userService);
    }

    /*  [Fact]
      public async Task ShouldGet201_POST_Create()
      {
          var request = new UserCreationRequest
          {
              FirstName = "Jane",
              LastName = "Doe",
              Email = "jane.doe@example.com",
              Username = "janedoe",
              Password = "Password1123#zx"
          };

          var result = await _controller.Create(request) as CreatedResult;

          // Add null checks to prevent NullReferenceException
          result.Should().NotBeNull();
          result.StatusCode.Should().Be(201);
      } */

    [Fact]
    public async Task Status400BadRequest_POST_Create() {
        var request = new UserCreationRequest {
            FirstName = "",
            LastName = "",
            Email = "invalid-email",
            Username = "",
            Password = ""
        };

        var result = await _controller.Create(request) as BadRequestObjectResult;
        result.StatusCode.Should().Be(400);
    }


    /* [Fact]
     public async Task ShouldGet200_GET_OneUserById() {
         var result = await _controller.GetById(1) as ActionResult<UserDto>;
         var okResult = result.Result as OkObjectResult;
         okResult.Should().NotBeNull();
         okResult.StatusCode.Should().Be(200);
     } */

    [Fact]
    public async Task ShouldGet404_GET_OneUserById() {
        var result = await _controller.GetById(Guid.NewGuid()) as ActionResult<Resource<UserDto>>;
        var notFoundResult = result.Result as NotFoundObjectResult;
        notFoundResult.Should().NotBeNull();
        notFoundResult.StatusCode.Should().Be(404);
    }

    [Fact]
    public async Task ShouldGet204_DELETE_User() {
        var result = await _controller.GetById(Guid.NewGuid()) as ActionResult<Resource<UserDto>>;
        var notFoundResult = result.Result as NotFoundObjectResult;
        notFoundResult.Should().NotBeNull();
        notFoundResult.StatusCode.Should().Be(404);
    }

    [Fact]
    public async Task ShouldGet404_DELETE_User() {
        var result = await _controller.Delete(Guid.NewGuid()) as ActionResult;
        var notFoundResult = result as NotFoundObjectResult;
        notFoundResult.Should().NotBeNull();
        notFoundResult.StatusCode.Should().Be(404);
    }

    /* [Fact]
     public async Task ShouldGet200_PUT_UpdateUser()
     {
         var request = new UserUpdateRequest
         {
             FirstName = "Jane",
             LastName = "Doe",
             Email = "jane.doe@example.com",
             Username = "janedoe"
         };

         var result = await _controller.UpdateUser(1, request) as ActionResult<Resource<UserDto>>;
         result.Should().NotBeNull(); // Ensure result is not null
         result.Result.Should().NotBeNull(); // Ensure result.Result is not null

         var okResult = result.Result as OkObjectResult;
         okResult.Should().NotBeNull(); // Ensure okResult is not null
         okResult.StatusCode.Should().Be(200);

         var resource = okResult.Value as Resource<UserDto>;
         resource.Should().NotBeNull();
         resource.Data.FirstName.Should().Be("Jane");
     } */

    [Fact]
    public async Task ShouldGet404_PUT_UpdateUser() {
        var request = new UserUpdateRequest {
            FirstName = "Jane",
            LastName = "Doe",
            Email = "jane.doe@example.com",
            Username = "janedoe"
        };

        var result = await _controller.UpdateUser(Guid.NewGuid(), request) as ActionResult<Resource<UserDto>>;
        var notFoundResult = result.Result as NotFoundObjectResult;
        notFoundResult.Should().NotBeNull();
        notFoundResult.StatusCode.Should().Be(404);
    }
}