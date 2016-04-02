use RandomDb

INSERT INTO Participants (ParticipantId, Name) VALUES (1, 'Tarzan')
INSERT INTO Participants (ParticipantId, Name) VALUES (2, 'Faccio')
INSERT INTO Participants (ParticipantId, Name) VALUES (3, 'Trusser')
INSERT INTO Participants (ParticipantId, Name) VALUES (4, 'Nosser')
INSERT INTO Participants (ParticipantId, Name) VALUES (5, 'Leffo')
INSERT INTO Participants (ParticipantId, Name) VALUES (6, 'Tennedz')
INSERT INTO Participants (ParticipantId, Name) VALUES (7, 'Aallex')

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


INSERT INTO Distribution (Measure, Small, Medium, Large, Walter) VALUES ('Own_1', 50, 30, 15, 5)
INSERT INTO Distribution (Measure, Small, Medium, Large, Walter) VALUES ('Own_2', 30, 30, 25, 15)
INSERT INTO Distribution (Measure, Small, Medium, Large, Walter) VALUES ('Own_3', 15, 25, 35, 25)
INSERT INTO Distribution (Measure, Small, Medium, Large, Walter) VALUES ('Other_1', 40, 30, 20, 10)
INSERT INTO Distribution (Measure, Small, Medium, Large, Walter) VALUES ('Other_2', 20, 30, 30, 20)
INSERT INTO Distribution (Measure, Small, Medium, Large, Walter) VALUES ('Other_3', 15, 25, 30, 30)

UPDATE Events SET SoundClipUrl = 'D:\Arbejde\randomizer\Randomizer\RandomizerSounds\goalCelebration.wav' where EventName LIKE 'GOAL%'





