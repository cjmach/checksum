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
using System.CommandLine;
using CJMach.Cryptography.Properties;

namespace CJMach.Cryptography {

    /// <summary>
    /// 
    /// </summary>
    class Program {

        /// <summary>
        /// Application main entry point.
        /// </summary>
        /// <param name="args">Command-line arguments</param>
        static void Main(String[] args) {
            Option<String> algoOpt = new Option<String>(
                aliases: new String[] { "-a", "--algorithm" },
                description: Resources.AlgorithmOptionDescription,
                getDefaultValue: () => "sha256"
            );
            Option<bool> checkOpt = new Option<bool>(
                aliases: new String[] { "-c", "--check" },
                description: Resources.CheckCommandDescription
            );
            Argument<String[]> filesArg = new Argument<String[]>(
                name: "FILEs",
                description: Resources.FilesArgumentDescription
            );

            RootCommand rootCommand = new RootCommand(Resources.RootCommandDescription);
            rootCommand.AddOption(algoOpt);
            rootCommand.AddOption(checkOpt);
            rootCommand.AddArgument(filesArg);
            rootCommand.SetHandler(Run, algoOpt, checkOpt, filesArg);
            rootCommand.Invoke(args);
        }

        /// <summary>
        /// Print or check hash checksums.
        /// </summary>
        /// <param name="algorithm">Name of the hash algorithm to use</param>
        /// <param name="check">True if it's to check the hash checksum or false to calculate and print the hash.</param>
        /// <param name="fileNames">The list of files to process.</param>
        static void Run(String algorithm, bool check, String[] fileNames) {
            if (check) {
                VerifyHash(algorithm, fileNames);
            } else {
                PrintHash(algorithm, fileNames);
            }
        }

        /// <summary>
        /// Print hash checksums.
        /// </summary>
        /// <param name="algorithm">Name of the hash algorithm to use</param>
        /// <param name="fileNames">The list of files to process.</param>
        static void PrintHash(String algorithm, String[] fileNames) {
            Checksum cs = new Checksum();
            cs.Print(algorithm, fileNames);
        }

        /// <summary>
        /// Check hash checksums.
        /// </summary>
        /// <param name="algorithm">Name of the hash algorithm to use.</param>
        /// <param name="checksumFileNames">The list of files to process.</param>
        static void VerifyHash(String algorithm, String[] checksumFileNames) {
            Checksum cs = new Checksum();
            cs.Verify(algorithm, checksumFileNames);
        }
    }
}
