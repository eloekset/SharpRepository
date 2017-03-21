using System;
using System.ComponentModel.DataAnnotations;
#if !NETSTANDARD1_6
using MongoDB.Bson.Serialization.Attributes;
#endif

namespace SharpRepository.Tests.TestObjects.PrimaryKeys
{
    public class ObjectKeys
    {
        public int Id { get; set; }

#if NETSTANDARD1_6
        [Key]
#else
        [BsonId]
#endif
        public int KeyInt1 { get; set; }

        [Key]
        public int KeyInt2 { get; set; }

        public Guid KeyGuid { get; set; }

        public string KeyString { get; set; }
    }
}
