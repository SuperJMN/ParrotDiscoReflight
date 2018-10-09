using System.Collections.Generic;

namespace FlightVisualizer.Core
{
    public class IntermediateData
    {
        public string version { get; set; }
        public string software_version { get; set; }
        public string hardware_version { get; set; }
        public string date { get; set; }
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
        public int run_time { get; set; }
        public int total_run_time { get; set; }
        public List<string> details_headers { get; set; }
        public List<List<object>> details_data { get; set; }
        public object controller_info { get; set; }
        public object battery { get; set; }
        public object drone_features { get; set; }
        public object accessories { get; set; }
        public object life_flighttime { get; set; }
        public object quality_versions { get; set; }
        public object details_map { get; set; }
    }
}