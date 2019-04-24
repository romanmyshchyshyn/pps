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
    public class ImageServiceTests
    {
        private readonly List<Image> _images;

        public ImageServiceTests()
        {
            _images = new List<Image>
            {
                new Image { Id = "1", Path = "P1" },
                new Image { Id = "2", Path = "P2" },
                new Image { Id = "3", Path = "P3" }
            };
        }

        [Fact]
        public void GetByFilterTest()
        {
            //Arange
            Mock<IUnitOfWork> unitOfWorkMock = new Mock<IUnitOfWork>();
            Mock<IRepository<Image>> repositoryMock = new Mock<IRepository<Image>>();
            repositoryMock.Setup(repo => repo.Get(It.IsAny<Expression<Func<Image, bool>>>())).Returns(_images.AsQueryable);
            unitOfWorkMock.Setup(getRepo => getRepo.GetRepository<Image>()).Returns(repositoryMock.Object);
            ImageService imageService = new ImageService(unitOfWorkMock.Object);
            ImageFilter imageFilter = new ImageFilter();

            //Act
            IEnumerable<ImageDto> imagesDto = imageService.Get(imageFilter);

            //Assert
            Assert.NotNull(imagesDto);
            Assert.NotEmpty(imagesDto);
            Assert.Equal(3, imagesDto.Count());
        }


        [Fact]
        public void GetByIdTest()
        {
            //Arange
            Mock<IUnitOfWork> unitOfWorkMock = new Mock<IUnitOfWork>();
            Mock<IRepository<Image>> repositoryMock = new Mock<IRepository<Image>>();
            repositoryMock.Setup(repo => repo.Get(It.IsAny<Expression<Func<Image, bool>>>()))
                .Returns<Expression<Func<Image, bool>>>(predicate =>
                    _images.Where(predicate.Compile()).AsQueryable());
            unitOfWorkMock.Setup(getRepo => getRepo.GetRepository<Image>()).Returns(repositoryMock.Object);
            ImageService imageService = new ImageService(unitOfWorkMock.Object);

            //Act
            ImageDto imageDto = imageService.Get("1");

            //Assert
            Assert.NotNull(imageDto);
            Assert.Equal("P1", imageDto.Path);
        }

        [Fact]
        public void AddTest()
        {
            //Arange
            ImageDto imageDto = new ImageDto()
            {
                Id = "0",
                Path = "P0"
            };
            bool isAdded = false;
            Mock<IUnitOfWork> unitOfWorkMock = new Mock<IUnitOfWork>();
            Mock<IRepository<Image>> repositoryMock = new Mock<IRepository<Image>>();
            repositoryMock.Setup(repo => repo.Get(It.IsAny<Expression<Func<Image, bool>>>()))
                .Returns<Expression<Func<Image, bool>>>(predicate =>
                    _images.Where(predicate.Compile()).AsQueryable());
            repositoryMock.Setup(repo => repo.Add(It.IsAny<Image>())).Callback(() => isAdded = true);
            unitOfWorkMock.Setup(getRepo => getRepo.GetRepository<Image>()).Returns(repositoryMock.Object);
            ImageService imageService = new ImageService(unitOfWorkMock.Object);

            //Act
            imageService.Add(imageDto);

            //Assert
            Assert.True(isAdded);
        }

        [Fact]
        public void RemoveTest()
        {
            //Arange
            bool isRemoved = false;
            Mock<IUnitOfWork> unitOfWorkMock = new Mock<IUnitOfWork>();
            Mock<IRepository<Image>> repositoryMock = new Mock<IRepository<Image>>();
            repositoryMock.Setup(repo => repo.Get(It.IsAny<Expression<Func<Image, bool>>>()))
                .Returns<Expression<Func<Image, bool>>>(predicate =>
                    _images.Where(predicate.Compile()).AsQueryable());
            repositoryMock.Setup(repo => repo.Remove(It.IsAny<Image>())).Callback(() => isRemoved = true);
            unitOfWorkMock.Setup(getRepo => getRepo.GetRepository<Image>()).Returns(repositoryMock.Object);
            ImageService imageService = new ImageService(unitOfWorkMock.Object);

            //Act
            imageService.Remove(_images[0].Id);

            //Assert
            Assert.True(isRemoved);
        }

        [Fact]
        public void UpdateTest()
        {
            //Arange
            ImageDto imageDto = new ImageDto()
            {
                Id = "1",
                Path = "P0"
            };
            bool isUpdate = false;
            Mock<IUnitOfWork> unitOfWorkMock = new Mock<IUnitOfWork>();
            Mock<IRepository<Image>> repositoryMock = new Mock<IRepository<Image>>();
            repositoryMock.Setup(repo => repo.Get(It.IsAny<Expression<Func<Image, bool>>>()))
                .Returns<Expression<Func<Image, bool>>>(predicate =>
                    _images.Where(predicate.Compile()).AsQueryable());
            repositoryMock.Setup(repo => repo.Update(It.Is<Image>(entity =>
                    (entity.Id == imageDto.Id) &&
                    (entity.Path == imageDto.Path))))
                .Callback(() => isUpdate = true);
            unitOfWorkMock.Setup(getRepo => getRepo.GetRepository<Image>()).Returns(repositoryMock.Object);
            ImageService imageService = new ImageService(unitOfWorkMock.Object);

            //Act
            imageService.Update(imageDto);

            //Assert
            Assert.True(isUpdate);
        }
    }
}
