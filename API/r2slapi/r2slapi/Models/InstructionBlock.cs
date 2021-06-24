using System;
using System.Collections.Generic;
using System.Text;

namespace r2slapi.Models
{
    public class InstructionBlock
    {
        private List<string> instructionLines = new List<string>();

        public InstructionBlock()
        {

        }
        
        public InstructionBlock(string blockHeading = "")
        {
            this.BlockHeading = blockHeading;
        }

        public int Id { get; set; }

        public string BlockHeading { get; set; }
        
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

        public void EditInstructionLine(int indexOfLineToEdit, string newInstructionLine)
        {
            instructionLines[indexOfLineToEdit] = newInstructionLine;
        }
    }
}
