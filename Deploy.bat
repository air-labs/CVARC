mkdir deploy
mkdir deploy\solutions
mkdir deploy\solutions\cs

xcopy Competitions\build\CVARC.Tutorial.exe deploy\ /y
xcopy Competitions\build\CVARC.Tutorial.exe.config deploy\ /y
xcopy Competitions\build\Fall2013.0.dll deploy\ /y
xcopy Competitions\build deploy\solutions\cs\build\ /y /e
xcopy Competitions\Client deploy\solutions\cs\Client\ /y /e
rd deploy\solutions\cs\Client\bin /s /q
rd deploy\solutions\cs\Client\obj /s /q

"C:\Program Files\WinRar\rar" a deploy deploy\*.*
REM rd deploy /s /q