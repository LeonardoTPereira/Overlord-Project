using System;

namespace Game.Events
{
    public delegate void NewEAGenerationEvent(object sender, NewEAGenerationEventArgs e);

    public class NewEAGenerationEventArgs : EventArgs
    {
        public float CompletionRate { get; }
        public bool HasFinished { get; }

        public NewEAGenerationEventArgs(float completionRate, bool hasFinished)
        {
            CompletionRate = completionRate;
            HasFinished = hasFinished;
        }

    }
}