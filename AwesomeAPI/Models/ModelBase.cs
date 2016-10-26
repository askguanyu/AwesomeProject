using System;

namespace AwesomeAPI.Models
{
    public abstract class ModelBase
    {
        public int Id { get; set; }
        public DateTime InsertedOn { get; set; }
        public DateTime UpdatedOn { get; set; }
    }
}