using System;

public delegate void FormAnsweredEvent(object sender, FormAnsweredEventArgs e);
public class FormAnsweredEventArgs : EventArgs
{
    private int formID;
    private int answerValue;

    public FormAnsweredEventArgs(int formID, int answerValue)
    {
        FormID = formID;
        AnswerValue = answerValue;
    }

    public int FormID { get => formID; set => formID = value; }
    public int AnswerValue { get => answerValue; set => answerValue = value; }
}