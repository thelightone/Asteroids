using Firebase;
using Firebase.Analytics;
using Firebase.Extensions;
using UnityEngine;

public class FirebaseInit : MonoBehaviour
{
    private void Start()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            var status = task.Result;

            if (status == DependencyStatus.Available)
            {
                FirebaseAnalytics.LogEvent("game_started");

                Debug.Log("Firebase Analytics initialized");
            }
            else
            {
                Debug.LogError($"Firebase dependencies error: {status}");
            }
        });
    }
}