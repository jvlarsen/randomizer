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

CREATE TABLE Players
(
	PlayerId INT PRIMARY KEY,
	Name VARCHAR(100),
	Number INT,
	TeamName VARCHAR(100) REFERENCES dbo.Teams (TeamName)
)

CREATE TABLE Events
(
	EventName VARCHAR(20) PRIMARY KEY,
	Measure VARCHAR(20),
	SoundClipUrl VARCHAR(300) DEFAULT ''
)
--ALTER TABLE Events ADD CONSTRAINT DefaultSoundClipUrlEmpty DEFAULT '' FOR SoundClipUrl

CREATE TABLE Matches
(
	MatchId INT NOT NULL PRIMARY KEY,
	Home VARCHAR(100),
	Away VARCHAR(100),
	Result VARCHAR(5)
)

CREATE TABLE MatchLog
(
	LogId INT IDENTITY,
	MatchId INT REFERENCES dbo.Matches (MatchId),
	PlayerId INT REFERENCES dbo.Players (PlayerId),
	ParticipantId INT REFERENCES dbo.Participants (ParticipantId),
	EventName VARCHAR(20) REFERENCES dbo.Events (EventName)
)

CREATE TABLE Distribution
(
	Measure VARCHAR(20),
	Small INT,
	Medium INT,
	Large INT,
	Walter INT
)

