using System;
using System.Collections.Generic;
using System.Text;

namespace Recipe2ShoppingList
{
    class CookingInstructions
    {       
        private List<InstructionBlock> instructionBlocks = new List<InstructionBlock>();

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

        public void PrintInstructions()
        {
            Console.WriteLine("INSTRUCTIONS:");

            for (int i = 0; i < this.InstructionBlocks.Length; i++)
            {
                InstructionBlock currentInstructionBlock = this.InstructionBlocks[i];
                int lineNumber = 1;
                string lineNumberString = $"{lineNumber}.";

                foreach (string instructionLine in currentInstructionBlock.InstructionLines)
                {
                    Console.WriteLine($"{lineNumberString,-2} {instructionLine}");
                    lineNumber++;
                    lineNumberString = $"{lineNumber}.";
                }

                if (i != this.InstructionBlocks.Length - 1)
                {
                    Console.WriteLine();
                }
            }
        }
    }
}
