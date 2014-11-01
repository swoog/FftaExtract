namespace FftaExtract.DatabaseModel
{
    using System;

    public class JobInfo
    {
        public string Url { get; set; }

        public JobStatus JobStatus { get; set; }

        public DateTime CreatedDateTime { get; set; }
    }
}