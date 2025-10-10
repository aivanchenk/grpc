namespace Clients;

using NLog;
using Grpc.Net.Client;

//this comes from GRPC generated code
using Services;


/// <summary>
/// Client example.
/// </summary>
class Client
{
	/// <summary>
	/// A set of names to choose from.
	/// </summary>
	private readonly List<string> NAMES = 
		new List<string> { 
			"John", "Peter", "Jack", "Steve"
		};

	/// <summary>
	/// A set of surnames to choose from.
	/// </summary>
	private readonly List<string> SURNAMES = 
		new List<String> { 
			"Johnson", "Peterson", "Jackson", "Steveson" 
		};


	/// <summary>
	/// Logger for this class.
	/// </summary>
	Logger mLog = LogManager.GetCurrentClassLogger();

	/// <summary>
	/// Configures logging subsystem.
	/// </summary>
	private void ConfigureLogging()
	{
		var config = new NLog.Config.LoggingConfiguration();

		var console =
			new NLog.Targets.ConsoleTarget("console")
			{
				Layout = @"${date:format=HH\:mm\:ss}|${level}| ${message} ${exception}"
			};
		config.AddTarget(console);
		config.AddRuleForAllLevels(console);

		LogManager.Configuration = config;
	}

	/// <summary>
	/// Program body.
	/// </summary>
	private void Run() {
		//configure logging
		ConfigureLogging();

		//initialize random number generator
		var rnd = new Random();

		//run everythin in a loop to recover from connection errors
		 while( true )
        {
            try {
                //connect to the server, get service proxy
                using var channel = GrpcChannel.ForAddress("http://127.0.0.1:5000");
                var client = new Farm.FarmClient(channel);

                // send food (double amount) - synchronous/blocking call (no async/await)
                var request = new SubmitRequest { Amount = 21.5 };
                var reply = client.SubmitWater(request);

                Console.WriteLine($"Accepted: {reply.IsAccepted}, Reason: {reply.FailReason}");
            }
            catch( Exception e )
            {
                //log whatever exception to console
                mLog.Warn(e, "Unhandled exception caught. Will restart main loop.");

                //prevent console spamming
                Thread.Sleep(2000);
            }
        }
	}

	/// <summary>
	/// Program entry point.
	/// </summary>
	/// <param name="args">Command line arguments.</param>
	static void Main(string[] args)
	{
		var self = new Client();
		self.Run();
	}
}