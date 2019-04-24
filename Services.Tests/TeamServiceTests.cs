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
    public class TeamServiceTests
    {
        private readonly List<Team> _teams;

        public TeamServiceTests()
        {
            _teams = new List<Team>
            {
                new Team { Id = "1", Name = "T1" },
                new Team { Id = "2", Name = "T2" },
                new Team { Id = "3", Name = "T3" }
            };
        }

        [Fact]
        public void GetByFilterTest()
        {
            //Arange
            Mock<IUnitOfWork> unitOfWorkMock = new Mock<IUnitOfWork>();
            Mock<IRepository<Team>> repositoryMock = new Mock<IRepository<Team>>();
            repositoryMock.Setup(repo => repo.Get(It.IsAny<Expression<Func<Team, bool>>>())).Returns(_teams.AsQueryable);
            unitOfWorkMock.Setup(getRepo => getRepo.GetRepository<Team>()).Returns(repositoryMock.Object);
            TeamService teamService = new TeamService(unitOfWorkMock.Object);
            TeamFilter teamFilter = new TeamFilter();

            //Act
            IEnumerable<TeamDto> teamsDto = teamService.Get(teamFilter);

            //Assert
            Assert.NotNull(teamsDto);
            Assert.NotEmpty(teamsDto);
            Assert.Equal(3, teamsDto.Count());
        }


        [Fact]
        public void GetByIdTest()
        {
            //Arange
            Mock<IUnitOfWork> unitOfWorkMock = new Mock<IUnitOfWork>();
            Mock<IRepository<Team>> repositoryMock = new Mock<IRepository<Team>>();
            repositoryMock.Setup(repo => repo.Get(It.IsAny<Expression<Func<Team, bool>>>()))
                .Returns<Expression<Func<Team, bool>>>(predicate =>
                    _teams.Where(predicate.Compile()).AsQueryable());
            unitOfWorkMock.Setup(getRepo => getRepo.GetRepository<Team>()).Returns(repositoryMock.Object);
            TeamService teamService = new TeamService(unitOfWorkMock.Object);

            //Act
            TeamDto teamDto = teamService.Get("1");

            //Assert
            Assert.NotNull(teamDto);
            Assert.Equal("T1", teamDto.Name);
        }

        [Fact]
        public void AddTest()
        {
            //Arange
            TeamDto teamDto = new TeamDto()
            {
                Id = "0",
                Name = "T0"
            };
            bool isAdded = false;
            Mock<IUnitOfWork> unitOfWorkMock = new Mock<IUnitOfWork>();
            Mock<IRepository<Team>> repositoryMock = new Mock<IRepository<Team>>();
            repositoryMock.Setup(repo => repo.Get(It.IsAny<Expression<Func<Team, bool>>>()))
                .Returns<Expression<Func<Team, bool>>>(predicate =>
                    _teams.Where(predicate.Compile()).AsQueryable());
            repositoryMock.Setup(repo => repo.Add(It.IsAny<Team>())).Callback(() => isAdded = true);
            unitOfWorkMock.Setup(getRepo => getRepo.GetRepository<Team>()).Returns(repositoryMock.Object);
            TeamService teamService = new TeamService(unitOfWorkMock.Object);

            //Act
            teamService.Add(teamDto);

            //Assert
            Assert.True(isAdded);
        }

        [Fact]
        public void RemoveTest()
        {
            //Arange
            bool isRemoved = false;
            Mock<IUnitOfWork> unitOfWorkMock = new Mock<IUnitOfWork>();
            Mock<IRepository<Team>> repositoryMock = new Mock<IRepository<Team>>();
            repositoryMock.Setup(repo => repo.Get(It.IsAny<Expression<Func<Team, bool>>>()))
                .Returns<Expression<Func<Team, bool>>>(predicate =>
                    _teams.Where(predicate.Compile()).AsQueryable());
            repositoryMock.Setup(repo => repo.Remove(It.IsAny<Team>())).Callback(() => isRemoved = true);
            unitOfWorkMock.Setup(getRepo => getRepo.GetRepository<Team>()).Returns(repositoryMock.Object);
            TeamService teamService = new TeamService(unitOfWorkMock.Object);

            //Act
            teamService.Remove(_teams[0].Id);

            //Assert
            Assert.True(isRemoved);
        }

        [Fact]
        public void UpdateTest()
        {
            //Arange
            TeamDto teamDto = new TeamDto()
            {
                Id = "1",
                Name = "T0"
            };
            bool isUpdate = false;
            Mock<IUnitOfWork> unitOfWorkMock = new Mock<IUnitOfWork>();
            Mock<IRepository<Team>> repositoryMock = new Mock<IRepository<Team>>();
            repositoryMock.Setup(repo => repo.Get(It.IsAny<Expression<Func<Team, bool>>>()))
                .Returns<Expression<Func<Team, bool>>>(predicate =>
                    _teams.Where(predicate.Compile()).AsQueryable());
            repositoryMock.Setup(repo => repo.Update(It.Is<Team>(entity =>
                    (entity.Id == teamDto.Id) &&
                    (entity.Name == teamDto.Name))))
                .Callback(() => isUpdate = true);
            unitOfWorkMock.Setup(getRepo => getRepo.GetRepository<Team>()).Returns(repositoryMock.Object);
            TeamService teamService = new TeamService(unitOfWorkMock.Object);

            //Act
            teamService.Update(teamDto);

            //Assert
            Assert.True(isUpdate);
        }
    }
}
