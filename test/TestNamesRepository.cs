using Xunit;
using Moq;
using System.Linq;
using NamesApi.Models;
using Microsoft.EntityFrameworkCore;

namespace NamesApi.Tests
{
    public class TestNamesRespository
    {
        [Fact]
        public void Can_Get_All()
        {
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
            NameEntry[] resultNames = mock.Object.Names.ToArray();

            Assert.Equal(resultNames.Length, namesMock.Length);
            Assert.Equal(resultNames[0].Name, namesMock[0].Name);
        }
        [Fact]
        public async void Can_Create_Name()
        {
            Mock<DbSet<NameEntry>> mockSet = new Mock<DbSet<NameEntry>>();
            Mock<NamesDbContext> mockContext = new Mock<NamesDbContext>();
            mockContext.Setup(m => m.Names).Returns(mockSet.Object);

            INamesRepository repo = new EFNamesRepository(mockContext.Object);
            NameEntry newEntry = new NameEntry
            {
                Name = "Dax",
                Weight = 0.5f
            };

            await repo.CreateName(newEntry);
            mockSet.Verify(m => m.Add(It.IsAny<NameEntry>()), Times.Once);
            mockContext.Verify(m => m.SaveChanges(), Times.Once);
        }
    }
}