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
    public class ImageService : Service<Image, ImageDto, ImageFilter>, IImageService
    {
        public ImageService(IUnitOfWork unitOfWork) :
            base(unitOfWork)
        {
        }

        public override ImageDto Get(string id)
        {
            Image entity = Repository
              .Get(e => e.Id == id)
              .SingleOrDefault();

            if (entity == null)
            {
                throw new ObjectNotFoundException();
            }

            return MapToDto(entity);
        }

        public override IEnumerable<ImageDto> Get(ImageFilter filter)
        {
            Func<Image, bool> predicate = GetFilter(filter);
            List<Image> entities = Repository
              .Get(p => predicate(p))
              .ToList();

            return entities.Select(e => MapToDto(e));
        }

        public override void Add(ImageDto dto)
        {
            Image checkEntity = Repository
                .Get(e => e.Id == dto.Id)
                .SingleOrDefault();

            if (checkEntity != null)
            {
                throw new DuplicateNameException();
            }

            Image entity = MapToEntity(dto);
            Repository.Add(entity);
            _unitOfWork.SaveChanges();
        }

        public override void Remove(string id)
        {
            Image entity = Repository
             .Get(e => e.Id == id)
             .SingleOrDefault();

            if (entity == null)
            {
                throw new ObjectNotFoundException();
            }

            Repository.Remove(entity);
            _unitOfWork.SaveChanges();
        }

        public override void Update(ImageDto dto)
        {
            Image entity = Repository
             .Get(e => e.Id == dto.Id)
             .SingleOrDefault();

            if (entity == null)
            {
                throw new ObjectNotFoundException();
            }

            entity.Path = dto.Path;

            Repository.Update(entity);
            _unitOfWork.SaveChanges();
        }

        protected override ImageDto MapToDto(Image entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException();
            }

            ImageDto dto = new ImageDto
            {
                Id = entity.Id,
                Path = entity.Path
            };

            return dto;
        }

        protected override Image MapToEntity(ImageDto dto)
        {
            if (dto == null)
            {
                throw new ArgumentNullException();
            }

            Image entity = new Image
            {
                Id = dto.Id,
                Path = dto.Path
            };

            return entity;
        }

        private Func<Image, bool> GetFilter(ImageFilter filter)
        {
            Func<Image, bool> result = e => true;
            if (!String.IsNullOrEmpty(filter?.Path))
            {
                result += e => e.Path == filter.Path;
            }

            return result;
        }
    }
}
