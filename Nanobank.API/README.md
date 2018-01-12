# Nanobank WebApi

## Features

* ASP NET WebApi 2
* ASP NET Identity
* Entity Framework
* Building an Initial Model & Database via EF Code First Migrations 
* Use Bearer Authentication Service for token-based access
* Use Unity DI
* Send Email notification via SendGrid Service
* Send Push notification on Android via Firebase Cloud Messaging Service (FCM)

## API

### User
* _GET_ api/user/all - **admin**
* _GET_ api/user/all/unapproved - **admin**
* _GET_ api/user/{userName} - **user**

### Account
* _POST_ api/account/register - **anonymous**
* _PUT_ api/account/approve/{username} - **admin**
* _PUT_ api/account/{username}/add/role - **admin**
* _DELETE_ api/account/{username} - **admin**

### Deal
* _GET_ api/deal/all - **admin**
* _GET_ api/deal/all/opened - **user**
* _GET_ api/deal/{username}/all - **user**
* _GET_ api/deal/{dealId} - **user**
* _POST_ api/deal/register - **user**
* _PUT_ api/deal/{dealId} - **user**
* _PUT_ api/deal/respond/{dealId} - **user**
* _PUT_ api/deal/close/{dealId} - **user**
* _PUT_ api/deal/{dealID}/set/rating - **user**
* _DELETE_ api/deal/{dealId}
	* **user** -> own deal
	* **admin** -> any deal

### Complain
* _GET_ api/complain/all - **admin**
* _GET_ api/complain/{complainId} - **admin**
* _POST_ api/complain/add - **user**
* _DELETE_ api/complain/{complainId} - **admin**

### Credit Card
* _GET_ api/creditcard/all - **admin**
* _PUT_ api/creditcard/transit - **user**
