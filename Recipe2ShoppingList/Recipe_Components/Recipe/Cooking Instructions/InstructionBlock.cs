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

        //Creates a string of an instruction block's text. When printVersion is true, the output is meant
        //to be displayed directly to the user on the console. When it is false, the output is meant to be
        //written to the database file so it can be parsed and loaded back into the program later.
        public void GetInstructionBlockFromText(string instructionBlockText)
        {
            string idStartMarker = "INSTRUCTION_BLOCK_ID:";
            string headingStartMarker = "BLOCK_HEADING:";
            string endMarker = "LINE:";

            //Default the ID to 1 if there is no ID saved in the file data
            int parsedId = 1;
            int.TryParse(FileIO.GetDataFromStartAndEndMarkers(instructionBlockText, idStartMarker, headingStartMarker), out parsedId);
            this.Id = parsedId;
            this.BlockHeading = FileIO.GetDataFromStartAndEndMarkers(instructionBlockText, headingStartMarker, endMarker);

            string[] instructionBlockLines = GetLinesForInstructionBlockFromText(instructionBlockText);

            foreach (string line in instructionBlockLines)
            {
                this.AddInstructionLine(line);
            }
        }

        //Used to parse the individual instruction lines from a text file to add the instruction lines to an instruction block in the program.
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
