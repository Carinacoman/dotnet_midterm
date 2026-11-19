# BUGS

The starter contains behaviors that contradict `ASSIGNMENT.md`. Write here every discrepancy you found.
Fill it in yourself, in your own words. There is no required number: what counts is that they are real.

| Which rule from ASSIGNMENT.md is violated (R1–R8) | Where the problem was in the starter (file, approximate line) | How you discovered it (test, reading, AI) | What you changed |
|---|---|---|---|
| R7 | Services/BookingService.cs, line 28: `<=` in the overlap check | reading the code + AI | Booking.OverlapsWith uses `<`, so touching bookings do not overlap |
| R7/R8 | Services/BookingService.cs, line 26: cancelled bookings were also checked | reading the code | only Active bookings count (OverlapsWith + GetActiveForRoomAsync) |
| R8 | Services/BookingService.cs, Cancel, lines 56-60: second cancel returned 204 | reading the code | Booking.Cancel() returns 409 if already cancelled |
| R5 | BookingService.cs: no check for start in the past | reading ASSIGNMENT.md | Booking.Create checks start < now, now from TimeProvider |
| R6 | BookingService.cs: no maximum duration | reading ASSIGNMENT.md | Booking.Create checks duration > 4 hours |
| Persistence | Repositories/*: data only in memory | reading the code | EF Core + SQLite (roomhub.db) |
