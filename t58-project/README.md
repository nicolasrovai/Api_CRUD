
#User Model Seeder

* The User Model Seeder creates 20 users.

* 10 users with the role of "standard user" and 10 with the role "Admin user".

* the users password is "user" with the concatenated Id.

##Admin users email format

Email: 		mail<1-10>@mail.com
Password:	Admin123

Use Example:
{"email":"mail10@mail.com","password":"Admin123"}

##Standard users email format

Email: 		mail<11-20>@mail.com
Password:	User123

Use Example:
{"email":"mail15@mail.com","password":"User123"}