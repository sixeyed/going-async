
rmdir /S /Q c:\in

powershell -Command "& {Get-MsmqQueue -Name trade-* -QueueType Private | Remove-MsmqQueue;}"