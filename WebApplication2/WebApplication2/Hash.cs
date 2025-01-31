﻿using System;
using System.Security.Cryptography;
using System.Text;

namespace WebApplication2;

static class Hash
{
    public static string createHash(string stringInput)
    {
        SHA256 hash = SHA256.Create();

        return BitConverter.ToString(hash.ComputeHash(Encoding.UTF8.GetBytes(stringInput)));
    }
}

