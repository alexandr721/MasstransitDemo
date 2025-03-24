# Masstransit Rabbit
1. To build & start docker's conteiner
./docker-compose up --build -d

2. Swagger AuthService
http://localhost:5001/swagger/index.html

3. The register user endpoint:
  - store user in the AuthDb.Users
  - sends UserRegisteredEvent message to UserService and OrderService
  These services stores the user data into UserDb.Users and OrderDb.Users

4. The logout endpoint:
  - delete user from the AuthDb.Users
  - sends UserLoggedOutEvent message to UserService and OrderService
  These services deletes the user data from UserDb.Users and OrderDb.Users

5. Rabbit interface:
  http://localhost:15672/
  login: guest
  passw: guest

6. pgAdmin 
  http://localhost:5050/browser/
  login: admin
  passw: admin
  