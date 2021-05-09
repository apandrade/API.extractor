using API.Extractor.Domain.Validators;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace UnitTests.Validators
{
    [TestClass]
    public class UrlValidatorTests
    {
        public IList<string> ValidUrl
        {
            get => new List<string> {
                 "https://www.altudo.co/"
                ,"https://www.altudo.co/industries-we-serve"
                ,"https://www.google.com/search?q=altudo&safe=active&sxsrf=ALeKk01LdL7y_2RJqjf0zS901itZ5YleLw%3A1620350043046&source=hp&ei=WpSUYPK3PLG65OUPge6w4A8&iflsig=AINFCbYAAAAAYJSia6tswczIoeYtUcp-RxO2w5VOhR7v&oq=&gs_lcp=Cgdnd3Mtd2l6EAEYADIJCCMQ6gIQJxATMgcIIxDqAhAnMgcIIxDqAhAnMgkIIxDqAhAnEBMyBwgjEOoCECcyCQgjEOoCECcQEzIJCCMQ6gIQJxATMgkIIxDqAhAnEBMyBwgjEOoCECcyBwgjEOoCECdQAFgAYPUgaAFwAHgAgAEAiAEAkgEAmAEAqgEHZ3dzLXdperABCg&sclient=gws-wiz"
                ,"http://www.altudo.co/"
                ,"http://altudo.co/"
            };
        }

        public IList<string> InvalidUrl
        {
            get => new List<string> {
                 "https//www.altudo.co/"
                ,"https//www.altudo.co"
                ,"https:/www.altudo.co/industries-we-serve"
                ,"https:www.google.com/search?q=altudo&safe=active&sxsrf=ALeKk01LdL7y_2RJqjf0zS901itZ5YleLw%3A1620350043046&source=hp&ei=WpSUYPK3PLG65OUPge6w4A8&iflsig=AINFCbYAAAAAYJSia6tswczIoeYtUcp-RxO2w5VOhR7v&oq=&gs_lcp=Cgdnd3Mtd2l6EAEYADIJCCMQ6gIQJxATMgcIIxDqAhAnMgcIIxDqAhAnMgkIIxDqAhAnEBMyBwgjEOoCECcyCQgjEOoCECcQEzIJCCMQ6gIQJxATMgkIIxDqAhAnEBMyBwgjEOoCECcyBwgjEOoCECdQAFgAYPUgaAFwAHgAgAEAiAEAkgEAmAEAqgEHZ3dzLXdperABCg&sclient=gws-wiz"
            };
        }

        [TestMethod]
        public void Should_Return_True_For_Valid_Url()
        {
            var validator = new UrlValidator();

            foreach (string url in ValidUrl)
            {
                Assert.IsTrue(validator.IsValid(url));
            }
        }

        [TestMethod]
        public void Should_Return_False_For_Invalid_Url()
        {
            var validator = new UrlValidator();

            foreach (string url in InvalidUrl)
            {
                Assert.IsFalse(validator.IsValid(url));
            }
        }
    }
}
