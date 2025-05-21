using System;

public class Rndm_Gen
{
	private int probabilidad;

	private int process_Identifier;

	

	public Rndm_Gen()
	{
		probabilidad = 50;
		process_Identifier = 1;
	}

	public Proceso getNewProcess(int minSize, int maxSize)
	{

		if (maxSize == 0) {  maxSize = 15; } // Asegurarse de que el tamaño maximo no sea menor que 1
		if (minSize < 0) { minSize = 1; } // Asegurarse de que el tamaño minimo no sea menor que 1

        Random rndm = new Random();

        int size = rndm.Next(minSize,maxSize);

		int priority = rndm.Next(1,6);

		int number = rndm.Next(1,100);

		if(number <= probabilidad)
		{
            return new Proceso(process_Identifier++ , size, 0, priority);

		} else
		{
			System.Console.WriteLine("No se creo un proceso nuevo.");
		}
		return null;

		
	}
}
