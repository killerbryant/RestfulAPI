using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestfulAPI.Model.Models
{
    [Table("Departments")]
    public class Departments : IEntity
    {
        /// <summary>
        /// DeptNo
        /// </summary>
        [Key]
        [Column("DeptNo")]
        public int DeptNo { get; set; }

        /// <summary>
        /// Dname
        /// </summary>
        [Column("Dname")]
        [StringLength(50)]
        public string Dname { get; set; }

        /// <summary>
        /// Location
        /// </summary>
        [Column("Location")]
        [StringLength(50)]
        public string Location { get; set; }
    }
}
