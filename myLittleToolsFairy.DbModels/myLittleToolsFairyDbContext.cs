using Microsoft.EntityFrameworkCore;
using myLittleToolsFairy.DbModels.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myLittleToolsFairy.DbModels
{
    public class myLittleToolsFairyDbContext : DbContext
    {
        /// <summary>
        /// 宣告資料庫連結字串變數
        /// </summary>
        private string ConnectionStr;

        /// <summary>
        /// 透過參數傳入資料庫連結字串
        /// </summary>
        public myLittleToolsFairyDbContext(string connectionStr)
        {
            ConnectionStr = connectionStr;
        }

        public myLittleToolsFairyDbContext(DbContextOptions<myLittleToolsFairyDbContext> options) : base(options)
        {
        }

        /// <summary>
        /// 命名規則: 將對應到的 EntityModel 實體類別的名稱後墜的Entity改為複數: Entities
        /// </summary>
        public virtual DbSet<UserEntity> UserEntities { get; set; }

        /// <summary>
        /// 設置DbContext需要的參數，例如: 資料庫連結字串...等
        /// </summary>
        /// <param name="optionsBuilder"></param>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // 判斷式: 如果DbContext中的optionsBuilder沒有配置
            if (!optionsBuilder.IsConfigured)
            {
                // 那就把資料庫連線字串傳進去
                optionsBuilder.UseSqlServer(ConnectionStr);
            }
        }

        /// <summary>
        /// 配置資料庫模型與實體之間的映射規則
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserEntity>(entity =>
            {
                entity.ToTable("User").HasKey(u => u.UserId);
            });
        }
    }
}