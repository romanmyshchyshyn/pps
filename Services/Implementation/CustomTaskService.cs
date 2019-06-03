using AutoMapper;
using DataAccess.Interfaces;
using DataAccess.Models;
using Microsoft.EntityFrameworkCore;
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
              .Include(e => e.UserAssignee)
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

            Mapper.Map<CustomTaskDto, CustomTask>(dto, entity);

            Repository.Update(entity);
            _unitOfWork.SaveChanges();
        }

        public void UpdateStatus(string id, string status)
        {
            CustomTask entity = Repository
             .Get(e => e.Id == id)
             .SingleOrDefault();

            if (entity == null)
            {
                throw new ObjectNotFoundException();
            }

            entity.Status = status;

            Repository.Update(entity);
            _unitOfWork.SaveChanges();
        }

        protected override CustomTaskDto MapToDto(CustomTask entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException();
            }

            return Mapper.Map<CustomTask, CustomTaskDto>(entity);
        }

        protected override CustomTask MapToEntity(CustomTaskDto dto)
        {
            if (dto == null)
            {
                throw new ArgumentNullException();
            }
            
            return Mapper.Map<CustomTaskDto, CustomTask>(dto);
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
