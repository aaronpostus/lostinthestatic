using UnityEngine;
public class InvisibleMazeTrigger : AudioTrigger
{
    private InvisibleMazeChannel channel;
    [SerializeField] RadioAudioComponent audioComponent;
    [SerializeField] InvisibleMazeController controller;
    [SerializeField] float channelNumber;
    public void Start()
    {
        channel = (InvisibleMazeChannel) audioComponent.GetRadioChannel(channelNumber);
    }
    public override void EnterAudioZone(GameObject gameObject)
    {
        channel.EnterMaze();
        controller.StartTrackingPlayer();
    }
    public override void ExitAudioZone()
    {
        channel.ExitMaze();
        controller.StopTrackingPlayer();
    }
}