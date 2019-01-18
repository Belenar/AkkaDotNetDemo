using System;
using System.Collections.Generic;
using Axxes.AkkaNetDemo.System.Messages;

namespace Axxes.AkkaNetDemo.System.Helpers
{
    static class QuarterlyConsumptionHelper
    {
        public static QuarterlyConsumptionResult ComputeQuarterlyConsumption
            (QuarterlyConsumptionParameters request)
        {
            var result = new QuarterlyConsumptionResult
            {
                NewReferenceReading = request.StartMeterReading
            };

            var startQuarterNumber = request.StartDate.QuarterNumber();
            var endQuarterNumber = request.CurrentMessage.Timestamp.QuarterNumber();

            for (int q = startQuarterNumber + 1; q <= endQuarterNumber; q++)
            {
                var quarterDate = DateTimeExtensions.GetQuarterDate(q);
                var reading = GetIntermediateReading(request, quarterDate);

                result.Quarters.Add(new Tuple<DateTime, decimal>(quarterDate, reading - result.NewReferenceReading));

                result.NewReferenceDate = quarterDate;
                result.NewReferenceReading = reading;
                result.NewReferenceQuarter = q;
            }

            return result;
        }

        private static decimal GetIntermediateReading(QuarterlyConsumptionParameters request, DateTime quarterDate)
        {
            if (quarterDate == request.CurrentMessage.Timestamp)
                return request.CurrentMessage.MeterValue;

            var totalMilliSeconds = (decimal)(request.CurrentMessage.Timestamp - request.PreviousMessage.Timestamp).TotalMilliseconds;
            var quarterMilliSeconds = (decimal)(quarterDate - request.PreviousMessage.Timestamp).TotalMilliseconds;

            var totalConsumption = request.CurrentMessage.MeterValue - request.PreviousMessage.MeterValue;
            var quarterConsumption = totalConsumption*(quarterMilliSeconds - totalMilliSeconds);

            return request.PreviousMessage.MeterValue + quarterConsumption;
        }
    }

    class QuarterlyConsumptionParameters
    {
        public DateTime StartDate { get; set; }
        public decimal StartMeterReading { get; set; }
        public MeterReadingReceived PreviousMessage { get; set; }
        public MeterReadingReceived CurrentMessage { get; set; }
    }

    public class QuarterlyConsumptionResult
    {
        public List<Tuple<DateTime, decimal>> Quarters { get; set; } = new List<Tuple<DateTime, decimal>>();
        public DateTime NewReferenceDate { get; set; }
        public decimal NewReferenceReading { get; set; }
        public int NewReferenceQuarter { get; set; }
    }
}