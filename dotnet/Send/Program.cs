namespace Send;

class Program
{
    public static async Task Main(string[] args)
    {
        Console.WriteLine("RPC client");

        string n = args.Length > 0 ? args[0] : "30";

        await InvokeAsync(n);

    }

    private static async Task InvokeAsync(string n)
    {
        RpcClient rpcClient = new RpcClient();

        Console.WriteLine("Requesting fib {0}", n);

        var response = await rpcClient.CallAsync(n);

        Console.WriteLine(" [.] Got '{0}'", response);
    }
}
