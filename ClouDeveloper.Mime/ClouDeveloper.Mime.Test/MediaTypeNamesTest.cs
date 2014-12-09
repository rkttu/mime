using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClouDeveloper.Mime.Test
{
    [TestFixture]
    public sealed class MediaTypeNamesTest
    {
        [Test]
        public void BasicTest()
        {
            Assert.That(
                MediaTypeNames.GetSuppportedExtensions(),
                Is.Not.Null.And.Count.GreaterThan(0));

            Assert.That(
                MediaTypeNames.GetMediaTypeNames(".txt"),
                Contains.Item(MediaTypeNames.text.plain));

            Assert.That(
                MediaTypeNames.GetMediaTypeNames(".mp4"),
                Contains.Item(MediaTypeNames.video.mp4));

            Assert.That(
                MediaTypeNames.GetMediaTypeNames(DateTime.Now.Ticks.ToString()),
                Contains.Item(MediaTypeNames.application.octet_stream));

            Assert.That(
                MediaTypeNames.GetMediaTypeNames("."),
                Contains.Item(MediaTypeNames.application.octet_stream));

            Assert.That(
                MediaTypeNames.GetMediaTypeNames(""),
                Contains.Item(MediaTypeNames.application.octet_stream));

            Assert.That(
                MediaTypeNames.GetMediaTypeNames(".*"),
                Contains.Item(MediaTypeNames.application.octet_stream));

            Assert.That(
                MediaTypeNames.GetMediaTypeNames("*"),
                Contains.Item(MediaTypeNames.application.octet_stream));

            Assert.That(
                MediaTypeNames.GetMediaTypeNames(default(string)),
                Contains.Item(MediaTypeNames.application.octet_stream));
        }
    }
}
