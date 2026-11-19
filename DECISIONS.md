# DECISIONS

Answer **yourself**, in your own words, in 3–5 sentences per question.
Refer to concrete files and classes from your own repo (for example `Features/Bookings/CreateBooking/...`).
An unanswered question gives 0 points.

## 1. A rule you moved
Pick one business rule (R1–R8). In which file did it live in the starter and in which file does it live now? Why there?

R7 (overlap) was in Services/BookingService.cs, in Create. Now it is in Domain/Bookings/Booking.cs, in Create and OverlapsWith. It belongs to the booking, so the domain checks it, not the endpoint. There I also fixed `<=` to `<` and count only Active bookings.

## 2. Result instead of an exception
Why is `Result` a better choice than an exception for "the room is occupied"? What happens in your code when an `Error` is returned?

"The room is occupied" is an expected situation, not a crash, so I return it as a value. Booking.Create returns Result<Booking> with an Error of type Conflict. The handler returns the error, and ErrorResults.ToProblem turns it into ProblemDetails with 409. No try/catch is needed.

## 3. Who knows about EF Core?
Which classes in your project know about EF Core and which do not? What do you gain and what do you pay with Repository + Unit of Work?

Only the classes in Infrastructure/Persistence know EF Core: RoomHubDbContext, EfRoomRepository, EfBookingRepository, EfUnitOfWork. The domain and the handlers only use IRoomRepository, IBookingRepository and IUnitOfWork. I gain code that does not depend on the database. I pay with more files and interfaces.

## 4. The flow of a request
Draw in text (ASCII arrows) the path of a `POST /api/rooms/{roomId}/bookings` request through your project, with the **real** names of files and classes, from the endpoint to the database.

POST /api/rooms/{roomId}/bookings
-> CreateBookingEndpoint (Features/Bookings/CreateBooking)
-> CreateBookingHandler
-> IRoomRepository.GetByIdAsync (EfRoomRepository) -> 404 if missing
-> IBookingRepository.GetActiveForRoomAsync + CountActiveForParticipantAsync
-> Booking.Create (R2-R7, R9) -> Result<Booking>
-> IBookingRepository.Add -> IUnitOfWork.CommitAsync (EfUnitOfWork)
-> RoomHubDbContext -> SQLite (roomhub.db)
-> 201 Created, or ProblemDetails 400/404/409

## 5. What you rejected from what the AI gave you
Give a concrete example of code or an idea proposed by the AI that you corrected or rejected, and say why.

_(your answer here)_

## 6. If you had 30 more minutes
What would you improve and what risk did you leave in the project?

I would add automated tests for R5-R9. A risk: two requests at the same moment can both pass the overlap check, because there is no lock or unique rule in the database.
