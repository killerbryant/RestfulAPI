using Newtonsoft.Json;
using System;

namespace RestfulAPI.Model.Models.Dto
{
    public class DeptDto : BaseEntity
    {
        /// <summary>
        /// DeptNo
        /// </summary>
        [JsonProperty("DeptID")]
        public int DeptNo { get; set; }

        /// <summary>
        /// Dname
        /// </summary>
        [JsonProperty("Name")]
        [System.Text.Json.Serialization.JsonPropertyName("Name")]
        public string Dname { get; set; }

        /// <summary>
        /// Location
        /// </summary>
        [JsonProperty("Location")]
        [System.Text.Json.Serialization.JsonPropertyName("Location")]
        public string Location { get; set; }
    }
}
