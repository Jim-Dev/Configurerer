﻿using UnityEngine;
using System.Collections;


public class ClockBehaviour {

    float timeSinceStart = 0, lastTimeSinceStart=0;
    const float INTERVAL = 0f, CICLE = 4F;
    public float executionTimeCounter = 0, executionTimeDuration=-1;
    public float timeBetweenRepetitionsTimeCounter = 0, timeBetweenRepetitionsTimeDuration = -1;



    /// <summary>
    /// Correr este metodo al inicio del Update del AnimationBehaviour
    /// </summary>
    public void Update()
    {

        timeSinceStart = timeSinceStart + Time.deltaTime;
        float diff = timeSinceStart - lastTimeSinceStart;
        if (diff >= INTERVAL)
        {

            lastTimeSinceStart = timeSinceStart;
            if(executionTimeDuration != -1)
            {
                executionTimeCounter = executionTimeCounter + diff;
            }
            if (timeBetweenRepetitionsTimeDuration != -1)
            {
                timeBetweenRepetitionsTimeCounter = timeBetweenRepetitionsTimeCounter + diff;
            }

        }

        if(timeSinceStart > CICLE)
        {
            timeSinceStart = timeSinceStart - CICLE;
            lastTimeSinceStart = lastTimeSinceStart - CICLE;
        }

        

        if(executionTimeDuration != -1 && executionTimeCounter >= executionTimeDuration)
        {
            Debug.Log("timer execucion " + executionTimeDuration + "!= -1 && " + executionTimeCounter + " > " + executionTimeDuration);
            executionTimeCounter = 0;
            executionTimerFinish();
        }

        if (timeBetweenRepetitionsTimeDuration != -1 && timeBetweenRepetitionsTimeCounter >= timeBetweenRepetitionsTimeDuration)
        {
            timeBetweenRepetitionsTimeCounter = 0;
            pauseBetweenRepetitionsFinish();
        }

    }

    /// <summary>
    /// Inicia el contador de duracion de una repeticion hasta cumplir con el "time". 
    /// </summary>
    /// <param name="time"></param>
    public void executeRepetitionTime(float time)
    {
        executionTimerStart();
        executionTimeDuration = time;
    }

    /// <summary>
    /// Detiene el contador de la duracion de un repeticion.
    /// </summary>
    public void stopExecutionTimer()
    {
        executionTimeDuration = -1;
        executionTimeCounter = 0;
    }


    /// <summary>
    /// Inicia el contador de duracion del tiempo entre repeticiones hasta cumplir con el "time". 
    /// </summary>
    /// <param name="time"></param>
    public void executeTimeBetweenRepetitions(float time)
    {
        timeBetweenRepetitionsTimeDuration = time;
        pauseBetweenRepetitionsStart();
    }

    /// <summary>
    /// Inicia el contador de duracion del tiempo entre repeticiones hasta cumplir con el "time". 
    /// </summary>
    public void stopTimeBetweenRepetitionsTimer()
    {
        timeBetweenRepetitionsTimeDuration = -1;
        timeBetweenRepetitionsTimeCounter = 0;
    }

    public delegate void ExecutionTimerStart();
    public ExecutionTimerStart executionTimerStart;

    public delegate void ExecutionTimerFinish();
    public ExecutionTimerFinish executionTimerFinish;

    public delegate void PauseBetweenRepetitionsStart();
    public PauseBetweenRepetitionsStart pauseBetweenRepetitionsStart;

    public delegate void PauseBetweenRepetitionsFinish();
    public PauseBetweenRepetitionsFinish pauseBetweenRepetitionsFinish;
}
