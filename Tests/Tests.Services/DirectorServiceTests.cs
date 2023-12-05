using System.Linq.Expressions;

namespace OpenMovies.Services.Tests;

public class DirectorServiceTests
{
    [Fact]
    public async Task CreateDirector_WithValidData_ShouldCreateDirector()
    {
        var mockRepository = new Mock<IDirectorRepository>();
        var directorService = new DirectorService(mockRepository.Object);
        var validDirector = new Director("John", "Doe");

        await directorService.CreateDirector(validDirector);

        mockRepository.Verify(repo => repo.AddAsync(validDirector), Times.Once);
    }

    [Fact]
    public async Task CreateDirector_WithInvalidData_ShouldThrowValidationException()
    {
        var mockRepository = new Mock<IDirectorRepository>();
        var directorService = new DirectorService(mockRepository.Object);

        var invalidDirector = new Director("", "");

        await Assert.ThrowsAsync<ValidationException>(() => directorService.CreateDirector(invalidDirector));
        mockRepository.Verify(repo => repo.AddAsync(It.IsAny<Director>()), Times.Never);
    }

    [Fact]
    public async Task CreateDirector_WithExistingDirector_ShouldThrowInvalidOperation()
    {
        var mockRepository = new Mock<IDirectorRepository>();
        mockRepository.Setup(repo => repo.GetAsync(It.IsAny<Expression<Func<Director, bool>>>()))
            .ReturnsAsync(new Director("John", "Doe"));

        var directorService = new DirectorService(mockRepository.Object);
        var existingDirector = new Director("John", "Doe");

        await Assert.ThrowsAsync<InvalidOperationException>(() => directorService.CreateDirector(existingDirector));
        mockRepository.Verify(repo => repo.AddAsync(It.IsAny<Director>()), Times.Never);
    }

    [Fact]
    public async Task GetAllDirectors_ShouldReturnAllDirectors()
    {
        var mockRepository = new Mock<IDirectorRepository>();
        var directors = new List<Director> { new Director("John", "Doe"), new Director("Jane", "Doe") };
        var directorService = new DirectorService(mockRepository.Object);

        mockRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(directors);

        var result = await directorService.GetAllDirectors();

        Assert.NotNull(result);
        Assert.Equal(directors.Count, result.Count());
    }

    [Fact]
    public async Task GetDirectorById_WithValidId_ShouldReturnDirector()
    {
        var mockRepository = new Mock<IDirectorRepository>();
        var directorService = new DirectorService(mockRepository.Object);

        var existingDirector = new Director("John", "Doe");
        mockRepository.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(existingDirector);

        var result = await directorService.GetDirectorById(1);

        Assert.NotNull(result);
        Assert.Equal(existingDirector.Id, result.Id);
        Assert.Equal(existingDirector.FirstName, result.FirstName);
    }

    [Fact]
    public async Task GetDirectorById_WithInvalidId_ShouldThrowInvalidOperationException()
    {
        var mockRepository = new Mock<IDirectorRepository>();
        var directorService = new DirectorService(mockRepository.Object);

        # pragma warning disable CS8620, CS8600
        mockRepository.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync((Director)null);

        await Assert.ThrowsAsync<InvalidOperationException>(() => directorService.GetDirectorById(1));
    }

        [Fact]
        public async Task UpdateDirector_WithValidData_ShouldUpdateDirector()
        {

            var mockRepository = new Mock<IDirectorRepository>();
            var directorService = new DirectorService(mockRepository.Object);
            var existingDirector = new Director("John", "Doe");

            mockRepository.Setup(repo => repo.GetByIdAsync(existingDirector.Id)).ReturnsAsync(existingDirector);
            var updatedDirector = new Director("Updated", "Director");

            await directorService.UpdateDirector(updatedDirector);

            mockRepository.Verify(repo => repo.UpdateAsync(updatedDirector), Times.Once);
        }

        [Fact]
        public async Task UpdateDirector_WithInvalidData_ShouldThrowValidationException()
        {

            var mockRepository = new Mock<IDirectorRepository>();
            var directorService = new DirectorService(mockRepository.Object);

            var invalidDirector = new Director("", "");

            await Assert.ThrowsAsync<ValidationException>(() => directorService.UpdateDirector(invalidDirector));
            mockRepository.Verify(repo => repo.UpdateAsync(It.IsAny<Director>()), Times.Never);
        }

        [Fact]
        public async Task UpdateDirector_NonExistingDirector_ShouldThrowInvalidOperationException()
        {

            var mockRepository = new Mock<IDirectorRepository>();
            var directorService = new DirectorService(mockRepository.Object);

            mockRepository.Setup(repo => repo.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((Director)null);
            var nonExistingDirector = new Director("Non", "Existing");


            await Assert.ThrowsAsync<InvalidOperationException>(() => directorService.UpdateDirector(nonExistingDirector));
            mockRepository.Verify(repo => repo.UpdateAsync(It.IsAny<Director>()), Times.Never);
        }

        [Fact]
        public async Task DeleteDirector_WithValidId_ShouldDeleteDirector()
        {

            var mockRepository = new Mock<IDirectorRepository>();
            var directorService = new DirectorService(mockRepository.Object);

            var existingDirector = new Director("John", "Doe");
            mockRepository.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(existingDirector);

            await directorService.DeleteDirector(1);

            mockRepository.Verify(repo => repo.DeleteAsync(existingDirector), Times.Once);
        }

        [Fact]
        public async Task DeleteDirector_WithInvalidId_ShouldThrowInvalidOperationException()
        {

            var mockRepository = new Mock<IDirectorRepository>();
            var directorService = new DirectorService(mockRepository.Object);

            mockRepository.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync((Director)null);

            await Assert.ThrowsAsync<InvalidOperationException>(() => directorService.DeleteDirector(1));
            mockRepository.Verify(repo => repo.DeleteAsync(It.IsAny<Director>()), Times.Never);
        }
}