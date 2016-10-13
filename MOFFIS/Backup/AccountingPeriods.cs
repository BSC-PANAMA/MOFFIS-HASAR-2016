using System;

namespace CSSDK
{

	public class AccountingPeriods
	{

		public Connect ptApp = new Connect();
		public DateTime[] StartDate;
		public DateTime[] EndDate;
		public int PeriodsPerYear;
		public int CurrentPeriod;

		public AccountingPeriods()
		{
			ptApp.app.GetAccountingPeriods(out PeriodsPerYear,out CurrentPeriod,out StartDate,out EndDate);

		}
		public string GetLastDayOfCurrPer()
		{
            return EndDate[CurrentPeriod].ToString();
		}
		public DateTime getFirstOpenDay()
		{
            DateTime dtStart = DateTime.Parse(StartDate[PeriodsPerYear].ToString());
            return dtStart;
		}
		public DateTime getLastOpenDay()
        {
            DateTime dtEnd = DateTime.Parse(EndDate[PeriodsPerYear + PeriodsPerYear].ToString());
            return dtEnd;
		}
	}
}
