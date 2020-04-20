using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TimestampTask
{
    public class TimeStampProvider : ITimeStampProvider
    {
        private static readonly Lazy<TimeStampProvider> sharedInstance =
            new Lazy<TimeStampProvider>(() => new TimeStampProvider());

        private int lastMillisecond;
        private object locker = new object();

        public DateTime GetTimeStampLockFree
        {
            get
            {
                int locallastMillisecond;
                DateTime result;
                do
                {
                    locallastMillisecond = lastMillisecond;
                    var now = DateTime.Now;
                    result = new DateTime(now.Year, now.Month, now.Day,
                            now.Hour, now.Minute, now.Second, now.Millisecond);
                } while (locallastMillisecond != Interlocked.CompareExchange(ref lastMillisecond, result.Millisecond, locallastMillisecond) ||
                    result.Millisecond == locallastMillisecond);
                return result;
            }
        }

        public DateTime GetTimeStampWithLock
        {
            get
            {
                lock (locker)
                {
                    int locallastMillisecond;
                    DateTime result;
                    do
                    {
                        locallastMillisecond = lastMillisecond;
                        var now = DateTime.Now;
                        result = new DateTime(now.Year, now.Month, now.Day,
                                now.Hour, now.Minute, now.Second, now.Millisecond);
                    } while (result.Millisecond == lastMillisecond);
                    lastMillisecond = result.Millisecond;
                    return result;
                }
            }
        }

        private TimeStampProvider()
        {
        }

        public static TimeStampProvider GetInstance()
        {
            return sharedInstance.Value;
        }
    }
}
