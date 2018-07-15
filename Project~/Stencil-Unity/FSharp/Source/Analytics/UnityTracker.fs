namespace AnalyticsNg

open UnityEngine

type UnityAnalytics = Analytics.Analytics

type UnityTracker() =

    interface ITracker with
        member this.Track event props =
            UnityAnalytics.CustomEvent(event, props) |> ignore
            this :> ITracker

        member this.Set key value = 
            this :> ITracker