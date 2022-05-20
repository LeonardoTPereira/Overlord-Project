using System;

namespace Game.Events
{
    public delegate void NewEAGenerationEvent(object sender, NewEAGenerationEventArgs e);

    public class NewEAGenerationEventArgs : EventArgs
    {
        private float completionRate;

        public NewEAGenerationEventArgs(float completionRate)
        {
            CompletionRate = completionRate;
        }

        public float CompletionRate { get => completionRate; set => completionRate = value; }
    }
}