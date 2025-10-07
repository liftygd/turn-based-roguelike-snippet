public interface AsyncAnimation
{
    public string clipName { get; }

    public struct CustomAnimation : AsyncAnimation
    {
        public string clipName { get; set; }
    }

    public struct IdleAnimation : AsyncAnimation
    {
        public string clipName => "Animation_Entity_Idle";
    }

    public struct DeathAnimation : AsyncAnimation
    {
        public string clipName => "Animation_Entity_Death";
    }

    public struct StartExecutionAnimation : AsyncAnimation
    {
        public string clipName => "Animation_Execution_Start";
    }

    public struct LoopExecutionAnimation : AsyncAnimation
    {
        public string clipName => "Animation_Execution_Loop";
    }

    public struct EndExecutionAnimation : AsyncAnimation
    {
        public string clipName => "Animation_Execution_End";
    }
}