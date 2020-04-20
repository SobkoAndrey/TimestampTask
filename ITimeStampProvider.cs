using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimestampTask
{
    public interface ITimeStampProvider
    {
        /// <summary>
        /// Возвращает уникальную отметку времени.
        /// </summary>
        DateTime GetTimeStampWithLock { get; }

        /// <summary>
        /// Возвращает уникальную отметку времени.
        /// </summary>
        DateTime GetTimeStampLockFree { get; }
    }
}
