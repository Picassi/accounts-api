using System;
using System.Linq;

namespace Picassi.Api.Accounts.Tests.Framework
{
    public class TestSecretGenerator
    {
        private static readonly Random Random = new Random();

        public static string GetSecret()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, 16)
                .Select(s => s[Random.Next(s.Length)]).ToArray());
        }

        public static string GetName()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, 16)
                .Select(s => s[Random.Next(s.Length)]).ToArray());
        }

        public static string GetPassword()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, 16)
                .Select(s => s[Random.Next(s.Length)]).ToArray());
        }

    }
}
