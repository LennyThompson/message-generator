#!/bin/sh

cp ~/dev/cougar-generator/generated/*.cpp ~/dev/cougar-messaging/cougar-messages/
cp ~/dev/cougar-generator/generated/*.h ~/dev/cougar-messaging/cougar-messages/include/
cp ~/dev/cougar-generator/generated/helpers/*.cpp ~/dev/cougar-messaging/message-helpers/
cp ~/dev/cougar-generator/generated/helpers/*.h ~/dev/cougar-messaging/cougar-messages/include/

# Copy the generated java source

cp ~/dev/cougar-generator/generated/java/*.java ~/dev/cougarmessagesclient/src/main/java/com/cougar/messages/
cp ~/dev/cougar-generator/generated/java/interfaces/*.java ~/dev/cougarmessagesclient/src/main/java/com/cougar/messages/interfaces/
cp ~/dev/cougar-generator/generated/java/enums/*.java ~/dev/cougarmessagesclient/src/main/java/com/cougar/messages/enums/
cp ~/dev/cougar-generator/generated/java/defines/*.java ~/dev/cougarmessagesclient/src/main/java/com/cougar/messages/defines/

echo "Copy Complete!"