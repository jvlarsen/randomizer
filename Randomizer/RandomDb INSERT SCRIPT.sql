use RandomDb


INSERT INTO dbo.Events (EventName, Measure, SoundClipUrl, RefereeSoundClip) VALUES ('MÅL!!! / Korrekt annulleret mål', 'Other_3', 'D:\Arbejde\randomizer\Randomizer\RandomizerSounds\goalshout.wav', 'D:\Arbejde\randomizer\Randomizer\RandomizerSounds\goalcancel.wav')
INSERT INTO dbo.Events (EventName, Measure, SoundClipUrl, RefereeSoundClip) VALUES ('Assist / Skiller folk / Reprimande uden kort', 'Other_2', 'D:\Arbejde\randomizer\Randomizer\RandomizerSounds\are_you_ready_for_some_football.wav', 'D:\Arbejde\randomizer\Randomizer\RandomizerSounds\moron.wav')
INSERT INTO dbo.Events (EventName, Measure, SoundClipUrl, RefereeSoundClip) VALUES ('Skud på mål / 1H 3+ / Dikterer placering', 'Other_1', 'D:\Arbejde\randomizer\Randomizer\RandomizerSounds\calmerThanYouAre.wav', 'D:\Arbejde\randomizer\Randomizer\RandomizerSounds\moveball.wav')
INSERT INTO dbo.Events (EventName, Measure, SoundClipUrl, RefereeSoundClip) VALUES ('Hjørne taget / 1H 0-1min / 2H 0-2min / 2H 5+', 'Other_1', 'D:\Arbejde\randomizer\Randomizer\RandomizerSounds\cornertaken.wav', 'D:\Arbejde\randomizer\Randomizer\RandomizerSounds\moretimeadded.wav')
INSERT INTO dbo.Events (EventName, Measure, SoundClipUrl, RefereeSoundClip) VALUES ('Medic!', 'Own_1', 'D:\Arbejde\randomizer\Randomizer\RandomizerSounds\refinjury.wav', 'D:\Arbejde\randomizer\Randomizer\RandomizerSounds\refinjury.wav')
INSERT INTO dbo.Events (EventName, Measure, SoundClipUrl, RefereeSoundClip) VALUES ('Offside / 1H 2min', 'Own_1', 'D:\Arbejde\randomizer\Randomizer\RandomizerSounds\The Big Lebowski - OVER THE LINE.wav', 'D:\Arbejde\randomizer\Randomizer\RandomizerSounds\normaltimeadded.wav')
INSERT INTO dbo.Events (EventName, Measure, SoundClipUrl, RefereeSoundClip) VALUES ('Frispark begået', 'Own_1', 'D:\Arbejde\randomizer\Randomizer\RandomizerSounds\moron.wav', 'D:\Arbejde\randomizer\Randomizer\RandomizerSounds\moron.wav')
INSERT INTO dbo.Events (EventName, Measure, SoundClipUrl, RefereeSoundClip) VALUES ('Skud udenfor rammen', 'Own_1', 'D:\Arbejde\randomizer\Randomizer\RandomizerSounds\holy_cow_x.wav', 'D:\Arbejde\randomizer\Randomizer\RandomizerSounds\holy_cow_x.wav')
INSERT INTO dbo.Events (EventName, Measure, SoundClipUrl, RefereeSoundClip) VALUES ('Begået straffe / Konfererer med meddommere', 'Own_2', 'D:\Arbejde\randomizer\Randomizer\RandomizerSounds\committedpenalty.wav', 'D:\Arbejde\randomizer\Randomizer\RandomizerSounds\1000dollars.wav')
INSERT INTO dbo.Events (EventName, Measure, SoundClipUrl, RefereeSoundClip) VALUES ('Gult kort / 2H 3-4', 'Own_2', 'D:\Arbejde\randomizer\Randomizer\RandomizerSounds\calmerThanYouAre.wav', 'D:\Arbejde\randomizer\Randomizer\RandomizerSounds\normaltimeadded.wav')
INSERT INTO dbo.Events (EventName, Measure, SoundClipUrl, RefereeSoundClip) VALUES ('Udskiftning / Falder', 'Own_2', 'D:\Arbejde\randomizer\Randomizer\RandomizerSounds\whoIsThisGuy.wav', 'D:\Arbejde\randomizer\Randomizer\RandomizerSounds\theresABeverageHere.wav')
INSERT INTO dbo.Events (EventName, Measure, SoundClipUrl, RefereeSoundClip) VALUES ('Selvmål / Rammes af bold', 'Own_3', 'D:\Arbejde\randomizer\Randomizer\RandomizerSounds\silly_farts_wav.wav', 'D:\Arbejde\randomizer\Randomizer\RandomizerSounds\refhitbyball.wav')
INSERT INTO dbo.Events (EventName, Measure, SoundClipUrl, RefereeSoundClip) VALUES ('Brændt straffe', 'Own_3', 'D:\Arbejde\randomizer\Randomizer\RandomizerSounds\silly_farts_wav.wav', 'D:\Arbejde\randomizer\Randomizer\RandomizerSounds\silly_farts_wav.wav')
INSERT INTO dbo.Events (EventName, Measure, SoundClipUrl, RefereeSoundClip) VALUES ('Rødt kort / Skadet', 'Own_3', 'D:\Arbejde\randomizer\Randomizer\RandomizerSounds\REDCARD_turn_out_the_lights.wav', 'D:\Arbejde\randomizer\Randomizer\RandomizerSounds\refinjury.wav')


INSERT INTO Measures (Measure, Small, Medium, Large, Walter) VALUES ('Own_1', 50, 30, 15, 5)
INSERT INTO Measures (Measure, Small, Medium, Large, Walter) VALUES ('Own_2', 30, 30, 25, 15)
INSERT INTO Measures (Measure, Small, Medium, Large, Walter) VALUES ('Own_3', 15, 25, 35, 25)
INSERT INTO Measures (Measure, Small, Medium, Large, Walter) VALUES ('Other_1', 40, 30, 20, 10)
INSERT INTO Measures (Measure, Small, Medium, Large, Walter) VALUES ('Other_2', 20, 30, 30, 20)
INSERT INTO Measures (Measure, Small, Medium, Large, Walter) VALUES ('Other_3', 15, 25, 30, 30)


select * from measures
select * from events

