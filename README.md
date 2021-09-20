<h1> Artful API</h1>
<p> 🟣 The project is a study of creating a web service with ASP.NET Core.</p>

<h2>How to test it? </h2>
<p>The API is published at Heroku : https://artfulapi.herokuapp.com/swagger </p>
<p> ❗ To interact with protected endpoints such as 'delete' you must authenticate first on 'login' endpoint and generate your json web token. After that click in the 'Authorize' button at page top and write in the dialog window 'bearer {token}' replacing '{token}' by the generated JWT. </p>
<p>Locally</p>
<ol>
  <li>git clone this repo</li>
  <li>move to the api folder and execute the command: 'dotnet restore' for dependencies checking</li>
  <li>assign values to environment variables at the appsettings.json file</li>
  <li>execute command: 'dotnet watch run' to get the api up at 'localhost:5001'</li>
  <li>If running the test project dont forget to type in the path and name of the file containing environment variables inside the test class </li>
</ol>

<h2>Technologies and methods used</h2>
<ul>
  <li>AspNet Core </li>
  <li>Postgres SQL</li>
  <li>Entity Framework Core</li>
  <li>Code First / migrations</li>
  <li>Input validation with FluentValidation</li>
  <li>Authentication JWT based</li>
  <li>Testing with xUnit</li>
  <li>Client with SwaggerUI</li>
  <li>Deploy in the Heroku Cloud</li>
  <li>Git Flow versioning flow</li>
</ul>
