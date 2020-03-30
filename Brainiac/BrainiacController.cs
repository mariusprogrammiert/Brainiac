using System;
using System.Collections.Generic;

namespace Brainiac
{
    class BrainiacController
    {
        public event Action<FieldColors> PlayField;
        public event Action<int> UpdatePoints;
        public event Action PlayGameOver;
        public event Action LockButton;
        public event Action UnlockButton;
        public bool isGameRunning { get; set; }

        private readonly Dictionary<int, FieldColors> fields;
        private Random random;
        private int points, internalPlayerCounter, internalComputerCounter;
        private bool isComputerTurn;

        public BrainiacController()
        {
            fields = new Dictionary<int, FieldColors>();
            random = new Random();
            isGameRunning = false;
        }
        public void startNewGame()
        {
            LockButton();
            fields.Clear();
            points = 0;
            internalPlayerCounter = 0;
            internalComputerCounter = 0;
            isComputerTurn = true;
            isGameRunning = true;
            UpdatePoints(points);
            performNewTurn();
        }
        private void performNewTurn()
        {
            fields.Add(fields.Count, (FieldColors)random.Next(4));
            PlayField(fields[0]);
        }
        public void handlePlayedColor(FieldColors color)
        {
            if (isComputerTurn)
            {
                if (fields.Count > internalComputerCounter)
                {
                    internalComputerCounter++;

                    if (fields.Count <= internalComputerCounter)
                    {
                        isComputerTurn = false;
                    }
                    else
                    {
                        PlayField(fields[internalComputerCounter]);
                    }
                }
            }
            else
            {
                if (color == fields[internalPlayerCounter])
                {
                    internalPlayerCounter++;

                    if (fields.Count == internalPlayerCounter)
                    {
                        points++;
                        UpdatePoints(points);
                        isComputerTurn = true;
                        internalPlayerCounter = 0;
                        internalComputerCounter = 0;
                        performNewTurn();
                    }
                }
                else
                {
                    isGameRunning = false;
                    PlayGameOver();
                    UnlockButton();
                }
            }
        }
    }
}
