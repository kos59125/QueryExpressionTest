using System;
using System.Linq;

namespace QueryExpressionTest
{
   class Program
   {
      static void CheckQuery(Models.User currentUser)
      {
         Console.WriteLine("----------------------------------------");
         Console.WriteLine($"Current User: {currentUser.Account.AccountName}");
         Console.WriteLine("----------------------------------------");
         
         Console.WriteLine("DB query expression (using contains)");
         foreach (var r in Data.db.Repository.Where(
            repository =>
            repository.OwnerId == currentUser.AccountId
            || Data.db.TeamMember.Where(m => m.MemberId == currentUser.AccountId).Select(m => m.TeamId).Contains(repository.OwnerId)
         ))
         {
            Console.WriteLine(r.RepositoryName);
         }
         Console.WriteLine();

         Console.WriteLine("DB query expression (using exists)");
         foreach (var r in Data.db.Repository.Where(
            repository =>
            repository.OwnerId == currentUser.AccountId
            || Data.db.TeamMember.Where(m => m.MemberId == currentUser.AccountId).Any(m => m.MemberId == repository.OwnerId)
         ))
         {
            Console.WriteLine(r.RepositoryName);
         }
         Console.WriteLine();

         Console.WriteLine("Local query expression (using contains)");
         foreach (var r in Data.inMemory.Repository.Where(
            repository =>
            repository.OwnerId == currentUser.AccountId
            || Data.inMemory.TeamMember.Where(m => m.MemberId == currentUser.AccountId).Select(m => m.TeamId).Contains(repository.OwnerId)
         ))
         {
            Console.WriteLine(r.RepositoryName);
         }
         Console.WriteLine();

         Console.WriteLine("Local query expression (using exists)");
         foreach (var r in Data.inMemory.Repository.Where(
            repository =>
            repository.OwnerId == currentUser.AccountId
            || Data.inMemory.TeamMember.Where(m => m.MemberId == currentUser.AccountId).Any(m => m.MemberId == repository.OwnerId)
         ))
         {
            Console.WriteLine(r.RepositoryName);
         }

      }
      
      static void Main(string[] args)
      {
         Data.clearDatabase();

         var user1 = Data.createUser("user1");
         var user2 = Data.createUser("user2");
         var user3 = Data.createUser("user3");

         var team1 = Data.createTeam("team1");
         var team2 = Data.createTeam("team2");

         Data.associateMember(team1, user1);
         Data.associateMember(team1, user2);
         Data.associateMember(team2, user2);
         Data.associateMember(team2, user3);

         foreach (var account in new Models.Account[] { user1.Account, user2.Account, user3.Account, team1.Account, team2.Account })
         {
            foreach (var isPublic in new bool[] { true, false })
            {
               var visibility = isPublic ? "public" : "private";
               var repositoryName = $"{account.AccountName}_{visibility}";
               Data.createRepository(account, repositoryName, isPublic);
            }
         }

         CheckQuery(user1);
         CheckQuery(user2);
      }
   }
}
