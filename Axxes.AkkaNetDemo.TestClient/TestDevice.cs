using System;
using System.ComponentModel;
using System.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using Axxes.AkkaNetDemo.TestClient.Model;

namespace Axxes.AkkaNetDemo.TestClient
{
    public class TestDevice
    {
        private readonly Guid _deviceId;
        private readonly DateTime _startTime;
        private int _minuteNumber;
        private decimal _meterReading;
        private BackgroundWorker _worker;
        private readonly Random _random;
        private readonly HttpClient _httpClient;

        public TestDevice(Guid deviceId, DateTime startTime)
        {
            _deviceId = deviceId;
            _startTime = startTime;

            // Get a fully random number
            _random = new Random((int)(DateTime.Now.Ticks % 2000000000));

            // Create an initial meter reading
            var value = _random.Next(10000, 200000);
            _meterReading = value / 1000M;

            // And create an HttpClient for all server communication
            _httpClient = CreateHttpClient();
        }

        public void StartSending()
        {
            // Let the service know about our device
            SendHello();

            // Create a backgroundworker for posting MeterReadings
            _worker = new BackgroundWorker();

            _worker.DoWork += Worker_DoWork;
            _worker.ProgressChanged += Worker_ProgressChanged;
            _worker.WorkerReportsProgress = true;
            _worker.WorkerSupportsCancellation = true;

            _worker.RunWorkerAsync();
        }

        protected void Worker_DoWork(object sender, DoWorkEventArgs e)
        {
            var sendingWorker = (BackgroundWorker)sender;
            while (!sendingWorker.CancellationPending)
            {
                // Increase the meter value and the simulated timestamp
                _meterReading += _random.Next(200) / 1000M;
                _minuteNumber++;

                // Report to the service
                SendMeterReading();

                // Tell the worker it's still busy
                sendingWorker.ReportProgress(50);

                // Wait a send for the next one
                Thread.Sleep(1000);
            }
        }

        private void SendHello()
        {
            _httpClient.GetAsync($"Device/{_deviceId}").GetAwaiter().GetResult();
        }

        private void SendMeterReading()
        {
            var meterReading = new MeterReading
            {
                DeviceId = _deviceId,
                MeterValue = _meterReading,
                Timestamp = _startTime.AddMinutes(_minuteNumber),
                Unit = "kWh"
            };

            _httpClient.PostAsJsonAsync("MeterReading", meterReading).GetAwaiter().GetResult();
        }

        protected void Worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            OnDeviceReadingsSent(new DeviceReadingsSentEventArgs(_deviceId, _minuteNumber));
        }

        private HttpClient CreateHttpClient()
        {
            var client = new HttpClient
            {
                BaseAddress = new Uri(ConfigurationManager.AppSettings["TargetUrl"])
            };

            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            return client;
        }

        public delegate void DeviceReadingsSentEventHandler(object sender, DeviceReadingsSentEventArgs e);

        public event DeviceReadingsSentEventHandler DeviceReadingsSent;

        protected virtual void OnDeviceReadingsSent(DeviceReadingsSentEventArgs e)
        {
            DeviceReadingsSent?.Invoke(this, e);
        }

        public class DeviceReadingsSentEventArgs : EventArgs
        {
            public DeviceReadingsSentEventArgs(Guid deviceId, int numberOfReadingsSent)
            {
                DeviceId = deviceId;
                NumberOfReadingsSent = numberOfReadingsSent;
            }
            public Guid DeviceId { get; set; }
            public int NumberOfReadingsSent { get; set; }
        }
    }
}
