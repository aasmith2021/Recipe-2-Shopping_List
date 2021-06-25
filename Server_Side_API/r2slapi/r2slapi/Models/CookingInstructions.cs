using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace r2slapi.Models
{
    public class CookingInstructions
    {       
        public CookingInstructions()
        {

        }

        public int Id { get; set; }

        [Required(ErrorMessage = "Recipe Cooking Instructions must have at least 1 instruction block.")]
        [MaxLength(5, ErrorMessage = "Recipe Cooking Instructions cannot have more than 5 instruction blocks.")]
        public List<InstructionBlock> InstructionBlocks { get; set; } =  new List<InstructionBlock>();

        public void AddInstructionBlock(InstructionBlock newInstructionBlock)
        {
            InstructionBlocks.Add(newInstructionBlock);
        }

        public void DeleteInstructionBlock(int indexOfBlockToDelete)
        {
            InstructionBlocks.RemoveAt(indexOfBlockToDelete);
        }
    }
}
