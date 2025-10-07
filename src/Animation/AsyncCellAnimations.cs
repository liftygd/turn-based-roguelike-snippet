public interface AsyncCellAnimation : AsyncAnimation
{
    public struct ExplodeAnimation : AsyncCellAnimation
    {
        public string clipName => "Animation_Cell_Explosion";
    }

    public struct ExecutionAnimation : AsyncCellAnimation
    {
        public string clipName => "Animation_Cell_Execution";
    }

    public struct ExecutionMarkerAppearAnimation : AsyncCellAnimation
    {
        public string clipName => "Animation_Cell_ExecutionMarker_Appear";
    }

    public struct ExecutionMarkerDisappearAnimation : AsyncCellAnimation
    {
        public string clipName => "Animation_Cell_ExecutionMarker_Disappear";
    }

    public struct SlashAnimation : AsyncCellAnimation
    {
        public string clipName => "Animation_Cell_Slash";
    }

    public struct BuffAnimation : AsyncCellAnimation
    {
        public string clipName => "Animation_Cell_Buff";
    }

    public struct DebuffAnimation : AsyncCellAnimation
    {
        public string clipName => "Animation_Cell_Debuff";
    }

    public struct SmokeAnimation : AsyncCellAnimation
    {
        public string clipName => "Animation_Cell_Smoke";
    }
}