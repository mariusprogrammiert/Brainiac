using System;
using System.Collections.Generic;

namespace Brainiac
{
    class BrainiacController
    {
        public event Action<FieldColors, bool> PlayField;
        public event Action<int> UpdatePoints;
        public event Action PlayGameOver;
        public event Action LockElements;
        public event Action UnlockElements;
        public bool isGameRunning { get; set; }

        private readonly Dictionary<int, FieldColors> fields;
        private Random random;
        private int points, internalPlayerCounter, internalComputerCounter;
        private bool isComputerTurn, shouldRotate;

        public BrainiacController()
        {
            fields = new Dictionary<int, FieldColors>();
            random = new Random();
            isGameRunning = false;
        }

        public void startNewGame(bool isHardcoreMode)
        {
            LockElements();
            fields.Clear();
            points = 0;
            internalPlayerCounter = 0;
            internalComputerCounter = 0;
            isComputerTurn = true;
            isGameRunning = true;
            shouldRotate = isHardcoreMode;
            UpdatePoints(points);
            performNewTurn();
        }

        private void performNewTurn()
        {
            fields.Add(fields.Count, (FieldColors)random.Next(4));
            PlayField(fields[0], shouldRotate);
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
                        PlayField(fields[internalComputerCounter], false);
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
                    UnlockElements();
                }
            }
        }
    }
}
