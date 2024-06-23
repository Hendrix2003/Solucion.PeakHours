namespace SolucionPeakHours.Interfaces
{
    public interface IBackgroundTaskService
    {
        Task ExpireEntitiesTaskJob();
        Task RestartTotalHourPerMonthJob();
    }
}
