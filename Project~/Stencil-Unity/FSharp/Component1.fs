namespace FSharp

open UnityEngine

[<ExecuteInEditMode>]
type Class1() =
    inherit MonoBehaviour()
    member this.Awake() = Debug.Log("Sup F#")
