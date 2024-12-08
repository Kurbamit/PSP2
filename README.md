# Point of sale system

Vilnius university, 3rd course Software Engineering Design course. Group project on creating POS system.

### Coding standard
- Coding standard are written [here](https://www.geeksforgeeks.org/c-sharp-coding-standards/).
- Instead of ```""``` write ```string.Empty```.
- <u>Every **DB** update **MUST** be done by migrations</u>.

### Pull-Request Conventions
- All changes should be done in different branch.
  - Branch naming: **tasks/TaskName**  or **bugs/BugName**
- PR should be reviewed at least by one contributor.
- Commits are made in logical blocks.
- Delete branches after merge.

### How to run the app.
Requirements:
- npm version: **20.11.0**
- dotnet version: **8.0.10**

1. Clone repo
2. Open reactapp1.client folder
3. Run ```npm install```
   1. There could be a problem running `npm install`. Then try running `npm install --legacy-peer-deps`.
4. Create postgreSql database with these parameters:
   1. name: ```postgres```
   2. password: ```1234```
   3. port: ```5432```
5. Run ```dotnet ef database update```

### HTTPS Certificate problem
There could be HTTPS problem starting reactapp1.client

If you get this error, you should open ```/Users/username/.aspnet/https`` folder with terminal.
Run following commands:
- ```openssl genrsa -out reactapp1.client.key 2048```
- ```openssl req -x509 -new -nodes -key pspapp.key -sha256 -days 365 -out reactapp1.client.pem```

### Dummy data
There is a `.sql` script for adding dummy data.
The database should be empty for it to work properly.
Run the `DummyData.sql` file in your database.
Login with admin:
- Email: `john.krasinski@gmail.com`
- Password: `123`