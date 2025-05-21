using Projecto_2_19_2025;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TrackBar;

public class OS
{

    //CONSTANTES
    const int MAX_PROCESSES_IN_READY = 5;
    private readonly object cpuLock;

    // Variables
    int processMaxSize = 15;
    int processMinSize = 1;
    private int maxBlockTicks = 5;  // tiempo máximo que un proceso puede estar bloqueado (configurable desde la GUI)

    // Listas
    List<Proceso> lstREADY;
    List<Proceso> newProcesses;
    List<Proceso> finished;
    List<Proceso> pcb;
    List<(Proceso, int)> blocked;
    Rndm_Gen rpg;
    List<CPU> availableCPUs;

    Proceso priority;

    public OS()
    {
        newProcesses = new List<Proceso>();

        lstREADY = new List<Proceso>();

        finished = new List<Proceso>();

        rpg = new Rndm_Gen();

        pcb = new List<Proceso>();

        blocked = new List<(Proceso, int)>();

        availableCPUs = new List<CPU>();

        cpuLock = new object();

        SetAvailableCPUs(1); // Inicializa CPUs disponibles, por defecto 1 CPU.

        Console.WriteLine("\nSistema Operativo: LilFattyOS");
    }


    /**
    * Este es basicamente el Main de la aplicacion.
    * 
    * Se llama cada vez que el reloj hace "tick".
    * Se encarga de crear nuevos procesos, moverlos a READY,
    * ejecutar el proceso en el CPU y mover procesos de BLOCKED
    * a READY.
    */
    public void ClockTick()
    {
        this.createNewProcess();  // Crea un nuevo proceso
        this.moveNewToready();   // Intenta mover procesos nuevos a READY
        this.moveNewToready();  // (se hace doble para llenar hasta 5 procesos)
        this.MoveReadyToRunning();   // Si hay un CPU disponible, mueve un proceso de READY al CPU
        this.executeProcess();  // Ejecuta un ciclo del proceso cargado
        this.removeExecutingProcess(); // Si el proceso en el CPU termino, lo mueve a FINISHED
        this.updateBlocked(); // Actualiza los procesos bloqueados

        if (new Random().Next(0, 4) == 0) // 25% de probabilidad
        {
            moveRandomToBlocked();
        }

    }


    /**
     * Cambia el quantum de cada CPU. (Tiempo maximo de ejecucion de un proceso)
     * @param quantumTime - El nuevo quantum que se le quiere asignar a cada CPU.
     */
    public void updateQuantumTime(int quantumTime)
    {
        foreach (CPU cpu in availableCPUs)
        {
            cpu.setQuantumTime(quantumTime); // Cambia el quantum de cada CPU
        }

        System.Console.WriteLine("\nNueva cantidad de tiempo de ejecucion de un proceso en el CPU (Quantum): " + quantumTime);
    }

    /**
     * Setea la cantidad de CPUs disponibles.
     * Este metodo es "thread safe". El proposito del "lock" es pa' que la variable 
     * availableCPUs se pueda leer por un thread/hilo a la vez y evitar que se corrompa.
     * @param amountOfCPUs - Cantidad de CPUs que se quieren crear.
     */
    public void SetAvailableCPUs(int amountOfCPUs)
    {
        List<CPU> newCpuList = new List<CPU>(); // Reinicia la lista de CPUs disponibles

        // Añade todos los procesos nuevamente a la lista de ready
        foreach (CPU cpu in availableCPUs)
        {
            if (cpu.isExecutingAProcess())
            {
                lstREADY.Add(cpu.getExecutingProcess());
            }
        }

        // Crea una nueva lista de CPUs disponibles con el nuevo tamaño.
        for (int i = 0; i < amountOfCPUs; i++)
        {
            newCpuList.Add(new CPU(i + 1)); // El parametro es el ID del CPU.
        }

        // Remplaza la lista de CPUs disponibles por la nueva lista.
        lock (cpuLock)
        {
            availableCPUs = newCpuList;
        }
        
        System.Console.WriteLine("\nNueva cantidad de CPUs: " + amountOfCPUs);

    }

    public CPU GetAvailableCPU()
    {
        foreach (CPU cpu in availableCPUs)
        {
            if (cpu.isExecutingAProcess() == false)
            {
                return cpu;
            }
        }
        return null;
    }

    public List<CPU> ListAllCPUs()
    {
        return availableCPUs;
    }   

    public List<Proceso> getNewProcesses()
    {
        return newProcesses;
    }

    public void createNewProcess()
    {
        Proceso newp = rpg.getNewProcess(processMinSize, processMaxSize);  // Genera un nuevo proceso

        //si se numero un proceso nuevo se guarda en el newProcesses//

        if (newp != null)
        {
            newProcesses.Add(newp);  // Si es válido, lo agrega a la lista de nuevos

            pcb.Add(newp);

            System.Console.WriteLine("Se creo un nuevo proceso: " + newp.ToString());
        }
    }


    public void executeProcess()
    {
        foreach (CPU cpu in availableCPUs) // Itera (hace un loop) sobre cada CPU disponible
        {
            if (cpu.isExecutingAProcess())
            {
                cpu.execute(); // Llama al metodo "execute" de la CPU para ejecutar el proceso
            }
            
        }
    }

    public void removeExecutingProcess()
    {
        foreach (CPU cpu in availableCPUs) // Itera sobre cada CPU disponible
        {
            if (cpu.isExecutingAProcess())
            {
                if (cpu.isExecutingProcessFinished())
                {
                    Proceso process = cpu.getExecutingProcess();  // Si terminó
                    finished.Add(process);
                    cpu.unloadProcess();
                }
                else if (cpu.hasQuantumExpired())  //5 
                {
                    // Si el quantum terminó pero el proceso aún no, se mueve de vuelta a READY
                    Proceso p = cpu.getExecutingProcess();
                    cpu.unloadProcess();

                    p.setState(Proceso.States.READY);
                    lstREADY.Add(p);
                    lstREADY = lstREADY.OrderBy(pro => pro.getPriority()).ToList(); // Se respeta la prioridad
                }

            }
        }
    }

    private void moveNewToready()
    {
        if (lstREADY.Count >= MAX_PROCESSES_IN_READY || newProcesses.Count == 0)
            return;

        Proceso proceso_temp = newProcesses.First();   // Toma el primer nuevo proceso
        proceso_temp.setState(Proceso.States.READY);   // Cambia su estado a READY
        lstREADY.Add(proceso_temp);                    // Lo mueve a la lista READY
        lstREADY = lstREADY.OrderBy(proceso => proceso.getPriority()).ToList();  // Ordena por prioridad
        newProcesses.RemoveAt(0);                      // Lo elimina de la lista de nuevos
    }

    private void moveRandomToBlocked()
    {
        if (lstREADY.Count == 0) return;

        Random r = new Random();
        int index = r.Next(lstREADY.Count);
        Proceso p = lstREADY[index];
        lstREADY.RemoveAt(index);
        p.setState(Proceso.States.BLOCKED);

        int ticks = r.Next(1, maxBlockTicks + 1);
        //int ticks = r.Next(2, 6); // bloqueado por 2-5 ticks
        blocked.Add((p, ticks));

        System.Console.WriteLine("Se movio proceso a bloqueado: " + p.ToString());
    }

    private void updateBlocked()
    {
        for (int i = blocked.Count - 1; i >= 0; i--)
        {
            var (p, t) = blocked[i];
            t--;

            if (t <= 0)
            {
                p.setState(Proceso.States.READY);
                lstREADY.Add(p);
                blocked.RemoveAt(i);
            }
            else
            {
                blocked[i] = (p, t);
            }
        }
    }

    public void setMaxBlockTick(int ticks)  // permite actualizar el tiempo máximo de bloqueo desde el formulario GUI
    { 
      this.maxBlockTicks = ticks;
      Console.WriteLine("Nuevo tirmpo maximo de bloqueoa:" + ticks);
    }

    public List<Proceso> getBlocked()  // método para acceder a los procesos bloqueados desde el PCB
    {
        return blocked.Select(b => b.Item1).ToList();
    }


    /**
     * Aqui se le asigna un proceso a un CPU disponible y
     * se mueve el proceso de ready a running.
     */
    private void MoveReadyToRunning()  //6
    {
        // Si no hay procesos en READY o no hay CPUs disponibles, no hace nada
        if (lstREADY.Count == 0 || this.GetAvailableCPU() == null)
            return;

        Proceso proceso = lstREADY.ElementAt(0); // Toma el primer proceso de la lista READY

        CPU cpu = this.GetAvailableCPU(); // Toma el CPU disponible de la lista CPUs

        cpu.loadProcess(proceso); // Carga el proceso al CPU disponible

        lstREADY.RemoveAt(0); // Remueve el proceso de la lista READY
    }

    public void updateMaxProcessesSize(int maxSize)
    {
        processMaxSize = maxSize;
        System.Console.WriteLine("\nNuevo tamaño maximo de un proceso: " + maxSize);
    }

    public void updateMinProcessesSize(int minSize)
    {
        processMinSize = minSize;
        System.Console.WriteLine("\nNuevo tamaño minimo de un proceso: " + minSize);
    }

    public void setMaxBlockTime(int ticks)
    {
        this.maxBlockTicks = ticks;
        Console.WriteLine("Nuevo tiempo maximo de bloqueo:" + ticks);
    }

    public int getMaxProcessesSize()
    {
        return this.processMaxSize;
    }

    public int getMinProcessesSize()
    {
        return this.processMinSize;
    }

    public List<Proceso> getnewProcesses()
    {
        return newProcesses;
    }

    public List<Proceso> getfinished()
    {
        return finished;
    }

    public List<Proceso> getlistReady()
    {
        return lstREADY;
    }

    public List<CPU> getCPUs()
    {
        return availableCPUs;
    }

    public List<Proceso> gy()
    {
        return lstREADY;
    }
}