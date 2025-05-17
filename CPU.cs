using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projecto_2_19_2025
{
    public class CPU
    {
        private Proceso executingProcess;
        private int quantum;
        private int quantumCounter;
        private int id;

        public CPU(int id)
        {
            executingProcess = null;
            quantum = 10; // Quantum por defecto 10
            quantumCounter = 0;
            this.id = id;   
        }
        
        public int getId()
        {
            return this.id;
        }

        public void setQuantumTime(int quantum)
        {
            this.quantum = quantum;
        }

        public void loadProcess(Proceso proceso)
        {
            // Carga un proceso y reinicia el contador de quantum
            executingProcess = proceso;
            quantumCounter = 0;
        }

        public Boolean isExecutingAProcess()
        {
            return executingProcess != null;
        }

        public void incrementProgramCounter()
        {
            if (!this.isExecutingAProcess())
            {
                return;
            }

            int pc = this.executingProcess.getPC();

            if (pc < this.executingProcess.getSize())
            {
                pc++;
                this.executingProcess.setPC(pc);
            }
        }

        public Boolean isExecutingProcessFinished()
        {
            if (this.isExecutingAProcess())
            {
                Proceso p = this.getExecutingProcess();
                return p.getPC() == p.getSize();
            }
            return false;
        }

        public void execute()
        {
            if (!this.isExecutingAProcess())
            {
                return;
            }

            this.incrementProgramCounter();
            quantumCounter++;

            Console.WriteLine(String.Format("Se ejecuto proceso en CPU[{0}]: {1}", this.id, this.executingProcess.ToString()));
        }

        public bool hasQuantumExpired()
        {
            return quantumCounter >= quantum;
        }

        public Proceso getExecutingProcess()
        {
            return this.executingProcess;
        }

        public void unloadProcess()
        {
            this.executingProcess = null;
        }
    }
}