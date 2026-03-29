using UnityEngine;

public class Invoker : MonoBehaviour
{
    private float recordingTime;
    private float replayStartTime;
    private bool isRecording;
    public bool IsRecording { get { return isRecording; } }

    void FixedUpdate()
    {
        if (isRecording)
            recordingTime += Time.deltaTime;
    }

    public void ExecuteCommand(Command command)
    {
        command.Execute();
        Debug.Log("Executed Command: " + command);

        if (isRecording)
        {
            command.timeStamp = recordingTime;
            CommandLog.recordedCommands.Enqueue(command);
            Debug.Log("Recorded Time: " + command.timeStamp);
        }
    }

    public void StartRecording()
    {
        Debug.Log("Starting Recording");
        recordingTime = 0.0f;
        isRecording = true;
    }

    public void StopRecording()
    {
        isRecording = false;
    }
}
