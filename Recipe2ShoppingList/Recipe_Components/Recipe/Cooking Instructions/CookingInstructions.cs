using System;
using System.Collections.Generic;
using System.Text;

namespace Recipe2ShoppingList
{
    public class CookingInstructions
    {       
        private List<InstructionBlock> instructionBlocks = new List<InstructionBlock>();
    
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

        public string ProduceInstructionsText(bool printVersion, bool includeEditHeadings = false)
        {
            string instructionsText = "";

            if (printVersion)
            {
                instructionsText += $"INSTRUCTIONS:{Environment.NewLine}";

                for (int i = 0; i < this.InstructionBlocks.Length; i++)
                {
                    InstructionBlock currentInstructionBlock = this.InstructionBlocks[i];
                    int lineNumber = 1;
                    string lineNumberString = $"{lineNumber}.";

                    if (includeEditHeadings)
                    {
                        instructionsText += $"{Environment.NewLine}<<<INSTRUCTION BLOCK {i + 1}>>>{Environment.NewLine}";
                    }

                    if (currentInstructionBlock.BlockHeading != "")
                    {
                        instructionsText += $"<{currentInstructionBlock.BlockHeading}>{Environment.NewLine}";
                    }

                    foreach (string instructionLine in currentInstructionBlock.InstructionLines)
                    {
                        instructionsText += UserInterface.MakeStringConsoleLengthLines($"{lineNumberString,-2} {instructionLine}{Environment.NewLine}");
                        lineNumber++;
                        lineNumberString = $"{lineNumber}.";
                    }

                    if (i != this.InstructionBlocks.Length - 1)
                    {
                        instructionsText += $"{Environment.NewLine}";
                    }
                }
            }
            else
            {
                instructionsText += $"-START_OF_INSTRUCTIONS-{Environment.NewLine}";
                
                for (int i = 0; i < this.InstructionBlocks.Length; i++)
                {
                    InstructionBlock currentInstructionBlock = this.InstructionBlocks[i];

                    instructionsText += $"-NEW_INSTRUCTION_BLOCK-{Environment.NewLine}";
                    instructionsText += $"BLOCK_HEADING:{currentInstructionBlock.BlockHeading}{Environment.NewLine}";

                    foreach (string instructionLine in currentInstructionBlock.InstructionLines)
                    {
                        instructionsText += $"LINE:{instructionLine}{Environment.NewLine}";
                    }

                    instructionsText += $"-END_OF_INSTRUCTION_BLOCK-{Environment.NewLine}";
                }
            }
            
            return instructionsText;
        }

    }
}
