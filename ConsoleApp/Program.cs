// See https://aka.ms/new-console-template for more information

var elementsPerProcces = 10;

MPI.Environment.Run(ref args, communicator =>
{
    if (communicator.Rank is 0)
    {
        int[][] array = new int[communicator.Size][];
        var random = new Random();

        for (int i = 0; i < communicator.Size; i++)
        {
            array[i] = new int[elementsPerProcces];

            for (int j = 0; j < elementsPerProcces; j++)
                array[i][j] = random.Next(0, 10);
        }

        var subArray = communicator.Scatter(array);
        var subArraySum = subArray.Sum();
        Console.WriteLine($"Processor with rank {communicator.Rank} counted sum value {subArraySum}");
        var totalSum = communicator.Reduce(subArraySum, MPI.Operation<int>.Add, 0);
        Console.WriteLine($"Sum elements of array is {totalSum}");
    }
    else
    {
        var subArray = communicator.Scatter<int[]>(0);
        var subArraySum = subArray.Sum();
        Console.WriteLine($"Processor with rank {communicator.Rank} counted sum value {subArraySum}");
        communicator.Reduce(subArraySum, MPI.Operation<int>.Add, 0);
    }
});