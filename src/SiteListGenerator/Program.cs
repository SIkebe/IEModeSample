using System;
using System.IO;
using System.Text;

var count = int.Parse(args[0]) - 1;

var builder = new StringBuilder();
for (var i = 0; i < count; i++)
{
    builder.AppendLine(CreateSite());
}

var version = args.Length == 2 ? int.Parse(args[1]) : 1;
var sitelist = Wrap(builder.ToString(), version);

File.WriteAllText("./sites.xml", sitelist);

static string CreateSite()
{
    return $@"  <site url=""example.com/{Guid.NewGuid()}"">
    <compat-mode>Default</compat-mode>
    <open-in>IE11</open-in>
  </site>";
}


static string Wrap(string sites, int version)
{
    return $@"<site-list version=""{version}"">
  <site url=""https://ja.wikipedia.org/wiki/%E3%83%A1%E3%82%A4%E3%83%B3%E3%83%9A%E3%83%BC%E3%82%B8"">
    <compat-mode>Default</compat-mode>
    <open-in>IE11</open-in>
  </site>
{sites}</site-list>";
}
