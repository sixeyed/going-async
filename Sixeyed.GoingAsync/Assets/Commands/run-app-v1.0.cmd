
sqlcmd -S SC-2013-DEV\SQL2012DEV -d GoingAsync -Q "truncate table incomingtrades"

del /Q c:\in\app-v1\*.*

cd C:\SCM\sixeyed.visualstudio.com\nsb-webinar\Sixeyed.GoingAsync\Sixeyed.GoingAsync.Tools.TradeGenerator\bin\Debug
Sixeyed.GoingAsync.Tools.TradeGenerator.exe /c 100

cd C:\SCM\sixeyed.visualstudio.com\nsb-webinar\Sixeyed.GoingAsync\Sixeyed.GoingAsync.AppV1\bin\Debug
Sixeyed.GoingAsync.AppV1.exe /v 1.0