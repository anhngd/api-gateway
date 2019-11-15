using System.Collections.Generic;
using System.Linq;

namespace IdentityServer.Common
{
    public static class ValidationExtension
    {

        public static string ValidationPassword(string password)
        {
            if(password.Length <= 8)
                return "Passwords must be at least 8 characters long!";

            var words1 = new List<string>();
            var words2 = new List<string>();
            var words3 = new List<string>();
            var words4 = new List<string>();

            words1.Add("a"); words2.Add("A"); words3.Add("!");
            words1.Add("b"); words2.Add("B"); words3.Add("@");
            words1.Add("c"); words2.Add("C"); words3.Add("#");
            words1.Add("d"); words2.Add("D"); words3.Add("$");
            words1.Add("e"); words2.Add("E"); words3.Add("%");
            words1.Add("f"); words2.Add("F"); words3.Add("^");
            words1.Add("g"); words2.Add("G"); words3.Add("&");
            words1.Add("h"); words2.Add("H"); words3.Add("*");
            words1.Add("i"); words2.Add("I"); words3.Add("(");
            words1.Add("j"); words2.Add("J"); words3.Add(")");
            words1.Add("k"); words2.Add("K"); words4.Add("1");
            words1.Add("l"); words2.Add("L"); words4.Add("2");
            words1.Add("m"); words2.Add("M"); words4.Add("3");
            words1.Add("n"); words2.Add("N"); words4.Add("4");
            words1.Add("o"); words2.Add("O"); words4.Add("5");
            words1.Add("p"); words2.Add("P"); words4.Add("6");
            words1.Add("q"); words2.Add("Q"); words4.Add("7");
            words1.Add("r"); words2.Add("R"); words4.Add("8");
            words1.Add("s"); words2.Add("S"); words4.Add("9");
            words1.Add("t"); words2.Add("T"); words4.Add("0");
            words1.Add("u"); words2.Add("U");
            words1.Add("v"); words2.Add("V");
            words1.Add("w"); words2.Add("W");
            words1.Add("y"); words2.Add("X");
            words1.Add("x"); words2.Add("Y");
            words1.Add("z"); words2.Add("Z");

            var count = 0;

            foreach (var word in words1)
            {
                if (password.Contains(word))
                    count++;
            }
            if (count == 0)
                return "password_contain";
            count = 0;

            count += words2.Count(password.Contains);
            if (count == 0)
                return "password_contain";
            count = 0;

            count += words4.Count(password.Contains);
            if (count == 0)
                return "password_contain";

            return "ok";
        }
    }
}
