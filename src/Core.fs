module Core

type Port = Port of int
type Host = Host of string

type Config = {
    StaticDir : string option
    Host : Host
    Port : Port
}

module Config =
    let defaultConfig = {
        StaticDir = None
        Host = Host "0.0.0.0"
        Port = Port 1001
    }