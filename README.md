# Export Directory
An exporter directory parser. Parse company registration information to a more readable format for further data manipulation.

## Export Malaysia Company Data
This is a console to parse output from http://edirectory.matrade.gov.my/application/edirectory.nsf/PrintCompany?OpenAgent&Option=detail&param=3100+3101+3102+3103+3104+3105.

see http://edirectory.matrade.gov.my/application/edirectory.nsf


## How To Use
    Open a console > ExtractBusinessCard.exe start end

where start and end is an integer ie. 3100. The result is save at the same location with file name _output.csv_ after finish.


## Change Log

### v0.15.3.25
Fix joined string when there is new line or more than 1 value.

### v0.15.3.23
Initial release.