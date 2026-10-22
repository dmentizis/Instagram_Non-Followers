# Instagram Connection Explorer

A local Windows Forms app for exploring Instagram JSON exports. No login is required.

## Features

- Compare following against followers to find accounts that do not follow you back.
- Select multiple follower export parts together (followers_1.json, followers_2.json, etc.).
- View pending follow requests in a separate tab.
- Open any result in your default browser.
- Resizable layout, read-only grids, result counts, and background loading.
- Case-insensitive matching, duplicate removal, and validation of malformed exports.

## Use

Extract your Instagram JSON export and locate the connections/followers_and_following folder.

1. On **Not following back**, browse for all follower JSON parts and following.json, then click **Compare profiles**.
2. On **Pending requests**, choose pending_follow_requests.json and click **Load requests**.
3. Click **Open in browser** beside a profile to visit Instagram.

Pending requests supports both the label_values export and the older relationships_follow_requests_sent format. Empty exports are valid. Results reflect the date of the export; they are not live account data.

## Build and run

Requires Windows and the .NET 10 SDK (or .NET 10 Desktop Runtime for a built app).

```powershell
dotnet build NonFollowers.sln
dotnet run --project NonFollowers
```

## Regression checks

The checks use generated fixtures and optionally your local exports; private export data is not included in the repository.

```powershell
dotnet run --project NonFollowers.Checks
# Optional: pass the local followers_and_following directory as the first argument.
```
