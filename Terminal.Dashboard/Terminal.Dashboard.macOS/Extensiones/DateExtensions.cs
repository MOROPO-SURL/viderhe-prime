namespace SmartSolucionesCuba.Viderhe.Terminal.Dashboard.macOS.Extensiones
{
	internal static class DateExtensions
	{
        private static System.DateTime _nsRef = new System.DateTime(2001, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);

        public static System.DateTime ToDateTime(this Foundation.NSDate nsDate)
        {
            return _nsRef.AddSeconds(nsDate.SecondsSinceReferenceDate);
        }

        [System.Obsolete]
        public static Foundation.NSDate ToNSDate(this System.DateTime date)
        {
            var reference = System.TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(2001, 1, 1, 0, 0, 0));

            return Foundation.NSDate.FromTimeIntervalSinceReferenceDate((date - reference).TotalSeconds);
        }

        public static Foundation.NSDateComponents ToNSDateComponents(this System.DateTime date)
        {
            return new Foundation.NSDateComponents()
            {
                Year = date.Year,
                Minute = date.Minute,
                Second = date.Second,
                Hour = date.Hour,
                Month = date.Month,
                Day = date.Day
            };
        }
    }
}

