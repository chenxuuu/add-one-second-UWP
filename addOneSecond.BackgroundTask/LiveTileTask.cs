using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;
using Windows.UI.Notifications;

namespace addOneSecond.BackgroundTask
{
    public sealed class LiveTileTask : IBackgroundTask
    {
        public void Run(IBackgroundTaskInstance taskInstance)
        {
            var tileContent = new Uri("https://www.chenxublog.com/getsecond.php");  //自建网站
            var requestedInterval = PeriodicUpdateRecurrence.HalfHour;   //半小时一次

            var updater = TileUpdateManager.CreateTileUpdaterForApplication();
            updater.StartPeriodicUpdate(tileContent, requestedInterval);
            updater.StopPeriodicUpdate();
        }
    }
}
