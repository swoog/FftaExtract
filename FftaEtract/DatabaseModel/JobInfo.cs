namespace FftaExtract.DatabaseModel
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class JobInfo
    {
        public int Id { get; set; }

        [MaxLength(300)]
        [Index("IX_Url", 2)]
        public string Url { get; set; }

        [Index("IX_JobStatus", 1)]
        [Index("IX_Url", 1)]
        [Index("IX_EndJob", 1)]
        public JobStatus JobStatus { get; set; }

        [Index("IX_JobStatus", 2)]
        public DateTime CreatedDateTime { get; set; }

        public string ReasonPhrase { get; set; }

        [Index("IX_EndJob", 2)]
        public DateTime? EndJob { get; set; }

        public DateTime? BeginJob { get; set; }
    }
}