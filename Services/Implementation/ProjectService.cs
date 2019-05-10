using DataAccess.Interfaces;
using DataAccess.Models;
using Services.Dto;
using Services.Exceptions;
using Services.Filters;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Services.Implementation
{
    public class ProjectService : Service<Project, ProjectDto, ProjectFilter>, IProjectService
    {
        public ProjectService(IUnitOfWork unitOfWork) :
            base(unitOfWork)
        {            
        }        

        public override ProjectDto Get(string id)
        {
            Project entity = Repository
              .Get(e => e.Id == id)
              .SingleOrDefault();

            if (entity == null)
            {
                throw new ObjectNotFoundException();
            }

            return MapToDto(entity);
        }

        public override IEnumerable<ProjectDto> Get(ProjectFilter filter)
        {
            Func<Project, bool> predicate = GetFilter(filter);
            List<Project> entities = Repository
              .Get(p => predicate(p))
              .ToList();

            return entities.Select(e => MapToDto(e));
        }

        public override void Add(ProjectDto dto)
        {
            Project checkEntity = Repository
                .Get(e => e.Id == dto.Id || e.Name == dto.Name)
                .SingleOrDefault();

            if (checkEntity != null)
            {
                throw new DuplicateNameException();
            }

            Project entity = MapToEntity(dto);
            Repository.Add(entity);
            _unitOfWork.SaveChanges();
        }

        public override void Remove(string id)
        {
            Project entity = Repository
             .Get(e => e.Id == id)
             .SingleOrDefault();

            if (entity == null)
            {
                throw new ObjectNotFoundException();
            }

            Repository.Remove(entity);
            _unitOfWork.SaveChanges();
        }

        public override void Update(ProjectDto dto)
        {
            Project entity = Repository
             .Get(e => e.Id == dto.Id)
             .SingleOrDefault();

            if (entity == null)
            {
                throw new ObjectNotFoundException();
            }

            entity.Name = dto.Name;
            entity.OwnerId = dto.OwnerId;
            entity.ProjectManagerId = dto.ProjectManagerId;

            Repository.Update(entity);
            _unitOfWork.SaveChanges();
        }

        protected override ProjectDto MapToDto(Project entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException();
            }

            ProjectDto dto = new ProjectDto
            {
                Id = entity.Id,
                Name = entity.Name,
                OwnerId = entity.OwnerId,
                ProjectManagerId = entity.ProjectManagerId
            };

            return dto;
        }

        protected override Project MapToEntity(ProjectDto dto)
        {
            if (dto == null)
            {
                throw new ArgumentNullException();
            }

            Project entity = new Project
            {
                Id = dto.Id,
                Name = dto.Name,
                OwnerId = dto.OwnerId,
                ProjectManagerId = dto.ProjectManagerId
            };

            return entity;
        }

        private Func<Project, bool> GetFilter(ProjectFilter filter)
        {
            Func<Project, bool> result = e => true;
            if (!String.IsNullOrEmpty(filter?.Name))
            {
                result += e => e.Name == filter.Name;
            }
            else if (!String.IsNullOrEmpty(filter?.OwnerId))
            {
                result += e => e.OwnerId == filter.OwnerId;
            }
            else if(!String.IsNullOrEmpty(filter?.ProjectManagerId))
            {
                result += e => e.ProjectManagerId == filter.ProjectManagerId;
            }

            return result;
        }
    }
}
