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
            
            bool exitProgram = false;

            while (!exitProgram)
            {
                ProgramExecution.RunProgram(userIO, out exitProgram);
            }
        }
    }
}
