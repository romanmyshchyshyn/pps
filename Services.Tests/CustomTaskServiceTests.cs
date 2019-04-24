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
    public class CustomTaskServiceTests
    {
        private readonly List<CustomTask> _customTasks;

        public CustomTaskServiceTests()
        {
            _customTasks = new List<CustomTask>
            {
                new CustomTask { Id = "1", Name = "CT1" },
                new CustomTask { Id = "2", Name = "CT2" },
                new CustomTask { Id = "3", Name = "CT3" }
            };
        }

        [Fact]
        public void GetByFilterTest()
        {
            //Arange
            Mock<IUnitOfWork> unitOfWorkMock = new Mock<IUnitOfWork>();
            Mock<IRepository<CustomTask>> repositoryMock = new Mock<IRepository<CustomTask>>();
            repositoryMock.Setup(repo => repo.Get(It.IsAny<Expression<Func<CustomTask, bool>>>())).Returns(_customTasks.AsQueryable);
            unitOfWorkMock.Setup(getRepo => getRepo.GetRepository<CustomTask>()).Returns(repositoryMock.Object);
            CustomTaskService customTaskService = new CustomTaskService(unitOfWorkMock.Object);
            CustomTaskFilter customTaskFilter = new CustomTaskFilter();

            //Act
            IEnumerable<CustomTaskDto> customTasksDto = customTaskService.Get(customTaskFilter);

            //Assert
            Assert.NotNull(customTasksDto);
            Assert.NotEmpty(customTasksDto);
            Assert.Equal(3, customTasksDto.Count());
        }


        [Fact]
        public void GetByIdTest()
        {
            //Arange
            Mock<IUnitOfWork> unitOfWorkMock = new Mock<IUnitOfWork>();
            Mock<IRepository<CustomTask>> repositoryMock = new Mock<IRepository<CustomTask>>();
            repositoryMock.Setup(repo => repo.Get(It.IsAny<Expression<Func<CustomTask, bool>>>()))
                .Returns<Expression<Func<CustomTask, bool>>>(predicate =>
                    _customTasks.Where(predicate.Compile()).AsQueryable());
            unitOfWorkMock.Setup(getRepo => getRepo.GetRepository<CustomTask>()).Returns(repositoryMock.Object);
            CustomTaskService customTaskService = new CustomTaskService(unitOfWorkMock.Object);

            //Act
            CustomTaskDto customTaskDto = customTaskService.Get("1");

            //Assert
            Assert.NotNull(customTaskDto);
            Assert.Equal("CT1", customTaskDto.Name);
        }

        [Fact]
        public void AddTest()
        {
            //Arange
            CustomTaskDto customTaskDto = new CustomTaskDto()
            {
                Id = "0",
                Name = "CT0"
            };
            bool isAdded = false;
            Mock<IUnitOfWork> unitOfWorkMock = new Mock<IUnitOfWork>();
            Mock<IRepository<CustomTask>> repositoryMock = new Mock<IRepository<CustomTask>>();
            repositoryMock.Setup(repo => repo.Get(It.IsAny<Expression<Func<CustomTask, bool>>>()))
                .Returns<Expression<Func<CustomTask, bool>>>(predicate =>
                    _customTasks.Where(predicate.Compile()).AsQueryable());
            repositoryMock.Setup(repo => repo.Add(It.IsAny<CustomTask>())).Callback(() => isAdded = true);
            unitOfWorkMock.Setup(getRepo => getRepo.GetRepository<CustomTask>()).Returns(repositoryMock.Object);
            CustomTaskService customTaskService = new CustomTaskService(unitOfWorkMock.Object);

            //Act
            customTaskService.Add(customTaskDto);

            //Assert
            Assert.True(isAdded);
        }

        [Fact]
        public void RemoveTest()
        {
            //Arange
            bool isRemoved = false;
            Mock<IUnitOfWork> unitOfWorkMock = new Mock<IUnitOfWork>();
            Mock<IRepository<CustomTask>> repositoryMock = new Mock<IRepository<CustomTask>>();
            repositoryMock.Setup(repo => repo.Get(It.IsAny<Expression<Func<CustomTask, bool>>>()))
                .Returns<Expression<Func<CustomTask, bool>>>(predicate =>
                    _customTasks.Where(predicate.Compile()).AsQueryable());
            repositoryMock.Setup(repo => repo.Remove(It.IsAny<CustomTask>())).Callback(() => isRemoved = true);
            unitOfWorkMock.Setup(getRepo => getRepo.GetRepository<CustomTask>()).Returns(repositoryMock.Object);
            CustomTaskService customTaskService = new CustomTaskService(unitOfWorkMock.Object);

            //Act
            customTaskService.Remove(_customTasks[0].Id);

            //Assert
            Assert.True(isRemoved);
        }

        [Fact]
        public void UpdateTest()
        {
            //Arange
            CustomTaskDto customTaskDto = new CustomTaskDto()
            {
                Id = "1",
                Name = "CT0"
            };
            bool isUpdate = false;
            Mock<IUnitOfWork> unitOfWorkMock = new Mock<IUnitOfWork>();
            Mock<IRepository<CustomTask>> repositoryMock = new Mock<IRepository<CustomTask>>();
            repositoryMock.Setup(repo => repo.Get(It.IsAny<Expression<Func<CustomTask, bool>>>()))
                .Returns<Expression<Func<CustomTask, bool>>>(predicate =>
                    _customTasks.Where(predicate.Compile()).AsQueryable());
            repositoryMock.Setup(repo => repo.Update(It.Is<CustomTask>(entity =>
                    (entity.Id == customTaskDto.Id) &&
                    (entity.Name == customTaskDto.Name))))
                .Callback(() => isUpdate = true);
            unitOfWorkMock.Setup(getRepo => getRepo.GetRepository<CustomTask>()).Returns(repositoryMock.Object);
            CustomTaskService customTaskService = new CustomTaskService(unitOfWorkMock.Object);

            //Act
            customTaskService.Update(customTaskDto);

            //Assert
            Assert.True(isUpdate);
        }
    }
}
