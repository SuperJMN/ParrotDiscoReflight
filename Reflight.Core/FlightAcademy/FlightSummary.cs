using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Reflight.Core.FlightAcademy
{
    public class FlightSummary
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("gps_longitude")]
        public double GpsLongitude { get; set; }

        [JsonProperty("gps_latitude")]
        public double GpsLatitude { get; set; }

        [JsonProperty("user")]
        public string User { get; set; }

        [JsonProperty("product_id")]
        public int ProductId { get; set; }

        [JsonProperty("serial_number")]
        public string SerialNumber { get; set; }

        [JsonProperty("run_origin")]
        public int RunOrigin { get; set; }

        [JsonProperty("uuid")]
        public string Uuid { get; set; }

        [JsonProperty("date")]
        public DateTimeOffset Date { get; set; }

        [JsonProperty("visible")]
        public bool Visible { get; set; }

        [JsonProperty("grade")]
        public int Grade { get; set; }

        [JsonProperty("controller_model")]
        public string ControllerModel { get; set; }

        [JsonProperty("controller_application")]
        public string ControllerApplication { get; set; }

        [JsonProperty("total_run_time")]
        public TimeSpan TotalRunTime { get; set; }

        [JsonProperty("crash")]
        public int Crash { get; set; }

        [JsonProperty("run_time")]
        public TimeSpan RunTime { get; set; }

        [JsonProperty("gps_available")]
        public bool GpsAvailable { get; set; }

        [JsonProperty("jump")]
        public int Jump { get; set; }

        [JsonProperty("software_version")]
        public string SoftwareVersion { get; set; }

        [JsonProperty("hardware_version")]
        public string HardwareVersion { get; set; }

        [JsonProperty("product_style")]
        public int ProductStyle { get; set; }

        [JsonProperty("deleted")]
        public bool Deleted { get; set; }

        [JsonProperty("last_modified")]
        public DateTime LastModified { get; set; }

        [JsonProperty("nlikes")]
        public int Likes { get; set; }

        [JsonProperty("captures")]
        public List<object> Captures { get; set; }

        [JsonProperty("videos")]
        public List<object> Videos { get; set; }
    }
}