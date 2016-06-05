use RandomDb


CREATE TABLE Participants
(
	ParticipantId INT IDENTITY PRIMARY KEY,
	Name VARCHAR(100),
	Red INT,
	Green INT,
	Blue INT
)

CREATE TABLE Teams
(
	TeamName VARCHAR(100) PRIMARY KEY,
	JerseyColor VARCHAR(10)
)

CREATE TABLE Matches
(
	Description VARCHAR(100) PRIMARY KEY,
	Created DATETIME DEFAULT GETDATE()
)

CREATE TABLE Players
(
	PlayerId INT IDENTITY PRIMARY KEY,
	Name VARCHAR(100),
	Number INT,
	TeamName VARCHAR(100) REFERENCES dbo.Teams (TeamName),
	GameName VARCHAR(100) REFERENCES dbo.Matches (Description),
	PlayerIndex INT
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
	MeasureZips INT,
	EventNumber INT
	)

CREATE TABLE Owners (
	MatchId VARCHAR(100),
	ParticipantId INT FOREIGN KEY REFERENCES Participants (ParticipantId),
	PlayerId INT FOREIGN KEY REFERENCES Players (PlayerId)
	)
GO

CREATE PROCEDURE [dbo].[CalculateGraph]
@MatchId VARCHAR(100)
AS
	BEGIN
	SELECT ParticipantId, GameMinute, MeasureZips, SUM(MeasureZips) OVER(PARTITION BY ParticipantId ORDER BY ParticipantId, GameMinute) AS CurrentTotal
	FROM Graph
	WHERE MatchId = @MatchId
	GROUP BY ParticipantId, GameMinute, MeasureZips
	END
GO

CREATE PROCEDURE [dbo].[UndoLatest]
AS
	BEGIN
		DECLARE @LatestEvent INT
		SELECT @LatestEvent = MAX(EventNumber) FROM Graph

		DELETE Graph WHERE EventNumber = @LatestEvent
	END
GO

CREATE PROCEDURE [dbo].[MapPlayerToOwner]
@MatchId VARCHAR(100),
@PlayerName VARCHAR(100),
@OwnerName VARCHAR(100)
AS
	BEGIN
		DECLARE @playerId INT, @ownerId INT
		SELECT @playerId = PlayerId FROM Players WHERE Name = @PlayerName
		SELECT @ownerId = ParticipantId FROM Participants WHERE Name = @OwnerName

		INSERT INTO Owners (MatchId, ParticipantId, PlayerId) VALUES (@MatchId, @ownerId, @playerId)
	END
GO

CREATE PROCEDURE [dbo].[GetOwnerFromPlayer]
@MatchId VARCHAR(100),
@PlayerName VARCHAR(100)
AS
	BEGIN		
		SELECT P.ParticipantId, P.Name
		FROM Participants P
		INNER JOIN Owners O ON P.ParticipantId = O.ParticipantId
		INNER JOIN Players PL ON PL.PlayerId = O.PlayerId AND PL.GameName = O.MatchId
		WHERE PL.Name = @PlayerName AND O.MatchId = @MatchId
	END
GO

CREATE PROCEDURE [dbo].[LogRandomizingOutcomeToGraph]
@MatchId VARCHAR(100),
@OwnerName VARCHAR(100),
@Zips INT,
@Time INT,
@EventNumber INT
AS
	BEGIN
		DECLARE @ParticipantId INT
		SELECT @ParticipantId = P.ParticipantId
		FROM Owners O INNER JOIN Participants P ON O.ParticipantId = P.ParticipantId
		WHERE O.MatchId = @MatchId AND P.Name = @OwnerName

		INSERT INTO Graph (MatchId, ParticipantId, MeasureZips, GameMinute, EventNumber)
		VALUES (@MatchId, @ParticipantId, @Zips, @Time, @EventNumber)
	END
GO

EXEC LogRandomizingOutcomeToGraph '1-2', 'Tarzan', 7, 28, 4

delete graph
delete Owners
delete Players
delete matches
select * from graph

select * from Participants

--DROP TABLE Measures
--DROP TABLE MatchLog
--DROP TABLE Graph
--DROP TABLE Owners
--DROP TABLE Players
--DROP TABLE Matches
--DROP TABLE Events


--DROP TABLE Teams
--DROP TABLE Participants

--DROP PROCEDURE CalculateGraph
--DROP PROCEDURE UndoLatest
--DROP PROCEDURE MapPlayerToOwner
--DROP PROCEDURE GetOwnerFromPlayer
--DROP PROCEDURE LogRandomizingOutcomeToGraph



select * from graph where matchid = 'e-q'

select * from participants