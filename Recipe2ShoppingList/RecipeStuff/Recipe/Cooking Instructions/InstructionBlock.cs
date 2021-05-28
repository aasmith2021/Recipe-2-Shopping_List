using System;
using System.Collections.Generic;
using System.Text;

namespace Recipe2ShoppingList
{
    public class InstructionBlock
    {
        public InstructionBlock(string blockHeading = "")
        {
            this.BlockHeading = blockHeading;
        }
        
        public string BlockHeading { get; set; }
        
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
