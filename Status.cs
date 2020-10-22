namespace WaitForStatusCheckAction
{
    public class Status
    {
        public long Id { get; set; }

        public string Context { get; set; }

        public string State { get; set; }

        public override string ToString()
        {
            return $"Id: {Id} Context: {Context} State: {State}";
        }
    }
}