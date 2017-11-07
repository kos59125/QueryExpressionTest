module QueryExpressionTest.Models

open System
open System.Collections.Generic
open Microsoft.EntityFrameworkCore
open Microsoft.EntityFrameworkCore.Metadata

type [<AllowNullLiteral>] Account() =
   let mutable repository = HashSet()
   
   member val AccountId : string = null with get, set
   member val AccountName : string= null with get, set
   
   member val Team : Team = null with get, set
   member val User : User = null with get, set
   member this.Repository
      with get() = repository
      and set(v) = repository <- v

and [<AllowNullLiteral>] User() =
   let mutable teamMembers = HashSet()
   
   member val AccountId : string = null with get, set
   
   member val Account : Account = null with get, set
   member this.TeamMember
      with get() = teamMembers
      and set(v) = teamMembers <- v

and [<AllowNullLiteral>] Team() =
   let mutable teamMembers = HashSet()
   
   member val AccountId : string = null with get, set
   
   member val Account : Account = null with get, set
   member this.TeamMember
      with get() = teamMembers
      and set(v) = teamMembers <- v

and [<AllowNullLiteral>] TeamMember() =
   member val TeamId : string = null with get, set
   member val MemberId : string= null with get, set
   
   member val Team : Team = null with get, set
   member val Member : User = null with get, set

and [<AllowNullLiteral>] Repository() =
   member val RepositoryId : string = null with get, set
   member val RepositoryName : string = null with get, set
   member val OwnerId : string = null with get, set
   member val Visibility : string = null with get, set

   member val Owner : Account = null with get, set

type TestDbContext() =
   inherit DbContext()

   member val Account : DbSet<Account> = null with get, set
   member val Repository : DbSet<Repository> = null with get, set
   member val Team : DbSet<Team> = null with get, set
   member val User : DbSet<User> = null with get, set
   member val TeamMember : DbSet<TeamMember> = null with get, set

   override __.OnConfiguring(optionsBuilder:DbContextOptionsBuilder) =
      if not optionsBuilder.IsConfigured then
         optionsBuilder.UseSqlite(@"Data Source=db.sqlite3") |> ignore

   override __.OnModelCreating(modelBuilder:ModelBuilder) =
      modelBuilder.Entity<Account>(fun entity ->
         entity.ToTable("account") |> ignore
         
         entity.Property(fun e -> e.AccountId)
            .HasColumnName("account_id")
            .HasColumnType("CHAR(36)")
            .ValueGeneratedNever()
            |> ignore
         
         entity.Property(fun e -> e.AccountName)
            .IsRequired()
            .HasColumnName("account_name")
            .HasColumnType("VARCHAR(10)")
            |> ignore
      ) |> ignore
      
      modelBuilder.Entity<Repository>(fun entity ->
         entity.ToTable("repository") |> ignore
         
         entity.Property(fun e -> e.RepositoryId)
            .HasColumnName("repository_id")
            .HasColumnType("CHAR(36)")
            .ValueGeneratedNever()
            |> ignore
         
         entity.Property(fun e -> e.OwnerId)
            .IsRequired()
            .HasColumnName("owner_id")
            .HasColumnType("CHAR(36)")
            |> ignore
         
         entity.Property(fun e -> e.RepositoryName)
            .IsRequired()
            .HasColumnName("repository_name")
            .HasColumnType("VARCHAR(10)")
            |> ignore
         
         entity.Property(fun e -> e.Visibility)
            .IsRequired()
            .HasColumnName("visibility")
            .HasColumnType("VARCHAR(10)")
            |> ignore
         
         entity.HasOne(fun d -> d.Owner)
            .WithMany(fun p -> p.Repository)
            .HasForeignKey(fun d -> d.OwnerId)
            |> ignore
      ) |> ignore
      
      modelBuilder.Entity<Team>(fun entity ->
         entity.HasKey(Builders.KeyBuilder(fun e -> e.AccountId)) |> ignore
         
         entity.ToTable("team") |> ignore
         
         entity.Property(fun e -> e.AccountId)
            .HasColumnName("account_id")
            .HasColumnType("CHAR(36)")
            .ValueGeneratedNever()
            |> ignore
         
         entity.HasOne(fun d -> d.Account)
            .WithOne(fun p -> p.Team)
            .HasForeignKey<Team>(fun d -> d.AccountId)
            |> ignore
      ) |> ignore
      
      modelBuilder.Entity<TeamMember>(fun entity ->
         entity.HasKey(fun e -> e.TeamId, e.MemberId) |> ignore
         
         entity.ToTable("team_member") |> ignore
         
         entity.Property(fun e -> e.TeamId)
            .HasColumnName("team_id")
            .HasColumnType("CHAR(36)")
            |> ignore
         
         entity.Property(fun e -> e.MemberId)
            .HasColumnName("member_id")
            .HasColumnType("CHAR(36)")
            |> ignore
         
         entity.HasOne(fun d -> d.Member)
            .WithMany(fun p -> p.TeamMember)
            .HasForeignKey(fun d -> d.MemberId)
            |> ignore
         
         entity.HasOne(fun d -> d.Team)
            .WithMany(fun p -> p.TeamMember)
            .HasForeignKey(fun d -> d.TeamId)
            |> ignore
      ) |> ignore
      
      modelBuilder.Entity<User>(fun entity ->
         entity.HasKey(fun e -> e.AccountId) |> ignore
         
         entity.ToTable("user") |> ignore
         
         entity.Property(fun e -> e.AccountId)
            .HasColumnName("account_id")
            .HasColumnType("CHAR(36)")
            .ValueGeneratedNever()
            |> ignore
         
         entity.HasOne(fun d -> d.Account)
            .WithOne(fun p -> p.User)
            .HasForeignKey<User>(fun d -> d.AccountId)
            |> ignore
      ) |> ignore
