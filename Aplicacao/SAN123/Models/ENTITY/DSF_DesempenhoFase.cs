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
    
    public partial class DSF_DesempenhoFase
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public DSF_DesempenhoFase()
        {
            this.DES_Desempenho = new HashSet<DES_Desempenho>();
        }
    
        public int DSF_ID { get; set; }
        public string DSF_Fase { get; set; }
        public Nullable<System.DateTime> DSF_DataCadastro { get; set; }
        public Nullable<System.DateTime> DSF_DataAlteracao { get; set; }
        public string DSF_Anotacao { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DES_Desempenho> DES_Desempenho { get; set; }
    }
}
