# Non-Followers

A simple Windows Forms application that helps you identify which Instagram accounts you follow that **do not follow you back**.

The app takes two JSON files exported from Instagram's **Data Download**:
- The list of accounts that follow you  
- The list of accounts you follow  

It then compares the two lists and displays all users you follow who aren't following you back.

This tool runs locally on your machine and does not require login or access to your Instagram account — only the exported data files.

## Features
- Load Instagram “followers” and “following” JSON exports  
- Automatically compare both lists  
- View which accounts don’t follow you back  
- Simple and lightweight Windows Forms interface  

## How to Use
1. Request your Instagram data export from the **Instagram Settings > Privacy and Security > Download Your Data** page.  
2. Extract the downloaded ZIP and locate the `followers_1.json` and `following.json` files.  
3. Open the application and select the two JSON files.  
4. View the generated list of accounts that don’t follow you back.

## Requirements
- Windows  
- .NET Framework / .NET Runtime (version your app uses)
