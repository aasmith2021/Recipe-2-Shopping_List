using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace r2slapi.Models
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

        [StringLength(100, MinimumLength = 0, ErrorMessage = "Block heading cannot be null, and cannot exceed 100 characters.")]
        public string BlockHeading { get; set; }

        [MaxLength(20, ErrorMessage = "An instruction block cannot have more than 20 instruction lines.")]
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
    }
}
