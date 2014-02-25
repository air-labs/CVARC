mkdir deploy
xcopy CVARC\CVARC.Network\bin\debug\*.dll deploy\ /y
xcopy CVARC\CVARC.Network\bin\debug\*.exe deploy\ /y
xcopy CVARC\CVARC.Network\bin\debug\*.config deploy\ /y
xcopy CVARC\CVARC.Tutorial\bin\debug\*.exe deploy\ /y
xcopy CVARC\CVARC.Tutorial\bin\debug\*.dll deploy\ /y
xcopy CVARC\CVARC.Tutorial\bin\debug\*.config deploy\ /y
xcopy Competitions\Fall2013.0\bin\Debug\*.dll deploy\ /y
xcopy Competitions\Fall2013.0\bin\Debug\*.png deploy\ /y
xcopy Competitions\DemoNetworkClient\bin\Debug\*.exe deploy\ /y
"C:\Program Files\WinRar\rar" a deploy deploy\*.*
rd deploy /s /q