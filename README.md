# Chat Aplication

## Technology
![](ChatReactSite/ClientApp/assets/img_6.png)![](ChatReactSite/ClientApp/assets/img_7.png)![](ChatReactSite/ClientApp/assets/img_8.png)![](ChatReactSite/ClientApp/assets/img_9.png)
 
### Login
![](ChatReactSite/ClientApp/assets/img.png)


## Chat Lobby
![](ChatReactSite/ClientApp/assets/img_1.png)


## API REST .net 6
![](ChatReactSite/ClientApp/assets/img_2.png)


## Installation
#### *** The ChatApp.db is included in the repository but it could be created from the code if necessary:
cd /ChatAplication/ChatInfrastructure
dotnet ef --startup-project ../ChatApi migrations add inicial
dotnet ef --startup-project ../ChatApi database update
*********************************************************

#### Running the ChatApi
cd /ChatAplication/ChatApi/ dotnet run

#### Running the React Front
Configure Chat URL "ChatReactSite/ClientApp/.env"
REACT_APP_CHAT_URL=https://localhost:{YOUR-PORT}/chat

cd /ChatAplication/ChatReactSite/ClientApp npm start
or
cd /ChatAplication/ChatReactSite dotnet run

## SingUp is need

#### Interacting with Api
![](ChatReactSite/ClientApp/assets/img_3.png)


#### Add as many rooms as you need
![](ChatReactSite/ClientApp/assets/img_5.png)

