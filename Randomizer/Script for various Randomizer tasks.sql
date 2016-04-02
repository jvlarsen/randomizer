select * from players
select * from Participants
select * from Matches
select * from Owners

--Saving playersAndOwners from Dictionary<string, string> and string gameName requires:
INSERT INTO Matches (Home, Away, Description, Created) VALUES (homeTeam, awayTeam, gameNameLabel.Text, DATETIME.Now()) --Start here to get the matchId
INSERT INTO Players (Name) VALUES (playersAndOwners.Keys[i]) --iterate i as key
INSERT INTO Participants (Name) VALUES (playersAndOwner[i]) --while iterating i, fetch the value associated with key i
INSERT INTO Owners (MatchId, PlayerId, ParticipantId) VALUES (returnedMatchId, playersAndOwners.Keys[i], playersAndOwners[i])



