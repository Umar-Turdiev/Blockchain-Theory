﻿using System;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;

namespace CryptoBlocks
{
    public class Block
    {
        public Block(DateTime TimeStamp, object Transaction, string PreviousHash)
        {
            this.Timestamp = TimeStamp;
            this.Transaction = Transaction;
            this.PreviousHash = PreviousHash;
            this.Hash = GenerateHash(3);
        }


        #region Properties
        public DateTime Timestamp { get; }

        public object Transaction { get; set; }

        public string Hash { get; set; }
        public string PreviousHash { get; set; }

        public int Nonce { get; set; } = 0;
        #endregion


        /// <summary>
        /// Calculate hash for current block using SHA256.
        /// </summary>
        /// <returns>
        /// The string representation of the current block's hash.
        /// </returns>
        public string CalculateHash()
        {
            string hashString = string.Empty;
            string rawData = $"{this.Timestamp:yyyy/MM/dd/HH:mm:ss}{this.Transaction}{this.PreviousHash}{this.Nonce}";

            SHA256 sha256Hash = SHA256.Create();


            //Compute hash
            byte[] hash = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));


            foreach (byte item in hash)
            {
                hashString += item.ToString("x2");
            }


            return hashString;
        }


        private string GenerateHash(int difficulty)
        {
            string hashString = this.CalculateHash();

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            while (hashString.Substring(0, difficulty) != new string('0', difficulty))
            {
                this.Nonce++;
                this.Hash = hashString;
                hashString = this.CalculateHash();

                //Console.WriteLine(hashString + "\t" + Nonce + "\t" + stopwatch.Elapsed);
            }

            stopwatch.Stop();

            Console.WriteLine("Mined\t" + stopwatch.Elapsed);

            return hashString;
        }


        public override string ToString()
        {
            return $"{Environment.NewLine}" +
                $"\tTimestamp: \"{this.Timestamp}\"{Environment.NewLine}" +
                $"\tTransaction: \"{this.Transaction}\"{Environment.NewLine}" +
                $"\tHash: \"{this.Hash}\"{Environment.NewLine}" +
                $"\tPreviousHash: \"{this.PreviousHash}\"{Environment.NewLine}" +
                $"\tNonece: \"{this.Nonce}\"{Environment.NewLine}";
        }
    }
}
