namespace TycoonCo.Domain
{
    public class WorkerActivity : Entity<char>
    {
        private readonly char workerId;
        private bool hasConflict;

        private WorkerActivity()
        {

        }

        internal WorkerActivity(char workerId) 
            : base(workerId)
        {
            this.workerId = workerId;
        }

        public char WorkerId => workerId;
        public bool HasConflict => hasConflict;

        public void AddConflict()
        {
            hasConflict = true;
        }

        public void RemoveConflict()
        {
            hasConflict = false;
        }
    }
}
