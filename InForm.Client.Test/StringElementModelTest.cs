using FluentValidation.TestHelper;
using InForm.Client.Features.Forms;
using InForm.Server.Core.Features.Common;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InForm.Client.Test
{
    public class StringElementModelTest
    {
        [Fact]
        void StringElementFillData_NotNull_After_MakeFillable()
        {
            var sut = new StringElementModel(new FormModel());

            sut.MakeFillable();

            Assert.NotNull(sut.FillData);
        }

        [Fact]
        void MaxAnswerLength_Must_Be_Positive()
        {
            var sut = new StringElementValidator();
            var bad = new StringElementModel(new FormModel()) { MaxAnswerLength = -2 };
            
            var res = sut.TestValidate(bad);
            res.ShouldHaveValidationErrorFor(x => x.MaxAnswerLength);
        }

        [Fact]
        void StringElementFillData_Doesnt_React_To_Incorrect_Visitor()
        {
            var sut = new StringElementModel(new FormModel());

            var mock = new Mock<ITypedVisitor<MultiChoiceElementModel>>();
            var badVtor = mock.Object;

            sut.Accept(badVtor);

            mock.Verify(x => x.Visit(It.IsAny<MultiChoiceElementModel>()), Times.Never());
            mock.VerifyNoOtherCalls();
        }

        [Fact]
        void StringElementFillData_React_To_Correct_Visitor()
        {
            var sut = new StringElementModel(new FormModel());

            var mock = new Mock<ITypedVisitor<StringElementModel>>();
            var badVtor = mock.Object;
            sut.Accept(badVtor);
            mock.Verify(x => x.Visit(It.IsAny<StringElementModel>()), Times.Once);
            mock.VerifyNoOtherCalls();
        }

        [Fact]
        void StringElementFillData_React_To_Correct_Visitor_With_Type()
        {
            var sut = new StringElementModel(new FormModel());

            var mock = new Mock<ITypedVisitor<StringElementModel,int>>();
            mock.Setup(x => x.Visit(It.IsAny<StringElementModel>())).Returns(42);
            var badVtor = mock.Object;
            var res = sut.Accept(badVtor);
            Assert.Equal(42, res);
        }

        [Fact]
        void StringElementFillData_React_To_InCorrect_Visitor_With_Type()
        {
            var sut = new StringElementModel(new FormModel());

            var mock = new Mock<ITypedVisitor<MultiChoiceElementModel, int>>();
            mock.Setup(x => x.Visit(It.IsAny<MultiChoiceElementModel>())).Returns(42);
            var badVtor = mock.Object;
            var res = sut.Accept(badVtor);
            Assert.NotEqual(42, res);
        }

        [Fact]
        void Required_String_Values_Must_Be_Filled_Incorrect()
        {
            var sut = new StringValueValidator();
            
            var bad = new StringElementModel(new FormModel()) { Required = true };
            var fd = new StringElementFillData(bad);

            var res = sut.TestValidate(fd);
            res.ShouldHaveValidationErrorFor(x => x.Value);
        }

        [Fact]
        void Required_String_Values_Must_Be_Filled_Correct()
        {
            var sut = new StringValueValidator();

            var bad = new StringElementModel(new FormModel()) { Required = true };
            var fd = new StringElementFillData(bad);
            fd.Value = "ok";

            var res = sut.TestValidate(fd);
            res.ShouldNotHaveValidationErrorFor(x => x.Value);
        }

        [Fact]
        void String_Values_Must_Be_Less_Then_MaxAnswerLength_Incorrect()
        {
            var sut = new StringValueValidator();

            var bad = new StringElementModel(new FormModel()) { MaxAnswerLength = 1 };
            var fd = new StringElementFillData(bad);
            fd.Value = "ok";
            var res = sut.TestValidate(fd);
            res.ShouldHaveValidationErrorFor(x => x.Value);
        }

        [Fact]
        void String_Values_Must_Be_Less_Then_MaxAnswerLength_Correct()
        {
            var sut = new StringValueValidator();

            var bad = new StringElementModel(new FormModel()) { MaxAnswerLength = 3 };
            var fd = new StringElementFillData(bad);
            fd.Value = "ok";
            var res = sut.TestValidate(fd);
            res.ShouldNotHaveValidationErrorFor(x => x.Value);
        }
    }
}
