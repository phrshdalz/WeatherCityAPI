________________________________________
How much time did you spend on this task?
I spent around 5 hours on this task.
________________________________________
If you had more time, what improvements or additions would you make?
•	Add real time weather updates using SignalR to display live temperature and conditions.
Although I havent worked with SignalR in production yet , Im familiar with its capabilities and know it can be used for this purpose.
•	Improve error handling and logging to make the service more reliable.
•	Write unit tests for better validation and maintainability.
________________________________________
What is the most useful feature recently added to your favorite programming language?
Please include a code snippet to demonstrate how you use it.
Although its not a brand new feature, one of the most practical and useful features in Asp.Net Core is JsonPatchDocument,
which allows partial updates to Json objects through http Patch requests.
Instead of sending the entire object for an update, we can send only the fields that have changed.
This makes APIs more efficient and network friendly, especially in large systems.

Example:
[HttpPatch("{id}")]
public IActionResult UpdateUser(int id, [FromBody] JsonPatchDocument<User> patchDoc)
{
    if (patchDoc == null)
        return BadRequest();

    var user = _context.Users.FirstOrDefault(u => u.Id == id);
    if (user == null)
        return NotFound();

    patchDoc.ApplyTo(user);
    _context.SaveChanges();

    return Ok(user);
}
Example Request (JSON body):
[
  { "op": "replace", "path": "/name", "value": "Farshad" }
]
________________________________________
How do you identify and diagnose a performance issue in a production environment?
Have you done this before?
To identify performance or functional issues in production, I usually:
1.	Check application logs to understand where the issue occurred.
2.	Reproduce the issue locally if possible and use debugging to trace the root cause.
3.	Add or improve unit tests to prevent similar problems from happening again.
4.	Review database queries if the problem seems related to data fetching or slow response.
Yes, I've done this before.
During the first six months of my career, I worked in a support role, where I analyzed logs, identified errors, and fixed them by debugging the application.
This experience helped me develop a strong understanding of how to detect and resolve production issues efficiently.
________________________________________
Whats the last technical book you read or technical conference you attended?
What did you learn from it?
I havent read a full technical book recently, but I’ve watched educational videos from:
•	IcodeNext (YouTube) — covering topics from the C# in a Nutshell series.
•	WithCodeMosh (YouTube) — focusing on clean architecture and .NET best practices.
These helped me deepen my understanding of C# fundamentals, architecture patterns, and code quality.
________________________________________
Whats your opinion about this technical test?
I found it interesting because its project based, which allows candidates to face real world implementation challenges.
It also helps demonstrate not only coding ability but also understanding of design, structure, and problem solving skills.
________________________________________
Please describe yourself using JSON format:
{
  "name": "Farshad Alizadeh Afshar",
  "role": ".NET Developer",
  "experience": "3 years",
  "skills": [
    "C#",
    ".NET Core",
    "Entity Framework",
    "SQL Server",
    "Redis",
    "Clean Architecture",
    "Unit Testing",
    "RESTful APIs"
  ],
  "interests": [
    "Learning new technologies",
    "Writing clean and testable code",
    "Exploring software architecture patterns"
  ]
}

