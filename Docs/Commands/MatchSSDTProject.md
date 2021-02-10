The goal here it to get two SSDT projects to match on a sub set of options
  * CLR .NET framework
  * SQL Target Version


```plantuml

actor "User" as user
boundary "Visual Studio UI" as vs
control "TSQLtTools" as ex
control "Solution Explorer" as sln

user -> vs ++ #FFBBBB: match projects
    vs -> ex ++ #FFBBBB: callback

        ex -> sln ++: list projects
        return list

        ex -> vs ++ : prompt
            vs -> user ++ : dialog
            return selection
        return {source, target}

        ex -> ex ++: Copy attribs
            ex -> sln++: set attribs
            return
        ' end callback
        return

    return
    ' end command

return
' end click


```
