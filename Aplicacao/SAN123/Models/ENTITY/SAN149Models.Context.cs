﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class db_SAN149Entities : DbContext
    {
        public db_SAN149Entities()
            : base("name=db_SAN149Entities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<UNG_UnidadeNegocio> UNG_UnidadeNegocio { get; set; }
        public virtual DbSet<DSP_Desemempenho> DSP_Desemempenho { get; set; }
        public virtual DbSet<PER_Periodos> PER_Periodos { get; set; }
        public virtual DbSet<USU_Usuario> USU_Usuario { get; set; }
    }
}
