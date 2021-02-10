


```plantuml

actor "User" as user
boundary "Visual Studio UI" as vs
control "Template Wizard" as wiz

user -> vs ++: Create Solution
vs -> wiz ** : Solution Wizard



wiz -> vs ++ : Add Project: SSDT Standard
return done

wiz -> vs ++: Add Project: TSQLt Unit Test Proj
return done

wiz -> vs ++: Add Item: TestClass
return done

wiz -> vs ++: Add Item: Unit Test
return done

return Add Solution

```
