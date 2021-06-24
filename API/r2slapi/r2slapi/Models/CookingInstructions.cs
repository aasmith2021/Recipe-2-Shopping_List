using System;
using System.Collections.Generic;
using System.Text;

namespace r2slapi.Models
{
    public class CookingInstructions
    {       
        private List<InstructionBlock> instructionBlocks = new List<InstructionBlock>();

        public CookingInstructions()
        {

        }

        public int Id { get; set; }

        public InstructionBlock[] InstructionBlocks
        {
            get { return this.instructionBlocks.ToArray(); }
        }

        public void AddInstructionBlock(InstructionBlock newInstructionBlock)
        {
            instructionBlocks.Add(newInstructionBlock);
        }

        public void DeleteInstructionBlock(int indexOfBlockToDelete)
        {
            instructionBlocks.RemoveAt(indexOfBlockToDelete);
        }
    }
}
