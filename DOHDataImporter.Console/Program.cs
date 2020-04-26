using covid19phlib.DTO_Models;
using COVID19Tracker.Library.DTO_Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using c = System.Console;
using System.IO;
using System.Data.SqlTypes;

namespace DOHDataImporter.Console
{
    class Program
    {
        string json = @"[{""CaseCode"":""C100119"",""Age"":30,""AgeGroup"":""30 to 34"",""Sex"":""Male"",""DateRepConf"":""4/12/2020"",""DateRecover"":"""",""DateDied"":"""",""RemovalType"":"""",""DateRepRem"":"""",""Admitted"":""Yes"",""RegionRes"":""NCR"",""ProvCityRes"":""City of Parañaque""},
{""CaseCode"":""C100264"",""Age"":57,""AgeGroup"":""55 to 59"",""Sex"":""Male"",""DateRepConf"":""3/29/2020"",""DateRecover"":"""",""DateDied"":"""",""RemovalType"":"""",""DateRepRem"":"""",""Admitted"":""Yes"",""RegionRes"":""NCR"",""ProvCityRes"":""City of Mandaluyong""},
{""CaseCode"":""C100648"",""Age"":33,""AgeGroup"":""30 to 34"",""Sex"":""Female"",""DateRepConf"":""4/16/2020"",""DateRecover"":"""",""DateDied"":"""",""RemovalType"":"""",""DateRepRem"":"""",""Admitted"":""Yes"",""RegionRes"":""NCR"",""ProvCityRes"":""Quezon City""},
{""CaseCode"":""C100660"",""Age"":42,""AgeGroup"":""40 to 44"",""Sex"":""Female"",""DateRepConf"":""4/2/2020"",""DateRecover"":"""",""DateDied"":"""",""RemovalType"":"""",""DateRepRem"":"""",""Admitted"":""Yes"",""RegionRes"":""NCR"",""ProvCityRes"":""City of Pasig""},
{""CaseCode"":""C100776"",""Age"":42,""AgeGroup"":""40 to 44"",""Sex"":""Male"",""DateRepConf"":""4/1/2020"",""DateRecover"":"""",""DateDied"":"""",""RemovalType"":"""",""DateRepRem"":"""",""Admitted"":""Yes"",""RegionRes"":""Laguna"",""ProvCityRes"":""Pila""},
{""CaseCode"":""C101015"",""Age"":79,""AgeGroup"":""75 to 79"",""Sex"":""Male"",""DateRepConf"":""4/3/2020"",""DateRecover"":"""",""DateDied"":"""",""RemovalType"":"""",""DateRepRem"":"""",""Admitted"":"""",""RegionRes"":""Quezon"",""ProvCityRes"":""Unisan""},
{""CaseCode"":""C101097"",""Age"":33,""AgeGroup"":""30 to 34"",""Sex"":""Male"",""DateRepConf"":""3/27/2020"",""DateRecover"":"""",""DateDied"":"""",""RemovalType"":"""",""DateRepRem"":"""",""Admitted"":""Yes"",""RegionRes"":""NCR"",""ProvCityRes"":""Quezon City""},
{""CaseCode"":""C101232"",""Age"":30,""AgeGroup"":""30 to 34"",""Sex"":""Male"",""DateRepConf"":""3/21/2020"",""DateRecover"":""3/25/2020"",""DateDied"":"""",""RemovalType"":""Recovered"",""DateRepRem"":""3/28/2020"",""Admitted"":""Yes"",""RegionRes"":""NCR"",""ProvCityRes"":""City of Mandaluyong""},
{""CaseCode"":""C101376"",""Age"":29,""AgeGroup"":""25 to 29"",""Sex"":""Male"",""DateRepConf"":""4/11/2020"",""DateRecover"":"""",""DateDied"":"""",""RemovalType"":"""",""DateRepRem"":"""",""Admitted"":""Yes"",""RegionRes"":"""",""ProvCityRes"":""""},
{""CaseCode"":""C101483"",""Age"":40,""AgeGroup"":""40 to 44"",""Sex"":""Female"",""DateRepConf"":""4/14/2020"",""DateRecover"":"""",""DateDied"":"""",""RemovalType"":"""",""DateRepRem"":"""",""Admitted"":""Yes"",""RegionRes"":""NCR"",""ProvCityRes"":""City of San Juan""},
{""CaseCode"":""C101718"",""Age"":38,""AgeGroup"":""35 to 39"",""Sex"":""Male"",""DateRepConf"":""4/17/2020"",""DateRecover"":"""",""DateDied"":"""",""RemovalType"":"""",""DateRepRem"":"""",""Admitted"":""Yes"",""RegionRes"":""Quezon"",""ProvCityRes"":""Calauag""},
{""CaseCode"":""C101875"",""Age"":30,""AgeGroup"":""30 to 34"",""Sex"":""Female"",""DateRepConf"":""3/28/2020"",""DateRecover"":"""",""DateDied"":"""",""RemovalType"":"""",""DateRepRem"":"""",""Admitted"":""Yes"",""RegionRes"":""NCR"",""ProvCityRes"":""City of Makati""},
{""CaseCode"":""C102019"",""Age"":46,""AgeGroup"":""45 to 49"",""Sex"":""Male"",""DateRepConf"":""3/31/2020"",""DateRecover"":"""",""DateDied"":"""",""RemovalType"":"""",""DateRepRem"":"""",""Admitted"":""Yes"",""RegionRes"":""NCR"",""ProvCityRes"":""City of Manila""},
{""CaseCode"":""C102036"",""Age"":33,""AgeGroup"":""30 to 34"",""Sex"":""Female"",""DateRepConf"":""3/25/2020"",""DateRecover"":""4/2/2020"",""DateDied"":"""",""RemovalType"":""Recovered"",""DateRepRem"":""4/8/2020"",""Admitted"":""Yes"",""RegionRes"":""NCR"",""ProvCityRes"":""Taguig City""},
{""CaseCode"":""C102195"",""Age"":70,""AgeGroup"":""70 to 74"",""Sex"":""Male"",""DateRepConf"":""3/29/2020"",""DateRecover"":"""",""DateDied"":"""",""RemovalType"":"""",""DateRepRem"":"""",""Admitted"":""Yes"",""RegionRes"":""NCR"",""ProvCityRes"":""Quezon City""},
{""CaseCode"":""C102368"",""Age"":44,""AgeGroup"":""40 to 44"",""Sex"":""Female"",""DateRepConf"":""4/14/2020"",""DateRecover"":"""",""DateDied"":"""",""RemovalType"":"""",""DateRepRem"":"""",""Admitted"":"""",""RegionRes"":""NCR"",""ProvCityRes"":""City of Parañaque""},
{""CaseCode"":""C102418"",""Age"":28,""AgeGroup"":""25 to 29"",""Sex"":""Female"",""DateRepConf"":""3/23/2020"",""DateRecover"":"""",""DateDied"":"""",""RemovalType"":"""",""DateRepRem"":"""",""Admitted"":"""",""RegionRes"":""NCR"",""ProvCityRes"":""City of Mandaluyong""},
{""CaseCode"":""C102433"",""Age"":61,""AgeGroup"":""60 to 64"",""Sex"":""Male"",""DateRepConf"":""4/3/2020"",""DateRecover"":"""",""DateDied"":""3/27/2020"",""RemovalType"":""Died"",""DateRepRem"":""4/11/2020"",""Admitted"":"""",""RegionRes"":""Pampanga"",""ProvCityRes"":""Candaba""},
{""CaseCode"":""C102654"",""Age"":32,""AgeGroup"":""30 to 34"",""Sex"":""Female"",""DateRepConf"":""4/11/2020"",""DateRecover"":"""",""DateDied"":"""",""RemovalType"":"""",""DateRepRem"":"""",""Admitted"":""Yes"",""RegionRes"":""NCR"",""ProvCityRes"":""City of Las Piñas""},
{""CaseCode"":""C102795"",""Age"":63,""AgeGroup"":""60 to 64"",""Sex"":""Male"",""DateRepConf"":""4/15/2020"",""DateRecover"":"""",""DateDied"":"""",""RemovalType"":""Died"",""DateRepRem"":""4/19/2020"",""Admitted"":""Yes"",""RegionRes"":""NCR"",""ProvCityRes"":""City of Pasig""},
{""CaseCode"":""C102803"",""Age"":54,""AgeGroup"":""50 to 54"",""Sex"":""Male"",""DateRepConf"":""4/1/2020"",""DateRecover"":"""",""DateDied"":"""",""RemovalType"":"""",""DateRepRem"":"""",""Admitted"":""Yes"",""RegionRes"":""NCR"",""ProvCityRes"":""Quezon City""},
{""CaseCode"":""C102885"",""Age"":63,""AgeGroup"":""60 to 64"",""Sex"":""Female"",""DateRepConf"":""4/6/2020"",""DateRecover"":"""",""DateDied"":"""",""RemovalType"":"""",""DateRepRem"":"""",""Admitted"":""Yes"",""RegionRes"":""NCR"",""ProvCityRes"":""Quezon City""},
{""CaseCode"":""C103118"",""Age"":56,""AgeGroup"":""55 to 59"",""Sex"":""Female"",""DateRepConf"":""4/5/2020"",""DateRecover"":"""",""DateDied"":"""",""RemovalType"":"""",""DateRepRem"":"""",""Admitted"":"""",""RegionRes"":""Bataan"",""ProvCityRes"":""Limay""},
{""CaseCode"":""C103281"",""Age"":42,""AgeGroup"":""40 to 44"",""Sex"":""Male"",""DateRepConf"":""4/3/2020"",""DateRecover"":"""",""DateDied"":"""",""RemovalType"":"""",""DateRepRem"":"""",""Admitted"":""Yes"",""RegionRes"":""NCR"",""ProvCityRes"":""City of Manila""},
{""CaseCode"":""C103520"",""Age"":31,""AgeGroup"":""30 to 34"",""Sex"":""Male"",""DateRepConf"":""4/16/2020"",""DateRecover"":"""",""DateDied"":"""",""RemovalType"":"""",""DateRepRem"":"""",""Admitted"":""Yes"",""RegionRes"":""NCR"",""ProvCityRes"":""Quezon City""},
{""CaseCode"":""C103595"",""Age"":40,""AgeGroup"":""40 to 44"",""Sex"":""Male"",""DateRepConf"":""4/11/2020"",""DateRecover"":"""",""DateDied"":"""",""RemovalType"":"""",""DateRepRem"":"""",""Admitted"":"""",""RegionRes"":""NCR"",""ProvCityRes"":""City of Pasig""},
{""CaseCode"":""C103631"",""Age"":47,""AgeGroup"":""45 to 49"",""Sex"":""Male"",""DateRepConf"":""4/6/2020"",""DateRecover"":"""",""DateDied"":"""",""RemovalType"":"""",""DateRepRem"":"""",""Admitted"":""No"",""RegionRes"":""NCR"",""ProvCityRes"":""Quezon City""},
{""CaseCode"":""C103691"",""Age"":30,""AgeGroup"":""30 to 34"",""Sex"":""Male"",""DateRepConf"":""4/17/2020"",""DateRecover"":"""",""DateDied"":"""",""RemovalType"":"""",""DateRepRem"":"""",""Admitted"":""Yes"",""RegionRes"":""NCR"",""ProvCityRes"":""Caloocan City""},
{""CaseCode"":""C103706"",""Age"":74,""AgeGroup"":""70 to 74"",""Sex"":""Female"",""DateRepConf"":""4/9/2020"",""DateRecover"":"""",""DateDied"":""4/11/2020"",""RemovalType"":""Died"",""DateRepRem"":""4/12/2020"",""Admitted"":""Yes"",""RegionRes"":""Quezon"",""ProvCityRes"":""Tiaong""},
{""CaseCode"":""C104241"",""Age"":60,""AgeGroup"":""60 to 64"",""Sex"":""Female"",""DateRepConf"":""3/29/2020"",""DateRecover"":""3/31/2020"",""DateDied"":"""",""RemovalType"":""Recovered"",""DateRepRem"":""4/19/2020"",""Admitted"":""Yes"",""RegionRes"":""NCR"",""ProvCityRes"":""City of Muntinlupa""},
{""CaseCode"":""C104303"",""Age"":48,""AgeGroup"":""45 to 49"",""Sex"":""Female"",""DateRepConf"":""3/20/2020"",""DateRecover"":""4/9/2020"",""DateDied"":"""",""RemovalType"":""Recovered"",""DateRepRem"":""4/19/2020"",""Admitted"":""Yes"",""RegionRes"":""NCR"",""ProvCityRes"":""City of Mandaluyong""},
{""CaseCode"":""C104325"",""Age"":33,""AgeGroup"":""30 to 34"",""Sex"":""Male"",""DateRepConf"":""4/14/2020"",""DateRecover"":"""",""DateDied"":"""",""RemovalType"":"""",""DateRepRem"":"""",""Admitted"":"""",""RegionRes"":"""",""ProvCityRes"":""""},
{""CaseCode"":""C104371"",""Age"":26,""AgeGroup"":""25 to 29"",""Sex"":""Male"",""DateRepConf"":""3/21/2020"",""DateRecover"":"""",""DateDied"":"""",""RemovalType"":"""",""DateRepRem"":"""",""Admitted"":""Yes"",""RegionRes"":""Palawan"",""ProvCityRes"":""Puerto Princesa City (Capital)""},
{""CaseCode"":""C104379"",""Age"":51,""AgeGroup"":""50 to 54"",""Sex"":""Male"",""DateRepConf"":""4/18/2020"",""DateRecover"":"""",""DateDied"":"""",""RemovalType"":"""",""DateRepRem"":"""",""Admitted"":""Yes"",""RegionRes"":""Batangas"",""ProvCityRes"":""City of Tanauan""},
{""CaseCode"":""C104408"",""Age"":40,""AgeGroup"":""40 to 44"",""Sex"":""Male"",""DateRepConf"":""4/2/2020"",""DateRecover"":"""",""DateDied"":"""",""RemovalType"":"""",""DateRepRem"":"""",""Admitted"":"""",""RegionRes"":"""",""ProvCityRes"":""""},
{""CaseCode"":""C104413"",""Age"":65,""AgeGroup"":""65 to 69"",""Sex"":""Male"",""DateRepConf"":""3/22/2020"",""DateRecover"":"""",""DateDied"":""3/21/2020"",""RemovalType"":""Died"",""DateRepRem"":""3/23/2020"",""Admitted"":""Yes"",""RegionRes"":""NCR"",""ProvCityRes"":""Quezon City""},
{""CaseCode"":""C104576"",""Age"":55,""AgeGroup"":""55 to 59"",""Sex"":""Female"",""DateRepConf"":""4/9/2020"",""DateRecover"":"""",""DateDied"":"""",""RemovalType"":"""",""DateRepRem"":"""",""Admitted"":""Yes"",""RegionRes"":""NCR"",""ProvCityRes"":""City of Las Piñas""},
{""CaseCode"":""C104618"",""Age"":35,""AgeGroup"":""35 to 39"",""Sex"":""Female"",""DateRepConf"":""3/31/2020"",""DateRecover"":"""",""DateDied"":"""",""RemovalType"":"""",""DateRepRem"":"""",""Admitted"":""Yes"",""RegionRes"":""Bulacan"",""ProvCityRes"":""Bulacan""},
{""CaseCode"":""C104790"",""Age"":36,""AgeGroup"":""35 to 39"",""Sex"":""Female"",""DateRepConf"":""4/16/2020"",""DateRecover"":"""",""DateDied"":"""",""RemovalType"":"""",""DateRepRem"":"""",""Admitted"":""Yes"",""RegionRes"":""Bataan"",""ProvCityRes"":""City of Balanga (Capital)""},
{""CaseCode"":""C104914"",""Age"":81,""AgeGroup"":""80+"",""Sex"":""Female"",""DateRepConf"":""4/6/2020"",""DateRecover"":"""",""DateDied"":"""",""RemovalType"":"""",""DateRepRem"":"""",""Admitted"":""Yes"",""RegionRes"":""NCR"",""ProvCityRes"":""City of Makati""},
{""CaseCode"":""C104939"",""Age"":67,""AgeGroup"":""65 to 69"",""Sex"":""Female"",""DateRepConf"":""4/1/2020"",""DateRecover"":"""",""DateDied"":"""",""RemovalType"":"""",""DateRepRem"":"""",""Admitted"":""No"",""RegionRes"":""NCR"",""ProvCityRes"":""City of Makati""},
{""CaseCode"":""C105000"",""Age"":44,""AgeGroup"":""40 to 44"",""Sex"":""Female"",""DateRepConf"":""4/15/2020"",""DateRecover"":"""",""DateDied"":"""",""RemovalType"":"""",""DateRepRem"":"""",""Admitted"":"""",""RegionRes"":"""",""ProvCityRes"":""""},
{""CaseCode"":""C105064"",""Age"":81,""AgeGroup"":""80+"",""Sex"":""Female"",""DateRepConf"":""3/31/2020"",""DateRecover"":""4/1/2020"",""DateDied"":"""",""RemovalType"":""Recovered"",""DateRepRem"":""4/17/2020"",""Admitted"":""Yes"",""RegionRes"":""NCR"",""ProvCityRes"":""City of Makati""},
{""CaseCode"":""C105104"",""Age"":59,""AgeGroup"":""55 to 59"",""Sex"":""Male"",""DateRepConf"":""4/8/2020"",""DateRecover"":"""",""DateDied"":"""",""RemovalType"":"""",""DateRepRem"":"""",""Admitted"":""Yes"",""RegionRes"":""NCR"",""ProvCityRes"":""Pasay City""},
{""CaseCode"":""C105379"",""Age"":63,""AgeGroup"":""60 to 64"",""Sex"":""Male"",""DateRepConf"":""4/6/2020"",""DateRecover"":"""",""DateDied"":"""",""RemovalType"":"""",""DateRepRem"":"""",""Admitted"":"""",""RegionRes"":""NCR"",""ProvCityRes"":""City of Makati""},
{""CaseCode"":""C105714"",""Age"":80,""AgeGroup"":""80+"",""Sex"":""Male"",""DateRepConf"":""3/28/2020"",""DateRecover"":"""",""DateDied"":"""",""RemovalType"":"""",""DateRepRem"":"""",""Admitted"":""Yes"",""RegionRes"":""NCR"",""ProvCityRes"":""City of San Juan""},
{""CaseCode"":""C105961"",""Age"":53,""AgeGroup"":""50 to 54"",""Sex"":""Male"",""DateRepConf"":""3/24/2020"",""DateRecover"":"""",""DateDied"":"""",""RemovalType"":"""",""DateRepRem"":"""",""Admitted"":""Yes"",""RegionRes"":""NCR"",""ProvCityRes"":""City of Las Piñas""},
{""CaseCode"":""C105982"",""Age"":68,""AgeGroup"":""65 to 69"",""Sex"":""Male"",""DateRepConf"":""4/14/2020"",""DateRecover"":"""",""DateDied"":"""",""RemovalType"":"""",""DateRepRem"":"""",""Admitted"":"""",""RegionRes"":"""",""ProvCityRes"":""""},
{""CaseCode"":""C106037"",""Age"":26,""AgeGroup"":""25 to 29"",""Sex"":""Male"",""DateRepConf"":""4/12/2020"",""DateRecover"":"""",""DateDied"":"""",""RemovalType"":"""",""DateRepRem"":"""",""Admitted"":""Yes"",""RegionRes"":""Batangas"",""ProvCityRes"":""Lipa City""},
{""CaseCode"":""C106112"",""Age"":63,""AgeGroup"":""60 to 64"",""Sex"":""Female"",""DateRepConf"":""3/23/2020"",""DateRecover"":"""",""DateDied"":"""",""RemovalType"":""Recovered"",""DateRepRem"":""4/11/2020"",""Admitted"":""Yes"",""RegionRes"":""NCR"",""ProvCityRes"":""City of San Juan""},
{""CaseCode"":""C106675"",""Age"":57,""AgeGroup"":""55 to 59"",""Sex"":""Male"",""DateRepConf"":""3/29/2020"",""DateRecover"":"""",""DateDied"":"""",""RemovalType"":"""",""DateRepRem"":"""",""Admitted"":""Yes"",""RegionRes"":""NCR"",""ProvCityRes"":""Quezon City""},
{""CaseCode"":""C106899"",""Age"":71,""AgeGroup"":""70 to 74"",""Sex"":""Male"",""DateRepConf"":""3/30/2020"",""DateRecover"":"""",""DateDied"":"""",""RemovalType"":"""",""DateRepRem"":"""",""Admitted"":""Yes"",""RegionRes"":"""",""ProvCityRes"":""""},
{""CaseCode"":""C106921"",""Age"":34,""AgeGroup"":""30 to 34"",""Sex"":""Male"",""DateRepConf"":""3/24/2020"",""DateRecover"":"""",""DateDied"":"""",""RemovalType"":"""",""DateRepRem"":"""",""Admitted"":""Yes"",""RegionRes"":""NCR"",""ProvCityRes"":""City of Makati""},
{""CaseCode"":""C106961"",""Age"":30,""AgeGroup"":""30 to 34"",""Sex"":""Male"",""DateRepConf"":""4/11/2020"",""DateRecover"":"""",""DateDied"":"""",""RemovalType"":"""",""DateRepRem"":"""",""Admitted"":""Yes"",""RegionRes"":""NCR"",""ProvCityRes"":""City of Muntinlupa""},
{""CaseCode"":""C107244"",""Age"":62,""AgeGroup"":""60 to 64"",""Sex"":""Female"",""DateRepConf"":""4/6/2020"",""DateRecover"":"""",""DateDied"":"""",""RemovalType"":"""",""DateRepRem"":"""",""Admitted"":""Yes"",""RegionRes"":""Cavite"",""ProvCityRes"":""Imus City""},
{""CaseCode"":""C107588"",""Age"":62,""AgeGroup"":""60 to 64"",""Sex"":""Male"",""DateRepConf"":""3/28/2020"",""DateRecover"":"""",""DateDied"":"""",""RemovalType"":"""",""DateRepRem"":"""",""Admitted"":""Yes"",""RegionRes"":""NCR"",""ProvCityRes"":""Quezon City""},
{""CaseCode"":""C107724"",""Age"":51,""AgeGroup"":""50 to 54"",""Sex"":""Female"",""DateRepConf"":""3/29/2020"",""DateRecover"":"""",""DateDied"":"""",""RemovalType"":"""",""DateRepRem"":"""",""Admitted"":""Yes"",""RegionRes"":""Rizal"",""ProvCityRes"":""San Mateo""},
{""CaseCode"":""C107814"",""Age"":61,""AgeGroup"":""60 to 64"",""Sex"":""Male"",""DateRepConf"":""4/6/2020"",""DateRecover"":"""",""DateDied"":"""",""RemovalType"":"""",""DateRepRem"":"""",""Admitted"":""Yes"",""RegionRes"":""Laguna"",""ProvCityRes"":""City of Santa Rosa""},
{""CaseCode"":""C107955"",""Age"":80,""AgeGroup"":""80+"",""Sex"":""Female"",""DateRepConf"":""4/6/2020"",""DateRecover"":"""",""DateDied"":"""",""RemovalType"":"""",""DateRepRem"":"""",""Admitted"":""Yes"",""RegionRes"":""NCR"",""ProvCityRes"":""City of Mandaluyong""},
{""CaseCode"":""C108109"",""Age"":18,""AgeGroup"":""15 to 19"",""Sex"":""Female"",""DateRepConf"":""4/16/2020"",""DateRecover"":"""",""DateDied"":"""",""RemovalType"":"""",""DateRepRem"":"""",""Admitted"":"""",""RegionRes"":""Cebu"",""ProvCityRes"":""Cebu City (Capital)""},
{""CaseCode"":""C108116"",""Age"":33,""AgeGroup"":""30 to 34"",""Sex"":""Female"",""DateRepConf"":""4/14/2020"",""DateRecover"":"""",""DateDied"":"""",""RemovalType"":"""",""DateRepRem"":"""",""Admitted"":""Yes"",""RegionRes"":""NCR"",""ProvCityRes"":""City of Pasig""},
{""CaseCode"":""C108474"",""Age"":18,""AgeGroup"":""15 to 19"",""Sex"":""Female"",""DateRepConf"":""4/11/2020"",""DateRecover"":"""",""DateDied"":"""",""RemovalType"":"""",""DateRepRem"":"""",""Admitted"":"""",""RegionRes"":""NCR"",""ProvCityRes"":""Quezon City""},
{""CaseCode"":""C108997"",""Age"":30,""AgeGroup"":""30 to 34"",""Sex"":""Female"",""DateRepConf"":""3/28/2020"",""DateRecover"":"""",""DateDied"":"""",""RemovalType"":"""",""DateRepRem"":"""",""Admitted"":""Yes"",""RegionRes"":""NCR"",""ProvCityRes"":""City of Mandaluyong""},
{""CaseCode"":""C109421"",""Age"":41,""AgeGroup"":""40 to 44"",""Sex"":""Female"",""DateRepConf"":""4/3/2020"",""DateRecover"":"""",""DateDied"":"""",""RemovalType"":"""",""DateRepRem"":"""",""Admitted"":"""",""RegionRes"":"""",""ProvCityRes"":""""},
{""CaseCode"":""C109492"",""Age"":62,""AgeGroup"":""60 to 64"",""Sex"":""Female"",""DateRepConf"":""4/3/2020"",""DateRecover"":"""",""DateDied"":"""",""RemovalType"":""Died"",""DateRepRem"":""4/14/2020"",""Admitted"":"""",""RegionRes"":"""",""ProvCityRes"":""""},
{""CaseCode"":""C110039"",""Age"":57,""AgeGroup"":""55 to 59"",""Sex"":""Male"",""DateRepConf"":""3/31/2020"",""DateRecover"":"""",""DateDied"":"""",""RemovalType"":"""",""DateRepRem"":"""",""Admitted"":""Yes"",""RegionRes"":""NCR"",""ProvCityRes"":""Quezon City""},
{""CaseCode"":""C110083"",""Age"":73,""AgeGroup"":""70 to 74"",""Sex"":""Male"",""DateRepConf"":""4/3/2020"",""DateRecover"":"""",""DateDied"":"""",""RemovalType"":"""",""DateRepRem"":"""",""Admitted"":""Yes"",""RegionRes"":""Rizal"",""ProvCityRes"":""Angono""},
{""CaseCode"":""C110094"",""Age"":35,""AgeGroup"":""35 to 39"",""Sex"":""Male"",""DateRepConf"":""4/13/2020"",""DateRecover"":"""",""DateDied"":"""",""RemovalType"":"""",""DateRepRem"":"""",""Admitted"":""Yes"",""RegionRes"":""NCR"",""ProvCityRes"":""City of Makati""},
{""CaseCode"":""C110127"",""Age"":49,""AgeGroup"":""45 to 49"",""Sex"":""Male"",""DateRepConf"":""4/14/2020"",""DateRecover"":"""",""DateDied"":"""",""RemovalType"":"""",""DateRepRem"":"""",""Admitted"":"""",""RegionRes"":"""",""ProvCityRes"":""""},
{""CaseCode"":""C110169"",""Age"":70,""AgeGroup"":""70 to 74"",""Sex"":""Male"",""DateRepConf"":""4/10/2020"",""DateRecover"":"""",""DateDied"":"""",""RemovalType"":"""",""DateRepRem"":"""",""Admitted"":"""",""RegionRes"":"""",""ProvCityRes"":""""},
{""CaseCode"":""C110327"",""Age"":29,""AgeGroup"":""25 to 29"",""Sex"":""Female"",""DateRepConf"":""4/13/2020"",""DateRecover"":"""",""DateDied"":"""",""RemovalType"":"""",""DateRepRem"":"""",""Admitted"":""Yes"",""RegionRes"":""Zambales"",""ProvCityRes"":""Olongapo City""},
{""CaseCode"":""C110414"",""Age"":28,""AgeGroup"":""25 to 29"",""Sex"":""Female"",""DateRepConf"":""4/16/2020"",""DateRecover"":"""",""DateDied"":"""",""RemovalType"":"""",""DateRepRem"":"""",""Admitted"":"""",""RegionRes"":"""",""ProvCityRes"":""""},
{""CaseCode"":""C110607"",""Age"":62,""AgeGroup"":""60 to 64"",""Sex"":""Male"",""DateRepConf"":""4/15/2020"",""DateRecover"":"""",""DateDied"":"""",""RemovalType"":"""",""DateRepRem"":"""",""Admitted"":"""",""RegionRes"":""Cebu"",""ProvCityRes"":""Cebu City (Capital)""},
{""CaseCode"":""C111057"",""Age"":71,""AgeGroup"":""70 to 74"",""Sex"":""Female"",""DateRepConf"":""4/6/2020"",""DateRecover"":"""",""DateDied"":"""",""RemovalType"":"""",""DateRepRem"":"""",""Admitted"":""Yes"",""RegionRes"":""NCR"",""ProvCityRes"":""City of Makati""},
{""CaseCode"":""C111083"",""Age"":38,""AgeGroup"":""35 to 39"",""Sex"":""Male"",""DateRepConf"":""3/17/2020"",""DateRecover"":"""",""DateDied"":"""",""RemovalType"":"""",""DateRepRem"":"""",""Admitted"":""Yes"",""RegionRes"":""NCR"",""ProvCityRes"":""City of Makati""},
{""CaseCode"":""C111284"",""Age"":48,""AgeGroup"":""45 to 49"",""Sex"":""Female"",""DateRepConf"":""3/27/2020"",""DateRecover"":"""",""DateDied"":"""",""RemovalType"":""Recovered"",""DateRepRem"":""4/16/2020"",""Admitted"":""Yes"",""RegionRes"":""NCR"",""ProvCityRes"":""City of San Juan""},
{""CaseCode"":""C111340"",""Age"":77,""AgeGroup"":""75 to 79"",""Sex"":""Male"",""DateRepConf"":""4/1/2020"",""DateRecover"":"""",""DateDied"":"""",""RemovalType"":"""",""DateRepRem"":"""",""Admitted"":""Yes"",""RegionRes"":""NCR"",""ProvCityRes"":""City of San Juan""},
{""CaseCode"":""C111569"",""Age"":65,""AgeGroup"":""65 to 69"",""Sex"":""Male"",""DateRepConf"":""3/30/2020"",""DateRecover"":"""",""DateDied"":""3/26/2020"",""RemovalType"":""Died"",""DateRepRem"":""4/2/2020"",""Admitted"":""Yes"",""RegionRes"":""NCR"",""ProvCityRes"":""City of Manila""},
{""CaseCode"":""C111606"",""Age"":66,""AgeGroup"":""65 to 69"",""Sex"":""Female"",""DateRepConf"":""4/6/2020"",""DateRecover"":"""",""DateDied"":"""",""RemovalType"":"""",""DateRepRem"":"""",""Admitted"":""Yes"",""RegionRes"":""NCR"",""ProvCityRes"":""Quezon City""},
{""CaseCode"":""C111675"",""Age"":23,""AgeGroup"":""20 to 24"",""Sex"":""Female"",""DateRepConf"":""4/9/2020"",""DateRecover"":"""",""DateDied"":"""",""RemovalType"":"""",""DateRepRem"":"""",""Admitted"":""Yes"",""RegionRes"":""Bulacan"",""ProvCityRes"":""Guiguinto""},
{""CaseCode"":""C111820"",""Age"":40,""AgeGroup"":""40 to 44"",""Sex"":""Male"",""DateRepConf"":""4/17/2020"",""DateRecover"":"""",""DateDied"":"""",""RemovalType"":"""",""DateRepRem"":"""",""Admitted"":""Yes"",""RegionRes"":""NCR"",""ProvCityRes"":""Quezon City""},
{""CaseCode"":""C112128"",""Age"":28,""AgeGroup"":""25 to 29"",""Sex"":""Male"",""DateRepConf"":""4/19/2020"",""DateRecover"":"""",""DateDied"":"""",""RemovalType"":"""",""DateRepRem"":"""",""Admitted"":"""",""RegionRes"":"""",""ProvCityRes"":""""},
{""CaseCode"":""C112220"",""Age"":62,""AgeGroup"":""60 to 64"",""Sex"":""Male"",""DateRepConf"":""4/19/2020"",""DateRecover"":"""",""DateDied"":"""",""RemovalType"":"""",""DateRepRem"":"""",""Admitted"":""Yes"",""RegionRes"":""NCR"",""ProvCityRes"":""City of Manila""},
{""CaseCode"":""C112297"",""Age"":79,""AgeGroup"":""75 to 79"",""Sex"":""Male"",""DateRepConf"":""4/4/2020"",""DateRecover"":"""",""DateDied"":""4/1/2020"",""RemovalType"":""Died"",""DateRepRem"":""4/7/2020"",""Admitted"":""Yes"",""RegionRes"":"""",""ProvCityRes"":""""},
{""CaseCode"":""C112357"",""Age"":60,""AgeGroup"":""60 to 64"",""Sex"":""Male"",""DateRepConf"":""4/2/2020"",""DateRecover"":"""",""DateDied"":"""",""RemovalType"":"""",""DateRepRem"":"""",""Admitted"":""Yes"",""RegionRes"":""NCR"",""ProvCityRes"":""City of Manila""},
{""CaseCode"":""C112473"",""Age"":27,""AgeGroup"":""25 to 29"",""Sex"":""Female"",""DateRepConf"":""4/16/2020"",""DateRecover"":"""",""DateDied"":"""",""RemovalType"":"""",""DateRepRem"":"""",""Admitted"":"""",""RegionRes"":""Cavite"",""ProvCityRes"":""Imus City""},
{""CaseCode"":""C112486"",""Age"":39,""AgeGroup"":""35 to 39"",""Sex"":""Female"",""DateRepConf"":""4/13/2020"",""DateRecover"":"""",""DateDied"":"""",""RemovalType"":"""",""DateRepRem"":"""",""Admitted"":""Yes"",""RegionRes"":""NCR"",""ProvCityRes"":""City of Makati""},
{""CaseCode"":""C112510"",""Age"":58,""AgeGroup"":""55 to 59"",""Sex"":""Female"",""DateRepConf"":""3/28/2020"",""DateRecover"":"""",""DateDied"":""4/4/2020"",""RemovalType"":""Died"",""DateRepRem"":""4/7/2020"",""Admitted"":""Yes"",""RegionRes"":""Rizal"",""ProvCityRes"":""Cainta""},
{""CaseCode"":""C112709"",""Age"":26,""AgeGroup"":""25 to 29"",""Sex"":""Female"",""DateRepConf"":""4/6/2020"",""DateRecover"":"""",""DateDied"":"""",""RemovalType"":"""",""DateRepRem"":"""",""Admitted"":""Yes"",""RegionRes"":""NCR"",""ProvCityRes"":""City of Manila""},
{""CaseCode"":""C112779"",""Age"":57,""AgeGroup"":""55 to 59"",""Sex"":""Male"",""DateRepConf"":""3/31/2020"",""DateRecover"":"""",""DateDied"":"""",""RemovalType"":"""",""DateRepRem"":"""",""Admitted"":""Yes"",""RegionRes"":""NCR"",""ProvCityRes"":""City of Parañaque""},
{""CaseCode"":""C112981"",""Age"":65,""AgeGroup"":""65 to 69"",""Sex"":""Male"",""DateRepConf"":""4/9/2020"",""DateRecover"":"""",""DateDied"":"""",""RemovalType"":"""",""DateRepRem"":"""",""Admitted"":""Yes"",""RegionRes"":""Laguna"",""ProvCityRes"":""San Pablo City""},
{""CaseCode"":""C113002"",""Age"":8,""AgeGroup"":""5 to 9"",""Sex"":""Female"",""DateRepConf"":""4/15/2020"",""DateRecover"":"""",""DateDied"":"""",""RemovalType"":"""",""DateRepRem"":"""",""Admitted"":""Yes"",""RegionRes"":""Rizal"",""ProvCityRes"":""City of Antipolo (Capital)""},
{""CaseCode"":""C113066"",""Age"":55,""AgeGroup"":""55 to 59"",""Sex"":""Male"",""DateRepConf"":""4/3/2020"",""DateRecover"":"""",""DateDied"":"""",""RemovalType"":"""",""DateRepRem"":"""",""Admitted"":""Yes"",""RegionRes"":""NCR"",""ProvCityRes"":""Pasay City""},
{""CaseCode"":""C113110"",""Age"":21,""AgeGroup"":""20 to 24"",""Sex"":""Male"",""DateRepConf"":""4/7/2020"",""DateRecover"":"""",""DateDied"":"""",""RemovalType"":"""",""DateRepRem"":"""",""Admitted"":""Yes"",""RegionRes"":""Cagayan"",""ProvCityRes"":""Tuguegarao City (Capital)""},
{""CaseCode"":""C113157"",""Age"":71,""AgeGroup"":""70 to 74"",""Sex"":""Male"",""DateRepConf"":""3/25/2020"",""DateRecover"":""4/8/2020"",""DateDied"":"""",""RemovalType"":""Recovered"",""DateRepRem"":""4/16/2020"",""Admitted"":""Yes"",""RegionRes"":""Misamis Oriental"",""ProvCityRes"":""Cagayan De Oro City (Capital)""},
{""CaseCode"":""C114247"",""Age"":63,""AgeGroup"":""60 to 64"",""Sex"":""Female"",""DateRepConf"":""3/29/2020"",""DateRecover"":""4/6/2020"",""DateDied"":"""",""RemovalType"":""Recovered"",""DateRepRem"":""4/14/2020"",""Admitted"":""Yes"",""RegionRes"":""NCR"",""ProvCityRes"":""City of Manila""},
{""CaseCode"":""C114436"",""Age"":66,""AgeGroup"":""65 to 69"",""Sex"":""Male"",""DateRepConf"":""4/2/2020"",""DateRecover"":"""",""DateDied"":"""",""RemovalType"":"""",""DateRepRem"":"""",""Admitted"":"""",""RegionRes"":""NCR"",""ProvCityRes"":""City of Manila""},
{""CaseCode"":""C114575"",""Age"":32,""AgeGroup"":""30 to 34"",""Sex"":""Male"",""DateRepConf"":""4/3/2020"",""DateRecover"":""4/13/2020"",""DateDied"":"""",""RemovalType"":""Recovered"",""DateRepRem"":""4/17/2020"",""Admitted"":""Yes"",""RegionRes"":""NCR"",""ProvCityRes"":""City of Manila""},
{""CaseCode"":""C114716"",""Age"":32,""AgeGroup"":""30 to 34"",""Sex"":""Male"",""DateRepConf"":""3/28/2020"",""DateRecover"":"""",""DateDied"":"""",""RemovalType"":"""",""DateRepRem"":"""",""Admitted"":""Yes"",""RegionRes"":"""",""ProvCityRes"":""""},
{""CaseCode"":""C114745"",""Age"":55,""AgeGroup"":""55 to 59"",""Sex"":""Male"",""DateRepConf"":""3/29/2020"",""DateRecover"":"""",""DateDied"":"""",""RemovalType"":"""",""DateRepRem"":"""",""Admitted"":""Yes"",""RegionRes"":""NCR"",""ProvCityRes"":""City of Makati""}]";

        List<DTO_Model_CaseInfo> JSONData = new List<DTO_Model_CaseInfo>();
        List<DTO_Model_CaseInfo> _cache_regions = new List<DTO_Model_CaseInfo>();
        List<DTO_Model_CaseInfo> _cache_city = new List<DTO_Model_CaseInfo>();

        // for filtering purposes
        public List<string> Regions { get; set; } = new List<string>();
        public List<string> City { get; set; } = new List<string>();
        public List<string> AgeGroup { get; set; } = new List<string>();
        public List<string> Gender { get; set; } = new List<string>();
        public List<string> Admitted { get; set; } = new List<string>();

        static void Main(string[] args)
        {
            c.WriteLine("parsing JSON");

            Program p = new Program();

            using(StreamReader reader = new StreamReader("regioncitydata.json"))
            {
                string j = reader.ReadToEnd();
                p.json = j;
            }

            p.JSONData = JsonConvert.DeserializeObject<List<DTO_Model_CaseInfo>>(p.json);

            p.Regions = p.JSONData.Select(x => x.RegionRes).Distinct().ToList();
            p.City = p.JSONData.Select(x => x.ProvCityRes).Distinct().ToList();

            var allregions = p.GetAllRegionsAsync();
            string jsonregion = JsonConvert.SerializeObject(allregions.Result);

            using (StreamWriter writer = new StreamWriter("PH_regions.json", false) { AutoFlush = true })
            {
                writer.Write(jsonregion);
            }

            foreach (var reg in (List<DTO_Model_Region>)allregions.Result)
            {
                var respcity = p.GetCitiesByRegionNameAsync(reg.RegionName);
                if(respcity.Status)
                {
                    var cities = (List<DTO_Model_City>)respcity.Result;
                    jsonregion = JsonConvert.SerializeObject(cities);

                    foreach (var city in cities)
                    {
                        string filename = city.CityName;
                        filename = filename.Replace(' ', '_');
                        filename = filename.Replace('/', '~');
                        filename = "PH_" + filename + ".json";

                        using (StreamWriter writer = new StreamWriter(filename, false) { AutoFlush = true })
                        {
                            writer.Write(jsonregion);
                        }
                    }
                }
            }
            
            //c.ReadLine();
        }

        public ResponseData GetAllRegionsAsync()
        {
            ResponseData responseData = new ResponseData();
            List<DTO_Model_Region> caseinfolist = new List<DTO_Model_Region>();

            for (int i = 0; i < this.Regions.Count; i++)
            {
                var currentRegion = this.JSONData.Where(x => x.RegionRes == this.Regions[i]).ToList();

                // get confirmed case
                DTO_Model_Region caseInfo = new DTO_Model_Region()
                {
                    RegionName = this.Regions[i],
                    Confirmed = currentRegion.Count
                };

                var recoveredList = currentRegion.Where(x => x.RemovalType.ToUpperInvariant() == "RECOVERED").ToList();
                var deceasedList = currentRegion.Where(x => x.RemovalType.ToUpperInvariant() == "DIED").ToList();

                caseInfo.Recovered = recoveredList.Count;
                caseInfo.Deceased = deceasedList.Count;

                caseinfolist.Add(caseInfo);
            }

            responseData.Result = caseinfolist;
            responseData.Status = true;
            responseData.Message = "GetByRegionAsync";

            return responseData;
        }

        public ResponseData GetCitiesByRegionNameAsync(string regionName)
        {
            ResponseData responseData = new ResponseData();
            List<DTO_Model_City> caseinfolist = new List<DTO_Model_City>();

            this._cache_city = this.JSONData.Where(x => x.RegionRes == regionName).ToList();
            var cityNames = this._cache_city.Select(x => x.ProvCityRes).Distinct().ToList();

            for (int i = 0; i < cityNames.Count; i++)
            {
                var currentCity = this._cache_city.Where(x => x.ProvCityRes == cityNames[i]).ToList();

                // get confirmed case
                DTO_Model_City caseInfo = new DTO_Model_City()
                {
                    CityName = cityNames[i],
                    Confirmed = currentCity.Count
                };

                var recoveredList = currentCity.Where(x => x.RemovalType.ToUpperInvariant() == "RECOVERED").ToList();
                var deceasedList = currentCity.Where(x => x.RemovalType.ToUpperInvariant() == "DIED").ToList();

                caseInfo.Recovered = recoveredList.Count;
                caseInfo.Deceased = deceasedList.Count;

                caseinfolist.Add(caseInfo);
            }

            responseData.Result = caseinfolist;
            responseData.Status = true;
            responseData.Message = "GetByCityAsync";

            return responseData;
        }
    }
}
