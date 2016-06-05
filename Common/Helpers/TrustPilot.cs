
using System.Collections.Generic;

namespace Common.Helpers.TrustPilot
{
    public class FeedModel
    {
        public Time FeedUpdateTime { get; set; }
        public string DomainName { get; set; }
        public string ReviewPageUrl { get; set; }
        public TrustScore TrustScore { get; set; }
        public List<Category> Categories { get; set; }
        public ReviewCount ReviewCount { get; set; }
        public List<Review> Reviews { get; set; }
    }

    public class Review
    {
        public Time Created { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public TrustScore TrustScore { get; set; }
        public string CompanyReply { get; set; }
        public User User { get; set; }
    }

    public class User
    {
        public string Name { get; set; }
        public string City { get; set; }
        public string Locale { get; set; }
        public int ReviewCount { get; set; }
        public bool IsVerified { get; set; }
        public bool HasImage { get; set; }
        public Dictionary<string, string> ImageUrls { get; set; }
    }

    public class ReviewCount
    {
        public int Total { get; set; }
        public int[] DistributionOverStars { get; set; }
    }

    public class TrustScore
    {
        public double Score { get; set; }
        public int Stars { get; set; }
        public string Human { get; set; }
        public Dictionary<string, string> StarsImageUrls { get; set; }
    }

    public class Category
    {
        public string Name { get; set; }
        public int Position { get; set; }
        public int Count { get; set; }
        public Dictionary<string, string> ImageUrls { get; set; }
    }

    public class Time
    {
        public long UnixTime { get; set; }
        public string Human { get; set; }
        public string HumanDate { get; set; }
    }
}