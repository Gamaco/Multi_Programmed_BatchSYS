using System;
using System.Data;

public class Proceso
{
    //Cosntantes 

    public enum States
    {
        NEW,
        READY,
        RUNNING,
        BLOCKED,
        FINISHED
    }

    private int pid;   
    private int size;
    private int pc;
    private int priority;
    private States states;

    // Corrected Constructor (Default Constructor)
    public Proceso()
    {
        pid = 1;       
        size = 15;
        pc = 1;
        priority = 5;
        states = States.NEW;
    }

    // Corrected Parameterized Constructor
    public Proceso(int pid, int size, int pc, int priority)
    {
        this.pid = pid;
        this.size = size;
        this.pc = pc;
        this.priority = priority;
    }
    public void setState(States s)
    {
        this.states = s;

    }
    public override string ToString()
    {
        return "PID: " + pid + " | Size: " + size + " | PC: " + pc +" | STATE: " + this.getState() + " | PRIORITY: " + this.getPriority();
    }
    // Getter Methods
    public int getPid() { return pid; }
    public int getSize() { return size; }
    public int getPC() { return pc; }
    public int getPriority() {  return priority; }

    // Setter Methods

    public void SetPriority(int n)
    {
        priority = n;
    }
    public void setPid(int pid)
    {
        this.pid = pid;  // Fixed incorrect assignment
    }

    public void setSize(int size)
    {
        this.size = size;
    }

    public void setPC(int pc)
    {
        this.pc = pc;
    }


    public String getState()
    {
        switch (states)
        {
            case States.NEW:
                return "NEW";
            case States.READY:
                return "READY";
            case States.RUNNING:
                return "RUNNING";
            case States.BLOCKED:
                return "BLOCKED";
            case States.FINISHED:
                return "FINISHED";
            default:
                return "UNKNOWN";
        }
    }

    
}

	

