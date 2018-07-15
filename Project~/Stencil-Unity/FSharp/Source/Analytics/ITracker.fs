namespace AnalyticsNg

type ITracker =
    abstract member Track: string -> Map<string,obj> -> ITracker
    abstract member Set: string -> obj -> ITracker