using System;
using System.IO;
using System.Text.RegularExpressions;

namespace Recipe2ShoppingList
{
    class Program
    {
        static void Main(string[] args)
        {            
            IUserIO userIO = new ConsoleIO();
            IDataIO dataIO = new ApiIO();
            IDataIO backupIO = new FileIO();
            IDataIO shoppingListIO = new FileIO();
            
            bool exitProgram = false;

            while (!exitProgram)
            {
                ProgramExecution.RunProgram(userIO, dataIO, backupIO, shoppingListIO, out exitProgram);
            }
        }
    }
}
