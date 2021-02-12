using Xunit;
using Moq;
using NamesApi.Controllers;
using System.Linq;
using System.Collections.Generic;
using NamesApi.Models;
namespace NamesApi.Tests
{
    public class TestNamesController
    {
        [Fact]
        public void Can_Use_Repository()
        {
            // Arrange
            Mock<INamesRepository> mock = new Mock<INamesRepository>();
            NameEntry[] namesMock = new NameEntry[]
            {
                new NameEntry { Name = "Dax", Weight = 0.07f },
                new NameEntry { Name = "Devon", Weight = 0.2f },
                new NameEntry { Name = "Michael", Weight = 0.5f },
                new NameEntry { Name = "Bruce", Weight = 0.15f },
                new NameEntry { Name = "Desmond", Weight = 0.0f },
                new NameEntry { Name = "John", Weight = 0.5f }
            };
            mock.Setup(m => m.Names).Returns(namesMock.AsQueryable<NameEntry>());

            NamesController controller = new NamesController(mock.Object);

            // Act
            IEnumerable<NameEntry> result = controller.GetNames(null);

            // Assert
            NameEntry[] nameArray = result.ToArray();
            Assert.True(nameArray.Length == namesMock.Length);
            Assert.Equal("Dax", nameArray[0].Name);
            Assert.Equal("Devon", nameArray[1].Name);
        }
        [Fact]
        public void Can_Filter_Name_Type_By_First()
        {
            // Arrange
            Mock<INamesRepository> mock = new Mock<INamesRepository>();
            NameEntry[] namesMock = new NameEntry[]
            {
                new NameEntry { Id = 1, Name = "Dax", Weight = 0.07f },
                new NameEntry { Id = 2, Name = "Michael", Weight = 0.6f },
                new NameEntry { Id = 3, Name = "Devon", Weight = 0.2f },
                new NameEntry { Id = 4, Name = "Bruce", Weight = 0.15f },
                new NameEntry { Id = 5, Name = "Desmond", Weight = 0.0f },
                new NameEntry { Id = 6, Name = "John", Weight = 0.5f }
            };
            mock.Setup(m => m.Names).Returns(namesMock.AsQueryable<NameEntry>());

            NamesController controller = new NamesController(mock.Object);
            
            // Act
            IEnumerable<NameEntry> result = controller.GetNames("first");
            NameEntry[] nameArray = result.OrderBy(n => n.Id).ToArray();
            NameEntry[] namesMockFirst = namesMock.Where(n => n.Weight <= 0.5f).ToArray();

            // Assert
            
            Assert.Equal(namesMockFirst.Length, nameArray.Length);
            Assert.Equal("Dax", nameArray[0].Name);
            Assert.Equal("Devon", nameArray[1].Name);
        }
        [Fact]
        public void Can_Filter_Name_Type_By_Middle()
        {
            // Arrange
            Mock<INamesRepository> mock = new Mock<INamesRepository>();
            NameEntry[] namesMock = new NameEntry[]
            {
                new NameEntry { Id = 1, Name = "Dax", Weight = 0.07f },
                new NameEntry { Id = 2, Name = "Michael", Weight = 0.6f },
                new NameEntry { Id = 3, Name = "Devon", Weight = 0.2f },
                new NameEntry { Id = 4, Name = "Bruce", Weight = 0.15f },
                new NameEntry { Id = 5, Name = "Desmond", Weight = 0.0f },
                new NameEntry { Id = 6, Name = "John", Weight = 0.5f }
            };
            mock.Setup(m => m.Names).Returns(namesMock.AsQueryable<NameEntry>());

            NamesController controller = new NamesController(mock.Object);
            
            // Act
            IEnumerable<NameEntry> result = controller.GetNames("middle");
            NameEntry[] nameArray = result.OrderBy(n => n.Id).ToArray();
            NameEntry[] namesMockMiddle = namesMock.Where(n => n.Weight >= 0.5f).ToArray();

            // Assert
            
            Assert.Equal(namesMockMiddle.Length, nameArray.Length);
            Assert.Equal("Michael", nameArray[0].Name);
            Assert.Equal("John", nameArray[1].Name);
        }
        [Fact]
        public async void Can_Post_New_Name()
        {
            // Arrange
            Mock<INamesRepository> mock = new Mock<INamesRepository>();
            NameEntry[] namesMock = new NameEntry[]
            {
                new NameEntry { Id = 1, Name = "Dax", Weight = 0.07f },
                new NameEntry { Id = 2, Name = "Michael", Weight = 0.6f },
                new NameEntry { Id = 3, Name = "Devon", Weight = 0.2f },
                new NameEntry { Id = 4, Name = "Bruce", Weight = 0.15f },
                new NameEntry { Id = 5, Name = "Desmond", Weight = 0.0f },
                new NameEntry { Id = 6, Name = "John", Weight = 0.5f }
            };
            mock.Setup(m => m.Names).Returns(namesMock.AsQueryable<NameEntry>());

            NamesController controller = new NamesController(mock.Object);
            
            // Act
            NameEntry newName = new NameEntry
            {
                Id = 7, Name = "Test", Weight = 0.5f
            };
            
            await controller.PostNameEntry(newName);

            // Assert

        }
    }
}
