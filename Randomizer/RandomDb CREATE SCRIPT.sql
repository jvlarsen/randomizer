use RandomDb

CREATE TABLE Matches
(
	MatchId INT IDENTITY(1,1) PRIMARY KEY,
	TeamNames VARCHAR(100),
	Created DATETIME DEFAULT GETDATE()
)

CREATE TABLE Participants
(
	ParticipantId INT IDENTITY PRIMARY KEY,
	Name VARCHAR(100),
	Color INT,
	MatchId INT REFERENCES Matches (MatchId)
)

CREATE TABLE Players
(
	PlayerId INT IDENTITY PRIMARY KEY,
	Name VARCHAR(100),
	Number INT,
	MatchId INT REFERENCES dbo.Matches (MatchId),
	PlayerIndex INT,
	RadioButton VARCHAR(50)
)

CREATE TABLE Events
(
	EventName VARCHAR(50) PRIMARY KEY,
	Measure VARCHAR(20),
	SoundClipUrl VARCHAR(300) DEFAULT '',
	RefereeSoundClip VARCHAR(300) DEFAULT ''
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
	MatchId INT FOREIGN KEY REFERENCES Matches (MatchId),
	ParticipantId INT FOREIGN KEY REFERENCES Participants (ParticipantId),
	GameMinute INT,
	MeasureZips INT,
	EventNumber INT,
	EventText VARCHAR(20)
)

CREATE TABLE Owners (
	MatchId INT REFERENCES Matches (MatchId),
	ParticipantId INT FOREIGN KEY REFERENCES Participants (ParticipantId),
	PlayerId INT FOREIGN KEY REFERENCES Players (PlayerId)
	)
GO

CREATE PROCEDURE [dbo].[CalculateGraph]
@MatchId INT
AS
	BEGIN
	SELECT ParticipantId, GameMinute, MeasureZips, EventText, SUM(MeasureZips) OVER(PARTITION BY ParticipantId ORDER BY ParticipantId, GameMinute) AS CurrentTotal
	FROM Graph
	WHERE MatchId = @MatchId
	AND ParticipantId IS NOT NULL
	GROUP BY ParticipantId, GameMinute, MeasureZips, EventText
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
@MatchId INT,
@PlayerName VARCHAR(100),
@OwnerName VARCHAR(100)
AS
	BEGIN
		DECLARE @playerId INT, @ownerId INT
		SELECT @playerId = PlayerId FROM Players WHERE Name = @PlayerName AND MatchId = @MatchId
		SELECT @ownerId = ParticipantId FROM Participants WHERE Name = @OwnerName AND MatchId = @MatchId

		INSERT INTO Owners (MatchId, ParticipantId, PlayerId) VALUES (@MatchId, @ownerId, @playerId)
	END
GO

CREATE PROCEDURE [dbo].[GetOwnerFromPlayerAndGame]
@MatchId INT,
@PlayerName VARCHAR(100)
AS
	BEGIN		
		SELECT P.ParticipantId, P.Name
		FROM Participants P
		INNER JOIN Owners O ON P.ParticipantId = O.ParticipantId
		INNER JOIN Players PL ON PL.PlayerId = O.PlayerId AND PL.MatchId = O.MatchId
		WHERE PL.Name = @PlayerName AND O.MatchId = @MatchId
	END
GO

CREATE PROCEDURE [dbo].[LogRandomizingOutcomeToGraph]
@MatchId INT,
@OwnerName VARCHAR(100),
@Zips INT,
@Time INT,
@EventNumber INT,
@EventText VARCHAR(20)
AS
	BEGIN
		DECLARE @ParticipantId INT
		SELECT @ParticipantId = P.ParticipantId
		FROM Owners O INNER JOIN Participants P ON O.ParticipantId = P.ParticipantId
		WHERE O.MatchId = @MatchId AND P.Name = @OwnerName

		INSERT INTO Graph (MatchId, ParticipantId, MeasureZips, GameMinute, EventNumber, EventText)
		VALUES (@MatchId, @ParticipantId, @Zips, @Time, @EventNumber, @EventText)
	END
GO

CREATE PROCEDURE [dbo].[GetPlayersAndOwners]
@MatchId INT
AS
	BEGIN
		SELECT PL.Name AS PlayerName, PL.PlayerIndex, PA.Name AS OwnerName, PL.RadioButton
		FROM Players PL INNER JOIN Owners O ON PL.PlayerId = O.PlayerId
		INNER JOIN Participants PA ON O.ParticipantId = PA.ParticipantId
		INNER JOIN Matches M ON O.MatchId = M.MatchId
		WHERE M.MatchId = @MatchId
	END
GO

CREATE PROCEDURE [dbo].[GetOldGames]
AS
	BEGIN
		SELECT M.MatchId, M.TeamNames, M.Created, ISNULL(MAX(EventNumber),0) AS LatestEvent, ISNULL(MAX(GameMinute),0) AS ProgressBarValue
		FROM Matches M LEFT JOIN Graph G ON M.MatchId = G.MatchId
		GROUP BY M.MatchId, M.TeamNames, M.Created
	END
GO

CREATE PROCEDURE [dbo].[SaveParticipants]
@MatchId INT,
@Name VARCHAR(100),
@Color INT
AS
	BEGIN
		INSERT INTO Participants (Name, Color, MatchId)
		VALUES (@Name, @Color, @MatchId)
	END
GO

CREATE PROCEDURE GetParticipantsFromMatchId
@MatchId INT
AS
	BEGIN
		SELECT Name, Color FROM Participants WHERE MatchId = @MatchId
	END
GO

--DROP PROCEDURE UpdatePlayerName
CREATE PROCEDURE [dbo].[UpdatePlayerName]
@NewPlayerName VARCHAR(100),
@RadioButton VARCHAR(50),
@MatchId INT
AS
	BEGIN
		UPDATE Players SET Name = @NewPlayerName WHERE RadioButton = @RadioButton AND MatchId = @MatchId
	END
GO

CREATE PROCEDURE RegisterNothing
@MatchId INT,
@GameMinute INT
AS
	BEGIN
		DECLARE @CurrentParticipant INT
		DECLARE @EventNumber INT
		SELECT @EventNumber = ISNULL(MAX(EventNumber),0) + 1 FROM Graph WHERE MatchId = @MatchId

		DECLARE participantCursor CURSOR FOR
			SELECT DISTINCT(ParticipantId) FROM Graph WHERE MatchId = @MatchId

		OPEN participantCursor
		FETCH NEXT FROM participantCursor INTO @CurrentParticipant

		WHILE @@FETCH_STATUS = 0
		BEGIN
			INSERT INTO Graph (MatchId, ParticipantId, gameMinute, MeasureZips, EventNumber)
			VALUES (@MatchId, @CurrentParticipant, @GameMinute, 0, @EventNumber)
			FETCH NEXT FROM ParticipantCursor INTO @CurrentParticipant
		END
		CLOSE participantCursor
		DEALLOCATE participantCursor
	END
GO

SELECT MatchId, Participant

--DROP TABLE Measures
--DROP TABLE Graph
--DROP TABLE Owners
--DROP TABLE Players
--DROP TABLE Events
--DROP TABLE Participants
--DROP TABLE Matches

--DROP PROCEDURE CalculateGraph
--DROP PROCEDURE UndoLatest
--DROP PROCEDURE MapPlayerToOwner
--DROP PROCEDURE GetOwnerFromPlayerAndGame
--DROP PROCEDURE LogRandomizingOutcomeToGraph
--DROP PROCEDURE GetPlayersAndOwners
--DROP PROCEDURE GetOldGames
--DROP PROCEDURE SaveParticipants
--DROP PROCEDURE RegisterNothing


exec CalculateGraph 1

select * from Participants

select * from owners
select * from Events

select * from graph where matchid = 15

exec GetPlayersAndOwners 15


select * from measures

select * from players where matchid = 31
