using DSharpPlus.EventArgs;
using DSharpPlus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSharpPlus.Entities;

namespace DarkBot.src.Common
{
    public static class ButtonPaginator
    {
        public static DiscordActionRowComponent[] BuildNavigationButtons(int currentPage, int totalPages)
        {
            var buttons = new List<DiscordButtonComponent>();

            // Vorherige Seite Button
            var previousButton = new DiscordButtonComponent(ButtonStyle.Primary, "previous", "Previous Page", false, null);
            buttons.Add(previousButton);

            // Nächste Seite Button
            var nextButton = new DiscordButtonComponent(ButtonStyle.Primary, "next", "Next Page", false, null);
            buttons.Add(nextButton);

            // Aktuelle Seite Anzeige
            var currentPageButton = new DiscordButtonComponent(ButtonStyle.Secondary, "current", $"Page {currentPage + 1}/{totalPages}", true, null);
            buttons.Add(currentPageButton);

            // In eine Zeile einfügen
            var actionRow = new DiscordActionRowComponent(buttons.ToArray());

            return new DiscordActionRowComponent[] { actionRow };
        }

        public static bool TryNavigate(ComponentInteractionCreateEventArgs interaction, ref int currentPageIndex, int totalPages)
        {
            if (interaction == null || interaction.Message == null || interaction.User == null)
                return false;

            var buttonId = interaction.Id;

            if (buttonId == "previous")
            {
                currentPageIndex = Math.Max(currentPageIndex - 1, 0);
                return true;
            }
            else if (buttonId == "next")
            {
                currentPageIndex = Math.Min(currentPageIndex + 1, totalPages - 1);
                return true;
            }

            return false;
        }
    }
}
