# Microsoft Teams Call
This repository is for developers who want to know..
- How to start Microsoft Teams group call from your app
- How to let user join existing Microsoft Teams online meeting by your app

## Background
[Microsoft Teams](https://products.office.com/en-us/microsoft-teams/group-chat-software) is popular collaboration tool. Users can chat, call and having online meeting with their colleagues.
Developers want to integrate their application with Teams. For example, their app triggers to start new group call with specific members. Thus users can collaborate well even if developers don't need to implement their own online meeting features and infrastructure.

## What you can see with this repo
A user can get group call from your app.

TODO: Will paste screen shot here.

A user can be invited to existing online meeting by your app.

TODO: Will paste screen shot here.

# Technical consideration
We uses following language and tools. As prerequistics, please read and try each tools tutorial.

## Architecture
TODO: Create architecture diagram with Azure components.
TOOD: Create wiki for describing sequrnece.
You can see detailed sequence diagrams in wiki.

## Language, SDK and utilities
### Programming language
- C# with .Net Core 3.1: Because we can utilize dependecy injection on Azure Functions and Azure SDKs, we picked up C# for this sample.

### Tools for back-end application
- [Azure Functions](https://azure.microsoft.com/en-us/services/functions/): Serverless platform to run your code. In this repository, we simply implement without database for keeping sample simple.

### Tools for Teams call
- [Microsoft Graph](https://developer.microsoft.com/en-us/graph/): You can utilize Micorosft Graph to utilize Microsoft 365 back-end. For example, you can fetch users' email, calendar.. etc. In this repository, we utilize it to integrate Microsoft Teams.
- [Azure Active Directory](https://azure.microsoft.com/en-us/services/active-directory/): This is identitiy platform. For utilizing Microsoft Graph, you need to utilize it for making secure connection between your app and Microsoft Graph (Microsoft Teams).
- [Azure Bot Service](https://azure.microsoft.com/en-us/services/bot-service/): Microsoft graph need bot to start Teams Call.

### Authorization flow
- [OAuth 2.0 client credential flow](https://docs.microsoft.com/en-us/azure/active-directory/develop/v2-oauth2-client-creds-grant-flow): Because our app is worker/deamon type service and can't have user interaction, we need to utilize client credential flow to fetch access token for Microsoft Graph.

# How to setup environment
TODO: Will update detailed steps. I'll create wiki and refer it from here

1. Create your Microsoft 365 environment with [developer program](https://developer.microsoft.com/en-us/microsoft-365/dev-program).
1. Register app in you Azure AD and memo following information (wiki)
   - Client Id
   - Client Secret
   - Tenant Id
1. Setup the app permission in Azure AD (wiki)
1. Install [Visual Studio Code](https://code.visualstudio.com/)
1. Clone repository and open with Visual Studio Code. If you get recommendation to install dependencies (extension and cli), please install them.
1. Update `local.settings.json` with copied `Client Id`, `Client Secret` and `Tenant Id`.
1. Setup Azure Bot Service and enable Teams feature
1. Setup ngrok for accepting webhook from Microsoft Graph (wiki)
1. Press [F5] key to run Azure Functions locally.
1. TODO: Update information how to call API (Don't have API now)


# Consideration for productions
TODO: Will write up about Key-Vault, multi-tenant related information here.