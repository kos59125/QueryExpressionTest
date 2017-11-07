module QueryExpressionTest.Data

open System
open Microsoft.EntityFrameworkCore

module inMemory =
   let Account = ResizeArray()
   let User = ResizeArray()
   let Team = ResizeArray()
   let TeamMember = ResizeArray()
   let Repository = ResizeArray()
   
open inMemory

let db = new Models.TestDbContext()

let clearDatabase () =
   use connection = db.Database.GetDbConnection()
   connection.OpenAsync() |> ignore
   
   ["repository"; "team_member"; "team"; "user"; "account"]
   |> List.iter (fun table ->
      use command = connection.CreateCommand()
      command.CommandText <- sprintf "DELETE FROM %s" table
      command.ExecuteNonQuery() |> ignore
   )

let createUser name =
   let accountId = Guid.NewGuid().ToString()
   let account = Models.Account(AccountId = accountId, AccountName = name)
   let user = Models.User(AccountId = accountId, Account = account)
   
   Account.Add(account)
   User.Add(user)
   
   db.Account.Add(account) |> ignore
   db.User.Add(user)|> ignore
   db.SaveChanges() |> ignore
   
   user

let createTeam name =
   let accountId = Guid.NewGuid().ToString()
   let account = Models.Account(AccountId = accountId, AccountName = name)
   let team = Models.Team(AccountId = accountId, Account = account)
   
   Account.Add(account)
   Team.Add(team)
   
   db.Account.Add(account) |> ignore
   db.Team.Add(team)|> ignore
   db.SaveChanges() |> ignore
   
   team

let associateMember (team:Models.Team) (user:Models.User) =
   let association = Models.TeamMember(TeamId = team.AccountId, MemberId = user.AccountId, Team = team, Member = user)
   
   TeamMember.Add(association)
   
   db.TeamMember.Add(association) |> ignore
   db.SaveChanges() |> ignore
   
   association

let createRepository (owner:Models.Account) name isPublic =
   let repositoryId = Guid.NewGuid().ToString()
   let visibility = match isPublic with | true -> "public" | false -> "private"
   let repository = Models.Repository(RepositoryId = repositoryId, RepositoryName = name, OwnerId = owner.AccountId, Owner = owner, Visibility = visibility)
   
   Repository.Add(repository)
   
   db.Repository.Add(repository) |> ignore
   db.SaveChanges() |> ignore
   
   repository
