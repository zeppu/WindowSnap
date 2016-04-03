namespace Snapinator.ViewModels
{
    public interface IConfigurationPartViewModel
    {
        string Title { get; }
        void CommitChanges();
    }
}