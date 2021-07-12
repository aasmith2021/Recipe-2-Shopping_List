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

        public int Id { get; set; }

        public string BlockHeading { get; set; }

        public List<string> InstructionLines { get; set; } = new List<string>();
        
        public void AddInstructionLine(string newInstructionLine)
        {
            InstructionLines.Add(newInstructionLine);
        }

        public void DeleteInstructionLine(int indexOfLineToDelete)
        {
            InstructionLines.RemoveAt(indexOfLineToDelete);
        }

        public void EditInstructionLine(int indexOfLineToEdit, string newInstructionLine)
        {
            InstructionLines[indexOfLineToEdit] = newInstructionLine;
        }

        public void GetInstructionBlockFromText(string instructionBlockText)
        {
            string idStartMarker = "INSTRUCTION_BLOCK_ID:";
            string headingStartMarker = "BLOCK_HEADING:";
            string endMarker = "LINE:";

            this.Id = Convert.ToInt32(FileIO.GetDataFromStartAndEndMarkers(instructionBlockText, idStartMarker, headingStartMarker));
            this.BlockHeading = FileIO.GetDataFromStartAndEndMarkers(instructionBlockText, headingStartMarker, endMarker);

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
            string linesText = FileIO.GetDataFromStartAndEndMarkers(instructionBlockText, lineStartMarker, endMarker);

            string[] instructionLines = linesText.Split(lineStartMarker, StringSplitOptions.RemoveEmptyEntries);

            return instructionLines;
        }
    }
}
