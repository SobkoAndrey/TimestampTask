using FluentAssertions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimestampTask
{
    [TestFixture]
    class TimeStampProviderTest
    {
        /// <summary>
        /// Тестовый объект.
        /// </summary>
        private TimeStampProvider _timeStampProvider;

        /// <summary>
        /// Настройка теста.
        /// </summary>
        [SetUp]
        public void Setup()
        {
            _timeStampProvider = TimeStampProvider.GetInstance();
        }

        /// <summary>
        /// Тест на то, что провайдер возвращает только уникальные объекты.
        /// </summary>
        [Test]
        public void TimeStampProviderShouldReturnUniqueValuesTest()
        {
            // act
            var timeStamps = Enumerable
                .Range(0, 10000)
                .Select(_ => _timeStampProvider.GetTimeStampWithLock).ToArray();

            // assert
            timeStamps.Should().OnlyHaveUniqueItems();
        }

        /// <summary>
        /// Тест на то, что провайдер возвращает только уникальные объекты.
        /// </summary>
        [Test]
        public void ConcurrentTimeStampProviderShouldReturnUniqueValuesTest()
        {
            // act
            var timeStamps = Enumerable
                .Range(0, 10000)
                .AsParallel()
                .Select(_ => _timeStampProvider.GetTimeStampWithLock).ToArray();

            // assert
            timeStamps.Should().OnlyHaveUniqueItems();
        }

        /// <summary>
        /// Тест на то, что провайдер возвращает только уникальные объекты.
        /// </summary>
        [Test]
        public void TimeStampProviderShouldReturnRoundedValuesTest()
        {
            // act
            var timeStamps = Enumerable
                .Range(0, 1000)
                .AsParallel()
                .Select(_ => _timeStampProvider.GetTimeStampWithLock).ToArray();

            // assert
            foreach (var timeStamp in timeStamps)
            {
                (timeStamp.Ticks % TimeSpan.TicksPerMillisecond).Should().Be(0);
            }
        }

        /// <summary>
        /// Тест на то, что провайдер возвращает только уникальные объекты.
        /// </summary>
        [Test]
        public void LockFreeTimeStampProviderShouldReturnUniqueValuesTest()
        {
            // act
            var timeStamps = Enumerable
                .Range(0, 10000)
                .Select(_ => _timeStampProvider.GetTimeStampLockFree).ToArray();

            // assert
            timeStamps.Should().OnlyHaveUniqueItems();
        }

        /// <summary>
        /// Тест на то, что провайдер возвращает только уникальные объекты.
        /// </summary>
        [Test]
        public void LockFreeConcurrentTimeStampProviderShouldReturnUniqueValuesTest()
        {
            // act
            var timeStamps = Enumerable
                .Range(0, 10000)
                .AsParallel()
                .Select(_ => _timeStampProvider.GetTimeStampLockFree).ToArray();

            // assert
            timeStamps.Should().OnlyHaveUniqueItems();
        }

        /// <summary>
        /// Тест на то, что провайдер возвращает только уникальные объекты.
        /// </summary>
        [Test]
        public void LockFreeTimeStampProviderShouldReturnRoundedValuesTest()
        {
            // act
            var timeStamps = Enumerable
                .Range(0, 1000)
                .AsParallel()
                .Select(_ => _timeStampProvider.GetTimeStampLockFree).ToArray();

            // assert
            foreach (var timeStamp in timeStamps)
            {
                (timeStamp.Ticks % TimeSpan.TicksPerMillisecond).Should().Be(0);
            }
        }
    }
}
