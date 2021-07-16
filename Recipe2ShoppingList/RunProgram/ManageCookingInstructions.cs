using System;
using System.Collections.Generic;
using System.Text;

namespace Recipe2ShoppingList
{
    public static class ManageCookingInstructions
    {
        public const string editRecipeBanner = "---------- EDIT RECIPE ----------";

        //Prompts user and captures input to add a new instruction block to a recipe
        public static void AddNewInstructionBlock(IUserIO userIO, Recipe recipe)
        {
            UserInterface.DisplayMenuHeader(userIO, editRecipeBanner, UserInterface.MakeStringConsoleLengthLines($"Recipe being edited: {recipe.Metadata.Title}"));

            InstructionBlock newInstructionBlock = GetUserInput.GetInstructionBlockFromUser(userIO);

            GetUserInput.AreYouSure(userIO, "add this new instruction block", out bool isSure);

            if (isSure)
            {
                recipe.CookingInstructions.AddInstructionBlock(newInstructionBlock);
            }

            UserInterface.DisplaySuccessfulChangeMessage(userIO, isSure, "new instruction block", "added");
        }

        //Prompts user and captures input to add a new instruction line to an instruction block
        public static void AddInstructionLine(IUserIO userIO, InstructionBlock instructionBlock, Recipe recipe)
        {
            UserInterface.DisplayMenuHeader(userIO, editRecipeBanner, UserInterface.MakeStringConsoleLengthLines($"Recipe being edited: {recipe.Metadata.Title}"));

            UserInterface.DisplayInstructionBlock(userIO, instructionBlock);

            UserInterface.DisplayRegularPrompt(userIO, "Enter the new instruction line to add");
            string newInstructionLine = GetUserInput.GetInstructionLineFromUser(userIO);

            instructionBlock.AddInstructionLine(newInstructionLine);
            UserInterface.SuccessfulChange(userIO, true, "new instruction line", "added");
        }

        //Prompts user and captures input to edit an instruction line in an instruction block
        public static void EditInstructionLine(IUserIO userIO, InstructionBlock instructionBlock, Recipe recipe)
        {
            UserInterface.DisplayMenuHeader(userIO, editRecipeBanner, UserInterface.MakeStringConsoleLengthLines($"Recipe being edited: {recipe.Metadata.Title}"));

            UserInterface.DisplayInstructionBlock(userIO, instructionBlock);

            List<string> allInstructionLines = instructionBlock.InstructionLines;
            List<string> instructionLineOptions = new List<string>();

            for (int i = 1; i <= allInstructionLines.Count; i++)
            {
                instructionLineOptions.Add(i.ToString());
            }
            UserInterface.DisplayLitePrompt(userIO, "Select the instruction line to edit");
            string instructionLineSelected = GetUserInput.GetUserOption(userIO, instructionLineOptions);

            UserInterface.DisplayRegularPrompt(userIO, "Enter the new text for the instruction line");
            string newInstructionLineText = GetUserInput.GetInstructionLineFromUser(userIO);

            instructionBlock.EditInstructionLine(int.Parse(instructionLineSelected) - 1, newInstructionLineText);
            UserInterface.SuccessfulChange(userIO, true, "instruction line", "edited");
        }

        //Prompts user and captures input to delete an instruction line in an instruction block
        public static void DeleteInstructionLine(IUserIO userIO, InstructionBlock instructionBlock, Recipe recipe)
        {
            UserInterface.DisplayMenuHeader(userIO, editRecipeBanner, UserInterface.MakeStringConsoleLengthLines($"Recipe being edited: {recipe.Metadata.Title}"));

            UserInterface.DisplayInstructionBlock(userIO, instructionBlock);

            List<string> allInstructionLines = instructionBlock.InstructionLines;
            List<string> instructionLineOptions = new List<string>();

            for (int i = 1; i <= allInstructionLines.Count; i++)
            {
                instructionLineOptions.Add(i.ToString());
            }
            UserInterface.DisplayLitePrompt(userIO, "Select the instruction line to delete");
            string instructionLineSelected = GetUserInput.GetUserOption(userIO, instructionLineOptions);

            GetUserInput.AreYouSure(userIO, "delete this instruction line", out bool isSure);

            if (isSure)
            {
                instructionBlock.DeleteInstructionLine(int.Parse(instructionLineSelected) - 1);
            }

            UserInterface.DisplaySuccessfulChangeMessage(userIO, isSure, "instruction line", "deleted");
        }

        //Prompts user and captures input to add an instruction block heading to an instruction block
        public static void AddInstructionBlockHeading(IUserIO userIO, InstructionBlock instructionBlock, Recipe recipe)
        {
            UserInterface.DisplayMenuHeader(userIO, editRecipeBanner, UserInterface.MakeStringConsoleLengthLines($"Recipe being edited: {recipe.Metadata.Title}"));

            UserInterface.DisplayInstructionBlock(userIO, instructionBlock);

            UserInterface.DisplayRegularPrompt(userIO, "Enter the new block heading to add");
            string newBlockHeading = GetUserInput.GetBlockHeadingFromUser(userIO);

            instructionBlock.BlockHeading = newBlockHeading;
            UserInterface.SuccessfulChange(userIO, true, "new block heading", "added");
        }

        //Prompts user and captures input to edit an instruction block heading
        public static void EditInstructionBlockHeading(IUserIO userIO, InstructionBlock instructionBlock, Recipe recipe)
        {
            UserInterface.DisplayMenuHeader(userIO, editRecipeBanner, UserInterface.MakeStringConsoleLengthLines($"Recipe being edited: {recipe.Metadata.Title}"));

            UserInterface.DisplayInstructionBlock(userIO, instructionBlock);

            UserInterface.DisplayRegularPrompt(userIO, "Enter the new block heading");
            string newBlockHeading = GetUserInput.GetBlockHeadingFromUser(userIO);

            instructionBlock.BlockHeading = newBlockHeading;
            UserInterface.SuccessfulChange(userIO, true, "block heading", "edited");
        }

        //Prompts user and captures input to delete an instruction block heading
        public static void DeleteInstructionBlockHeading(IUserIO userIO, InstructionBlock instructionBlock, Recipe recipe)
        {
            UserInterface.DisplayMenuHeader(userIO, editRecipeBanner, UserInterface.MakeStringConsoleLengthLines($"Recipe being edited: {recipe.Metadata.Title}"));

            UserInterface.DisplayInstructionBlock(userIO, instructionBlock);

            GetUserInput.AreYouSure(userIO, "delete the block heading", out bool isSure);

            if (isSure)
            {
                instructionBlock.BlockHeading = "";
            }

            UserInterface.DisplaySuccessfulChangeMessage(userIO, isSure, "block heading", "deleted");
        }

        //Prompts user and captures input to delete an existing instruction block
        public static void DeleteExistingInstructionBlock(IUserIO userIO, Recipe recipe)
        {
            UserInterface.DisplayMenuHeader(userIO, editRecipeBanner, UserInterface.MakeStringConsoleLengthLines($"Recipe being edited: {recipe.Metadata.Title}"));

            UserInterface.DisplayInformation(userIO, recipe.CookingInstructions.ProduceInstructionsText(true, true), false);
            UserInterface.DisplayRegularPrompt(userIO, UserInterface.MakeStringConsoleLengthLines("Enter the instruction block you would like to delete"));

            int numberOfInstructionBlocks = recipe.CookingInstructions.InstructionBlocks.Count;
            List<string> instructionBlockOptions = new List<string>();
            for (int i = 1; i <= numberOfInstructionBlocks; i++)
            {
                instructionBlockOptions.Add(i.ToString());
            }

            string userBlockSelection = GetUserInput.GetUserOption(userIO, instructionBlockOptions);

            GetUserInput.AreYouSure(userIO, "delete this instruction block", out bool isSure);

            if (isSure)
            {
                recipe.CookingInstructions.DeleteInstructionBlock(int.Parse(userBlockSelection) - 1);
            }

            UserInterface.DisplaySuccessfulChangeMessage(userIO, isSure, "instruction block", "deleted");
        }
    }
}
