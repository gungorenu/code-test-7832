namespace CodeTest
{
    /// <summary>
    /// Basic interface for job application, the return for every function is next available steps
    /// </summary>
    public interface IJobApplication
    {
        /// <summary>
        /// Runs message loop inside the JobApplication. exits when user opts to
        /// </summary>
        void Run();

        // SELF NOTE: since I moved run flow into class itself (instead of Program), below are not necessary anymore, can be gladly deleted
        // I leave for showcase purposes

        /// <summary>
        /// Is job application system initialized
        /// </summary>
        public bool IsInitialized { get; }

        /// <summary>
        /// Finalized application
        /// </summary>
        /// <returns>Next available operations</returns>
        Operations MakeApplication();

        /// <summary>
        /// Deletes application without finalizing
        /// </summary>
        /// <returns>Next available operations</returns>
        Operations DeleteApplication();

        /// <summary>
        /// Registers application, first step
        /// </summary>
        /// <returns>Next available operations</returns>
        Operations Register();

        /// <summary>
        /// Initializes system, must be called first
        /// </summary>
        /// <returns>Next available operations</returns>
        Operations Initialize();

        /// <summary>
        /// Updates initial information
        /// </summary>
        /// <returns>Next available operations</returns>
        Operations Update();

        /// <summary>
        /// Updates metadata information
        /// </summary>
        /// <returns>Next available operations</returns>
        Operations UpdateMetadata();

        /// <summary>
        /// Adds attachment
        /// </summary>
        /// <returns>Next available operations</returns>
        Operations AddAttachment();

        /// <summary>
        /// Uploads attachment file
        /// </summary>
        /// <returns>Next available operations</returns>
        Operations UploadAttachment();

        /// <summary>
        /// Views what is sent to system
        /// </summary>
        /// <returns>Next available operations</returns>
        Operations ViewApplication();
    }

    /// <summary>
    /// This interface is only supposed to be used in testing, hence it is "Internal"
    /// </summary>
    internal interface IJobApplicationInternal : IJobApplication
    {
        string UserKey { get; }
        string AppKey { get; }
    }
}
