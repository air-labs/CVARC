namespace CVARC.Basic
{
    public abstract class Bot : Participant
    {
        public Competitions Competitions { get; private set; }

        public virtual void Initialize(Competitions competitions)
        {
            Competitions = competitions;
        }
    }
}
