# Install Baby Tracker with 3uTools

## Option 1: Build on Your Mac (Recommended)

### Prerequisites
- macOS with Xcode installed
- .NET 10 SDK: https://dotnet.microsoft.com/download

### Steps

```bash
# 1. Clone or copy the project to your Mac
# 2. Open Terminal and navigate to the project
cd /path/to/BabyTracker

# 3. Run the build script
chmod +x build_ipa.sh
./build_ipa.sh
```

### Or build manually:
```bash
dotnet publish -f net10.0-ios -c Release -r ios-arm64 -p:ArchiveOnBuild=true
```

### Find the IPA:
```bash
# The IPA will be in one of these locations:
find ~/Library/Developer/Xcode/DerivedData -name "*.ipa" -type f

# OR in the project folder:
ls -la bin/Release/net10.0-ios/ios-arm64/publish/*.ipa
```

---

## Option 2: Build with GitHub Actions (No Mac Needed)

1. Push this project to GitHub
2. Go to Actions tab → "Build iOS IPA" → Run workflow
3. Wait ~10-15 minutes
4. Download the IPA artifact from the workflow run

---

## Installing with 3uTools

### Step 1: Connect Your iPhone
- Connect iPhone to PC via USB
- Open 3uTools
- Make sure iPhone is detected

### Step 2: Install IPA
1. In 3uTools, go to **Toolbox**
2. Click **"Install IPA"** or **"IPA Signature"**
3. Drag and drop the `.ipa` file
4. Click **"Install"**

### Step 3: Trust the Developer Profile
On your iPhone:
1. Go to **Settings** → **General** → **VPN & Device Management**
2. Find the developer profile (your Apple ID or enterprise cert)
3. Tap **"Trust"**

### Step 4: Open the App
- Find "Baby Tracker" on your home screen
- Open it and start recording!

---

## Troubleshooting

| Problem | Solution |
|---------|----------|
| "Untrusted Developer" | Go to Settings → General → VPN & Device Management → Trust |
| "Unable to install" | Make sure your Apple ID is added to the device |
| App crashes | Rebuild with a different signing certificate |
| 3uTools can't find IPA | Make sure IPA is not in a folder with special characters |

---

## Free Apple Developer Account

If you don't have a paid Apple Developer account ($99/year):

1. Use your free Apple ID
2. In Xcode: Xcode → Settings → Accounts → Add Apple ID
3. Create a provisioning profile with your Apple ID
4. Build with your Apple ID as the signing identity

The app will work for 7 days before you need to rebuild (free account limitation).
