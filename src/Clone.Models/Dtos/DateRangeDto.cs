namespace Clone.Models.Dtos
{
    public readonly record struct DateRangeDto(DateTime From, DateTime To)
    {
        public static DateRangeDto Today()
        {
            var today = DateTime.Today;
            return new DateRangeDto(today, today.AddDays(1).AddSeconds(-1));
        }

        public override string ToString() => $"{From:dd/MM/yyyy HH:mm} - {To:dd/MM/yyyy HH:mm}";
    }
}


