using MobileBff.Utilities;

namespace Tests.Utilities
{
    public class JwtParserTests
    {
        [Fact]
        public void GetUserId_WhenJwtContainsUserId_ShouldReturnUserId()
        {
            var jwt = "eyJjdHkiOiJsb2dpbi12MSIsInR5cCI6IkpXVCIsImtpZCI6ImZtc1dvbDZSOHhvSGR2cHhyVTFhZGl6VGhYQ1VjRlZnY3FxLWE1LWtMNHMiLCJhbGciOiJSUzI1NiJ9.eyJzdWIiOiIiLCJhbXIiOlsiRElHSVBBU1NfQVBQTDIiXSwic2lkIjoiMGY0NDdkZDAtOWRlYi00OWRhLWIyNzktMWYyMWMyNjQ4NTZmIiwiYXV0aF90aW1lIjoxNjYxODQ2NzcxLCJhdXRoX2NoYW5uZWwiOiJ0cHAtYXV0aC1kZWNvdXBsZWQuc3RnLnNlYmFuay5zZSIsImdyb3VwcyI6WyJFQkJBX0lLRl8xNSIsIkVCQkFfSUtQXzA1Il0sIm5hbWUiOiJJU0FCRUwgSEpPUlQiLCJnaXZlbl9uYW1lIjoiSVNBQkVMIiwibWlkZGxlX25hbWUiOiJJU0FCRUwiLCJmYW1pbHlfbmFtZSI6IkhKT1JUIiwiYW1yX2lzc3VlciI6IlNrYW5kaW5hdmlza2EgRW5za2lsZGEgQmFua2VuIEFCIChwdWJsKSIsImFtcl9zZXJpYWxfbnVtYmVyIjoiMDAwMDAwMDAwMTAwMDgxOTE3MyIsImF1ZCI6WyJTRUIiLCI3MTE1NDQzNi1hZTVkLWY0NWUtOWU0Yi00NTNjNTk2ZjY2MzgiXSwicHJlZmVycmVkX3VzZXJuYW1lIjoiMTk3MTA3MjQ3NDQyIiwiaXNzIjoiaHR0cHM6Ly9pc2FtIiwiZXhwIjoxNjYxODQ3MzcxLCJqdGkiOiJ0RmR6RXVHMW5wRjNXTTlzYkhiWDJtVHJ1WnFSZmNwS1Q5VzY1RnYyIiwiaWF0IjoxNjYxODQ2NzcxLCJuYmYiOjE2NjE4NDY3NzF9.R1dj0cGg_sNDVwh8mJqR3XuXlA_h3HdcdHAnydEPlffJNd0ZI5XtD8WR_EOUaw_L-Ke992TUi1zF1qv3emLnCK2n9FghrUeldBgrWvTUTH9aL-sQlNAWHCfrgm9SDJvUcqp1JM2Mx6s7W0MfzefKUHs6kyAyDJRUETYM8DyTp6npvhnqme2FegdJPjsxQTDmmdZf0uom2nrDKMskDFycB6kmFrEyqe5Ml_P34pCTBIwo5fpA5mmDV8Bq0LXERAA4rzAKprslkn98sZGpt2WoKGRP58N_QH6VOhAWd4MLq2STmxG75dRx2cX2vnp_Z32qisNR3deS-bO0a5dPOK4hOw";

            var jwtParser = new JwtParser();
            var userId = jwtParser.GetUserId(jwt);

            Assert.Equal("197107247442", userId);
        }

        [Fact]
        public void GetUserId_WhenJwtIsNull_ShouldThrowException()
        {
            var jwtParser = new JwtParser();
            Assert.Throws<Exception>(() => jwtParser.GetUserId(null));
        }

        [Fact]
        public void GetUserId_WhenJwtIsNotCorrect_ShouldThrowException()
        {
            var jwtParser = new JwtParser();
            Assert.Throws<Exception>(() => jwtParser.GetUserId("Test jwt"));
        }
    }
}
