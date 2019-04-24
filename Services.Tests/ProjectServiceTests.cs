using DataAccess.Interfaces;
using DataAccess.Models;
using Moq;
using Services.Dto;
using Services.Filters;
using Services.Implementation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Xunit;

namespace Services.Tests
{
    public class ProjectServiceTests
    {
        private readonly List<Project> _projects;

        public ProjectServiceTests()
        {
            _projects = new List<Project>
            {
                new Project { Id = "1", Name = "P1" },
                new Project { Id = "2", Name = "P2" },
                new Project { Id = "3", Name = "P3" }
            };
        }

        [Fact]
        public void GetByFilterTest()
        {
            //Arange
            Mock<IUnitOfWork> unitOfWorkMock = new Mock<IUnitOfWork>();
            Mock<IRepository<Project>> repositoryMock = new Mock<IRepository<Project>>();
            repositoryMock.Setup(repo => repo.Get(It.IsAny<Expression<Func<Project, bool>>>())).Returns(_projects.AsQueryable);
            unitOfWorkMock.Setup(getRepo => getRepo.GetRepository<Project>()).Returns(repositoryMock.Object);
            ProjectService projectService = new ProjectService(unitOfWorkMock.Object);
            ProjectFilter projectFilter = new ProjectFilter();

            //Act
            IEnumerable<ProjectDto> projectsDto = projectService.Get(projectFilter);

            //Assert
            Assert.NotNull(projectsDto);
            Assert.NotEmpty(projectsDto);
            Assert.Equal(3, projectsDto.Count());
        }


        [Fact]
        public void GetByIdTest()
        {
            //Arange
            Mock<IUnitOfWork> unitOfWorkMock = new Mock<IUnitOfWork>();
            Mock<IRepository<Project>> repositoryMock = new Mock<IRepository<Project>>();
            repositoryMock.Setup(repo => repo.Get(It.IsAny<Expression<Func<Project, bool>>>()))
                .Returns<Expression<Func<Project, bool>>>(predicate =>
                    _projects.Where(predicate.Compile()).AsQueryable());
            unitOfWorkMock.Setup(getRepo => getRepo.GetRepository<Project>()).Returns(repositoryMock.Object);
            ProjectService projectService = new ProjectService(unitOfWorkMock.Object);

            //Act
            ProjectDto projectDto = projectService.Get("1");

            //Assert
            Assert.NotNull(projectDto);
            Assert.Equal("P1", projectDto.Name);
        }

        [Fact]
        public void AddTest()
        {
            //Arange
            ProjectDto projectDto = new ProjectDto()
            {
                Id =  "0",
                Name = "P0"
            };
            bool isAdded = false;
            Mock<IUnitOfWork> unitOfWorkMock = new Mock<IUnitOfWork>();
            Mock<IRepository<Project>> repositoryMock = new Mock<IRepository<Project>>();
            repositoryMock.Setup(repo => repo.Get(It.IsAny<Expression<Func<Project, bool>>>()))
                .Returns<Expression<Func<Project, bool>>>(predicate =>
                    _projects.Where(predicate.Compile()).AsQueryable());
            repositoryMock.Setup(repo => repo.Add(It.IsAny<Project>())).Callback(() => isAdded = true);
            unitOfWorkMock.Setup(getRepo => getRepo.GetRepository<Project>()).Returns(repositoryMock.Object);
            ProjectService projectService = new ProjectService(unitOfWorkMock.Object);

            //Act
            projectService.Add(projectDto);

            //Assert
            Assert.True(isAdded);
        }

        [Fact]
        public void RemoveTest()
        {
            //Arange
            bool isRemoved = false;
            Mock<IUnitOfWork> unitOfWorkMock = new Mock<IUnitOfWork>();
            Mock<IRepository<Project>> repositoryMock = new Mock<IRepository<Project>>();
            repositoryMock.Setup(repo => repo.Get(It.IsAny<Expression<Func<Project, bool>>>()))
                .Returns<Expression<Func<Project, bool>>>(predicate =>
                    _projects.Where(predicate.Compile()).AsQueryable());
            repositoryMock.Setup(repo => repo.Remove(It.IsAny<Project>())).Callback(() => isRemoved = true);
            unitOfWorkMock.Setup(getRepo => getRepo.GetRepository<Project>()).Returns(repositoryMock.Object);
            ProjectService projectService = new ProjectService(unitOfWorkMock.Object);

            //Act
            projectService.Remove(_projects[0].Id);

            //Assert
            Assert.True(isRemoved);
        }

        [Fact]
        public void UpdateTest()
        {
            //Arange
            ProjectDto projectDto = new ProjectDto()
            {
                Id = "1",
                Name = "P0"
            };
            bool isUpdate = false;
            Mock<IUnitOfWork> unitOfWorkMock = new Mock<IUnitOfWork>();
            Mock<IRepository<Project>> repositoryMock = new Mock<IRepository<Project>>();
            repositoryMock.Setup(repo => repo.Get(It.IsAny<Expression<Func<Project, bool>>>()))
                .Returns<Expression<Func<Project, bool>>>(predicate =>
                    _projects.Where(predicate.Compile()).AsQueryable());
            repositoryMock.Setup(repo => repo.Update(It.Is<Project>(entity =>
                    (entity.Id == projectDto.Id) &&
                    (entity.Name == projectDto.Name))))
                .Callback(() => isUpdate = true);
            unitOfWorkMock.Setup(getRepo => getRepo.GetRepository<Project>()).Returns(repositoryMock.Object);
            ProjectService projectService = new ProjectService(unitOfWorkMock.Object);

            //Act
            projectService.Update(projectDto);

            //Assert
            Assert.True(isUpdate);
        }
    }
}
