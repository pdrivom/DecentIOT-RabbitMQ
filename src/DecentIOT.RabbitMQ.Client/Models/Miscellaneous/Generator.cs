using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace DecentIOT.RabbitMQ.Miscellaneous
{
    public static class Generator
    {
        public static string GenerateShortUid()
        {
            return Regex.Replace(Convert.ToBase64String(Guid.NewGuid().ToByteArray()), "[/+=]", "");
        }
    }
}
