module QueryExpressionTest.Program

open QueryExpressionTest.Data

let checkQuery (currentUser:Models.User) =
   printfn "----------------------------------------"
   printfn "Current User: %s" currentUser.Account.AccountName
   printfn "----------------------------------------"
   printfn "DB query expression (using contains)"
   query {
      for repository in db.Repository do
      where (
         repository.OwnerId = currentUser.AccountId
         || query {
            for m in db.TeamMember do
            where (m.MemberId = currentUser.AccountId)
            select m.TeamId
            contains repository.OwnerId
         }
      )
      select repository
   } |> Seq.iter (fun r -> printfn "%s" r.RepositoryName)
   printfn ""
   
   printfn "DB query expression (using exists)"
   query {
      for repository in db.Repository do
      where (
         repository.OwnerId = currentUser.AccountId
         || query {
            for m in db.TeamMember do
            where (m.MemberId = currentUser.AccountId)
            exists (m.TeamId = repository.OwnerId)
         }
      )
      select repository
   } |> Seq.iter (fun r -> printfn "%s" r.RepositoryName)
   printfn ""
   
   let q = query {
      for repository in db.Repository do
      select repository
   }
   
   printfn "Local query expression (using contains)"
   query {
      for repository in inMemory.Repository do
      where (
         repository.OwnerId = currentUser.AccountId
         || query {
            for m in inMemory.TeamMember do
            where (m.MemberId = currentUser.AccountId)
            select m.TeamId
            contains repository.OwnerId
         }
      )
      select repository
   } |> Seq.iter (fun r -> printfn "%s" r.RepositoryName)
   printfn ""

   printfn "Local query expression (using exists)"
   query {
      for repository in inMemory.Repository do
      where (
         repository.OwnerId = currentUser.AccountId
         || query {
            for m in inMemory.TeamMember do
            where (m.MemberId = currentUser.AccountId)
            exists (m.TeamId = repository.OwnerId)
         }
      )
      select repository
   } |> Seq.iter (fun r -> printfn "%s" r.RepositoryName)


[<EntryPoint>]
let main argv =
   clearDatabase ()
   
   let user1 = createUser "user1"
   let user2 = createUser "user2"
   let user3 = createUser "user3"
   
   let team1 = createTeam "team1"
   let team2 = createTeam "team2"
   
   associateMember team1 user1 |> ignore
   associateMember team1 user2 |> ignore
   associateMember team2 user2 |> ignore
   associateMember team2 user3 |> ignore
   
   for account in [user1.Account; user2.Account; user3.Account; team1.Account; team2.Account] do
      for isPublic in [true; false] do
         let visibility = match isPublic with | true -> "public" | false -> "private"
         let repositoryName = sprintf "%s_%s"  account.AccountName visibility
         createRepository account repositoryName isPublic |> ignore

   
   checkQuery user1
   checkQuery user2

   0
