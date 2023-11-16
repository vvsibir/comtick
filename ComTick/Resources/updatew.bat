set p=cmdWork
UpdatorUrl urljson:http://xoxoxoesy.esy.es/cmdComTickCommon/updatecfg.json
UpdatorUrl urljson:http://xoxoxoesy.esy.es/%p%/updatecfg.json
copy DOWNLOAD\* * /y
start comtick.exe