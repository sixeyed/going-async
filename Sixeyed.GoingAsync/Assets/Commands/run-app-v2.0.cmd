

start "Validator" /D C:\SCM\sixeyed.visualstudio.com\nsb-webinar\Sixeyed.GoingAsync\Sixeyed.GoingAsync.AppV2.Consumer\bin\Debug C:\SCM\sixeyed.visualstudio.com\nsb-webinar\Sixeyed.GoingAsync\Sixeyed.GoingAsync.AppV2.Consumer\bin\Debug\Sixeyed.GoingAsync.AppV2.Consumer.exe /i .\private$\trade-validate
start "Party1Enricher" /D C:\SCM\sixeyed.visualstudio.com\nsb-webinar\Sixeyed.GoingAsync\Sixeyed.GoingAsync.AppV2.Consumer\bin\Debug C:\SCM\sixeyed.visualstudio.com\nsb-webinar\Sixeyed.GoingAsync\Sixeyed.GoingAsync.AppV2.Consumer\bin\Debug\Sixeyed.GoingAsync.AppV2.Consumer.exe /i .\private$\trade-enrich-party1
start "Party2Enricher" /D C:\SCM\sixeyed.visualstudio.com\nsb-webinar\Sixeyed.GoingAsync\Sixeyed.GoingAsync.AppV2.Consumer\bin\Debug C:\SCM\sixeyed.visualstudio.com\nsb-webinar\Sixeyed.GoingAsync\Sixeyed.GoingAsync.AppV2.Consumer\bin\Debug\Sixeyed.GoingAsync.AppV2.Consumer.exe /i .\private$\trade-enrich-party2


sqlcmd -S SC-2013-DEV\SQL2012DEV -d GoingAsync -Q "truncate table incomingtrades"

powershell -Command "& {Get-MsmqQueue -Name trade-* -QueueType Private | Clear-MsmqQueue;}"

del /Q c:\in\app-v2\*.*

cd C:\SCM\sixeyed.visualstudio.com\nsb-webinar\Sixeyed.GoingAsync\Sixeyed.GoingAsync.Tools.TradeGenerator\bin\Debug
Sixeyed.GoingAsync.Tools.TradeGenerator.exe /a v2 /c 100


cd C:\SCM\sixeyed.visualstudio.com\nsb-webinar\Sixeyed.GoingAsync\Sixeyed.GoingAsync.AppV2.Producer\bin\Debug
Sixeyed.GoingAsync.AppV2.Producer.exe 