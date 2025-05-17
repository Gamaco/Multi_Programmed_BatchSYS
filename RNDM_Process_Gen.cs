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

	public Proceso getNewProcess()
	{
		Random rndm = new Random();

		int size= rndm.Next(1,15);

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
