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
using System.Globalization;
using System.Resources;

namespace CJMach.Cryptography.Properties {
    internal class Resources {
        private static ResourceManager resourceMan;

        internal static ResourceManager ResourceManager {
            get {
                if (Object.ReferenceEquals(resourceMan, null)) {
                    ResourceManager temp = new ResourceManager("CJMach.Cryptography.Properties.Resources", typeof(Resources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }

        internal static CultureInfo Culture { get; set; }

        internal static String RootCommandDescription {
            get { return ResourceManager.GetString("RootCommandDescription", Culture); }
        }

        internal static String PrintCommandDescription {
            get { return ResourceManager.GetString("PrintCommandDescription", Culture); }
        }

        internal static String CheckCommandDescription {
            get { return ResourceManager.GetString("CheckCommandDescription", Culture); }
        }

        internal static String AlgorithmOptionDescription {
            get { return ResourceManager.GetString("AlgorithmOptionDescription", Culture); }
        }

        internal static String FilesArgumentDescription {
            get { return ResourceManager.GetString("FilesArgumentDescription", Culture); }
        }

        internal static String IOFileError {
            get { return ResourceManager.GetString("IOFileError", Culture); }
        }

        internal static String InvalidHashAlgorithmError {
            get { return ResourceManager.GetString("InvalidHashAlgorithmError", Culture); }
        }

        internal static String InvalidHashStringError {
            get { return ResourceManager.GetString("InvalidHashStringError", Culture); }
        }

        internal static String InvalidModeError {
            get { return ResourceManager.GetString("InvalidModeError", Culture); }
        }

        internal static String InvalidFileNameError {
            get { return ResourceManager.GetString("InvalidFileNameError", Culture); }
        }

        internal static String InvalidChecksumStringError {
            get { return ResourceManager.GetString("InvalidChecksumStringError", Culture); }
        }

        internal static String VerifySuccessMessage {
            get { return ResourceManager.GetString("VerifySuccessMessage"); }
        }

        internal static String VerifyFailedMessage {
            get { return ResourceManager.GetString("VerifyFailedMessage"); }
        }
    }
}