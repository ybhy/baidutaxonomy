﻿//------------------------------------------------------------------------------
// <auto-generated>
//     此代码已从模板生成。
//
//     手动更改此文件可能导致应用程序出现意外的行为。
//     如果重新生成代码，将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace traversal
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class BaidudataEntities11 : DbContext
    {
        public BaidudataEntities11()
            : base("name=BaidudataEntities11")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<All> All { get; set; }
        public virtual DbSet<Concept> Concept { get; set; }
        public virtual DbSet<Distinct> Distinct { get; set; }
        public virtual DbSet<Distinct_copy> Distinct_copy { get; set; }
        public virtual DbSet<Entity> Entity { get; set; }
        public virtual DbSet<Instance> Instance { get; set; }
        public virtual DbSet<Relation> Relation { get; set; }
        public virtual DbSet<Relation2> Relation2 { get; set; }
        public virtual DbSet<Relation3> Relation3 { get; set; }
        public virtual DbSet<Relation4> Relation4 { get; set; }
        public virtual DbSet<Entity_copy> Entity_copy { get; set; }
    }
}
