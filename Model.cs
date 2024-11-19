using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Net;

public class BloggingContext : DbContext
{
    public DbSet<Blog> Blogs { get; set; }
    public DbSet<Post> Posts { get; set; }

    public DbSet<Author> Authors { get; set; }

    public DbSet<Customer> CustomerEntries { get; set; }

    public string DbPath { get; }

    public BloggingContext()
    {
    
    }

    // The following configures EF to create a Sqlite database file in the
    // special "local" folder for your platform.
    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        options.UseNpgsql("Server=127.0.0.1;Port=5432;Database=blog;User Id=postgres;Password=PFG2cpm~sJ.WC0rGyqFKf0;");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<FeaturedPost>();

        modelBuilder.Entity<Website>()
            .HasOne(e => e.Blog)
            .WithOne(e => e.Site)
            .HasPrincipalKey<Website>(e => e.Uri)
            .HasForeignKey<Blog>(e => e.SiteUri);

    

        modelBuilder.Entity<Author>().OwnsOne(
            author => author.Contact, ownedNavigationBuilder =>
            {
                ownedNavigationBuilder.ToJson();
                ownedNavigationBuilder.OwnsOne(contactDetails => contactDetails.Address);
            });

        #region PostMetadataConfig
        modelBuilder.Entity<Post>().OwnsOne(
            post => post.Metadata, ownedNavigationBuilder =>
            {
                ownedNavigationBuilder.ToJson();
                ownedNavigationBuilder.OwnsMany(metadata => metadata.TopSearches);
                ownedNavigationBuilder.OwnsMany(metadata => metadata.TopGeographies);
                ownedNavigationBuilder.OwnsMany(
                    metadata => metadata.Updates,
                    ownedOwnedNavigationBuilder => ownedOwnedNavigationBuilder.OwnsMany(update => update.Commits));
            });
        #endregion
        modelBuilder.Entity<Customer>()
            .OwnsOne(c => c.Details, d =>
            {
                d.ToJson();
                d.OwnsMany(d => d.Orders);
            });
    }
    public async Task Seed()
    {
        var tagEntityFramework = new Tag("TagEF", "Entity Framework");
        var tagDotNet = new Tag("TagNet", ".NET");
        var tagDotNetMaui = new Tag("TagMaui", ".NET MAUI");
        var tagAspDotNet = new Tag("TagAsp", "ASP.NET");
        var tagAspDotNetCore = new Tag("TagAspC", "ASP.NET Core");
        var tagDotNetCore = new Tag("TagC", ".NET Core");
        var tagHacking = new Tag("TagHx", "Hacking");
        var tagLinux = new Tag("TagLin", "Linux");
        var tagSqlite = new Tag("TagLite", "SQLite");
        var tagVisualStudio = new Tag("TagVS", "Visual Studio");
        var tagGraphQl = new Tag("TagQL", "GraphQL");
        var tagCosmosDb = new Tag("TagCos", "CosmosDB");
        var tagBlazor = new Tag("TagBl", "Blazor");

        var maddy = new Author("Maddy Montaquila")
        {
            Contact = new() { Address = new("1 Main St", "Camberwick Green", "CW1 5ZH", "UK"), Phone = "01632 12345" }
        };
        var jeremy = new Author("Jeremy Likness")
        {
            Contact = new() { Address = new("2 Main St", "Chigley", "CW1 5ZH", "UK"), Phone = "01632 12346" }
        };
        var dan = new Author("Daniel Roth")
        {
            Contact = new() { Address = new("3 Main St", "Camberwick Green", "CW1 5ZH", "UK"), Phone = "01632 12347" }
        };
        var arthur = new Author("Arthur Vickers")
        {
            Contact = new() { Address = new("15a Main St", "Chigley", "CW1 5ZH", "UK"), Phone = "01632 12348" }
        };
        var brice = new Author("Brice Lambson")
        {
            Contact = new() { Address = new("4 Main St", "Chigley", "CW1 5ZH", "UK"), Phone = "01632 12349" }
        };

        var blogs = new List<Blog>
        {
            new(".NET Blog")
            {
                Posts =
                {
                    new Post(
                        "Productivity comes to .NET MAUI in Visual Studio 2022",
                        "Visual Studio 2022 17.3 is now available and...",
                        new DateOnly(2022, 8, 9)) { Tags = { tagDotNetMaui, tagDotNet }, Author = maddy, Metadata = BuildPostMetadata() },
                    new Post(
                        "Announcing .NET 7 Preview 7", ".NET 7 Preview 7 is now available with improvements to System.LINQ, Unix...",
                        new DateOnly(2022, 8, 9)) { Tags = { tagDotNet }, Author = jeremy, Metadata = BuildPostMetadata() },
                    new Post(
                        "ASP.NET Core updates in .NET 7 Preview 7", ".NET 7 Preview 7 is now available! Check out what's new in...",
                        new DateOnly(2022, 8, 9))
                    {
                        Tags = { tagDotNet, tagAspDotNet, tagAspDotNetCore }, Author = dan, Metadata = BuildPostMetadata()
                    },
                    new FeaturedPost(
                        "Announcing Entity Framework 7 Preview 7: Interceptors!",
                        "Announcing EF7 Preview 7 with new and improved interceptors, and...",
                        new DateOnly(2022, 8, 9),
                        "Loads of runnable code!")
                    {
                        Tags = { tagEntityFramework, tagDotNet, tagDotNetCore }, Author = arthur, Metadata = BuildPostMetadata()
                    }
                },
                Site = new(new("https://devblogs.microsoft.com/dotnet/"), "dotnet@example.com")
            },
            new("1unicorn2")
            {
                Posts =
                {
                    new Post(
                        "Hacking my Sixth Form College network in 1991",
                        "Back in 1991 I was a student at Franklin Sixth Form College...",
                        new DateOnly(2020, 4, 10)) { Tags = { tagHacking }, Author = arthur, Metadata = BuildPostMetadata() },
                    new FeaturedPost(
                        "All your versions are belong to us",
                        "Totally made up conversations about choosing Entity Framework version numbers...",
                        new DateOnly(2020, 3, 26),
                        "Way funny!") { Tags = { tagEntityFramework }, Author = arthur, Metadata = BuildPostMetadata() },
                    new Post(
                        "Moving to Linux", "A few weeks ago, I decided to move from Windows to Linux as...",
                        new DateOnly(2020, 3, 7)) { Tags = { tagLinux }, Author = arthur, Metadata = BuildPostMetadata(), Archived = true },
                    new Post(
                        "Welcome to One Unicorn 2.0!", "I created my first blog back in 2011..",
                        new DateOnly(2020, 2, 29)) { Tags = { tagEntityFramework }, Author = arthur, Metadata = BuildPostMetadata() }
                },
                Site = new(new("https://blog.oneunicorn.com/"), "unicorn@example.com")
            },
            new("Brice's Blog")
            {
                Posts =
                {
                    new FeaturedPost(
                        "SQLite in Visual Studio 2022", "A couple of years ago, I was thinking of ways...",
                        new DateOnly(2022, 7, 26), "Love for VS!")
                    {
                        Tags = { tagSqlite, tagVisualStudio }, Author = brice, Metadata = BuildPostMetadata()
                    },
                    new Post(
                        "On .NET - Entity Framework Migrations Explained",
                        "This week, @JamesMontemagno invited me onto the On .NET show...",
                        new DateOnly(2022, 5, 4))
                    {
                        Tags = { tagEntityFramework, tagDotNet }, Author = brice, Metadata = BuildPostMetadata()
                    },
                    new Post(
                        "Dear DBA: A silly idea", "We have fun on the Entity Framework team...",
                        new DateOnly(2022, 3, 31)) { Tags = { tagEntityFramework }, Author = brice, Metadata = BuildPostMetadata(), Archived = true },
                    new Post(
                        "Microsoft.Data.Sqlite 6", "It�s that time of year again. Microsoft.Data.Sqlite version...",
                        new DateOnly(2021, 11, 8)) { Tags = { tagSqlite, tagDotNet }, Author = brice, Metadata = BuildPostMetadata() }
                },
                Site = new(new("https://www.bricelam.net/"), "brice@example.com")
            },
            new("Developer for Life")
            {
                Posts =
                {
                    new Post(
                        "GraphQL for .NET Developers", "A comprehensive overview of GraphQL as...",
                        new DateOnly(2021, 7, 1))
                    {
                        Tags = { tagDotNet, tagGraphQl, tagAspDotNetCore }, Author = jeremy, Metadata = BuildPostMetadata()
                    },
                    new FeaturedPost(
                        "Azure Cosmos DB With EF Core on Blazor Server",
                        "Learn how to build Azure Cosmos DB apps using Entity Framework Core...",
                        new DateOnly(2021, 5, 16),
                        "Blazor FTW!")
                    {
                        Tags =
                        {
                            tagDotNet,
                            tagEntityFramework,
                            tagAspDotNetCore,
                            tagCosmosDb,
                            tagBlazor
                        },
                        Author = jeremy,
                        Metadata = BuildPostMetadata()
                    },
                    new Post(
                        "Multi-tenancy with EF Core in Blazor Server Apps",
                        "Learn several ways to implement multi-tenant databases in Blazor Server apps...",
                        new DateOnly(2021, 4, 29))
                    {
                        Tags = { tagDotNet, tagEntityFramework, tagAspDotNetCore, tagBlazor },
                        Author = jeremy,
                        Metadata = BuildPostMetadata()
                    },
                    new Post(
                        "An Easier Blazor Debounce", "Where I propose a simple method to debounce input without...",
                        new DateOnly(2021, 4, 12))
                    {
                        Tags = { tagDotNet, tagAspDotNetCore, tagBlazor }, Author = jeremy, Metadata = BuildPostMetadata()
                    }
                },
                Site = new(new("https://blog.jeremylikness.com/"), "jeremy@example.com")
            }
        };

        await AddRangeAsync(blogs);
        await SaveChangesAsync();

        PostMetadata BuildPostMetadata()
        {
            var random = new Random(Guid.NewGuid().GetHashCode());

            var metadata = new PostMetadata(random.Next(10000));

            for (var i = 0; i < random.Next(5); i++)
            {
                var update = new PostUpdate(IPAddress.Loopback, 
                    DateTime.UtcNow - TimeSpan.FromDays(random.Next(1, 10000)))
                {
                    UpdatedBy = "Admin"
                };

                for (var j = 0; j < random.Next(3); j++)
                {
                    update.Commits.Add(new(DateTime.Today, $"Commit #{j + 1}"));
                }

                metadata.Updates.Add(update);
            }

            for (var i = 0; i < random.Next(5); i++)
            {
                metadata.TopSearches.Add(new($"Search #{i + 1}", 10000 - random.Next(i * 1000, i * 1000 + 900)));
            }

            for (var i = 0; i < random.Next(5); i++)
            {
                metadata.TopGeographies.Add(
                    new(
                        // Issue https://github.com/dotnet/efcore/issues/28811 (Support spatial types in JSON columns)
                        // new Point(115.7930 + 20 - random.Next(40), 37.2431 + 10 - random.Next(20)) { SRID = 4326 },
                        115.7930 + 20 - random.Next(40),
                        37.2431 + 10 - random.Next(20),
                        1000 - random.Next(i * 100, i * 100 + 90))
                    { Browsers = new() { "Firefox", "Netscape" } });
            }

            return metadata;
        }
    }
}
public class ContactDetails
{
    public Address Address { get; set; } = null!;
    public string? Phone { get; set; }
}

public class Address
{
    public Address(string street, string city, string postcode, string country)
    {
        Street = street;
        City = city;
        Postcode = postcode;
        Country = country;
    }

    public string Street { get; set; }
    public string City { get; set; }
    public string Postcode { get; set; }
    public string Country { get; set; }
}
public class Tag
{
    public Tag(string id, string text)
    {
        Id = id;
        Text = text;
    }

    public string Id { get; private set; }
    public string Text { get; set; }
    public virtual List<Post> Posts { get; } = new();
}
public class Customer
{
    public int Id { get; set; }
    public CustomerDetails Details { get; set; }
}

public class CustomerDetails // Map to a JSON column in the table
{
    public string Name { get; set; }
    public int Age { get; set; }
    public List<Order> Orders { get; set; }
}

public class Order // Part of the JSON column
{
    public decimal Price { get; set; }
    public string ShippingAddress { get; set; }
}
public class PostTag
{
    public int PostId { get; private set; }
    public string TagId { get; private set; } = null!;
}
public class Author
{
    public Author(string name)
    {
        Name = name;
    }

    public int Id { get; private set; }
    public string Name { get; set; }

    public ContactDetails Contact { get; set; } = null!;
    public List<Post> Posts { get; } = new();
}
public class Blog
{
    public Blog(string name)
    {
        Name = name;
    }

    public int Id { get; private set; }
    public string Name { get; set; }
    public virtual Uri SiteUri { get; set; } = null!;
    public virtual Website Site { get; set; } = null!;
    public virtual List<Post> Posts { get; } = new();
}
public class FeaturedPost : Post
{
    public FeaturedPost(string title, string content, DateOnly publishedOn, string promoText)
        : base(title, content, publishedOn)
    {
        PromoText = promoText;
    }

    public string PromoText { get; set; }
}
public class Website
{
    public Website(Uri uri, string email)
    {
        Uri = uri;
        Email = email;
    }

    public Guid Id { get; private set; }
    public Uri Uri { get; private set; }
    public string Email { get; private set; }
    public virtual Blog Blog { get; set; } = null!;
}


public class Post
{
    public Post(string title, string content, DateOnly publishedOn)
    {
        Title = title;
        Content = content;
        PublishedOn = publishedOn;
    }

    public int Id { get; private set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public DateOnly PublishedOn { get; set; }
    public bool Archived { get; set; }
    public int BlogId { get; set; }
    public virtual Blog Blog { get; set; } = null!;
    public virtual List<Tag> Tags { get; } = new();
    public virtual Author? Author { get; set; }
    public PostMetadata? Metadata { get; set; }
}

public class PostMetadata
{
    public PostMetadata(int views)
    {
        Views = views;
    }

    public int Views { get; set; }
    public List<SearchTerm> TopSearches { get; } = new();
    public List<Visits> TopGeographies { get; } = new();
    public List<PostUpdate> Updates { get; } = new();
}

public class SearchTerm
{
    public SearchTerm(string term, int count)
    {
        Term = term;
        Count = count;
    }

    public string Term { get; private set; }
    public int Count { get; private set; }
}

public class Visits
{
    public Visits(double latitude, double longitude, int count)
    {
        Latitude = latitude;
        Longitude = longitude;
        Count = count;
    }

    public double Latitude { get; private set; }
    public double Longitude { get; private set; }
    public int Count { get; private set; }
    public List<string>? Browsers { get; set; }
}

public class PostUpdate
{
    public PostUpdate(IPAddress postedFrom, DateTime updatedOn)
    {
        PostedFrom = postedFrom;
        UpdatedOn = updatedOn;
    }

    public IPAddress PostedFrom { get; private set; }
    public string? UpdatedBy { get; init; }
    public DateTime UpdatedOn { get; private set; }
    public List<Commit> Commits { get; } = new();
}

public class Commit
{
    public Commit(DateTime committedOn, string comment)
    {
        CommittedOn = committedOn;
        Comment = comment;
    }

    public DateTime CommittedOn { get; private set; }
    public string Comment { get; set; }
}