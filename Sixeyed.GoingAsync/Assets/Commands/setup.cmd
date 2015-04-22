
mkdir c:\in\app-v1\
mkdir c:\in\app-v2\

powershell -Command "& {New-MsmqQueue -Name trade-validate;}"
powershell -Command "& {New-MsmqQueue -Name trade-enrich-party1;}"
powershell -Command "& {New-MsmqQueue -Name trade-enrich-party2;}"
powershell -Command "& {Get-MsmqQueue -Name trade-* -QueueType Private | Set-MsmqQueueACL -UserName Everyone -Allow FullControl;}"