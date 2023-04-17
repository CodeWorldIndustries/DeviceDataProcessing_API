namespace Domain.Devices.Contracts
{
    public class CreateIotDataRequest
    {
        /// <summary>
        /// Gets or sets the ID of the company that owns the device.
        /// </summary>
        /// <remarks>
        /// This property corresponds to the 'PartnerId' field in 'Foo1' data and the 'CompanyId' field in 'Foo2' data.
        /// </remarks>
        public int CompanyId { get; set; }

        /// <summary>
        /// Gets or sets the name of the company that owns the device.
        /// </summary>
        /// <remarks>
        /// This property corresponds to the 'PartnerName' field in 'Foo1' data and the 'Company' field in 'Foo2' data.
        /// </remarks>
        public string CompanyName { get; set; }

        /// <summary>
        /// Gets or sets the ID of the device.
        /// </summary>
        /// <remarks>
        /// This property corresponds to the 'Id' field in 'Foo1' data and the 'DeviceID' field in 'Foo2' data.
        /// </remarks>
        public int? DeviceId { get; set; }

        /// <summary>
        /// Gets or sets the name of the device model.
        /// </summary>
        /// <remarks>
        /// This property corresponds to the 'Model' field in 'Foo1' data and the 'Name' field in 'Foo2' data.
        /// </remarks>
        public string DeviceName { get; set; }

        /// <summary>
        /// Gets or sets the datetime of the first sensor reading for the device.
        /// </summary>
        /// <remarks>
        /// This property corresponds to the 'Trackers.Sensors.Crumbs' field in 'Foo1' data and the 'Devices.SensorData' field in 'Foo2' data.
        /// </remarks>
        public DateTime? FirstReadingDtm { get; set; }

        /// <summary>
        /// Gets or sets the datetime of the last sensor reading for the device.
        /// </summary>
        public DateTime? LastReadingDtm { get; set; }

        /// <summary>
        /// Gets or sets the number of temperature records for the device.
        /// </summary>
        public int? TemperatureCount { get; set; }

        /// <summary>
        /// Gets or sets the average temperature for the device.
        /// </summary>
        public double? AverageTemperature { get; set; }

        /// <summary>
        /// Gets or sets the number of humidity records for the device.
        /// </summary>
        public int? HumidityCount { get; set; }

        /// <summary>
        /// Gets or sets the average humidity for the device.
        /// </summary>
        public double? AverageHumidity { get; set; }
    }
}
