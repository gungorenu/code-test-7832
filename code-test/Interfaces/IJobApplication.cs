namespace CodeTest
{
    /// <summary>
    /// Basic interface for job application, the return for every function is next available steps
    /// </summary>
    public interface IJobApplication
    {
        Operations MakeApplication();

        Operations DeleteApplication();

        Operations Register();

        Operations Initialize();

        Operations Update();

        Operations UpdateMetadata();

        Operations AddAttachment();

        Operations UploadAttachment();

        Operations ViewApplication();
    }
}
