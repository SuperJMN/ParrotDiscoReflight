using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Reflight.Core.Reader
{
    public class FlightDetails
    {
        [JsonProperty("version")] 
        public string Version { get; set; }

        [JsonProperty("software_version")] 
        public string SoftwareVersion { get; set; }

        [JsonProperty("hardware_version")] 
        public string HardwareVersion { get; set; }

        [JsonProperty("date")] 
        public DateTimeOffset Date { get; set; }

        [JsonProperty("product_id")] 
        public long ProductId { get; set; }

        [JsonProperty("serial_number")] 
        public string SerialNumber { get; set; }

        [JsonProperty("product_name")] 
        public string ProductName { get; set; }

        [JsonProperty("uuid")] 
        public string Uuid { get; set; }

        [JsonProperty("run_origin")] 
        public long RunOrigin { get; set; }

        [JsonProperty("controller_model")] 
        public string ControllerModel { get; set; }

        [JsonProperty("controller_application")]
        public string ControllerApplication { get; set; }

        [JsonProperty("product_style")] 
        public long ProductStyle { get; set; }

        [JsonProperty("product_accessory")] 
        public long? ProductAccessory { get; set; }

        [JsonProperty("gps_available")] 
        public bool GpsAvailable { get; set; }

        [JsonProperty("gps_latitude")] 
        public double GpsLatitude { get; set; }

        [JsonProperty("gps_longitude")] 
        public double GpsLongitude { get; set; }

        [JsonProperty("crash")] 
        public long Crash { get; set; }

        [JsonProperty("jump")]
        public object Jump { get; set; }

        [JsonProperty("run_time")] 
        public TimeSpan RunTime { get; set; }

        [JsonProperty("total_run_time")] 
        public TimeSpan TotalRunTime { get; set; }

        [JsonProperty("details_headers")] 
        public string[] DetailsHeaders { get; set; }

        [JsonProperty("details_data")]
        public List<List<object>> DetailsData { get; set; }

        [JsonProperty("controller_info")] 
        public object ControllerInfo { get; set; }

        [JsonProperty("battery")] 
        public object Battery { get; set; }

        [JsonProperty("drone_features")] 
        public object DroneFeatures { get; set; }

        [JsonProperty("accessories")]
        public object Accessories { get; set; }

        [JsonProperty("life_flighttime")] 
        public object LifeFlighttime { get; set; }

        [JsonProperty("quality_versions")] 
        public object QualityVersions { get; set; }

        [JsonProperty("details_map")]
        public object DetailsMap { get; set; }
    }

    public class FlightDetails2
    {
        public string version { get; set; }
        public string software_version { get; set; }
        public string hardware_version { get; set; }
        public DateTimeOffset date { get; set; }
        public int product_id { get; set; }
        public string serial_number { get; set; }
        public string product_name { get; set; }
        public string uuid { get; set; }
        public int run_origin { get; set; }
        public string controller_model { get; set; }
        public string controller_application { get; set; }
        public int product_style { get; set; }
        public int product_accessory { get; set; }
        public bool gps_available { get; set; }
        public double gps_latitude { get; set; }
        public double gps_longitude { get; set; }
        public int crash { get; set; }
        public object jump { get; set; }
        public TimeSpan run_time { get; set; }
        public TimeSpan total_run_time { get; set; }
        public List<string> details_headers { get; set; }
        public List<List<object>> details_data { get; set; }
        public object controller_info { get; set; }
        public string battery { get; set; }
        public object drone_features { get; set; }
        public object accessories { get; set; }
        public object life_flighttime { get; set; }
        public object quality_versions { get; set; }
        public object details_map { get; set; }
    }
}