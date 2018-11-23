"# InterviewWebApi" 

Comments:
- I have write only WebAPI, without UI, as this was not mentioned in task.
- all logic put into controller, as this task was not having a lot. 
  However in real situations, that should be covered by some other object to encapsulte business logic away from asp part.
- there is only model created for use in API. In bigger projects, I would create a separate domain / business object in core project
  and the one presented here will be only a DTO one, used for purpose of communication (but all depends on scope / design / final complexity)
- Have tested using Postman

Answers to Questions:
- implementing testing: I have wrote unit tests. I would wrote some integration tests also in real project.
- implementing security: this is WebAPI, I would use e.g. JsonWebToken for authentication.
- documentation: I would use e.g. Swagger. We are using it in my current company (as a consumer of API)

Remarks:
- To be Agile, I am sending a first version of task. In case of comments or request, I can implement some additional parts during remaining time for the task.
  But I have some time constraints on my own this week, so I would not like to code more if not really required, if possible.
