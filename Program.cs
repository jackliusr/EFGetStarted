using EFGetStarted.Migrations;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

var context = new BloggingContext();
//await context.Seed();
//context.SaveChanges();
// ContactDetails Contact
var joes = context.Authors
    .Where(a => a.Contact.Address.City == "Chigley")
    .ToQueryString();
Console.WriteLine(joes);
Console.WriteLine($"Database path:.");