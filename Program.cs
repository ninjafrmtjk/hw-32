using System.Threading;
using System.Threading.Tasks;


internal static class Program
{
    internal const int twoSecond = 2000;
    internal static async Task Main()
    {
        DoSomethingAsync();
        DoMultipleAsyncOperations();
        Console.ReadLine();
    }

    // 1. Пример асинхронного метода:
    internal static async Task<string> DoSomethingAsync()
    {
        return await Task.Run(() =>
        {
            Console.WriteLine("...");
            Thread.Sleep(twoSecond);

            // Возвращаем результат
            return "Результат выполнения операции";
        });
    }

    // 2. Пример цепочки из нескольких асинхронных операций с использованием async и await:

    internal static async Task<string> DoSomethingElseAsync(string str)
    {
        return "Some Stuff";
    }

    internal static async Task<string> DoAnotherAsyncOperation(string str1, string str2)
    {
        return "Doing Another Async...";
    }

    internal static async Task<string> DoMultipleAsyncOperations()
    {
        // Выполняем первую асинхронную операцию и ждем ее завершения
        var result = await DoSomethingAsync();

        // Выполняем вторую асинхронную операцию и ждем ее завершения, используя результат из первой операции
        var result2 = await DoSomethingElseAsync(result);

        // Выполняем третью асинхронную операцию и ждем ее завершения, используя результаты из первой и второй операций
        var result3 = await DoAnotherAsyncOperation(result, result2);

        // Возвращаем конечный результат
        return result3;
    }

    // 3. Пример асинхронного метода, который выполняет несколько операций параллельно и возвращая результаты всех операций:
    internal static async Task<List<string>> DoMultipleAsyncOperationsInParallel()
    {
        //// Создаем список задач на асинхронное выполнение
        var tasks = new List<Task<string>>
        {
            DoSomethingAsync(),
            DoSomethingElseAsync("Hello"),
            DoAnotherAsyncOperation("Hello, ", "World")
        };

        // Ждем выполнения всех задач
        await Task.WhenAll(tasks);

        // Извлекаем результаты выполнения операций
        var results = tasks.Select(t => t.Result).ToList();

        // Возвращаем результаты всех операций
        return results;
    }

    // 4. Для добавления возможности отмены выполнения асинхронного метода в определенный момент времени, можно использовать CancellationToken. 
    // Для этого нужно передать CancellationToken в качестве параметра в метод, который выполняется асинхронно, и проверять его состояние внутри метода с помощью метода ThrowIfCancellationRequested. 

    internal static async Task MyMethodAsync(CancellationToken cancellationToken)
    {
        await Task.Delay(TimeSpan.FromSeconds(10), cancellationToken);
        cancellationToken.ThrowIfCancellationRequested();
        // дальнейшая логика метода


        // Чтобы отменить выполнение метода, нужно вызвать метод Cancel у CancellationTokenSource, который был использован для создания CancellationToken.
        var cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(5));
        await MyMethodAsync(cancellationTokenSource.Token);
        // В этом примере выполнение метода MyMethodAsync будет остановлено, если он не закончится за 5 секунд.
    }

    internal static async Task TaskAsync()
    {
        try
        {
            //асинхронная операция
            await Task.Delay(TimeSpan.FromSeconds(5));
        }
        catch (Exception ex)
        {
            //обработка исключения
        }
    }

    // 6. Для ожидания выполнения первой из нескольких асинхронных операций и получения результата этой операции можно использовать метод Task.WhenAny.

    internal static async Task MyMethodAsync1()
    {
        var task1 = Task.Delay(TimeSpan.FromSeconds(5));
        var task2 = Task.Delay(TimeSpan.FromSeconds(10));
        var completedTask = await Task.WhenAny(task1, task2);
        
        if (completedTask == task1) Console.WriteLine("Task1 - OK");
        else if (completedTask == task2) Console.WriteLine("Task2 - OK");
    }
}

