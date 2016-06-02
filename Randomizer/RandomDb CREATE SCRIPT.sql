use RandomDb


CREATE TABLE Participants
(
	ParticipantId INT IDENTITY PRIMARY KEY,
	Name VARCHAR(100)
)

CREATE TABLE Teams
(
	TeamName VARCHAR(100) PRIMARY KEY,
	JerseyColor VARCHAR(10)
)

CREATE TABLE Matches
(
	Description VARCHAR(100) PRIMARY KEY,
	Home VARCHAR(100),
	Away VARCHAR(100),
	Result VARCHAR(5),
	Created DATETIME
)

CREATE TABLE Players
(
	PlayerId INT IDENTITY PRIMARY KEY,
	Name VARCHAR(100),
	Number INT,
	TeamName VARCHAR(100) REFERENCES dbo.Teams (TeamName),
	GameName VARCHAR(100) REFERENCES dbo.Matches (Description)
)

CREATE TABLE Events
(
	EventName VARCHAR(20) PRIMARY KEY,
	Measure VARCHAR(20),
	SoundClipUrl VARCHAR(300) DEFAULT ''
)
--ALTER TABLE Events ADD CONSTRAINT DefaultSoundClipUrlEmpty DEFAULT '' FOR SoundClipUrl

CREATE TABLE MatchLog
(
	LogId INT IDENTITY,
	Description VARCHAR(100) REFERENCES dbo.Matches (Description),
	PlayerId INT REFERENCES dbo.Players (PlayerId),
	ParticipantId INT REFERENCES dbo.Participants (ParticipantId),
	EventName VARCHAR(20) REFERENCES dbo.Events (EventName),
	PlayClock INT
)

CREATE TABLE Measures
(
	Measure VARCHAR(20) PRIMARY KEY,
	Small INT,
	Medium INT,
	Large INT,
	Walter INT
)


CREATE TABLE Graph (
	MatchId VARCHAR(100) FOREIGN KEY REFERENCES Matches (Description),
	ParticipantId INT FOREIGN KEY REFERENCES Participants (ParticipantId),
	GameMinute INT,
	MeasureZips INT)


--DROP TABLE Measures
--DROP TABLE MatchLog
--DROP TABLE Matches
--DROP TABLE Events
--DROP TABLE Owners
--DROP TABLE Players
--DROP TABLE Teams
--DROP TABLE Participants
--DROP TABLE Graph