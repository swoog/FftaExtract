namespace FftaExtract.DatabaseModel
{
    using System;
    using System.ComponentModel.DataAnnotations.Schema;

    public class JobInfo
    {
        public int Id { get; set; }

        public string Url { get; set; }

        [Index("IX_JobStatus", 1)]
        public JobStatus JobStatus { get; set; }

        [Index("IX_JobStatus", 2)]
        public DateTime CreatedDateTime { get; set; }
    }
}