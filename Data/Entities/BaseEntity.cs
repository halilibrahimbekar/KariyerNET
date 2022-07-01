using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Service.Entities
{
    public class BaseEntity
    {
        /// <summary>Id değer</summary>
        [Column("id")]
        public Guid Id { get; set; }

        /// <summary>Oluşturulma Zamanı</summary>
        [Column("createddatetime")]
        public DateTime CreatedDateTime { get; set; }
    }
}
