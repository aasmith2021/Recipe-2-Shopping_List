using System;
using System.Collections.Generic;
using System.Text;

namespace Recipe2ShoppingList
{
    //Used to specifically run the program as a console application
    public class ConsoleIO : IUserIO
    {
        public string GetInput()
        {
            return Console.ReadLine();
        }
        
        public void DisplayData(string dataToDisplay = "")
        {
            Console.WriteLine(dataToDisplay);
        }

        public void DisplayDataLite(string dataToDisplay = "")
        {
            Console.Write(dataToDisplay);
        }

        public void ClearDisplay()
        {
            Console.Clear();
        }
    }
}
