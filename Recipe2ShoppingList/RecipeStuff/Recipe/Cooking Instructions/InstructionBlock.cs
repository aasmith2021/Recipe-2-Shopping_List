using System;
using System.Collections.Generic;
using System.Text;

namespace Recipe2ShoppingList
{
    public class InstructionBlock
    {
        public InstructionBlock()
        {

        }
        
        public InstructionBlock(string blockHeading = "")
        {
            this.BlockHeading = blockHeading;
        }
        
        public string BlockHeading { get; set; }
        
        public List<string> instructionLines = new List<string>();

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

        public void GetInstructionBlockFromText(string instructionBlockText)
        {
            string headingStartMarker = "BLOCK_HEADING:";
            string endMarker = "LINE:";

            this.BlockHeading = DataHelperMethods.GetDataFromStartAndEndMarkers(instructionBlockText, headingStartMarker, endMarker);

            string[] instructionBlockLines = GetLinesForInstructionBlockFromText(instructionBlockText);

            foreach (string line in instructionBlockLines)
            {
                this.AddInstructionLine(line);
            }
        }

        private static string[] GetLinesForInstructionBlockFromText(string instructionBlockText)
        {
            string lineStartMarker = "LINE:";
            string endMarker = "-END_OF_INSTRUCTION_BLOCK-";
            string linesText = DataHelperMethods.GetDataFromStartAndEndMarkers(instructionBlockText, lineStartMarker, endMarker);

            string[] instructionLines = linesText.Split(lineStartMarker, StringSplitOptions.RemoveEmptyEntries);

            return instructionLines;
        }
    }
}
