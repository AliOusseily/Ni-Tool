# Ni-Tool
# create two files one named unparsed and one named parsed and move the raw file into the unparsed directory and the output parsed file will be generated in the parsed directory 
# The project is built from one solution and 3 different apis in this solution (Aggregator,Parser,Loader)
# the project should be cloned and the criteria of running the project is as following
# open 3 power shells and go the path of the apis on each power shell respectively
# dotnet watch run the 3 apis and move the raw file into the unparsed directory
# The watcher running in the background process will catch this file parse it and send and http request to loader api that it's done parsing 
# then the loader will send an http request to the aggregator api that data has been loaded to the database in order to aggregate and run the aggregate script 
# in the aggregator api i created a controller and a service to fetch the data to the ui from the database
# P.S. Make sure to update the ports of the swagger when fetching the data and the path of the unparsed file 
# Database scripts:

Create table TRANS_MW_ERC_PM_TN_RADIO_LINK_POWER_hourly
(
    "Time" DATETIME,
    Link varchar,
    NeAlias varchar,
    NeType varchar,
    SLOT int , 
    MaxRxLevel float,
    MaxTxLevel float,
    RSL_DEVIATION float
)

Create table TRANS_MW_ERC_PM_TN_RADIO_LINK_POWER_Daily
(
   "Time" DATETIME,
    Link varchar,
    NeAlias varchar,
    NeType varchar,
    SLOT int , 
    MaxRxLevel float,
    MaxTxLevel float,
    RSL_DEVIATION float
)

////////////////////////////////////////////////////////////////////////////////////////////
//Hourly
INSERT INTO TRANS_MW_ERC_PM_TN_RADIO_LINK_POWER_hourly(Time,Link,NeAlias,NeType,SLOT,MaxRxLevel,MaxTxLevel , RSL_DEVIATION)
select date_trunc('hour', Time),Link,NeAlias, NeType,SLOT,MaxRxLevel,MaxTxLevel,abs(MaxRxLevel - MaxTxLevel) as RSL_DEVIATION
from TRANS_MW_ERC_PM_TN_RADIO_LINK_POWER where date_trunc('hour', Time)
NOT IN(select date_trunc('hour',Time) from TRANS_MW_ERC_PM_TN_RADIO_LINK_POWER_hourly) 
group by 1,2,3,4,5,6,7
//select 
select count(*) from TRANS_MW_ERC_PM_TN_RADIO_LINK_POWER_hourly
select * from TRANS_MW_ERC_PM_TN_RADIO_LINK_POWER_hourly
Truncate table TRANS_MW_ERC_PM_TN_RADIO_LINK_POWER_hourly

select Time,Link,Max(MaxRxLevel) as MaxRxLevel ,Max(MaxTxLevel) as MaxTxLevel ,Max(RSL_DEVIATION) as RSL_DEVIATION
from TRANS_MW_ERC_PM_TN_RADIO_LINK_POWER_hourly
where NeAlias=' TN-ALT13.1'
group by 1,2
////////////////////////////////////////////////////////////////////////////////////////////
//Daily slot level
INSERT INTO TRANS_MW_ERC_PM_TN_RADIO_LINK_POWER_Daily(Time,Link,NeAlias,NeType,SLOT,MaxRxLevel,MaxTxLevel , RSL_DEVIATION)
select date_trunc('day', Time),Link,NeAlias, NeType,SLOT,MaxRxLevel,MaxTxLevel,abs(MaxRxLevel - MaxTxLevel) as RSL_DEVIATION
from TRANS_MW_ERC_PM_TN_RADIO_LINK_POWER_hourly where date_trunc('day', Time)
NOT IN(select date_trunc('day',Time) from TRANS_MW_ERC_PM_TN_RADIO_LINK_POWER_Daily) 
group by 1,2,3,4,5,6,7
//////
// Link Level aggregation
select Time,Link,NeType,Max(MaxRxLevel) as MaxRxLevel ,Max(MaxTxLevel) as MaxTxLevel ,Max(RSL_DEVIATION) as RSL_DEVIATION
from TRANS_MW_ERC_PM_TN_RADIO_LINK_POWER_Daily
where NeAlias=' TN-ALT13.1'  # Ne Alias to be passed as a parameter in the frontend
group by 1,2,3

////////////////////////////////////////////////////////////////////////////////////////////
COPY TRANS_MW_ERC_PM_TN_RADIO_LINK_POWER FROM LOCAL 'C:\Users\User\Desktop\Projects\Parsed\SOEM1_TN_RADIO_LINK_POWER_20200312_001500_Parsed.csv' with DELIMITER as ',' skip 1
