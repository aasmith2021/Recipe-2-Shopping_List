using Microsoft.VisualStudio.TestTools.UnitTesting;
using Recipe2ShoppingList;

namespace Recipe2ShoppingList.Test
{
    [TestClass]
    public class PrepTimesTests
    {
        [DataTestMethod]
        [DataRow(45, 95, 140)]
        [DataRow(100, 200, 300)]
        public void TotalTimeCorrect(int prepTime, int cookTime, int expected)
        {
            //Arrange

            //Act
            Times newPrepTime = new Times(prepTime, cookTime);

            //Assert
            int actual = newPrepTime.TotalTime;
            Assert.AreEqual(expected, actual, 0, "Prep time and cook time do not add up to total time");
        }
    }
}
