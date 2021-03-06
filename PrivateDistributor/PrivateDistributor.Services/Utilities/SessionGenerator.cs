﻿using System;
using System.Text;

namespace PrivateDistributor.Services.Utilities
{
    public static class SessionGenerator
    {
        private static Random rand = new Random();
        private const int SessionKeyLength = 50;
        private const int SessionKeyCharsLength = 52;

        private const string SessionKeyChars =
            "qwertyuioplkjhgfdsazxcvbnmQWERTYUIOPLKJHGFDSAZXCVBNM";

        public static string GenerateSessionKey(int userId)
        {
            StringBuilder sKeyBuilder = new StringBuilder(SessionKeyLength);
            sKeyBuilder.Append(userId);
            while (sKeyBuilder.Length < SessionKeyLength)
            {
                var index = rand.Next(SessionKeyCharsLength);
                sKeyBuilder.Append(SessionKeyChars[index]);
            }
            return sKeyBuilder.ToString();
        }
    }
}