using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Axxes.AkkaNetDemo.TestClient
{
    public partial class LoadGenerator : Form
    {
        private readonly List<DeviceStatus> _deviceStatusList = new List<DeviceStatus>();
        public LoadGenerator()
        {
            InitializeComponent();
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            var now = DateTime.UtcNow;
            var startDate = new DateTime(now.Year, now.Month, now.Day, now.Hour, 0, 0, DateTimeKind.Utc);

            for (int i = 0; i < txtNoDevices.Value; i++)
            {
                var deviceId = Guid.NewGuid();
                _deviceStatusList.Add(new DeviceStatus {DeviceId = deviceId, NumberOfReadingsSent = 0});
                var device = new TestDevice(deviceId, startDate);
                device.DeviceReadingsSent += Device_DeviceReadingsSent;
                device.StartSending();
            }
            lstProgress.DataSource = _deviceStatusList;
        }

        private int _updateCounter;
        private void Device_DeviceReadingsSent(object sender, TestDevice.DeviceReadingsSentEventArgs e)
        {
            var item = _deviceStatusList.First(x => x.DeviceId == e.DeviceId);
            item.NumberOfReadingsSent = e.NumberOfReadingsSent;
            _updateCounter++;

            if (_updateCounter > txtNoDevices.Value/2)
            {
                lstProgress.DataSource = null;
                lstProgress.DataSource = _deviceStatusList;
                _updateCounter = 0;
            }
        }
    }

    internal class DeviceStatus
    {
        public Guid DeviceId { get; set; }
        public int NumberOfReadingsSent { get; set; }

        public override string ToString()
        {
            return $"{DeviceId} - messages sent: {NumberOfReadingsSent}";
        }
    }
}
