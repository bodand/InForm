using EntityFrameworkCoreMock;
using InForm.Server.Core.Features.Fill;
using InForm.Server.Db;
using InForm.Server.Features.Common;
using InForm.Server.Features.FillForms;
using InForm.Server.Features.FillForms.Db;
using InForm.Server.Features.FillForms.Service;
using InForm.Server.Features.Forms.Db;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using TechTalk.SpecFlow;

namespace InForm.Specs.StepDefinitions
{
    [Binding]
    public class FillsStepDefinitions
    {
        
        public FillsStepDefinitions()
        {
            testGuid = Guid.NewGuid();
            var options = new DbContextOptions<InFormDbContext>();
            context = new DbContextMock<InFormDbContext>(options);
            context.CreateDbSetMock(x => x.Forms, new[]
            {
                new Form { Id = 1, IdGuid = testGuid, Title = "test", Subtitle = "test", PasswordHash = "test" ,FormElementBases = 
                    { new StringFormElement {Title = "test", Id = 1 ,ParentFormId = 1, Subtitle = "test"} } }
            });
            context.CreateDbSetMock(x => x.StringFormElements, new[]
            {
                new StringFormElement {Title = "test", Id = 1 ,ParentFormId = 1, Subtitle = "test"}
            });
            context.CreateDbSetMock(x => x.MultiChoiceFormElements);

            
            pwhasher = new Mock<IPasswordHasher>();
            pwhasher.Setup(x => x.Hash(It.IsAny<string>())).Returns(It.IsAny<string>);
            pwhasher.Setup(x => x.VerifyAndUpdate(It.Is<string>(x => x.Contains("test")), It.IsAny<string>())).Returns(new HashVerificationResult(true,null));
            fillsController = new FillsController(dbContext: context.Object, fillService: new FillService(pwhasher.Object));
        }

        private readonly Guid testGuid;
        private Guid formGuid;

        DbContextMock<InFormDbContext> context;

        Mock<IPasswordHasher> pwhasher;

        private readonly FillsController fillsController;

        private FillRequest fillRequest;
        private RetrieveFillsRequest retrieveFillsRequest;
        private ActionResult<RetrieveFillsResponse> retrieveFillsResponse;
        private ActionResult result;


        [Given(@"a fillrequest with data that is accepted by the controller")]
        public void GivenAFillrequestWithDataThatIsAcceptedByTheController()
        {
            formGuid = testGuid;
            fillRequest = new FillRequest
            {
                FormId = testGuid,
                Elements = new List<FillElement>
                {
                    new StringFillElement(1, "test")
                }
            };
        }

        [When(@"the fillrequest is sent to the controller")]
        public void WhenTheFillrequestIsSentToTheController()
        {
            result = fillsController.AddFillData(formGuid, fillRequest).Result;
        }

        [Then(@"the controller should return Accepted")]
        public void ThenTheControllerShouldReturnAccepted()
        {
            result.Should().BeOfType<AcceptedResult>();
        }

        [Given(@"a fillrequest with data that is not accepted by the controller")]
        public void GivenAFillrequestWithDataThatIsNotAcceptedByTheController()
        {
            formGuid = testGuid;
            fillRequest = new FillRequest
            {
                FormId = Guid.NewGuid(),
                Elements = new List<FillElement>
                {
                    new StringFillElement(1, "test")
                }
            };
        }

        [Then(@"the controller should return BadRequest")]
        public void ThenTheControllerShouldReturnBadRequest()
        {
            result.Should().BeOfType<BadRequestResult>();
        }

        [Given(@"a fillrequest with data that is rejected because of non existing form")]
        public void GivenAFillrequestWithDataThatIsRejectedBecauseOfNonExistingForm()
        {
            formGuid = Guid.NewGuid();
            fillRequest = new FillRequest
            {
                FormId = formGuid,
                Elements = new List<FillElement>
                {
                    new StringFillElement(1, "test")
                }
            };
        }

        [Then(@"the controller should return NotFound")]
        public void ThenTheControllerShouldReturnNotFound()
        {
            result.Should().BeOfType<NotFoundResult>();
        }

        [Given(@"a getrequest with data that is accepted by the controller")]
        public void GivenAGetrequestWithDataThatIsAcceptedByTheController()
        {
            formGuid = testGuid;
            retrieveFillsRequest = new RetrieveFillsRequest
            {
                Id = formGuid,
                Password = "test"
            };
        }

        [When(@"the getrequest is sent to the controller")]
        public void WhenTheGetrequestIsSentToTheController()
        {
            retrieveFillsResponse = fillsController.GetFillData(formGuid, retrieveFillsRequest).Result;
        }

        [Then(@"the controller should return Ok")]
        public void ThenTheControllerShouldReturnOk()
        {
            retrieveFillsResponse.Result.Should().BeOfType<OkObjectResult>();
        }

        [Given(@"a getrequest with data that is rejected because of non existing form")]
        public void GivenAGetrequestWithDataThatIsRejectedBecauseOfNonExistingForm()
        {
            formGuid = Guid.NewGuid();
            retrieveFillsRequest = new RetrieveFillsRequest
            {
                Id = formGuid,
                Password = "test"
            };
        }

        [Given(@"a getrequest with data that is rejected because of bad request")]
        public void GivenAGetrequestWithDataThatIsRejectedBecauseOfBadRequest()
        {
            formGuid = testGuid;
            retrieveFillsRequest = new RetrieveFillsRequest
            {
                Id = Guid.NewGuid()
            };
        }

        [Then(@"the controller should return BadRequest for getfilldata")]
        public void ThenTheControllerShouldReturnBadRequestForGetfilldata()
        {
            retrieveFillsResponse.Result.Should().BeOfType<BadRequestResult>();
        }

        [Then(@"the controller should return NotFound for getfilldata")]
        public void ThenTheControllerShouldReturnNotFoundForGetfilldata()
        {
            retrieveFillsResponse.Result.Should().BeOfType<NotFoundResult>();
        }



        [Given(@"a getrequest with data that is rejected because of unauthorized request")]
        public void GivenAGetrequestWithDataThatIsRejectedBecauseOfUnauthorizedRequest()
        {
            
            formGuid = testGuid;
            retrieveFillsRequest = new RetrieveFillsRequest
            {
                Id = formGuid,
                Password = "NotTest"
            };
        }

        [Then(@"the controller should return Unauthorized")]
        public void ThenTheControllerShouldReturnUnauthorized()
        {
            retrieveFillsResponse.Result.Should().BeOfType<UnauthorizedResult>();
        }
    }
}
