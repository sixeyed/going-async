
-- truncate table incomingtrades


select count(*) as Trades, datediff(second, min(processedat), max(processedat)) as TotalSeconds
from incomingtrades