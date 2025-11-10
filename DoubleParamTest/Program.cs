namespace DoubleParamTest;

internal abstract class Program
{
    private static void Main()
    {
        var test = new Test();
        test.Method1("Test");
        Console.WriteLine(test.Method2(1,1));
        Console.WriteLine(test.Method3(1));
        Console.WriteLine(test.Method4(1));
        Console.WriteLine(test.Method5("Test"));
    }
}