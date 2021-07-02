using System;
using System.Collections.Generic;
using System.Text;

namespace Recipe2ShoppingList
{
    public static class ManageCookingInstructions
    {
        public static void EditRecipeInstructions(IUserIO userIO, Recipe recipe)
        {
            string header = "---------- EDIT RECIPE ----------";
            string additionalMessage = UserInterface.MakeStringConsoleLengthLines($"Recipe being edited: {recipe.Metadata.Title}");
            UserInterface.DisplayMenuHeader(userIO, header, additionalMessage);

            userIO.DisplayDataLite(recipe.CookingInstructions.ProduceInstructionsText(true));
            userIO.DisplayData();

            List<string[]> menuOptions = new List<string[]>()
            {
                new string[] { "A", "Add New Instruction Block" },
                new string[] { "E", "Edit an Instruction Block"},
                new string[] { "D", "Delete an Instruction Block"},
                new string[] { "R", "Return to Previous Menu"},
            };
            List<string> options = new List<string>();

            if (recipe.CookingInstructions.InstructionBlocks.Count == 0)
            {
                menuOptions.RemoveAt(1);
                menuOptions.RemoveAt(1);
            }

            UserInterface.DisplayOptionsMenu(userIO, menuOptions, out options);
            userIO.DisplayData();
            userIO.DisplayDataLite("Select an editing option: ");
            string userOption = GetUserInput.GetUserOption(userIO, options);

            userIO.DisplayData();
            switch (userOption)
            {
                case "A":
                    AddNewInstructionBlock(userIO, recipe);
                    break;
                case "E":
                    EditExistingInstructionBlock(userIO, recipe);
                    break;
                case "D":
                    DeleteExistingInstructionBlock(userIO, recipe);
                    break;
                case "R":
                    return;
            }
        }

        public static void AddNewInstructionBlock(IUserIO userIO, Recipe recipe)
        {
            string header = "---------- EDIT RECIPE ----------";
            string additionalMessage = UserInterface.MakeStringConsoleLengthLines($"Recipe being edited: {recipe.Metadata.Title}");
            UserInterface.DisplayMenuHeader(userIO, header, additionalMessage);

            InstructionBlock newInstructionBlock = GetUserInput.GetInstructionBlockFromUser(userIO);

            GetUserInput.AreYouSure(userIO, "add this new instruction block", out bool isSure);

            if (isSure)
            {
                recipe.CookingInstructions.AddInstructionBlock(newInstructionBlock);
                UserInterface.SuccessfulChange(userIO, true, "new instruction block", "added");
            }
            else
            {
                UserInterface.SuccessfulChange(userIO, false, "new instruction block", "added");
            }
        }

        public static void EditExistingInstructionBlock(IUserIO userIO, Recipe recipe)
        {
            string header = "---------- EDIT RECIPE ----------";
            string additionalMessage = UserInterface.MakeStringConsoleLengthLines($"Recipe being edited: {recipe.Metadata.Title}");
            UserInterface.DisplayMenuHeader(userIO, header, additionalMessage);

            userIO.DisplayDataLite(recipe.CookingInstructions.ProduceInstructionsText(true, true));
            InstructionBlock instructionBlockToEdit;
            int numberOfInstructionBlocks = recipe.CookingInstructions.InstructionBlocks.Count;
            List<string> instructionBlockOptions = new List<string>();
            for (int i = 1; i <= numberOfInstructionBlocks; i++)
            {
                instructionBlockOptions.Add(i.ToString());
            }

            if (numberOfInstructionBlocks > 1)
            {
                userIO.DisplayData();
                userIO.DisplayData(UserInterface.MakeStringConsoleLengthLines("Enter the instruction block you would like to edit:"));

                string userBlockSelection = GetUserInput.GetUserOption(userIO, instructionBlockOptions);
                int instructionBlockIndex = int.Parse(userBlockSelection);

                instructionBlockToEdit = recipe.CookingInstructions.InstructionBlocks[instructionBlockIndex - 1];
            }
            else if (numberOfInstructionBlocks == 1)
            {
                instructionBlockToEdit = recipe.CookingInstructions.InstructionBlocks[0];
            }
            else
            {
                userIO.DisplayData(UserInterface.MakeStringConsoleLengthLines("This recipe does not have any instruction blocks. Add a new instruction block in order to edit the recipe."));
                userIO.DisplayData();
                userIO.DisplayData("Press \"Enter\" to continue...");
                userIO.GetInput();

                return;
            }

            List<string[]> editBlockMenuOptions = new List<string[]>()
            {
                new string[] { "", "Add Instruction Line" },
                new string[] { "", "Edit Instruction Line" },
                new string[] { "", "Delete Instruction Line" },
                new string[] { "", "Add Block Heading" },
                new string[] { "", "Edit Block Heading" },
                new string[] { "", "Delete Block Heading" },
            };
            List<string> editBlockOptions = new List<string>();

            bool instructionLinesAreBlank = instructionBlockToEdit.InstructionLines.Count == 0;
            bool blockHeadingIsBlank = instructionBlockToEdit.BlockHeading == "";

            if (instructionLinesAreBlank)
            {
                editBlockMenuOptions.RemoveAt(1);
                editBlockMenuOptions.RemoveAt(1);
            }

            if (blockHeadingIsBlank)
            {
                editBlockMenuOptions.RemoveAt(editBlockMenuOptions.Count - 1);
                editBlockMenuOptions.RemoveAt(editBlockMenuOptions.Count - 1);
            }

            if (!blockHeadingIsBlank)
            {
                editBlockMenuOptions.RemoveAt(editBlockMenuOptions.Count - 3);
            }

            for (int i = 0; i < editBlockMenuOptions.Count; i++)
            {
                editBlockMenuOptions[i][0] = (i + 1).ToString();
            }

            userIO.DisplayData();
            UserInterface.DisplayOptionsMenu(userIO, editBlockMenuOptions, out editBlockOptions);
            userIO.DisplayData();
            userIO.DisplayDataLite("Enter an editing option from the menu: ");
            string editBlockOption = GetUserInput.GetUserOption(userIO, editBlockOptions);
            string menuSelection = editBlockMenuOptions[int.Parse(editBlockOption) - 1][1];

            userIO.DisplayData();
            switch (menuSelection)
            {
                case "Add Instruction Line":
                    AddInstructionLine(userIO, instructionBlockToEdit, recipe);
                    break;
                case "Edit Instruction Line":
                    EditInstructionLine(userIO, instructionBlockToEdit, recipe);
                    break;
                case "Delete Instruction Line":
                    DeleteInstructionLine(userIO, instructionBlockToEdit, recipe);
                    break;
                case "Add Block Heading":
                    AddInstructionBlockHeading(userIO, instructionBlockToEdit, recipe);
                    break;
                case "Edit Block Heading":
                    EditInstructionBlockHeading(userIO, instructionBlockToEdit, recipe);
                    break;
                case "Delete Block Heading":
                    DeleteInstructionBlockHeading(userIO, instructionBlockToEdit, recipe);
                    break;
                default:
                    break;
            }
        }

        public static void AddInstructionLine(IUserIO userIO, InstructionBlock instructionBlock, Recipe recipe)
        {
            string header = "---------- EDIT RECIPE ----------";
            string additionalMessage = UserInterface.MakeStringConsoleLengthLines($"Recipe being edited: {recipe.Metadata.Title}");
            UserInterface.DisplayMenuHeader(userIO, header, additionalMessage);

            UserInterface.DisplayInstructionBlock(userIO, instructionBlock);

            userIO.DisplayData();
            userIO.DisplayData("Enter the new instruction line to add:");
            string newInstructionLine = GetUserInput.GetUserInputString(userIO, false, 360);

            instructionBlock.AddInstructionLine(newInstructionLine);
            UserInterface.SuccessfulChange(userIO, true, "new instruction line", "added");
        }

        public static void EditInstructionLine(IUserIO userIO, InstructionBlock instructionBlock, Recipe recipe)
        {
            string header = "---------- EDIT RECIPE ----------";
            string additionalMessage = UserInterface.MakeStringConsoleLengthLines($"Recipe being edited: {recipe.Metadata.Title}");
            UserInterface.DisplayMenuHeader(userIO, header, additionalMessage);

            UserInterface.DisplayInstructionBlock(userIO, instructionBlock);

            List<string> allInstructionLines = instructionBlock.InstructionLines;
            List<string> instructionLineOptions = new List<string>();

            for (int i = 1; i <= allInstructionLines.Count; i++)
            {
                instructionLineOptions.Add(i.ToString());
            }
            userIO.DisplayData();
            userIO.DisplayDataLite("Select the instruction line to edit: ");
            string instructionLineSelected = GetUserInput.GetUserOption(userIO, instructionLineOptions);

            userIO.DisplayData();
            userIO.DisplayData("Enter the new text for the instruction line:");
            string newInstructionLineText = GetUserInput.GetUserInputString(userIO, false, 360);

            instructionBlock.EditInstructionLine(int.Parse(instructionLineSelected) - 1, newInstructionLineText);
            UserInterface.SuccessfulChange(userIO, true, "instruction line", "edited");
        }

        public static void DeleteInstructionLine(IUserIO userIO, InstructionBlock instructionBlock, Recipe recipe)
        {
            string header = "---------- EDIT RECIPE ----------";
            string additionalMessage = UserInterface.MakeStringConsoleLengthLines($"Recipe being edited: {recipe.Metadata.Title}");
            UserInterface.DisplayMenuHeader(userIO, header, additionalMessage);

            UserInterface.DisplayInstructionBlock(userIO, instructionBlock);

            List<string> allInstructionLines = instructionBlock.InstructionLines;
            List<string> instructionLineOptions = new List<string>();

            for (int i = 1; i <= allInstructionLines.Count; i++)
            {
                instructionLineOptions.Add(i.ToString());
            }
            userIO.DisplayData();
            userIO.DisplayDataLite("Select the instruction line to delete: ");
            string instructionLineSelected = GetUserInput.GetUserOption(userIO, instructionLineOptions);

            GetUserInput.AreYouSure(userIO, "delete this instruction line", out bool isSure);

            if (isSure)
            {
                instructionBlock.DeleteInstructionLine(int.Parse(instructionLineSelected) - 1);
                UserInterface.SuccessfulChange(userIO, true, "instruction line", "deleted");
            }
            else
            {
                UserInterface.SuccessfulChange(userIO, false, "instruction line", "deleted");
            }
        }

        public static void AddInstructionBlockHeading(IUserIO userIO, InstructionBlock instructionBlock, Recipe recipe)
        {
            string header = "---------- EDIT RECIPE ----------";
            string additionalMessage = UserInterface.MakeStringConsoleLengthLines($"Recipe being edited: {recipe.Metadata.Title}");
            UserInterface.DisplayMenuHeader(userIO, header, additionalMessage);

            UserInterface.DisplayInstructionBlock(userIO, instructionBlock);

            userIO.DisplayData();
            userIO.DisplayData("Enter the new block heading to add:");
            string newBlockHeading = GetUserInput.GetUserInputString(userIO, false, 100);

            instructionBlock.BlockHeading = newBlockHeading;
            UserInterface.SuccessfulChange(userIO, true, "new block heading", "added");
        }

        public static void EditInstructionBlockHeading(IUserIO userIO, InstructionBlock instructionBlock, Recipe recipe)
        {
            string header = "---------- EDIT RECIPE ----------";
            string additionalMessage = UserInterface.MakeStringConsoleLengthLines($"Recipe being edited: {recipe.Metadata.Title}");
            UserInterface.DisplayMenuHeader(userIO, header, additionalMessage);

            UserInterface.DisplayInstructionBlock(userIO, instructionBlock);

            userIO.DisplayData();
            userIO.DisplayData("Enter the new block heading:");
            string newBlockHeading = GetUserInput.GetUserInputString(userIO, false, 100);

            instructionBlock.BlockHeading = newBlockHeading;
            UserInterface.SuccessfulChange(userIO, true, "block heading", "edited");
        }

        public static void DeleteInstructionBlockHeading(IUserIO userIO, InstructionBlock instructionBlock, Recipe recipe)
        {
            string header = "---------- EDIT RECIPE ----------";
            string additionalMessage = UserInterface.MakeStringConsoleLengthLines($"Recipe being edited: {recipe.Metadata.Title}");
            UserInterface.DisplayMenuHeader(userIO, header, additionalMessage);

            UserInterface.DisplayInstructionBlock(userIO, instructionBlock);

            GetUserInput.AreYouSure(userIO, "delete the block heading", out bool isSure);

            if (isSure)
            {
                instructionBlock.BlockHeading = "";
                UserInterface.SuccessfulChange(userIO, true, "block heading", "deleted");
            }
            else
            {
                UserInterface.SuccessfulChange(userIO, false, "block heading", "deleted");
            }
        }

        public static void DeleteExistingInstructionBlock(IUserIO userIO, Recipe recipe)
        {
            string header = "---------- EDIT RECIPE ----------";
            string additionalMessage = UserInterface.MakeStringConsoleLengthLines($"Recipe being edited: {recipe.Metadata.Title}");
            UserInterface.DisplayMenuHeader(userIO, header, additionalMessage);

            userIO.DisplayDataLite(recipe.CookingInstructions.ProduceInstructionsText(true, true));
            userIO.DisplayData();
            userIO.DisplayData(UserInterface.MakeStringConsoleLengthLines("Enter the instruction block you would like to delete:"));

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
                UserInterface.SuccessfulChange(userIO, true, "instruction block", "deleted");
            }
            else
            {
                UserInterface.SuccessfulChange(userIO, false, "instruction block", "deleted");
            }
        }
    }
}
