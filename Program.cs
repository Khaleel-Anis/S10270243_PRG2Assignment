namespace PRG_2_Assignment
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to Flight Information Display System");

            Terminal terminal = new Terminal("Changi Terminal 5");
            Console.WriteLine($"Terminal Name: {terminal.TerminalName}");
        }
    }
}
