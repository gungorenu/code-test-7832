using System;

namespace CodeTest
{
    /// <summary>
    /// Enum of Operations that can be done with application
    /// </summary>
    [Flags]
    public enum Operations
    {
        /// <summary>
        /// Base
        /// </summary>
        None,
        /// <summary>
        /// Exit application
        /// </summary>
        Exit,
        /// <summary>
        /// Make application, can be done if all information is added
        /// </summary>
        MakeApplication,
        /// <summary>
        /// Delete application
        /// </summary>
        DeleteApplication,
        /// <summary>
        /// Register application
        /// </summary>
        Register,
        /// <summary>
        /// Generate key to initiate application registration
        /// </summary>
        GenerateKey,
        /// <summary>
        /// Updates the current application info
        /// </summary>
        Update,
        /// <summary>
        /// Updates metadata
        /// </summary>
        UpdateMetadata,
        /// <summary>
        /// Adds new attachment
        /// </summary>
        AddAttachment,
        /// <summary>
        /// Uploads attachment
        /// </summary>
        UploadAttachment,
        /// <summary>
        /// Prints current application info
        /// </summary>
        ViewApplication,

    }
}
