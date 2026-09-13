#!/bin/bash
# Baby Tracker - iOS IPA Build Script for 3uTools
# Run this on a MAC with .NET 10 SDK installed

set -e

echo "🍼 Building Baby Tracker IPA..."
echo ""

# Check for dotnet
if ! command -v dotnet &> /dev/null; then
    echo "❌ .NET SDK not found. Install from: https://dotnet.microsoft.com/download"
    exit 1
fi

# Check for MAUI workload
if ! dotnet workload list | grep -q "maui-ios"; then
    echo "📦 Installing MAUI iOS workload..."
    dotnet workload install maui-ios
fi

echo "🔨 Building IPA..."
dotnet publish \
    -f net10.0-ios \
    -c Release \
    -r ios-arm64 \
    -p:ArchiveOnBuild=true \
    -p:EnableAssemblyILStripping=false

# Find the IPA
IPA_DIR="$HOME/Library/Developer/Xcode/DerivedData"
IPA_FILE=$(find "$IPA_DIR" -name "*.ipa" -type f 2>/dev/null | head -1)

if [ -z "$IPA_FILE" ]; then
    # Alternative location
    IPA_FILE="bin/Release/net10.0-ios/ios-arm64/publish/BabyTracker.ipa"
fi

if [ -f "$IPA_FILE" ]; then
    echo ""
    echo "✅ IPA built successfully!"
    echo "📁 Location: $IPA_FILE"
    echo ""
    echo "📱 To install with 3uTools:"
    echo "   1. Connect your iPhone via USB"
    echo "   2. Open 3uTools"
    echo "   3. Go to 'Toolbox' → 'Install IPA'"
    echo "   4. Drag and drop the IPA file"
    echo "   5. Click 'Install'"
else
    echo "⚠️  IPA not found. Check the build output above."
    echo "   Look for the .ipa file in the DerivedData folder."
fi
