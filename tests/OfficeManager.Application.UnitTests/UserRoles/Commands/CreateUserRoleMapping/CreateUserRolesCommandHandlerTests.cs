﻿using OfficeManager.Application.Common.Models;
using OfficeManager.Application.Feature.UserRoles.Commands;
using OfficeManager.Application.UnitTests.Mocks;

namespace OfficeManager.Application.UnitTests.UserRoles.Commands.CreateUserRoleMapping
{
    public class CreateUserRolesCommandHandlerTests : MockUserRoleContext
    {
        private readonly CreateUserRolesCommandHandler _handler;
        public CreateUserRolesCommandHandlerTests()
        {
            _handler = new CreateUserRolesCommandHandler(mockContext.Object);
        }

        [Fact]
        public async Task When_ValidUserRole_Added()
        {
            var result = await _handler.Handle(AddUserRoleCommand(), CancellationToken.None);

            result.ShouldBeOfType<Result>();

            result.Message.Equals("Role Added Successfully");

            var userRoleMappings = mockContext.Object.UserRoleMapping;

            userRoleMappings.Count().ShouldBe(2);
        }
    }
}
