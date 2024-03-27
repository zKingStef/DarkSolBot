using DarkBot.src.Handler;
using DarkBot.src.SlashCommands;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using DSharpPlus.Interactivity;
using DSharpPlus.Interactivity.Enums;
using DSharpPlus.Interactivity.Extensions;
using DSharpPlus.Lavalink;
using DSharpPlus.SlashCommands;
using DSharpPlus.VoiceNext;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace DarkBot
{
    internal sealed class DarkBot
    {
        private static DiscordClient Client { get; set; }
        private IServiceProvider Services { get; }
        private static EventId EventId { get; } = new (1000, Program.Settings.Name);
        private CommandsNextExtension Commands { get; }
        private InteractivityExtension Interactivity { get; }
        private VoiceNextExtension Voice { get; }
        private LavalinkExtension Lavalink { get; }
        private SlashCommandsExtension Slash { get; }

        public DarkBot(int shardId = 0)
        {
            // Get Settings
            var settings = Program.Settings;

            Client = new DiscordClient(new DiscordConfiguration
            {
                Token = settings.Tokens.DiscordToken,
                TokenType = TokenType.Bot,
                AutoReconnect = true,
                ReconnectIndefinitely = true,
                MinimumLogLevel = LogLevel.Information,
                GatewayCompressionLevel = GatewayCompressionLevel.Stream,
                LargeThreshold = 250,
                MessageCacheSize = 2048,
                LogTimestampFormat = "yyyy-MM-dd HH:mm:ss zzz",
                ShardId = shardId,
                ShardCount = settings.ShardCount
            });


            // Setup Services
            Services = new ServiceCollection()
                //.AddSingleton<MusicService>()
                //.AddSingleton(new LavalinkService(Client))
                .AddSingleton(this)
                .BuildServiceProvider(true);

            // Setup Commands
            Commands = Client.UseCommandsNext(new CommandsNextConfiguration
            {
                CaseSensitive = false,
                IgnoreExtraArguments = true,
                Services = Services,
                PrefixResolver = PrefixResolverAsync, // Set the command prefix that will be used by the bot
                EnableMentionPrefix = true,
                EnableDms = true,
                EnableDefaultHelp = true,
            });
            //Commands.CommandExecuted += Command_Executed;
            //Commands.CommandErrored += Command_Errored;
            //Commands.SetHelpFormatter<HelpFormatter>();

            //4. Set the default timeout for Commands that use interactivity
            Interactivity = Client.UseInteractivity(new InteractivityConfiguration
            {
                PaginationBehaviour = PaginationBehaviour.Ignore,
                Timeout = TimeSpan.FromSeconds(30)
            });

            Voice = Client.UseVoiceNext(new VoiceNextConfiguration
            {
                AudioFormat = AudioFormat.Default,
                EnableIncoming = true
            });

            // Register Slash/Prefix Commands
            RegisterSlashCommands(Client);

            Client.ComponentInteractionCreated += UserInteractionHandler;

            // Start the uptime counter
            Console.Title = $"{settings.Name}-{settings.Version}";
            settings.ProcessStarted = DateTime.Now;
        }

        private static async Task UserInteractionHandler(DiscordClient sender, DSharpPlus.EventArgs.ComponentInteractionCreateEventArgs e)
        {
            await UserInteraction_Handler.RespondToInteraction(e);
        }

        private static void RegisterSlashCommands(DiscordClient Client)
        {
            var slash = Client.UseSlashCommands();

            slash.RegisterCommands<Troll_SL>(); // 1076192773776081029 GuildID
            slash.RegisterCommands<Moderation_SL>();
            slash.RegisterCommands<Poll_SL>();
            //slash.RegisterCommands<BasicSL>();
            slash.RegisterCommands<Ticket_SL>();
            //slash.RegisterCommands<GiveawaySL>();
            //slash.RegisterCommands<CalculatorSL>();
            //slash.RegisterCommands<ImageSL>();
            //slash.RegisterCommands<CasinoSL>();
            //slash.RegisterCommands<MusicBotSL>();
            slash.RegisterCommands<AutoRole_SL>();
            slash.RegisterCommands<MiniGame_SL>();
            //slash.SlashCommandErrored += SlashCommand_Errored;
        }

        public async Task RunAsync()
        {
            // Update any other services that are being used.
            Client.Logger.LogInformation(EventId, "Bot wird gestartet...");

            // Set the initial activity and connect the bot to Discord
            var act = new DiscordActivity("zKingStef", ActivityType.ListeningTo);
            await Client.ConnectAsync(act, UserStatus.DoNotDisturb).ConfigureAwait(false);
        }

        public async Task StopAsync()
        {
            await Client.DisconnectAsync().ConfigureAwait(false);
        }

        private static Task<int> PrefixResolverAsync(DiscordMessage m)
        {
            return Task.FromResult(m.GetStringPrefixLength(Program.Settings.Prefix));
        }
    }
}
