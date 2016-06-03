use RandomDb

INSERT INTO Participants (Name) VALUES ('Tarzan')
INSERT INTO Participants (Name) VALUES ('Faccio')
INSERT INTO Participants (Name) VALUES ('Trusser')
INSERT INTO Participants (Name) VALUES ('Nosser')
INSERT INTO Participants (Name) VALUES ('Leffo')
INSERT INTO Participants (Name) VALUES ('Tennedz')
INSERT INTO Participants (Name) VALUES ('Aallex')

INSERT INTO dbo.Events (EventName, Measure, SoundClipUrl) VALUES ('GOAL!!!', 'Other_3', 'D:\Arbejde\randomizer\Randomizer\RandomizerSounds\1000dollars.wav')
INSERT INTO dbo.Events (EventName, Measure, SoundClipUrl) VALUES ('Assist', 'Other_2', 'D:\Arbejde\randomizer\Randomizer\RandomizerSounds\are_you_ready_for_some_football.wav')
INSERT INTO dbo.Events (EventName, Measure, SoundClipUrl) VALUES ('Shot On Target', 'Other_1', 'D:\Arbejde\randomizer\Randomizer\RandomizerSounds\calmerThanYouAre.wav')
INSERT INTO dbo.Events (EventName, Measure, SoundClipUrl) VALUES ('Corner Taken', 'Other_1', 'D:\Arbejde\randomizer\Randomizer\RandomizerSounds\fuckItDudeLetsGoBowling.wav')
INSERT INTO dbo.Events (EventName, Measure, SoundClipUrl) VALUES ('Medic!', 'Own_1', 'D:\Arbejde\randomizer\Randomizer\RandomizerSounds\theDudeAbides.wav')
INSERT INTO dbo.Events (EventName, Measure, SoundClipUrl) VALUES ('Offside', 'Own_1', 'D:\Arbejde\randomizer\Randomizer\RandomizerSounds\luckiest_man_on_the_earth.wav')
INSERT INTO dbo.Events (EventName, Measure, SoundClipUrl) VALUES ('Foul Committed', 'Own_1', 'D:\Arbejde\randomizer\Randomizer\RandomizerSounds\moron.wav')
INSERT INTO dbo.Events (EventName, Measure, SoundClipUrl) VALUES ('Shot Off Target', 'Own_1', 'D:\Arbejde\randomizer\Randomizer\RandomizerSounds\fuckItDudeLetsGoBowling.wav')
INSERT INTO dbo.Events (EventName, Measure, SoundClipUrl) VALUES ('Penalty Committed', 'Own_2', 'D:\Arbejde\randomizer\Randomizer\RandomizerSounds\theDudeAbides.wav')
INSERT INTO dbo.Events (EventName, Measure, SoundClipUrl) VALUES ('Yellow card', 'Own_2', 'D:\Arbejde\randomizer\Randomizer\RandomizerSounds\calmerThanYouAre.wav')
INSERT INTO dbo.Events (EventName, Measure, SoundClipUrl) VALUES ('Substitution', 'Own_2', 'D:\Arbejde\randomizer\Randomizer\RandomizerSounds\whoIsThisGuy.wav')
INSERT INTO dbo.Events (EventName, Measure, SoundClipUrl) VALUES ('Own Goal', 'Own_3', 'D:\Arbejde\randomizer\Randomizer\RandomizerSounds\silly_farts_wav.wav')
INSERT INTO dbo.Events (EventName, Measure, SoundClipUrl) VALUES ('Penalty Missed', 'Own_3', 'D:\Arbejde\randomizer\Randomizer\RandomizerSounds\silly_farts_wav.wav')
INSERT INTO dbo.Events (EventName, Measure, SoundClipUrl) VALUES ('Red card', 'Own_3', 'D:\Arbejde\randomizer\Randomizer\RandomizerSounds\REDCARD_turn_out_the_lights.wav')


INSERT INTO Measures (Measure, Small, Medium, Large, Walter) VALUES ('Own_1', 50, 30, 15, 5)
INSERT INTO Measures (Measure, Small, Medium, Large, Walter) VALUES ('Own_2', 30, 30, 25, 15)
INSERT INTO Measures (Measure, Small, Medium, Large, Walter) VALUES ('Own_3', 15, 25, 35, 25)
INSERT INTO Measures (Measure, Small, Medium, Large, Walter) VALUES ('Other_1', 40, 30, 20, 10)
INSERT INTO Measures (Measure, Small, Medium, Large, Walter) VALUES ('Other_2', 20, 30, 30, 20)
INSERT INTO Measures (Measure, Small, Medium, Large, Walter) VALUES ('Other_3', 15, 25, 30, 30)

INSERT INTO Matches (Description, Home, Away, Created) VALUES ('Frankrig-Rumænien', 'Frankrig', 'Rumænien', '2016-06-10')
INSERT INTO Matches (Description, Home, Away, Created) VALUES ('Albanien-Schweiz', 'Albanien', 'Schweiz', '2016-06-11')
INSERT INTO Matches (Description, Home, Away, Created) VALUES ('Wales-Slovakiet', 'Wales', 'Slovakiet', '2016-06-11')

delete graph
INSERT INTO Graph (MatchId, ParticipantId, GameMinute, MeasureZips, EventNumber) VALUES ('Frankrig-Rumænien', 1, 10, 5, 1)
INSERT INTO Graph (MatchId, ParticipantId, GameMinute, MeasureZips, EventNumber) VALUES ('Frankrig-Rumænien', 2, 15, 5, 2)
INSERT INTO Graph (MatchId, ParticipantId, GameMinute, MeasureZips, EventNumber) VALUES ('Frankrig-Rumænien', 3, 10, 12, 1)
INSERT INTO Graph (MatchId, ParticipantId, GameMinute, MeasureZips, EventNumber) VALUES ('Frankrig-Rumænien', 1, 20, 5, 3)
INSERT INTO Graph (MatchId, ParticipantId, GameMinute, MeasureZips, EventNumber) VALUES ('Frankrig-Rumænien', 1, 23, 5, 4)
INSERT INTO Graph (MatchId, ParticipantId, GameMinute, MeasureZips, EventNumber) VALUES ('Frankrig-Rumænien', 2, 28, 5, 5)
INSERT INTO Graph (MatchId, ParticipantId, GameMinute, MeasureZips, EventNumber) VALUES ('Frankrig-Rumænien', 3, 32, 12, 6)
INSERT INTO Graph (MatchId, ParticipantId, GameMinute, MeasureZips, EventNumber) VALUES ('Frankrig-Rumænien', 1, 48, 5, 7)
INSERT INTO Graph (MatchId, ParticipantId, GameMinute, MeasureZips, EventNumber) VALUES ('Frankrig-Rumænien', 1, 65, 5, 8)
INSERT INTO Graph (MatchId, ParticipantId, GameMinute, MeasureZips, EventNumber) VALUES ('Frankrig-Rumænien', 2, 68, 5, 9)
INSERT INTO Graph (MatchId, ParticipantId, GameMinute, MeasureZips, EventNumber) VALUES ('Frankrig-Rumænien', 3, 78, 12, 10)
INSERT INTO Graph (MatchId, ParticipantId, GameMinute, MeasureZips, EventNumber) VALUES ('Frankrig-Rumænien', 1, 78, 5, 10)
INSERT INTO Graph (MatchId, ParticipantId, GameMinute, MeasureZips, EventNumber) VALUES ('Frankrig-Rumænien', 4, 10, 5, 1)
INSERT INTO Graph (MatchId, ParticipantId, GameMinute, MeasureZips, EventNumber) VALUES ('Frankrig-Rumænien', 5, 15, 5, 2)
INSERT INTO Graph (MatchId, ParticipantId, GameMinute, MeasureZips, EventNumber) VALUES ('Frankrig-Rumænien', 6, 10, 12, 1)
INSERT INTO Graph (MatchId, ParticipantId, GameMinute, MeasureZips, EventNumber) VALUES ('Frankrig-Rumænien', 7, 20, 5, 3)
INSERT INTO Graph (MatchId, ParticipantId, GameMinute, MeasureZips, EventNumber) VALUES ('Frankrig-Rumænien', 6, 23, 5, 4)
INSERT INTO Graph (MatchId, ParticipantId, GameMinute, MeasureZips, EventNumber) VALUES ('Frankrig-Rumænien', 5, 28, 5, 5)
INSERT INTO Graph (MatchId, ParticipantId, GameMinute, MeasureZips, EventNumber) VALUES ('Frankrig-Rumænien', 4, 32, 12, 6)
INSERT INTO Graph (MatchId, ParticipantId, GameMinute, MeasureZips, EventNumber) VALUES ('Frankrig-Rumænien', 5, 48, 5, 7)
INSERT INTO Graph (MatchId, ParticipantId, GameMinute, MeasureZips, EventNumber) VALUES ('Frankrig-Rumænien', 2, 65, 5, 8)
INSERT INTO Graph (MatchId, ParticipantId, GameMinute, MeasureZips, EventNumber) VALUES ('Frankrig-Rumænien', 3, 68, 5, 9)
INSERT INTO Graph (MatchId, ParticipantId, GameMinute, MeasureZips, EventNumber) VALUES ('Frankrig-Rumænien', 4, 78, 12, 10)
INSERT INTO Graph (MatchId, ParticipantId, GameMinute, MeasureZips, EventNumber) VALUES ('Frankrig-Rumænien', 2, 78, 5, 10)





