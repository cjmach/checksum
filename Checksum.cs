/*
 *    Copyright 2023 Carlos Machado
 *
 *   Licensed under the Apache License, Version 2.0 (the "License");
 *   you may not use this file except in compliance with the License.
 *   You may obtain a copy of the License at
 *
 *       http://www.apache.org/licenses/LICENSE-2.0

 *   Unless required by applicable law or agreed to in writing, software
 *   distributed under the License is distributed on an "AS IS" BASIS,
 *   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 *   See the License for the specific language governing permissions and
 *   limitations under the License.
 */
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

using CJMach.Cryptography.Properties;

namespace CJMach.Cryptography {

    /// <summary>
    /// Simplifies the calculation and verification of one or 
    /// more file hash checksums.
    /// </summary>
    public class Checksum {

        /// <summary>
        /// Calculates and prints file hash checksums.
        /// </summary>
        /// <param name="algorithm">Name of the algorithm to use.</param>
        /// <param name="fileNames">The list of files to process.</param>
        public void Print(String algorithm, String[] fileNames) {
            using (HashAlgorithm algo = HashAlgorithm.Create(algorithm)) {
                if (algo == null) {
                    Console.Error.WriteLine(Resources.InvalidHashAlgorithmError, algorithm);
                    return;
                }
                if (fileNames == null || fileNames.Length == 0 || fileNames[0] == "-") {
                    using (Stream inStream = Console.OpenStandardInput()) {
                        String hash = CalculateHash(algo, inStream);
                        Console.WriteLine("{0}  -", hash);
                    }
                } else {
                    foreach (String fileName in fileNames) {
                        String hash = CalculateHash(algo, fileName);
                        if (hash != null) {
                            Console.WriteLine("{0}  {1}", hash, fileName);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Calculates a file hash checksum.
        /// </summary>
        /// <param name="algorithm">The hash algorithm to use.</param>
        /// <param name="fileName">The file path.</param>
        /// <returns>The file hash checksum or null if an error occurred.</returns>
        public String CalculateHash(HashAlgorithm algorithm, String fileName) {
            try {
                using (FileStream fs = File.OpenRead(fileName)) {
                    String hash = CalculateHash(algorithm, fs);
                    return hash;
                }
            } catch (IOException ex) {
                Console.Error.WriteLine(Resources.IOFileError, fileName, ex.Message);
                return null;
            }
        }

        /// <summary>
        /// Calculates a stream hash checksum.
        /// </summary>
        /// <param name="algorithm">The hash algorithm to use.</param>
        /// <param name="stream">The stream to read.</param>
        /// <returns>The stream hash checksum.</returns>
        public String CalculateHash(HashAlgorithm algorithm, Stream stream) {
            byte[] hashBytes = algorithm.ComputeHash(stream);
            return ToHexString(hashBytes);
        }

        /// <summary>
        /// Verify (or check) the file hash checksums.
        /// </summary>
        /// <param name="algorithm">Name of the hash algorithm to use.</param>
        /// <param name="checksumFileNames">List of files to process.</param>
        public void Verify(String algorithm, String[] checksumFileNames) {
            using (HashAlgorithm algo = HashAlgorithm.Create(algorithm)) {
                if (algo == null) {
                    Console.Error.WriteLine(Resources.InvalidHashAlgorithmError, algorithm);
                    return;
                }
                if (checksumFileNames == null || checksumFileNames.Length == 0 || checksumFileNames[0] == "-") {
                    VerifyResult result = Verify(algo, Console.OpenStandardInput());
                    TextWriter writer = result.Success ? Console.Out : Console.Error;
                    writer.WriteLine(result.Message);
                } else {
                    foreach (String checksumFileName in checksumFileNames) {
                        VerifyResult result = Verify(algo, checksumFileName);
                        TextWriter writer = result.Success ? Console.Out : Console.Error;
                        writer.WriteLine(result.Message);
                    }
                }
            }
        }

        /// <summary>
        /// Verify (or check) the file hash checksum.
        /// </summary>
        /// <param name="algorithm">The hash algorithm to use</param>
        /// <param name="checksumFileName">The file path.</param>
        /// <returns>The result of the hash verification.</returns>
        public VerifyResult Verify(HashAlgorithm algorithm, String checksumFileName) {
            try {
                using (FileStream fs = File.OpenRead(checksumFileName)) {
                    VerifyResult result = Verify(algorithm, fs);
                    return result;
                }
            } catch (IOException ex) {
                Console.Error.WriteLine(Resources.IOFileError, checksumFileName, ex.Message);
                return null;
            }
        }

        /// <summary>
        /// Verify (or check) the stream hash checksum.
        /// </summary>
        /// <param name="algorithm">The hash algorithm to use.</param>
        /// <param name="stream">The stream to process.</param>
        /// <returns>The result of the hash verification.</returns>
        public VerifyResult Verify(HashAlgorithm algorithm, Stream stream) {
            using (StreamReader reader = new StreamReader(stream, Encoding.UTF8)) {
                String line = reader.ReadLine();

                int hashStringSize = (algorithm.HashSize / 8) * 2;
                int indexOfSeparator = line.IndexOf(' ');
                if (indexOfSeparator != hashStringSize) {
                    return new VerifyResult(false, Resources.InvalidHashStringError);
                }
                if (indexOfSeparator + 1 >= line.Length) {
                    return new VerifyResult(false, Resources.InvalidChecksumStringError);
                }
                char mode = line[indexOfSeparator + 1];
                if (mode != ' ' && mode != '*') {
                    return new VerifyResult(false, Resources.InvalidModeError);
                }
                if (indexOfSeparator + 2 >= line.Length) {
                    return new VerifyResult(false, Resources.InvalidFileNameError);
                }
                String hash = line.Substring(0, hashStringSize);
                String file = line.Substring(indexOfSeparator + 2);
                try {
                    using (FileStream fs = File.OpenRead(file)) {
                        String fileHash = CalculateHash(algorithm, fs);
                        if (hash.Equals(fileHash)) {
                            return new VerifyResult(true, String.Format(Resources.VerifySuccessMessage, file));
                        }
                        return new VerifyResult(false, String.Format(Resources.VerifyFailedMessage, file));
                    }
                } catch (IOException ex) {
                    return new VerifyResult(false, String.Format(Resources.IOFileError, file, ex.Message));
                }
            }
        }

        /// <summary>
        /// Converts a byte array to a hexadecimal string.
        /// </summary>
        /// <param name="bytes">The byte array to convert.</param>
        /// <returns>A hexadecimal string representing the byte array that was passed by parameter.</returns>
        public static String ToHexString(byte[] bytes) {
            StringBuilder builder = new StringBuilder();
            foreach (byte b in bytes) {
                builder.AppendFormat("{0:x2}", b);
            }
            String hash = builder.ToString();
            return hash;
        }

        /// <summary>
        /// 
        /// </summary>
        public class VerifyResult {
            /// <summary>
            /// 
            /// </summary>
            public bool Success { get; private set; }

            /// <summary>
            /// 
            /// </summary>
            public String Message { get; private set; }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="success"></param>
            /// <param name="message"></param>
            internal VerifyResult(bool success, String message) {
                Success = success;
                Message = message;
            }
        }
    }
}