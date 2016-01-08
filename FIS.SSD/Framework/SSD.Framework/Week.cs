
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
namespace UG.Framework
{
    public class Week
    {
        public DateTime WeekStart { get; set; }
        public DateTime WeekFinish { get; set; }
        public int WeekNum { get; set; }
        public string WeekStr { get; set; }
        public static List<Week> WeeksOfMonth(DateTime startMonth)
        {
            CultureInfo defaultCultureInfo = CultureInfo.CurrentCulture;

            // Check for the starting date of the initial week.
            int diff = startMonth.DayOfWeek - defaultCultureInfo.DateTimeFormat.FirstDayOfWeek;

            List<Week> lstW = new List<Week>();

            Week w1 = new Week()
            {
                WeekStart = startMonth,
                WeekFinish = startMonth.AddDays(7 - diff),
                WeekNum = 1,
                WeekStr = "Tuần 1"
            };
            lstW.Add(w1);
            DateTime start = DateTime.Now;
            for (int i = 2; i <= 6; i++)
            {
                start = lstW[i - 2].WeekFinish;
                if (start.AddDays(7).Month == lstW[i - 2].WeekFinish.Month)
                {
                    Week w = new Week()
                    {
                        WeekStart = start,
                        WeekFinish = start.AddDays(7),
                        WeekNum = i,
                        WeekStr = "Tuần " + i
                    };
                    lstW.Add(w);
                }
                else break;
            }

            DateTime endMonth = startMonth.AddDays(DateTime.DaysInMonth(startMonth.Year, startMonth.Month));

            if (endMonth.Day != start.Day && start.AddDays(7).Month > start.Month)
            {
                Week w = new Week()
                {
                    WeekStart = start,
                    WeekFinish = endMonth,
                    WeekNum = lstW.Count + 1,
                    WeekStr = "Tuần " + (lstW.Count + 1).ToString()
                };
                lstW.Add(w);
            }

            return lstW;
        }
        public static List<Week> WeeksOfYear
        {
            get
            {
                return WeeksOfYears(DateTime.Now.Year);
            }
        }
        public static List<Week> WeeksOfYears(int year)
        {
            DateTime jan1 = new DateTime(year, 1, 1);
                //beware different cultures, see other answers
            DateTime startOfFirstWeek = jan1.AddDays(1 - (int)(jan1.DayOfWeek)).AddDays(-1);
                var weeks =
                    Enumerable
                        .Range(0, 54)
                        .Select(i => new
                        {
                            weekStart = startOfFirstWeek.AddDays(i * 7)
                        })
                        .TakeWhile(x => x.weekStart.Year <= jan1.Year)
                        .Select(x => new
                        {
                            x.weekStart,
                            weekFinish = x.weekStart.AddDays(6)
                        })
                        .SkipWhile(x => x.weekFinish.Year < jan1.Year)
                        .Select((x, i) => new Week()
                        {
                            WeekStart = x.weekStart,
                            WeekFinish = x.weekFinish,
                            WeekNum = i + 1,
                            WeekStr = "Tuần " + (i + 1)
                        });
                return weeks.ToList();
        }
        public static Week CurrentWeek
        {
            get
            {
                CultureInfo cultInfo = CultureInfo.CurrentCulture;
                int weekNumNow = cultInfo.Calendar.GetWeekOfYear(DateTime.Now,
                                     cultInfo.DateTimeFormat.CalendarWeekRule,
                                         cultInfo.DateTimeFormat.FirstDayOfWeek);

                for (int i = 0; i < WeeksOfYear.Count; i++)
                {
                    var w = WeeksOfYear[i];
                    if (weekNumNow == w.WeekNum)
                        return w;
                }
                return null;
            }
        }
        public static Week GetWeekByDate(DateTime date)
        {
            CultureInfo cultInfo = CultureInfo.CurrentCulture;
            int weekNumNow = cultInfo.Calendar.GetWeekOfYear(date,
                                    cultInfo.DateTimeFormat.CalendarWeekRule,
                                        cultInfo.DateTimeFormat.FirstDayOfWeek);
            for (int i = 0; i < WeeksOfYear.Count; i++)
            {
                var w = WeeksOfYear[i];
                if (weekNumNow == w.WeekNum)
                    return w;
            }
            return null;
        }
    }
}
