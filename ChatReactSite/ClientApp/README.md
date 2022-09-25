# Chat Aplication

## Technology
![img_6.png](img_6.png)
![img_7.png](img_7.png)
![img_8.png](img_8.png)
![img_9.png](img_9.png)
### Login

![img.png](img.png)

## Chat Lobby

![img_1.png](img_1.png)

## API REST .net 6 

![img_2.png](img_2.png)

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

![img_3.png](img_3.png)

#### Add as many rooms as you need

![img_5.png](img_5.png)