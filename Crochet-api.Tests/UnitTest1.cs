namespace Crochet_api.Tests
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            // Arrange
            int a = 3;
            int b = 5;

            // Act
            int result = a + b;

            // Assert
            Assert.Equal(8, result); // Check if the result is as expected.
        }
    }
}