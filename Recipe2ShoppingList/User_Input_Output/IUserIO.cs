using System;
using System.Collections.Generic;
using System.Text;

namespace Recipe2ShoppingList
{
    public interface IUserIO
    {
        public string GetInput();
    
        public void DisplayData(string dataToDisplay = "");

        public void DisplayDataLite(string dataToDisplay = "");

        public void ClearDisplay();
    }
}
