REM Copy code output to c++ project destination

copy .\generated\*.cpp C:\lenny\projects\cougar-messages\cougar-messages\
copy .\generated\*.h C:\lenny\projects\cougar-messages\cougar-messages\include\
copy .\generated\helpers\*.cpp C:\lenny\projects\cougar-messages\message-helpers\
copy .\generated\helpers\*.h C:\lenny\projects\cougar-messages\cougar-messages\include\

REM Copy the generated java source

REM copy .\generated\java/*.java C:\lenny\projects\cougarmessagesclient\src\main\java\com\cougar\messages\
REM copy .\generated\java/interfaces/*.java c:\lenny\projects\cougarmessagesclient\src\main\java\com\cougar\messages\interfaces\
REM copy .\generated\java/enums/*.java c:\lenny\projects\cougarmessagesclient\src\main\java\com\cougar\messages\enums\
REM copy .\generated\java/defines/*.java c:\lenny\projects\cougarmessagesclient\src\main\java\com\cougar\messages\defines\

echo "Copy Complete!"
