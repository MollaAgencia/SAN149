//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Aplicacao.Models.ENTITY
{
    using System;
    using System.Collections.Generic;
    
    public partial class USU_Usuario
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public USU_Usuario()
        {
            this.DES_Desempenho = new HashSet<DES_Desempenho>();
            this.RKG_RankingGrupo = new HashSet<RKG_RankingGrupo>();
            this.LAU_LogAutenticacaoUsuario = new HashSet<LAU_LogAutenticacaoUsuario>();
        }
    
        public int USU_ID { get; set; }
        public string USU_Nome { get; set; }
        public string USU_CPF { get; set; }
        public int USU_CAR_ID { get; set; }
        public string USU_Login { get; set; }
        public string USU_Senha { get; set; }
        public string USU_EMAIL { get; set; }
        public string USU_Telefone { get; set; }
        public Nullable<System.DateTime> USU_DataCadastro { get; set; }
        public Nullable<System.DateTime> USU_DataAlteracao { get; set; }
        public string USU_Anotacao { get; set; }
        public Nullable<System.DateTime> USU_DataAceiteRegulamento { get; set; }
        public bool USU_AcessoFake { get; set; }
        public bool USU_Ativo { get; set; }
    
        public virtual CAR_Cargo CAR_Cargo { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DES_Desempenho> DES_Desempenho { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RKG_RankingGrupo> RKG_RankingGrupo { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<LAU_LogAutenticacaoUsuario> LAU_LogAutenticacaoUsuario { get; set; }
    }
}
