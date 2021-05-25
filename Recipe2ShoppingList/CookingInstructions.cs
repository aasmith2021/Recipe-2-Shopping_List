using System;
using System.Collections.Generic;
using System.Text;

namespace Recipe2ShoppingList
{
    class CookingInstructions
    {        
        private List<InstructionBlock> allInstructions = new List<InstructionBlock>();

        public InstructionBlock[] AllInstructions
        {
            get { return this.allInstructions.ToArray(); }
        }

        public void AddInstructionBlock(InstructionBlock newInstructionBlock)
        {
            allInstructions.Add(newInstructionBlock);
        }

        public void DeleteInstructionBlock(int indexOfBlockToDelete)
        {
            allInstructions.RemoveAt(indexOfBlockToDelete);
        }
    }
}
