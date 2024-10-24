﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CodeData_Connection.Models.Database.Entidade
{
    public class Documento
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(45)]
        public string Numero { get; set; }

        [MaxLength(100)]
        public string Nome { get; set; }

        [Column(TypeName = "MEDIUMBLOB")]
        public string Anexo { get; set; }

        [MaxLength(20)]
        public string Tipo { get; set; }

        [Column(TypeName = "DATETIME")]
        public DateTime? DataCadastro { get; set; }

        [Column(TypeName = "DATETIME")]
        public DateTime? DataAtualizado { get; set; }
    }
}
