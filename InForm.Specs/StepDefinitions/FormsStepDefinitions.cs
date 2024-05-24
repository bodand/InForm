using InForm.Server.Core.Features.Forms;
using InForm.Server.Features.Forms;
using InForm.Server.Features.Forms.Db;
using InForm.Server.Features.Forms.Service;
using InForm.Server.Features.Common;
using System;
using TechTalk.SpecFlow;
using InForm.Server.Db;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Moq;
using EntityFrameworkCoreMock;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace InForm.Specs.StepDefinitions
{
    [Binding]
    public class FormsStepDefinitions
    {
        public FormsStepDefinitions()
        {
            testGuid = Guid.NewGuid();
            var options = new DbContextOptions<InFormDbContext>();
            var passwordHasher = new Mock<IPasswordHasher>();
            passwordHasher.Setup(x => x.Hash(It.IsAny<string>())).Returns("hash");
            passwordHasher.Setup(x => x.VerifyAndUpdate(It.IsAny<string>(), It.IsAny<string>())).Returns(new HashVerificationResult(true,"hash"));
            context = new DbContextMock<InFormDbContext>(options);
            context.CreateDbSetMock(x => x.Forms, new[]
            {
                new Form { Id = 1,IdGuid = testGuid, Title = "test", Subtitle = "test" }
            });

            formsController = new FormsController(formsService: new FormsService(), passwordHasher: passwordHasher.Object, dbContext: context.Object, logger: new LoggerFactory().CreateLogger<FormsController>());

        }

        DbContextMock<InFormDbContext> context;
        private FormsController formsController;

        private CreateFormRequest formRequest;

        private ActionResult<CreateFormResponse> createFormResponse;
        private ActionResult<GetFormReponse> getFormResponse;
        private ActionResult<GetFormNameResponse> getFormNameResponse;

        private readonly Guid testGuid;
        private Guid formGuid;

        private CreateFormRequest makeCreateFormRequest(string title,string subtitle,string elementTitle, string elementSubtitle, bool required, bool multiline, int elementMaxLength)
        {
            var request = new CreateFormRequest
            {
                Title = title,
                Subtitle = subtitle,
                Elements = new List<CreateFormElement>
                {
                    new CreateStringElement(Title: elementTitle, Required: required, Subtitle: elementSubtitle, MaxLength: elementMaxLength, Multiline: multiline)
                }
            };

            return request;
        }


        [Given(@"the form with guid")]
        public void GivenTheFormWithGuid()
        {
            formGuid = testGuid;
        }

        [When(@"the client requests the form with guid")]
        public void WhenTheClientRequestsTheFormWithGuid()
        {
            getFormResponse =  formsController.GetForm(formGuid).Result;
        }

        [Then(@"the client receives the form with guid")]
        public void ThenTheClientReceivesTheFormWithGuid()
        {
            getFormResponse.Should().NotBeNull();
            getFormResponse.Value.Id.Should().Be(formGuid);
        }

        [Given(@"A form does not exist with the guid")]
        public void GivenAFormDoesNotExistWithTheGuid()
        {
            formGuid = Guid.NewGuid();
        }

        [When(@"I request the form with the guid")]
        public void WhenIRequestTheFormWithTheGuid()
        {
            getFormResponse = formsController.GetForm(formGuid).Result;
        }

        [Then(@"I should get a NotFound response")]
        public void ThenIShouldGetAResponse()
        {
            getFormResponse.Result.Should().BeOfType<NotFoundResult>();
        }

        [Then(@"I should get a NotFound response for formname")]
        public void ThenIShouldGetAResponseforformname()
        {
            getFormNameResponse.Result.Should().BeOfType<NotFoundResult>();
        }

        [When(@"the client requests the form's name with guid")]
        public void WhenTheClientRequestsTheFormsNameWithGuid()
        {
            getFormNameResponse = formsController.GetFormName(formGuid).Result;
        }

        [Then(@"the client receives the form's name with guid")]
        public void ThenTheClientReceivesTheFormsNameWithGuid()
        {
            getFormNameResponse.Should().NotBeNull();
            getFormNameResponse.Value.Id.Should().Be(formGuid);
        }



        [Given(@"I have a form with the following attributes:")]
        public void GivenIHaveAFormWithTheFollowingAttributes(Table table)
        {
            formRequest = makeCreateFormRequest(table.Rows[0]["title"], table.Rows[0]["subtitle"],
                table.Rows[0]["elementTitle"], table.Rows[0]["elementSubtitle"],
                bool.Parse(table.Rows[0]["required"]), bool.Parse(table.Rows[0]["multiline"]),
                int.Parse(table.Rows[0]["elementMaxLength"]));
        }

        [When(@"I create the form")]
        public  void WhenICreateTheForm()
        {
            var result =  formsController.CreateForm(formRequest).Result;
            createFormResponse = result;
        }

        [Then(@"I should get a CreatedStatusCode and form should have a guid")]
        public void IshouldgetaCreatedStatusCodeandformshouldhaveaguid()
        {
            createFormResponse.Result.Should().BeOfType<CreatedAtActionResult>();
            var response = createFormResponse.Value;
            response.Should().NotBeNull();
        }


    }
}
