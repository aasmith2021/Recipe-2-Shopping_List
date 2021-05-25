using System;
using System.Collections.Generic;
using System.Text;

namespace Recipe2ShoppingList
{
    class InstructionBlock
    {
        private List<string> instructionLines = new List<string>();

        public string[] InstructionLines
        {
            get { return this.instructionLines.ToArray(); }
        }
        
        public void AddInstructionLine(string newInstructionLine)
        {
            instructionLines.Add(newInstructionLine);
        }

        public void DeleteInstructionLine(int indexOfLineToDelete)
        {
            instructionLines.RemoveAt(indexOfLineToDelete);
        }
    }
}
