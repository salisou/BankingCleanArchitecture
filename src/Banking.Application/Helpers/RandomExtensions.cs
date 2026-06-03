using System;
using System.Text;

namespace Banking.Application.Helpers 
{
    public static class RandomExtensions
    {
        public static string NextAllDigits(this Random random, int length)
        {
            var sb = new StringBuilder();
            for (int i = 0; i < length; i++)
            {
                sb.Append(random.Next(0, 10));
            }
            return sb.ToString();
        }
    }
}