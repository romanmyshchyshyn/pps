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
    public class CustomTaskService : Service<CustomTask, CustomTaskDto, CustomTaskFilter>, ICustomTaskService
    {
        public CustomTaskService(IUnitOfWork unitOfWork) :
            base(unitOfWork)
        {
        }

        public override CustomTaskDto Get(string id)
        {
            CustomTask entity = Repository
              .Get(e => e.Id == id)
              .SingleOrDefault();

            if (entity == null)
            {
                throw new ObjectNotFoundException();
            }

            return MapToDto(entity);
        }

        public override IEnumerable<CustomTaskDto> Get(CustomTaskFilter filter)
        {
            Func<CustomTask, bool> predicate = GetFilter(filter);
            List<CustomTask> entities = Repository
              .Get(p => predicate(p))
              .ToList();

            return entities.Select(e => MapToDto(e));
        }

        public override void Add(CustomTaskDto dto)
        {
            CustomTask checkEntity = Repository
                .Get(e => e.Id == dto.Id)
                .SingleOrDefault();

            if (checkEntity != null)
            {
                throw new DuplicateNameException();
            }

            CustomTask entity = MapToEntity(dto);
            Repository.Add(entity);
            _unitOfWork.SaveChanges();
        }

        public override void Remove(string id)
        {
            CustomTask entity = Repository
             .Get(e => e.Id == id)
             .SingleOrDefault();

            if (entity == null)
            {
                throw new ObjectNotFoundException();
            }

            Repository.Remove(entity);
            _unitOfWork.SaveChanges();
        }

        public override void Update(CustomTaskDto dto)
        {
            CustomTask entity = Repository
             .Get(e => e.Id == dto.Id)
             .SingleOrDefault();

            if (entity == null)
            {
                throw new ObjectNotFoundException();
            }

            entity.Name = dto.Name;
            entity.CreationDate = dto.CreationDate;
            entity.Deadline = dto.Deadline;
            entity.Description = dto.Description;
            entity.Status = dto.Status;
            entity.UserAssigneeId = dto.UserAssigneeId;
            entity.UserCreatorId = dto.UserCreatorId;
            entity.ProjectId = dto.ProjectId;

            Repository.Update(entity);
            _unitOfWork.SaveChanges();
        }

        protected override CustomTaskDto MapToDto(CustomTask entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException();
            }

            CustomTaskDto dto = new CustomTaskDto
            {
                Id = entity.Id,
                Name = entity.Name,
                CreationDate = entity.CreationDate,
                Deadline = entity.Deadline,
                Description = entity.Description,
                Status = entity.Status,
                UserAssigneeId = entity.UserAssigneeId,
                UserCreatorId = entity.UserCreatorId,
                ProjectId = entity.ProjectId
            };

            return dto;
        }

        protected override CustomTask MapToEntity(CustomTaskDto dto)
        {
            if (dto == null)
            {
                throw new ArgumentNullException();
            }

            CustomTask entity = new CustomTask
            {
                Id = dto.Id,
                Name = dto.Name,
                CreationDate = dto.CreationDate,
                Deadline = dto.Deadline,
                Description = dto.Description,
                Status = dto.Status,
                UserAssigneeId = dto.UserAssigneeId,
                UserCreatorId = dto.UserCreatorId,
                ProjectId = dto.ProjectId
            };

            return entity;
        }

        private Func<CustomTask, bool> GetFilter(CustomTaskFilter filter)
        {
            Func<CustomTask, bool> result = e => true;
            if (!String.IsNullOrEmpty(filter?.Name))
            {
                result += e => e.Name == filter.Name;
            }

            if (!String.IsNullOrEmpty(filter?.ProjectId))
            {
                result += e => e.ProjectId == filter.ProjectId;
            }

            return result;
        }
    }
}
