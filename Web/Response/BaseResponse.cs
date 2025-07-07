using System;

namespace CrediAPI.Web.DTOs.Response
{
    public class BaseResponse
    {
        public int StatusCode { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public object Data { get; set; }
        public string Timestamp { get; set; } = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
    }
}