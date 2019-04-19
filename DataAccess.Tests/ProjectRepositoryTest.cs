using DataAccess.Interfaces;
using DataAccess.Models;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Tests
{
    [TestFixture]
    public class ProjectRepositoryTest
    {
        private readonly List<Project> _projects;
        private const int OneElement = 1;
        private TmDbContext _dataContext;
        private IUnitOfWork _unitOfWork;

        public ProjectRepositoryTest()
        {
            _projects = new List<Project>
            {
                new Project { Name = "P1" },
                new Project { Name = "P2" },
                new Project { Name = "P3" }
            };
        }
    }
}
