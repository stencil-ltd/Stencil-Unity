namespace AnalyticsNg

type Tracking private() =

    static let Instance = new Tracking()
    static let Enabled = true

    let Trackers: ITracker list = [
        UnityTracker()
    ]

    interface ITracker with

        member this.Track event props =
            Trackers |> List.map (fun x-> x.Track event props) |> ignore
            this :> ITracker

        member this.Set key value =
            Trackers |> List.map (fun x-> x.Set key value) |> ignore
            this :> ITracker