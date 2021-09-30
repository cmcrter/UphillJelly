using System.Collections;
using NUnit.Framework;
using UnityEngine.TestTools;

//This script uses these namespaces
using SleepyCat;

namespace Tests
{
    public class TimerTests
    {
        // A Test behaves as an ordinary method
        [Test]
        public void TimerTestsSimplePasses()
        {
            // Use the Assert class to test conditions
        }

        // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
        // `yield return null;` to skip a frame.
        [UnityTest]
        public IEnumerator TimerTestsWithEnumeratorPasses()
        {
            // Use the Assert class to test conditions.
            // Use yield to skip a frame.
            yield return null;
        }

        [Test]
        public void TimerWorks()
        {
            //Assign
            var timer = new Timer(1.0f);

            //Act
            timer.Tick(1.0f);

            //Assert
            Assert.AreEqual(timer.current_time, 0.0f);
            Assert.AreEqual(timer.isActive, false);
        }

        [UnityTest]
        public IEnumerator TimerBehaviour()
        {
            //Assign
            var timer = new Timer(1.0f);

            //Act
            while (timer.isActive)
            {
                timer.Tick(UnityEngine.Time.deltaTime);
                yield return null;
            }

            //Assert
            Assert.AreEqual(timer.current_time, 0.0f);
            Assert.AreEqual(timer.isActive, false);
        }
    }
}
