using DataAccess.Implementation;
using DataAccess.Interfaces;
using DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Xunit;

namespace DataAccess.Tests
{
    public class ProjectRepositoryTest
    {
        private readonly List<Project> _projects;
        private const int _oneElement = 1;
        private TmDbContext _dataContext;
        private IUnitOfWork _unitOfWork;

        public ProjectRepositoryTest()
        {
            _projects = new List<Project>
            {
                new Project { Id = "1", Name = "P1" },
                new Project { Id = "2", Name = "P2" },
                new Project { Id = "3", Name = "P3" }
            };

            _dataContext = GetContext();
            _unitOfWork = new UnitOfWork(_dataContext);
        }

        [Fact]
        public void AddTest()
        {
            // Act
            _unitOfWork.GetRepository<Project>().Add(_projects[0]);
            _unitOfWork.SaveChanges();

            // Assert
            Assert.Equal(_oneElement, _dataContext.Projects.Count());
            Assert.Equal(_projects[0].Name, _dataContext.Projects.Single().Name);
            Assert.Equal(_projects[0].Id, _dataContext.Projects.Single().Id);
            Assert.IsType<Project>(_dataContext.Projects.Single());
        }

        [Fact]
        public void AddRangeTest()
        {
            // Act
            _unitOfWork.GetRepository<Project>().Add(_projects);
            _unitOfWork.SaveChanges();

            // Assert
            Assert.Equal(_projects.Count, _dataContext.Projects.Count());
            foreach (var project in _projects)
            {
                Assert.Contains(project, _dataContext.Projects);
            }
        }

        [Fact]
        public void RemoveTest()
        {
            // Act
            _unitOfWork.GetRepository<Project>().Add(_projects[0]);
            _unitOfWork.SaveChanges();

            Assert.Equal(_oneElement, _dataContext.Projects.Count());

            _unitOfWork.GetRepository<Project>().Remove(_dataContext.Projects.Single());
            _unitOfWork.SaveChanges();

            // Assert
            Assert.Empty(_dataContext.Projects);
        }

        [Fact]
        public void RemoveRangeTest()
        {
            // Act
            _unitOfWork.GetRepository<Project>().Add(_projects);
            _unitOfWork.SaveChanges();

            Assert.Equal(_projects.Count, _dataContext.Projects.Count());

            _unitOfWork.GetRepository<Project>().Remove(_projects);
            _unitOfWork.SaveChanges();

            // Assert
            Assert.Empty(_dataContext.Projects);
        }

        [Fact]
        public void GetTest()
        {
            // Arange
            _unitOfWork.GetRepository<Project>().Add(_projects);
            _unitOfWork.SaveChanges();

            // Act
            var result = _unitOfWork.GetRepository<Project>().Get();

            //Assert
            Assert.Equal(_projects.Count, result.Count());
            foreach (var project in _projects)
            {
                Assert.Contains(project, result);
            }
        }

        [Fact]
        public void GetPredicateTest()
        {
            // Arange
            _unitOfWork.GetRepository<Project>().Add(_projects);
            _unitOfWork.SaveChanges();
            Expression<Func<Project, bool>> predicate = a => a.Id.CompareTo("1") > 0;
            List<Project> filtredProjects = _projects.Where(predicate.Compile()).ToList();

            // Act
            var result = _unitOfWork.GetRepository<Project>().Get(predicate);

            //Assert
            Assert.Equal(filtredProjects.Count, result.Count());
            foreach (var project in filtredProjects)
            {
                Assert.Contains(project, result);
            }
        }

        private TmDbContext GetContext()
        {
            var options = new DbContextOptionsBuilder<TmDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var dataContext = new TmDbContext(options);
            dataContext.Database.EnsureCreated();
            return dataContext;
        }
    }
}
