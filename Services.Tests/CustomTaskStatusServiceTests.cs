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
    public class CustomTaskStatusServiceTests

    {
        private readonly List<CustomTaskStatus> _customTasks;

        public CustomTaskStatusServiceTests()
        {
            try
            {
                AutoMapper.Mapper.Initialize(p => { });
            }
            catch (Exception)
            {
            }

            _customTasks = new List<CustomTaskStatus>
            {
                new CustomTaskStatus { Name = "CT1", Index = 1 },
                new CustomTaskStatus { Name = "CT2", Index = 2 },
                new CustomTaskStatus { Name = "CT3", Index = 3 }
            };
        }

        [Fact]
        public void GetByFilterTest()
        {
            //Arange
            Mock<IUnitOfWork> unitOfWorkMock = new Mock<IUnitOfWork>();
            Mock<IRepository<CustomTaskStatus>> repositoryMock = new Mock<IRepository<CustomTaskStatus>>();
            repositoryMock.Setup(repo => repo.Get(It.IsAny<Expression<Func<CustomTaskStatus, bool>>>())).Returns(_customTasks.AsQueryable);
            unitOfWorkMock.Setup(getRepo => getRepo.GetRepository<CustomTaskStatus>()).Returns(repositoryMock.Object);
            CustomTaskStatusService customTaskService = new CustomTaskStatusService(unitOfWorkMock.Object);
            CustomTaskStatusFilter customTaskFilter = new CustomTaskStatusFilter();

            //Act
            IEnumerable<CustomTaskStatusDto> customTasksDto = customTaskService.Get(customTaskFilter);

            //Assert
            Assert.NotNull(customTasksDto);
            Assert.NotEmpty(customTasksDto);
            Assert.Equal(3, customTasksDto.Count());
        }
    }
}


