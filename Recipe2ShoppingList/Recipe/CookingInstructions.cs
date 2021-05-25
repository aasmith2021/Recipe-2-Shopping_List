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

        public string ProduceInstructionsText()
        {
            string instructionsText = "";
            
            instructionsText += $"INSTRUCTIONS:{Environment.NewLine}";

            for (int i = 0; i < this.InstructionBlocks.Length; i++)
            {
                InstructionBlock currentInstructionBlock = this.InstructionBlocks[i];
                int lineNumber = 1;
                string lineNumberString = $"{lineNumber}.";

                if(currentInstructionBlock.BlockHeading != "")
                {
                    instructionsText += $"<{currentInstructionBlock.BlockHeading}>{Environment.NewLine}";
                }
                
                foreach (string instructionLine in currentInstructionBlock.InstructionLines)
                {
                    instructionsText += $"{lineNumberString,-2} {instructionLine}{Environment.NewLine}";
                    lineNumber++;
                    lineNumberString = $"{lineNumber}.";
                }

                if (i != this.InstructionBlocks.Length - 1)
                {
                    instructionsText += $"{Environment.NewLine}";
                }
            }

            return instructionsText;
        }
    }
}
