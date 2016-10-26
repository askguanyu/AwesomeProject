In order to build the project execute the following commands:

1. From terminal:
~~~
dotnet restore
~~~
~~~
dotnet build
~~~
~~~
npm install
~~~
~~~
bower install
~~~

2. From inside VSCode (Ctrl + P):
~~~
task gulp
~~~

In order to publish the project (in 'publish' directory) execute the following command from terminal:

~~~
dotnet publish -o publish
~~~

In order to add user secrets for the development environment execute the following commands from terminal:
~~~
dotnet user-secrets set EmailServer {email_server}
~~~
~~~
dotnet user-secrets set EmailPort {email_port}
~~~
~~~
dotnet user-secrets set EmailSSL {true / false}
~~~
~~~
dotnet user-secrets set EmailAccount {email_account}
~~~
~~~
dotnet user-secrets set EmailPassword {email_password}
~~~
~~~
dotnet user-secrets set Secret {secret}
~~~