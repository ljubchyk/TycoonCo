namespace TycoonCo.Domain
{
    public class Worker
    {
        private readonly char id;

        private Worker()
        {

        }

        public Worker(char id)
        {
            if (!char.IsLetter(id))
            {
                throw new ArgumentException("Worker's id must be a char.", nameof(id));
            }

            this.id = id;
        }

        public char Id => id;
    }

    //public class WorkerId
    //{
    //    private readonly char id;

    //    public WorkerId(char id)
    //    {
    //        if (!char.IsLetter(id))
    //        {
    //            throw new ArgumentException("Worker's id must be a char.", nameof(id));
    //        }

    //        this.id = id;
    //    }

    //    public char Id => id;
    //}
}
