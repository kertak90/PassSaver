using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace PassSaver
{
    public class CrypterService
    {
        public CrypterService(string[] args)
        {
            if(args != null && args.Length > 0)
            {
                using (SHA256 mySHA256 = SHA256.Create())
                {
                    try
                    {
                        var command = args[0];
                        var fileName = args[1];
                        var passwordWord1 = args[2];
                        var passwordWord2 = args[3];
                        var fileLines = File.ReadAllText(fileName);

                        var passwordBytes1 = Encoding.UTF8.GetBytes(passwordWord1);
                        var passwordBytes2 = Encoding.UTF8.GetBytes(passwordWord1);
                        passwordBytes1 = Combine(passwordBytes1, passwordBytes2);
                        var passwordHashBytes = SHA512.Create().ComputeHash(passwordBytes1);
                        
                        if(command == "encrypt")
                        {
                            var lineBytes = Encoding.UTF8.GetBytes(fileLines);
                            byte[] bytesEncrypted = null;
                            var encryptedline = string.Empty;
                            for(int i = 0; i < 3; i++)
                            {
                                bytesEncrypted = Encrypt(lineBytes, passwordHashBytes);

                                encryptedline = Convert.ToBase64String(bytesEncrypted);
                            }

                            // System.Console.WriteLine(encryptedline);

                            File.WriteAllText(fileName, encryptedline);
                        }
                        else if(command == "decrypt")
                        {
                            var lineBytes = Convert.FromBase64String(fileLines);
                            byte[] bytesDecrypted = null;
                            var decryptedline = string.Empty;
                            for(int i = 0; i < 3; i++)
                            {
                                bytesDecrypted = Decrypt(lineBytes, passwordHashBytes);

                                decryptedline = Encoding.UTF8.GetString(bytesDecrypted);
                            }

                            // System.Console.WriteLine(decryptedline);

                            File.WriteAllText(fileName, decryptedline);
                        }
                        
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine($"I/O Exception: {e}");
                    }
                }
            }
        }
        public byte[] Encrypt(byte[] line, byte[] password)
        {
            // System.Console.WriteLine("encrypting");

            byte[] encryptedBytes = null;

            var saltBytes = new byte[]{1, 2, 3, 4, 5, 6, 7, 8};

            using(MemoryStream MS = new MemoryStream())
            {
                using(RijndaelManaged AES = new RijndaelManaged())
                {
                    var key = new Rfc2898DeriveBytes(password, saltBytes, 1000);

                    AES.KeySize = 256;
                    AES.BlockSize = 128;
                    AES.Key = key.GetBytes(AES.KeySize / 8);
                    AES.IV = key.GetBytes(AES.BlockSize / 8);
                    AES.Mode = CipherMode.CBC;
                    using(var cs = new CryptoStream(MS, AES.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(line, 0, line.Length);
                        cs.Close();
                    }
                    encryptedBytes = MS.ToArray();
                }
            }
            return encryptedBytes;
        }
        public byte[] Decrypt(byte[] bytesToBeDecrypted, byte[] password)
        {
            // System.Console.WriteLine("decrypting");

            byte[] decryptedBytes = null;

            var saltBytes = new byte[]{1, 2, 3, 4, 5, 6, 7, 8};

            using(MemoryStream MS = new MemoryStream())
            {
                using(RijndaelManaged AES = new RijndaelManaged())
                {
                    var key = new Rfc2898DeriveBytes(password, saltBytes, 1000);

                    AES.KeySize = 256;
                    AES.BlockSize = 128;
                    AES.Key = key.GetBytes(AES.KeySize / 8);
                    AES.IV = key.GetBytes(AES.BlockSize / 8);
                    AES.Mode = CipherMode.CBC;
                    using(var cs = new CryptoStream(MS, AES.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(bytesToBeDecrypted, 0, bytesToBeDecrypted.Length);
                        cs.Close();
                    }
                    decryptedBytes = MS.ToArray();
                }
            }
            return decryptedBytes;
        }
        public static byte[] Combine(byte[] first, byte[] second)
        {
            byte[] ret = new byte[first.Length + second.Length];
            Buffer.BlockCopy(first, 0, ret, 0, first.Length);
            Buffer.BlockCopy(second, 0, ret, first.Length, second.Length);
            return ret;
        }
    }    
}