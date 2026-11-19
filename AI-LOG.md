# AI-LOG

Write down, in order, what you asked the AI. It is not a punishment: a missing or false log lowers the level D grade.

| Time (approx.) | What you asked (briefly) | What you kept | What you changed or rejected |
|---|---|---|---|
| 09:02 | Transform the starter into vertical slices, Result, handlers, EF Core | Features, Domain, Infrastructure, Common | I moved the files into my repo myself |
| 09:16 | How to open the project in Rider and push to GitHub | the git commands | I made a new clean repo |
| 09:30 | Implement R9 (max 2 active bookings per participant) | the rule in Booking.Create and the repository count | I checked the build myself |
| 09:36 | Implement R9 (2 active bookings per UTC day), R10 (2h for rooms > 10), R11 (cancel at least 60 min before) | rules in Booking.Create and Booking.Cancel, new GetActiveForParticipantAsync | I ran the build and pushed |