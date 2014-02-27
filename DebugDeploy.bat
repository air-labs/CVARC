mkdir deploy
mkdir deploy\Server
xcopy CVARC\CVARC.Network\bin\Debug\*.dll deploy\Server /y
xcopy CVARC\CVARC.Network\bin\Debug\*.exe deploy\Server /y
xcopy CVARC\CVARC.Network\bin\Debug\*.config deploy\Server /y
xcopy CVARC\CVARC.Network\bin\Debug\*.png deploy\Server /y

xcopy CVARC\CVARC.Tutorial\bin\Debug\*.exe deploy\Server /y
xcopy CVARC\CVARC.Tutorial\bin\Debug\*.dll deploy\Server /y
xcopy CVARC\CVARC.Tutorial\bin\Debug\*.config deploy\Server /y

xcopy Competitions\Client\bin\Debug\Client.exe deploy\ /y
xcopy Competitions\Client\bin\Debug\Client.exe.config deploy\ /y

"C:\Program Files\WinRar\rar" a deploy deploy\*.*
REM rd deploy /s /q