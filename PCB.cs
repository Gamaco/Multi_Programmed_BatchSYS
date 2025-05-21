using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Projecto_2_19_2025
{
    public partial class PCB : Form
    {
        public PCB()
        {
            InitializeComponent();
        }

        // Este método carga los procesos en el ListBox cuando se llama desde otro formulario
        public void CargarProcesos(OS sistema)
        {
            lstProcessControlBlock.Items.Clear(); // Limpia la lista visual

            // Cabecera de tabla
            lstProcessControlBlock.Items.Add("PID   PC   SIZE   ESTADO");
            lstProcessControlBlock.Items.Add("----------------------------------");

            // Procesos en NEW
            foreach (Proceso p in sistema.getnewProcesses())
            {
                lstProcessControlBlock.Items.Add($" {p.getPid(),-5}{p.getPC(),-5}{p.getSize(),-7}NEW");
            }

            // Procesos en READY
            foreach (Proceso p in sistema.getlistReady())
            {
                lstProcessControlBlock.Items.Add($" {p.getPid(),-5}{p.getPC(),-5}{p.getSize(),-7}READY");
            }

            // Proceso en RUNNING
            foreach (CPU cpu in sistema.ListAllCPUs())
            {
                if (cpu.isExecutingAProcess())
                {
                    Proceso p = cpu.getExecutingProcess();
                    lstProcessControlBlock.Items.Add($" {p.getPid(),-5}{p.getPC(),-5}{p.getSize(),-7}RUNNING");
                }
            }
            // Proceso en BLOCKED
            foreach (Proceso p in sistema.getBlocked())
            {
                lstProcessControlBlock.Items.Add($" {p.getPid(),-5}{p.getPC(),-5}{p.getSize(),-7}BLOCKED");
            }

            // Procesos en FINISHED
            foreach (Proceso p in sistema.getfinished())
            {
                lstProcessControlBlock.Items.Add($" {p.getPid(),-5}{p.getPC(),-5}{p.getSize(),-7}FINISHED");
            }
        }

        private void PCB_Load(object sender, EventArgs e)
        {
            // Opcional: puedes poner que cargue automáticamente los procesos si pasas el OS aquí
        }

        private void btnBackToOs_Click(object sender, EventArgs e)
        {
            this.Close(); // Cierra el formulario y regresa a la pantalla anterior
        }
    }
}
