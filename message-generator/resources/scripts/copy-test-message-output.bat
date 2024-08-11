REM Copy code output to c++ project destination

copy .\generated\test\*.cpp C:\lenny\projects\test-cougar-messages\test-cougar-messages\
copy .\generated\test\*.h C:\lenny\projects\test-cougar-messages\test-cougar-messages\
copy .\generated\test\helpers\*.cpp C:\lenny\projects\test-cougar-messages\test-cougar-messages\
copy .\generated\test\helpers\*.h C:\lenny\projects\test-cougar-messages\test-cougar-messages\

REM Copy the generated java source

REM copy .\generated\java/*.java C:\lenny\projects\cougarmessagesclient\src\main\java\com\cougar\messages\
REM copy .\generated\java/interfaces/*.java c:\lenny\projects\cougarmessagesclient\src\main\java\com\cougar\messages\interfaces\
REM copy .\generated\java/enums/*.java c:\lenny\projects\cougarmessagesclient\src\main\java\com\cougar\messages\enums\
REM copy .\generated\java/defines/*.java c:\lenny\projects\cougarmessagesclient\src\main\java\com\cougar\messages\defines\

echo "Copy Complete!"
