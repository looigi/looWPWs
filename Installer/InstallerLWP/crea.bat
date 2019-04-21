cd %5
del %2
AppCmd delete site %1 >> %2
AppCmd add site /name:%1 /id:111 /physicalPath:%6 /bindings:http://localhost:%7 >> %2
AppCmd add vdir /app.name:"%1/" /path:/%3 /physicalPath:%4 >> %2
AppCmd add vdir /app.name:"%1/" /path:/%8 /physicalPath:%9 >> %2
appcmd add apppool /name:apppoolLWP /managedRuntimeVersion:"v4.0" /managedPipelineMode:"Integrated" /enable32BitAppOnWin64:true >> %2
APPCMD.exe set app "%1/" /applicationPool:apppoolLWP
