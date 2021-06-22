using System;
using System.Collections.Generic;
using System.Text;
using Recipe2ShoppingList;

namespace Recipe2ShoppingList.Test
{
    public class FakeUserIO : IUserIO
    {
        public FakeUserIO(string fakeInput = "")
        {
            this.FakeInput = fakeInput;
        }

        public string FakeInput { get; }

        public string GetInput() 
        {
            return this.FakeInput;
        }

        public void DisplayData(string dataToDisplay)
        {
        }

        public void DisplayDataLite(string dataToDisplay)
        {
        }

        public void ClearDisplay()
        {

        }
    }
}
