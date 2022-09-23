using NetArchTest.Rules;
using OfficeManager.Application.Wrappers.Abstract;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace OfficeManager.Architecture.Tests
{
    public class ArchitectureTests
    {
        private const string DomainNamespace = "OfficeManager.Domain";
        private const string ApplicationNamespace = "OfficeManager.Application";
        private const string InfrastructureNamespace = "OfficeManager.Infrastructure";
        private const string APINamespace = "OfficeManager.API";

        [Fact]
        public void Domain_Should_Not_HaveDependencyOnOtherProjects()
        {
            //Arrange
            var assembly = Assembly.Load(DomainNamespace);

            var otherProjects = new[]
            {
                ApplicationNamespace,
                InfrastructureNamespace,
                APINamespace
            };

            //Act
            var result = Types
                .InAssembly(assembly)
                .ShouldNot()
                .HaveDependencyOnAll(otherProjects)
                .GetResult();


            //Assert
            result.IsSuccessful.ShouldBe(true);
        }

        [Fact]
        public void Application_Should_Not_HaveDependencyOnOtherProjects()
        {
            //Arrange
            var assembly = Assembly.Load(ApplicationNamespace);

            var otherProjects = new[]
            {
                InfrastructureNamespace,
                APINamespace
            };

            //Act
            var result = Types
                .InAssembly(assembly)
                .ShouldNot()
                .HaveDependencyOnAll(otherProjects)
                .GetResult();


            //Assert
            result.IsSuccessful.ShouldBe(true);
        }

        [Fact]
        public void Infrastructure_Should_Not_HaveDependencyOnOtherProjects()
        {
            //Arrange
            var assembly = Assembly.Load(InfrastructureNamespace);

            var otherProjects = new[]
            {
                APINamespace
            };

            //Act
            var result = Types
                .InAssembly(assembly)
                .ShouldNot()
                .HaveDependencyOnAll(otherProjects)
                .GetResult();


            //Assert
            result.IsSuccessful.ShouldBe(true);
        }

        [Fact]
        public void Handlers_Should_Not_Have_DependencyOninfrastructure_And_API()
        {
            var assembly = Assembly.Load(ApplicationNamespace);

            var result = Types
                .InAssembly(assembly)
                .That()
                .HaveNameEndingWith("Handler")
                .Should()
                .NotHaveDependencyOnAll(new[] { InfrastructureNamespace, APINamespace })
                .GetResult();

            result.IsSuccessful.ShouldBe(true);

        }

        [Fact]
        public void Controllers_Should_Have_DependencyOnMediatR()
        {
            var assembly = Assembly.Load(APINamespace);

            var result = Types
                .InAssembly(assembly)
                .That()
                .HaveNameEndingWith("Controller")
                .Should()
                .HaveDependencyOn("MediatR")
                .GetResult();

            result.IsSuccessful.ShouldBe(true);

        }

        //[Fact]
        //public void Handlers_Should_Have_ReturnResponse()
        //{
        //    var assembly = Assembly.Load(ApplicationNamespace);

        //    var result = Types
        //        .InAssembly(assembly)
        //        .That()
        //        .HaveNameEndingWith("Handler")
        //        .GetType()
        //        .FindMembers(MemberTypes.Method, BindingFlags.Public, null, null)
        //        .All(a => a.MemberType.Equals("IResponse"));

        //    result.ShouldBe(true);

        //}
    }
}
