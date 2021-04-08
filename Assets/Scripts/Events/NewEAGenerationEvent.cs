using System;

public delegate void NewEAGenerationEvent(object sender, NewEAGenerationEventArgs e);

public class NewEAGenerationEventArgs : EventArgs
{
    private int completionRate;

    public NewEAGenerationEventArgs(int completionRate)
    {
        CompletionRate = completionRate;
    }

    public int CompletionRate { get => completionRate; set => completionRate = value; }
}
