#!/bin/bash
# Baby Tracker - iOS IPA Build Script
# Run this on macOS with Xcode and .NET 10 SDK

set -e

echo "🍼 Building Baby Tracker IPA..."
echo ""

# Check for dotnet
if ! command -v dotnet &> /dev/null; then
    echo "❌ .NET SDK not found. Install from: https://dotnet.microsoft.com/download"
    exit 1
fi

# Check for Xcode
if ! command -v xcodebuild &> /dev/null; then
    echo "❌ Xcode not found. Install from Mac App Store"
    exit 1
fi

# Check for MAUI workload
echo "📦 Checking MAUI workload..."
dotnet workload install maui maui-ios 2>/dev/null || true

echo ""
echo "🔨 Step 1: Restoring packages..."
dotnet restore

echo ""
echo "🔨 Step 2: Building MAUI iOS..."
dotnet build \
    -f net10.0-ios \
    -c Release \
    -r ios-arm64

echo ""
echo "🔨 Step 3: Finding Xcode project..."

# Find the generated Xcode project
XCODEPROJ=$(find ~/.net -name "*.xcodeproj" -type d 2>/dev/null | grep -v "xcframeworks" | head -1)

if [ -z "$XCODEPROJ" ]; then
    echo "Trying alternative locations..."
    XCODEPROJ=$(find ~/Library/Developer/Xcode/DerivedData -name "*.xcodeproj" -type d 2>/dev/null | head -1)
fi

if [ -z "$XCODEPROJ" ]; then
    echo "❌ Could not find Xcode project. Try: dotnet build -f net10.0-ios -c Release first"
    exit 1
fi

echo "✅ Found Xcode project: $XCODEPROJ"

# Get scheme
SCHEME=$(xcodebuild -list -project "$XCODEPROJ" -json 2>/dev/null | python3 -c "import sys,json; d=json.load(sys.stdin); print(d['project']['schemes'][0])" 2>/dev/null || echo "BabyTracker")

echo "📱 Using scheme: $SCHEME"

echo ""
echo "🔨 Step 4: Archiving..."

mkdir -p ./build

xcodebuild archive \
    -project "$XCODEPROJ" \
    -scheme "$SCHEME" \
    -archivePath ./build/BabyTracker.xcarchive \
    -destination "generic/platform=iOS" \
    -allowProvisioningUpdates \
    2>&1 | tail -20

echo ""
echo "🔨 Step 5: Exporting IPA..."

# Create export options
cat > exportOptions.plist << EOF
<?xml version="1.0" encoding="UTF-8"?>
<!DOCTYPE plist PUBLIC "-//Apple//DTD PLIST 1.0//EN" "http://www.apple.com/DTDs/PropertyList-1.0.dtd">
<plist version="1.0">
<dict>
    <key>method</key>
    <string>development</string>
    <key>teamID</key>
    <string></string>
    <key>signingStyle</key>
    <string>automatic</string>
</dict>
</plist>
EOF

xcodebuild -exportArchive \
    -archivePath ./build/BabyTracker.xcarchive \
    -exportOptionsPlist exportOptions.plist \
    -exportPath ./build/ipa

echo ""
IPA_PATH=$(find ./build -name "*.ipa" -type f 2>/dev/null | head -1)

if [ -f "$IPA_PATH" ]; then
    echo "✅ IPA built successfully!"
    echo ""
    echo "📁 IPA Location: $IPA_PATH"
    echo ""
    echo "📱 To install with 3uTools:"
    echo "   1. Connect your iPhone via USB"
    echo "   2. Open 3uTools"
    echo "   3. Go to 'Toolbox' → 'Install IPA'"
    echo "   4. Drag and drop: $IPA_PATH"
    echo "   5. Click 'Install'"
else
    echo "⚠️  IPA not found. Check the build output above."
fi
