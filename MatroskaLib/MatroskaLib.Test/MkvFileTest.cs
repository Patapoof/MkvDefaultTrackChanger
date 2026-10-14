using AwesomeAssertions;
using MatroskaLib.Types;
using Xunit;

namespace MatroskaLib.Test;

public class MkvFileTest
{
    [Fact]
    public void TestToString()
    {
        var mkvFile = new MkvFile
        {
            filePath = "/home/some-path",
            tracks = [new Track(null!) { type = TrackTypeEnum.subtitle, flagForced = true }],
            endPosition = 20,
            beginHeaderPosition = 30,
            seekHeadCheckSum = null,
            seekList = [],
            tracksCheckSum = 50,
            tracksPosition = 30,
            voidPosition = 40
        };

        string json = mkvFile.ToString();

        json.Should().Be("""
                         {
                           "filePath": "",
                           "tracks": [
                             {
                               "trackLengthByteNumber": 0,
                               "number": 0,
                               "flagDefault": false,
                               "flagDefaultByteNumber": 0,
                               "flagForced": true,
                               "flagForcedByteNumber": 0,
                               "flagTypebytenumber": 0,
                               "type": "subtitle",
                               "name": "",
                               "language": "eng"
                             }
                           ],
                           "seekList": [],
                           "seekHeadCheckSum": null,
                           "tracksCheckSum": 50,
                           "voidPosition": 40,
                           "endPosition": 20,
                           "tracksPosition": 30,
                           "beginHeaderPosition": 30
                         }
                         """);
    }

    [Fact]
    public void CompareToGetErrorShouldBeSymmetricWhenTrackCountDiffers()
    {
        var small = CreateFile(new Track(null!) { number = 1, type = TrackTypeEnum.video, language = "und" },
                               new Track(null!) { number = 2, type = TrackTypeEnum.audio, language = "eng" });
        var big = CreateFile(new Track(null!) { number = 1, type = TrackTypeEnum.video, language = "und" },
                             new Track(null!) { number = 2, type = TrackTypeEnum.audio, language = "eng" },
                             new Track(null!) { number = 3, type = TrackTypeEnum.subtitle, language = "fre" });

        small.CompareToGetError(big).Should().NotBeNull();
        big.CompareToGetError(small).Should().NotBeNull();
    }

    private static MkvFile CreateFile(params Track[] tracks) => new()
    {
        filePath = "file.mkv",
        tracks = [..tracks],
        endPosition = 0,
        beginHeaderPosition = 0,
        seekHeadCheckSum = null,
        seekList = [],
        tracksCheckSum = null,
        tracksPosition = 0,
        voidPosition = 0
    };
}
