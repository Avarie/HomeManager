using System;
using System.Web.Script.Serialization;
using Newtonsoft.Json;

namespace home.manager.Models.db
{
    public class Document
    {
        public int Id { get; set; }

        public virtual Int32 ContentLength { get; set; }
        public virtual String ContentType { get; set; }
        public virtual String FileName { get; set; }
        public virtual DateTime CreatedTime { get; set; }
        public virtual String Description { get; set; }
        public virtual UserProfile Owner { get; set; }

        [JsonIgnore]
        [ScriptIgnore]
        public virtual Byte[] Data { get; set; }
    }
}