# Baby Tracker - iOS Installation Instructions

## Prerequisites

### Required Software
- **macOS** (Monterey 12.0 or later)
- **Xcode** (14.0 or later) - Download from Mac App Store
- **.NET 10 SDK** or later - Download from [dotnet.microsoft.com](https://dotnet.microsoft.com/download)

### Required Workloads
Install the MAUI workload for iOS development:

```bash
dotnet workload install maui
```

## Building the App

### Step 1: Clone or Copy the Project
```bash
cd /path/to/project
cd BabyTracker
```

### Step 2: Restore Dependencies
```bash
dotnet restore
```

### Step 3: Build for iOS Simulator
```bash
dotnet build -t:Build -f net10.0-ios -r ios-arm64
```

### Step 4: Build for Physical Device (Requires Apple Developer Account)
```bash
dotnet build -t:Build -f net10.0-ios -r ios-arm64 -p:CodesignKey="Apple Development: Your Name (TEAMID)" -p:CodesignProvision="BabyTracker"
```

## Installing on iOS Device

### Option A: Using Xcode (Recommended)

1. Open the solution in Xcode:
   ```bash
   open ~/.net/maui/10.0.*/packs/Microsoft.iOS.Runtime.*/ios-arm64/BabyTracker.iOS.xcodeproj
   ```

2. Or open the project folder in Xcode:
   ```bash
   open /path/to/BabyTracker/Platforms/iOS/
   ```

3. In Xcode:
   - Select your Apple Developer account in **Signing & Capabilities**
   - Connect your iOS device via USB
   - Select your device from the device dropdown
   - Click the **Run** button (▶️)

### Option B: Using dotnet CLI

1. Build the IPA file:
   ```bash
   dotnet publish -f net10.0-ios -c Release -r ios-arm64
   ```

2. Find the IPA file:
   ```bash
   ls -la ~/Library/Developer/Xcode/DerivedData/BabyTracker-*/Build/Products/Release-iphoneos/
   ```

3. Install using Apple Configurator 2 or Transporter app

### Option C: Using TestFlight

1. Archive the app in Xcode:
   - Product → Archive
   - Select "Distribute App" → "App Store Connect"
   - Upload to TestFlight

2. Install via TestFlight app on your iOS device

## Running on iOS Simulator

### List Available Simulators
```bash
dotnet build -t:Run -f net10.0-ios --no-deploy -p:_BuiildSimulator=true
```

### Run on Specific Simulator
```bash
dotnet build -t:Run -f net10.0-ios -p:_BuiildSimulator=true -p:DeviceName="iPhone 15"
```

Or use Visual Studio:
1. Open the solution in Visual Studio
2. Select the iOS Simulator from the device dropdown
3. Click the Run button

## Troubleshooting

### Common Issues

1. **"No iOS device connected"**
   - Ensure your device is connected via USB
   - Trust the computer on your iOS device
   - Enable Developer Mode: Settings → Privacy & Security → Developer Mode

2. **"Code signing failed"**
   - Verify your Apple Developer account is active
   - Check Bundle Identifier matches your provisioning profile
   - Regenerate provisioning profiles if needed

3. **"MAUI workload not installed"**
   ```bash
   dotnet workload install maui
   dotnet workload install maui-ios
   ```

4. **Build errors after Xcode update**
   ```bash
   dotnet workload update
   ```

### Clean and Rebuild
```bash
dotnet clean
dotnet build -f net10.0-ios
```

## App Features

- **Record Wakeup Times**: Tap the "Record Wakeup" button to log when the baby wakes up
- **Record Feeding Times**: Tap the "Record Feeding" button to log feeding times
- **View History**: See all recorded activities with timestamps
- **Filter Records**: Filter by wakeup or feeding records
- **Delete Records**: Swipe left on a record to delete it
- **Today's Summary**: See how many wakeups and feedings occurred today

## Data Storage

The app uses SQLite for in-memory storage. Data persists across app sessions but:
- Data is stored locally on the device
- No cloud sync (add your own implementation if needed)
- Data can be cleared by uninstalling the app

## Customization

### Change Colors
Edit `Resources/Styles/Styles.xaml` to modify the color scheme:
- `PrimaryColor` - Pink (#FF6B9D)
- `SecondaryColor` - Teal (#4ECDC4)
- `BackgroundColor` - Light gray (#F7F8FC)

### Add Notes to Records
To add notes to records, modify the `BabyRecord` model and UI to include a text input field.
