using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace BookStoreWebApplication.Models;

public partial class DbbookStoreContext : DbContext
{
    public DbbookStoreContext()
    {
    }

    public DbbookStoreContext(DbContextOptions<DbbookStoreContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Author> Authors { get; set; }

    public virtual DbSet<AuthorsBook> AuthorsBooks { get; set; }

    public virtual DbSet<AuthorsGenre> AuthorsGenres { get; set; }

    public virtual DbSet<Availability> Availabilities { get; set; }

    public virtual DbSet<Book> Books { get; set; }

    public virtual DbSet<BooksGenre> BooksGenres { get; set; }

    public virtual DbSet<Bookstore> Bookstores { get; set; }

    public virtual DbSet<Buyer> Buyers { get; set; }

    public virtual DbSet<Genre> Genres { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderItem> OrderItems { get; set; }

    public virtual DbSet<Worker> Workers { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Server=DESKTOP-97Q88N1; Database=DBBookStore; Trusted_Connection=True; Trust Server Certificate=true");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Author>(entity =>
        {
            entity.Property(e => e.BirthDate)
                .HasColumnType("datetime")
                .HasColumnName("Birth_Date");
            //entity.Property(e => e.Genres).HasColumnType("ntext");
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.ShortBiography)
                .HasColumnType("ntext")
                .HasColumnName("Short_Biography");
        });

        modelBuilder.Entity<AuthorsBook>(entity =>
        {
            entity.Property(e => e.AuthorId).HasColumnName("Author_Id");
            entity.Property(e => e.BookId).HasColumnName("Book_Id");

            entity.HasOne(d => d.Author).WithMany(p => p.AuthorsBooks)
                .HasForeignKey(d => d.AuthorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_AuthorsBooks_Authors");

            entity.HasOne(d => d.Book).WithMany(p => p.AuthorsBooks)
                .HasForeignKey(d => d.BookId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_AuthorsBooks_Books");
        });

        modelBuilder.Entity<AuthorsGenre>(entity =>
        {
            entity.Property(e => e.AuthorId).HasColumnName("Author_Id");
            entity.Property(e => e.GenreId).HasColumnName("Genre_Id");

            entity.HasOne(d => d.Author).WithMany(p => p.AuthorsGenres)
                .HasForeignKey(d => d.AuthorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_AuthorsGenres_Authors");

            entity.HasOne(d => d.Genre).WithMany(p => p.AuthorsGenres)
                .HasForeignKey(d => d.GenreId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_AuthorsGenres_Genres");
        });

        modelBuilder.Entity<Availability>(entity =>
        {
            entity.ToTable("Availability");

            entity.Property(e => e.BookId).HasColumnName("Book_Id");
            entity.Property(e => e.BookstoreId).HasColumnName("Bookstore_Id");

            entity.HasOne(d => d.Book).WithMany(p => p.Availabilities)
                .HasForeignKey(d => d.BookId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Availability_Books");

            entity.HasOne(d => d.Bookstore).WithMany(p => p.Availabilities)
                .HasForeignKey(d => d.BookstoreId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Availability_Bookstores");
        });

        modelBuilder.Entity<Book>(entity =>
        {
            entity.Property(e => e.CoverType)
                .HasMaxLength(50)
                .HasColumnName("Cover_Type");
            entity.Property(e => e.PublicationYear).HasColumnName("Publication_Year");
        });

        modelBuilder.Entity<BooksGenre>(entity =>
        {
            entity.Property(e => e.BookId).HasColumnName("Book_Id");
            entity.Property(e => e.GenreId).HasColumnName("Genre_Id");

            entity.HasOne(d => d.Book).WithMany(p => p.BooksGenres)
                .HasForeignKey(d => d.BookId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_BooksGenres_Books");

            entity.HasOne(d => d.Genre).WithMany(p => p.BooksGenres)
                .HasForeignKey(d => d.GenreId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_BooksGenres_Genres");
        });

        modelBuilder.Entity<Bookstore>(entity =>
        {
            entity.Property(e => e.City).HasMaxLength(100);
        });

        modelBuilder.Entity<Buyer>(entity =>
        {
            entity.ToTable("Buyer");

            entity.Property(e => e.Address).HasColumnType("ntext");
            entity.Property(e => e.BirthDate)
                .HasColumnType("date")
                .HasColumnName("Birth_Date");
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(50)
                .HasColumnName("Phone_Number");
        });

        modelBuilder.Entity<Genre>(entity =>
        {
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Buys");

            entity.Property(e => e.BuyerId).HasColumnName("Buyer_Id");
            entity.Property(e => e.Date).HasColumnType("datetime");
            entity.Property(e => e.SellerId).HasColumnName("Seller_Id");
            entity.Property(e => e.Price).HasColumnName("Price");
            
            entity.HasOne(d => d.Buyer).WithMany(p => p.Orders)
                .HasForeignKey(d => d.BuyerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Buys_Buyer");

            entity.HasOne(d => d.Seller).WithMany(p => p.Orders)
                .HasForeignKey(d => d.SellerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Buys_Workers");
        });

        modelBuilder.Entity<OrderItem>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Items");

            entity.Property(e => e.BookId).HasColumnName("Book_Id");
            entity.Property(e => e.BuyId).HasColumnName("Buy_Id");

            entity.HasOne(d => d.Book).WithMany(p => p.OrderItems)
                .HasForeignKey(d => d.BookId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Items_Books");

            entity.HasOne(d => d.Buy).WithMany(p => p.OrderItems)
                .HasForeignKey(d => d.BuyId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Items_Buys");
        });

        modelBuilder.Entity<Worker>(entity =>
        {
            entity.Property(e => e.BirthDate)
                .HasColumnType("date")
                .HasColumnName("Birth_Date");
            entity.Property(e => e.BookstoreId).HasColumnName("Bookstore_Id");

            entity.Property(e => e.Name).HasMaxLength(100);

            entity.HasOne(d => d.Bookstore).WithMany(p => p.Workers)
                .HasForeignKey(d => d.BookstoreId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Workers_Bookstores");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
