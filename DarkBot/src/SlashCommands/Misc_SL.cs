using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using DSharpPlus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext.Attributes;
using Newtonsoft.Json;

namespace DarkBot.src.SlashCommands
{
    public class Misc_SL : ApplicationCommandModule
    {
        private const string JsonFilePath = "user_data.json";

        [SlashCommand("show-table", "Displays a table")]
        public async Task ShowTableAsync(InteractionContext ctx)
        {
            // Erstelle eine Discord-Nachricht mit einer Tabelle als Embed
            var embedBuilder = new DiscordEmbedBuilder()
                .WithTitle("Example Table")
                .WithColor(DiscordColor.Green)
                .AddField("Column 1", "Value 1")
                .AddField("Column 2", "Value 2")
                .AddField("Column 3", "Value 3");

            // Sende die Nachricht mit der Tabelle als Antwort
            await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource,
                new DiscordInteractionResponseBuilder().AddEmbed(embedBuilder.Build()));
        }

        [SlashCommand("avatar", "Zeigt die Avatar-URL eines Users an")]
        [Aliases("profilbild")]
        public async Task AvatarCommand(InteractionContext ctx, [Option("user", "Der User, dessen Avatar angezeigt werden soll")] DiscordUser user = null)
        {
            var targetUser = user ?? ctx.User;

            var avatarUrl = targetUser.AvatarUrl;

            var embed = new DiscordEmbedBuilder
            {
                Title = $"{targetUser.Username}'s Avatar",
                ImageUrl = avatarUrl,
                Color = DiscordColor.HotPink,
                Description = ctx.User.AvatarUrl,
            };

            await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder().AddEmbed(embed.Build()));
        }

        [SlashCommand("server", "Zeigt Informationen zum Server an")]
        [Aliases("info", "serverinfo")]
        public async Task ServerEmbed(InteractionContext ctx)
        {
            string serverDescription = $"**Servername:** {ctx.Guild.Name}\n" +
                                        $"**Server ID:** {ctx.Guild.Id}\n" +
                                        $"**Erstellt am:** {ctx.Guild.CreationTimestamp:dd/M/yyyy}\n" +
                                        $"**Owner:** {ctx.Guild.Owner.Mention}\n\n" +
                                        $"**Users:** {ctx.Guild.MemberCount}\n" +
                                        $"**Channels:** {ctx.Guild.Channels.Count}\n" +
                                        $"**Rollen:** {ctx.Guild.Roles.Count}\n" +
                                        $"**Emojis:** {ctx.Guild.Emojis.Count}";

            var serverInformation = new DiscordEmbedBuilder
            {
                Color = DiscordColor.Gold,
                Title = "Server Informationen",
                Description = serverDescription
            };

            var response = new DiscordInteractionResponseBuilder().AddEmbed(serverInformation.WithImageUrl(ctx.Guild.IconUrl));
            await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, response);
        }

        [SlashCommand("currencytable", "Show Currency")]
        public async Task StatsCommand(InteractionContext ctx)
        {
            if (!ctx.Member.Roles.Any(r => r.Name == "Techniker"))
            {
                await ctx.CreateResponseAsync("Du hast nicht die erforderliche Rolle, um diesen Befehl auszuführen.");
                return;
            }

            UserData[] allUsers = ReadAllUsers();

            var embed = new DiscordEmbedBuilder
            {
                Title = "List of Turkish Prices",
                Color = DiscordColor.Green
            };

            var sb = new StringBuilder();

            sb.AppendLine("```md");
            sb.AppendLine("# Username        | Gesammelt  | Abgabe  | Kurs  | Betrag");
            sb.AppendLine("--------------------------------------------------------");

            foreach (UserData user in allUsers)
            {
                decimal abgabe = user.Legendentitel ? 0 : GlobalAbgabe;
                string legendentitelMarkierung = user.Legendentitel ? "*" : "";
                decimal betrag = (user.Gesammelt - (user.Legendentitel ? 0 : abgabe)) * GlobalKurs;

                sb.AppendLine($"  {user.Name + legendentitelMarkierung,-14} | {user.Gesammelt,-10:F0} | {abgabe,-7} | {GlobalKurs,-5} | {betrag,-7:F0}");
            }

            sb.AppendLine("```");

            embed.Description = sb.ToString();

            await ctx.CreateResponseAsync(embed: embed);
        }

        private UserData[] ReadAllUsers()
        {
            if (File.Exists(JsonFilePath))
            {
                string json = File.ReadAllText(JsonFilePath);
                return JsonConvert.DeserializeObject<UserData[]>(json);
            }

            return new UserData[0];
        }
    }
    public class UserData
    {
        public string Name { get; set; }
        public decimal Gesammelt { get; set; }
        public decimal Betrag { get; set; }
        public bool Legendentitel { get; set; }
        public bool Checked { get; set; }
    }

    
}
