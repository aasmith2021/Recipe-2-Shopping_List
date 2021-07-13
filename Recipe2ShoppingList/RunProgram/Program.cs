using System;
using System.IO;
using System.Text.RegularExpressions;

namespace Recipe2ShoppingList
{
    class Program
    {
        static void Main(string[] args)
        {            
            //IUserIO is used to get input from and display data to the user
            //IDataIO is used to read data from and write data to a data source (currently can either be a file or an API)
            //The FileIO <fileBackupIO> is used to:
            //   -Backup all program data to a local file in case the API server is down
            //   -Read the backup data at the start of the program to import backedup changes
            //   -Write a backup of the Shopping List data to a local file
            IUserIO userIO = new ConsoleIO();
            IDataIO dataIO = new ApiIO();
            FileIO fileBackupIO = new FileIO();
            
            bool exitProgram = false;


            //The program will run until <exitProgram> is set to true
            while (!exitProgram)
            {
                ProgramExecution.RunProgram(userIO, dataIO, fileBackupIO, out exitProgram);
            }
        }
    }
}
