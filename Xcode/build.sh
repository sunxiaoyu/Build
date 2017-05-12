#!/bin/sh

xcodePath=$1
projectPath=$2

archivePath="build/Unity-iPhone.xcarchive"
exportPath=$3 #Unity-iPhone.ipa
provisioningProfile=$4 #iOS_Distribution

cd $projectPath

${xcodePath}/Contents/Developer/usr/bin/xcodebuild -scheme Unity-iPhone clean archive -archivePath $archivePath
${xcodePath}/Contents/Developer/usr/bin/xcodebuild -exportArchive -exportFormat ipa -archivePath $archivePath -exportPath $exportPath -exportProvisioningProfile "$provisioningProfile"


# ${xcodePath}/Contents/Developer/usr/bin/xcodebuild build

# cd ${projectPath}/build/Release-iphoneos/

# ${xcodePath}/Contents/Developer/usr/bin/xcrun -sdk iphoneos packageapplication -v *.app -o $exportPath

exit $?