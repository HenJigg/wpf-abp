namespace AppFramework.Tenants.Dashboard.Dto
{
    public class MemberActivity
    {
        public string Name { get; set; }
        public string Earnings { get; set; }
        public int Cases { get; set; }
        public int Closed { get; set; }
        public string Rate { get; set; }

        public MemberActivity(string name, string earnings, int cases, int closed, string rate)
        {
            Name = name;
            Earnings = earnings;
            Cases = cases;
            Closed = closed;
            Rate = rate;
        }
    }
}