using System;
using System.Collections.Generic;
using System.Text;

namespace Recipe2ShoppingList
{
    public class CookingInstructions
    {         
        public int Id { get; set; }
        
        public List<InstructionBlock> InstructionBlocks { get; set; } = new List<InstructionBlock>();

        public void AddInstructionBlock(InstructionBlock newInstructionBlock)
        {
            InstructionBlocks.Add(newInstructionBlock);
        }

        public void DeleteInstructionBlock(int indexOfBlockToDelete)
        {
            InstructionBlocks.RemoveAt(indexOfBlockToDelete);
        }

        public string ProduceInstructionsText(bool printVersion, bool includeEditHeadings = false)
        {
            string instructionsText = "";

            if (printVersion)
            {
                instructionsText += $"INSTRUCTIONS:{Environment.NewLine}";

                for (int i = 0; i < this.InstructionBlocks.Count; i++)
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

                    if (i != this.InstructionBlocks.Count - 1)
                    {
                        instructionsText += $"{Environment.NewLine}";
                    }
                }
            }
            else
            {
                instructionsText += $"COOKING_INSTRUCTIONS_ID:{this.Id}{Environment.NewLine}";
                instructionsText += $"-START_OF_INSTRUCTIONS-{Environment.NewLine}";
                
                for (int i = 0; i < this.InstructionBlocks.Count; i++)
                {
                    InstructionBlock currentInstructionBlock = this.InstructionBlocks[i];

                    instructionsText += $"-NEW_INSTRUCTION_BLOCK-{Environment.NewLine}";
                    instructionsText += $"INSTRUCTION_BLOCK_ID:{this.InstructionBlocks[i].Id}{Environment.NewLine}";
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
