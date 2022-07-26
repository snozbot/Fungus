﻿using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Fungus.TimeSys;
using TimeSpan = System.TimeSpan;

namespace Fungus.Tests.TimeSystemTests
{
    public class TimerTests
    {
        [SetUp]
        protected virtual void SetUp()
        {
            managerPrefab = Resources.Load<GameObject>(managerPrefabPath);
            managerGO = MonoBehaviour.Instantiate<GameObject>(managerPrefab, Vector3.zero, Quaternion.identity);
            timerManager = managerGO.GetComponent<TimerManager>();
        }

        protected string managerPrefabPath = "Prefabs/TimerManager";
        protected GameObject managerPrefab, managerGO;
        TimerManager timerManager;

        [TearDown]
        protected virtual void TearDown()
        {
            MonoBehaviour.Destroy(managerGO.gameObject);
        }

        protected virtual IEnumerator SetUpForCountupTimerTesting()
        {
            // To make sure that the test timer exists and has a clean slate
            timerManager.SetModeOfTimerWithID(testID, TimerMode.Countup);
            RegisterAndSetTestTimerToHaveCleanSlate();
            yield return null;
        }

        protected virtual void RegisterAndSetTestTimerToHaveCleanSlate()
        {
            timerManager.StopTimerWithID(testID);
            timerManager.ResetTimerWithID(testID);
            testTimer = timerManager.Timers[testID];
        }

        protected int testID = 999;
        protected Timer testTimer;

        protected virtual void FetchTestTimer()
        {
            // The TimerManager's Timers dict returns a copy of the actual timers dict, so we'll need to 
            // fetch the test timer again at multiple points
            testTimer = timerManager.Timers[testID];
        }

        [UnityTest]
        public virtual IEnumerator CountsUpCorrectly()
        {
            // Arrange
            yield return SetUpForCountupTimerTesting();
            float expectedSeconds = 2;

            // Act
            yield return WaitAndLetTimerRunFor(expectedSeconds);
            FetchTestTimer();
            TimeSpan timeRecorded = testTimer.TimeRecorded;
            float secondsCounted = Mathf.Round((float)timeRecorded.TotalSeconds);

            bool success = secondsCounted == expectedSeconds;

            // Assert
            Assert.IsTrue(success);
        }

        protected virtual IEnumerator WaitAndLetTimerRunFor(int expectedSeconds)
        {
            timerManager.StartTimerWithID(testID);
            yield return new WaitForSeconds(expectedSeconds);
        }


        protected virtual IEnumerator WaitAndLetTimerRunFor(float expectedSeconds)
        {
            timerManager.StartTimerWithID(testID);
            yield return new WaitForSeconds(expectedSeconds);
        }

        [UnityTest]
        public virtual IEnumerator StopsCountingUpOnRequest()
        {
            // Arrange
            yield return SetUpForCountupTimerTesting();

            int expectedSeconds = 2;

            // Act
            yield return WaitAndLetTimerRunFor(expectedSeconds);
            timerManager.StopTimerWithID(testID);

            yield return new WaitForSeconds(expectedSeconds);
            // ^This should be ignored by the timer
            FetchTestTimer();
            TimeSpan timeRecorded = testTimer.TimeRecorded;
            float secondsRecorded = Mathf.Round((float)timeRecorded.TotalSeconds);
            bool success = secondsRecorded == expectedSeconds;

            Assert.IsTrue(success);
        }

        [UnityTest]
        public virtual IEnumerator ResetsCountingUpOnRequest()
        {
            // Arrange
            yield return SetUpForCountupTimerTesting();

            float secondsToWait = 1, expectedSeconds = 0;

            // Act
            yield return WaitAndLetTimerRunFor(secondsToWait);
            timerManager.ResetTimerWithID(testID);

            TimeSpan timeRecorded = testTimer.TimeRecorded;

            bool success = timeRecorded.Seconds == expectedSeconds;

            Assert.IsTrue(success);

        }

        [UnityTest]
        public virtual IEnumerator CountsDownCorrectly()
        {
            // Arrange
            yield return SetUpForCountdownTimerTesting();

            float timeToWait = 2;
            float expectedRemainingSeconds = CountdownStartTime.Seconds - timeToWait;

            // Act
            yield return WaitAndLetTimerRunFor(timeToWait);
            // ^ We are negating the time offset since for some reason, the timer is more precise when counting down
            FetchTestTimer();
            TimeSpan timeRemaining = testTimer.TimeRecorded;
            float secondsRemaining = Mathf.Round((float)timeRemaining.TotalSeconds);

            // Assert
            bool success = secondsRemaining == expectedRemainingSeconds;
            Assert.IsTrue(success);

        }

        protected virtual IEnumerator SetUpForCountdownTimerTesting()
        {
            // To make sure that the test timer exists and has a clean slate
            timerManager.SetModeOfTimerWithID(testID, TimerMode.Countdown);
            timerManager.SetCountdownStartingTimeOfTimerWithID(testID, ref CountdownStartTime);
            RegisterAndSetTestTimerToHaveCleanSlate();
            yield return null;
        }

        protected TimeSpan CountdownStartTime = new TimeSpan(0, 0, 5);

        [UnityTest]
        public virtual IEnumerator StopsCountingDownOnRequest()
        {
            // Arrange
            yield return SetUpForCountdownTimerTesting();

            float secondsToWait = 1;
            float expectedRemainingSeconds = CountdownStartTime.Seconds - secondsToWait;

            // Act
            yield return WaitAndLetTimerRunFor(secondsToWait);
            timerManager.StopTimerWithID(testID);
            yield return new WaitForSeconds(secondsToWait); // <- We don't want to call Start right after stopping the timer
            FetchTestTimer();
            TimeSpan timeRemaining = testTimer.TimeRecorded;
            float secondsRemaining = Mathf.Round((float)timeRemaining.TotalSeconds);
            // ^ So we don't have to worry about the slight inaccuracies of WaitForSeconds

            // Assert
            bool success = secondsRemaining == expectedRemainingSeconds;
            Assert.IsTrue(success);
        }

        [UnityTest]
        public virtual IEnumerator ResetsCountingDownOnRequest()
        {
            // Arrange
            yield return SetUpForCountdownTimerTesting();

            float secondsToWait = 1.5f;
            float expectedRemainingSeconds = CountdownStartTime.Seconds;

            // Act
            yield return WaitAndLetTimerRunFor(secondsToWait);
            timerManager.ResetTimerWithID(testID);
            TimeSpan timeRemaining = testTimer.TimeRecorded;
            float remainingSeconds = Mathf.Round((float)timeRemaining.TotalSeconds);

            // Assert
            bool success = remainingSeconds == expectedRemainingSeconds;
            Assert.IsTrue(success);
        }

        [UnityTest]
        public virtual IEnumerator CountdownTimerStopsSelfCorrectly()
        {
            // Arrange
            yield return SetUpForCountdownTimerTesting();

            float secondsToWait = CountdownStartTime.Seconds + 0.2f;
            float expectedRemainingMilliseconds = 0;

            // Act
            yield return WaitAndLetTimerRunFor(secondsToWait);
            FetchTestTimer();
            
            TimeSpan timeRemaining = testTimer.TimeRecorded;
            float millisecondsRemaining = (float) timeRemaining.TotalMilliseconds;

            // Assert
            bool success = millisecondsRemaining == expectedRemainingMilliseconds;
            Assert.IsTrue(success);
        }

    }
}