﻿using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Projecto_2_19_2025
{

    public partial class GUI_OS : Form
    {
        OS os;
        PCB pcbGUI;
        Boolean autoTrigger;

        public GUI_OS()
        {
            os = new OS();
            autoTrigger = false;
            InitializeComponent();

        }

        private void UpdateREADY()
        {

            lstReady.DataSource = null;
            lstReady.DataSource = os.getlistReady();
        }

        private void updateNewProcesses()      
        {
            RAM.DataSource = null;
            RAM.DataSource = os.getnewProcesses();


        }

        private void UpdateFinished()
        {
            List_Finish.DataSource = null;
            List_Finish.DataSource = os.getfinished();
        }

        private void UpdateRunning()
        {
            CPU_List.Items.Clear();

            foreach (CPU cpu in os.getCPUs())
            {
                if (cpu.isExecutingAProcess())
                {
                    CPU_List.Items.Add(String.Format("CPU [{0}]: {1}", cpu.getId(), cpu.getExecutingProcess().ToString()));
                }
            }
        }

            private async void btnClock_Click(object sender, EventArgs e)
            {
                // La mitad de un segundo en MS
                int velocidadClick = 300; 

                // Chequea si esta en modo automatico
                if (autoTrigger == false && checkBox1.Checked == true)
                {
                    autoTrigger = true;
                    btnClock.Text = "Stop";

                    await Task.Run(() =>
                    {
                        while (autoTrigger)
                        {
                            Console.WriteLine("\n-----New Tick-----");
                            os.ClockTick();

                            Invoke(new Action(() =>
                            {
                                UpdateRunning();
                                UpdateFinished();
                                updateNewProcesses();
                                UpdateREADY();
                                UpdateRunning();
                            }));

                            Thread.Sleep(velocidadClick);
                        }
                    });

                    btnClock.Text = "Clock tick";
                }
                else
                {
                    autoTrigger = false;
                    btnClock.Text = "Clock tick";
                }

            // Si no esta en modo automatico, hace un solo tick y ya
            if (!checkBox1.Checked)
                {
                    Console.WriteLine("\n-----New Tick-----");
                    os.ClockTick();
                    UpdateRunning();
                    UpdateFinished();
                    updateNewProcesses();
                    UpdateREADY();
                    UpdateRunning();
                }
            }


        private void btnPCB_Click(object sender, EventArgs e)
        {
            pcbGUI = new PCB();                  // Crea una nueva ventana PCB
            pcbGUI.CargarProcesos(os);          // Le pasa el sistema operativo actual
            pcbGUI.Show();                      // Muestra la ventana

        }

        private void numericUpDown_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown numericUpDown = sender as NumericUpDown;

            // Cambia la cantidad de CPUs disponibles
            os.SetAvailableCPUs((int)numericUpDown.Value); 
        }

        private void numericUpDown_QuantumTime_ValueChanged(object sender, EventArgs e)
        {
            os.updateQuantumTime((int)numericUpDown_QuantumTime.Value); // Cambia el quantum de cada CPU
        }
    }
}
    
