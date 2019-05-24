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
    public class CustomTaskStatusService : Service<CustomTaskStatus, CustomTaskStatusDto, CustomTaskStatusFilter>, ICustomTaskStatusService
    {
        public CustomTaskStatusService(IUnitOfWork unitOfWork) :
            base(unitOfWork)
        {
        }

        public override CustomTaskStatusDto Get(string name)
        {
            CustomTaskStatus entity = Repository
              .Get(e => e.Name == name)
              .SingleOrDefault();

            if (entity == null)
            {
                throw new ObjectNotFoundException();
            }

            return MapToDto(entity);
        }

        public override IEnumerable<CustomTaskStatusDto> Get(CustomTaskStatusFilter filter)
        {
            Func<CustomTaskStatus, bool> predicate = GetFilter(filter);
            List<CustomTaskStatus> entities = Repository
              .Get(p => predicate(p))
              .ToList();

            return entities.Select(e => MapToDto(e));
        }

        public override void Add(CustomTaskStatusDto dto)
        {
            CustomTaskStatus checkEntity = Repository
                .Get(e => e.Name == dto.Name)
                .SingleOrDefault();

            if (checkEntity != null)
            {
                throw new DuplicateNameException();
            }

            CustomTaskStatus entity = MapToEntity(dto);
            Repository.Add(entity);
            _unitOfWork.SaveChanges();
        }

        public override void Remove(string name)
        {
            CustomTaskStatus entity = Repository
             .Get(e => e.Name == name)
             .SingleOrDefault();

            if (entity == null)
            {
                throw new ObjectNotFoundException();
            }

            Repository.Remove(entity);
            _unitOfWork.SaveChanges();
        }

        public override void Update(CustomTaskStatusDto dto)
        {
            CustomTaskStatus entity = Repository
             .Get(e => e.Name == dto.Name)
             .SingleOrDefault();

            if (entity == null)
            {
                throw new ObjectNotFoundException();
            }

            entity.Name = dto.Name;

            Repository.Update(entity);
            _unitOfWork.SaveChanges();
        }

        protected override CustomTaskStatusDto MapToDto(CustomTaskStatus entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException();
            }

            CustomTaskStatusDto dto = new CustomTaskStatusDto
            {
                Name = entity.Name
            };

            return dto;
        }

        protected override CustomTaskStatus MapToEntity(CustomTaskStatusDto dto)
        {
            if (dto == null)
            {
                throw new ArgumentNullException();
            }

            CustomTaskStatus entity = new CustomTaskStatus
            {
                Name = dto.Name
            };

            return entity;
        }

        private Func<CustomTaskStatus, bool> GetFilter(CustomTaskStatusFilter filter)
        {
            Func<CustomTaskStatus, bool> result = e => true;
            if (!String.IsNullOrEmpty(filter?.Name))
            {
                result += e => e.Name == filter.Name;
            }

            return result;
        }
    }
}
