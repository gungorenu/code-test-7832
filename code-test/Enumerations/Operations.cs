using System;

namespace CodeTest
{
    /// <summary>
    /// Enum of Operations that can be done with application
    /// </summary>
    [Flags]
    public enum Operations : int
    {
        /// <summary>
        /// Base
        /// </summary>
        Refresh = 0x1,
        /// <summary>
        /// Exit application
        /// </summary>
        Exit = 0x2,
        /// <summary>
        /// Make application, can be done if all information is added
        /// </summary>
        MakeApplication = 0x4,
        /// <summary>
        /// Delete application
        /// </summary>
        DeleteApplication = 0x8,
        /// <summary>
        /// Register application
        /// </summary>
        Register = 0x10,
        /// <summary>
        /// Generate key to initiate application registration
        /// </summary>
        GenerateKey = 0x20,
        /// <summary>
        /// Updates the current application info
        /// </summary>
        Update = 0x40,
        /// <summary>
        /// Updates metadata
        /// </summary>
        UpdateMetadata = 0x80,
        /// <summary>
        /// Adds new attachment
        /// </summary>
        AddAttachment = 0x100,
        /// <summary>
        /// Uploads attachment
        /// </summary>
        UploadAttachment = 0x200,
        /// <summary>
        /// Prints current application info
        /// </summary>
        ViewApplication = 0x400,
        /// <summary>
        /// Removes existing attachment
        /// </summary>
        RemoveAttachment = 0x800

    }
}
