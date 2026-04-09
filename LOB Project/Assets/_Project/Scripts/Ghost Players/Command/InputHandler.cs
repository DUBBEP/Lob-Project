using UnityEditor.Timeline.Actions;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;
public class InputHandler : MonoBehaviour
{
    [SerializeField]
    FirstPersonController player;
    Replay replay;
    Invoker invoker;

    public delegate void MakeClone(Replay replay);
    public static event MakeClone OnMakeClone;

    public delegate void StartRecording(FirstPersonController player);
    public static event StartRecording OnStartRecording;

    private void Start()
    {
        replay = gameObject.AddComponent<Replay>();
        invoker = gameObject.AddComponent<Invoker>();
    }

    void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.A))
            invoker.ExecuteCommand(new MoveLeft(player));
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W))
            invoker.ExecuteCommand(new Jump(player));

        if (Input.GetKeyDown(KeyCode.R) && !replay.IsReplaying)
            ToggleRecording();

        if (Input.GetKeyDown(KeyCode.P) && CommandLog.recordedCommands.Count > 0 && !replay.IsReplaying && !invoker.IsRecording)
            OnMakeClone?.Invoke(replay);
    }

    void ToggleRecording()
    {
        if (replay.IsReplaying)
            return;

        if (!invoker.IsRecording)
        {
            OnStartRecording?.Invoke(player);
            invoker.StartRecording();
            Debug.Log("Recording Started");
        }
        else
        {
            invoker.StopRecording();
            Debug.Log("Recording Stopped");
        }
    }
}