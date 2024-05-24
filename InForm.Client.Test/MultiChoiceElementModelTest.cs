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
    public class multiChoiceElementModelTest
    {
        [Fact]
        void MultiChoiceElementFillData_NotNull_After_MakeFillable()
        {
            var sut = new MultiChoiceElementModel(new FormModel());

            sut.MakeFillable();

            Assert.NotNull(sut.FillData);
        }

        [Fact]
        void MultiChoiceElementFillData_Doesnt_React_To_Correct_Visitor()
        {
            var sut = new MultiChoiceElementModel(new FormModel());

            var mock = new Mock<ITypedVisitor<MultiChoiceElementModel>>();
            var badVtor = mock.Object;

            sut.Accept(badVtor);

            mock.Verify(x => x.Visit(It.IsAny<MultiChoiceElementModel>()), Times.Once());
            mock.VerifyNoOtherCalls();
        }

        [Fact]
        void StringElementFillData_React_To_Incorrect_Visitor()
        {
            var sut = new MultiChoiceElementModel(new FormModel());

            var mock = new Mock<ITypedVisitor<StringElementModel>>();
            var badVtor = mock.Object;
            sut.Accept(badVtor);
            mock.Verify(x => x.Visit(It.IsAny<StringElementModel>()), Times.Never);
            mock.VerifyNoOtherCalls();
        }

        [Fact]
        void MultiChoiceElementFillData_React_To_Correct_Visitor_With_Type()
        {
            var sut = new MultiChoiceElementModel(new FormModel());

            var mock = new Mock<ITypedVisitor<MultiChoiceElementModel, int>>();
            mock.Setup(x => x.Visit(It.IsAny<MultiChoiceElementModel>())).Returns(42);
            var badVtor = mock.Object;
            var res = sut.Accept(badVtor);
            Assert.Equal(42, res);
        }

        [Fact]
        void MaxSelected_Must_Be_Positive()
        {
            var sut = new MultiChoiceElementValidator();
            var bad = new MultiChoiceElementModel(new FormModel()) { MaxSelected = -2 };

            var res = sut.TestValidate(bad);
            res.ShouldHaveValidationErrorFor(x => x.MaxSelected);
        }

        [Fact]
        void Must_Be_One_Option_Supplied_Incorrect()
        {
            var sut = new MultiChoiceElementValidator();
            var bad = new MultiChoiceElementModel(new FormModel()) { Options = { } };

            var res = sut.TestValidate(bad);
            res.ShouldHaveValidationErrorFor(x => x.Options);
        }

        [Fact]
        void Must_Be_One_Option_Supplied_Correct()
        {
            var sut = new MultiChoiceElementValidator();
            var bad = new MultiChoiceElementModel(new FormModel()) { Options = { "teszt"} };

            var res = sut.TestValidate(bad);
            res.ShouldNotHaveValidationErrorFor(x => x.Options);
        }

        [Fact]
        void Required_Fields_Must_Be_Filled_Incorrect()
        {
            var sut = new MultiChoiceValueValidator();
            var bad = new MultiChoiceElementModel(new FormModel()) { Required = true };
            var fd = new MultiChoiceElementFillData(bad);
            var res = sut.TestValidate(fd);
            res.ShouldHaveValidationErrorFor(x => x.Selected);
        }

        [Fact]
        void Required_Fields_Must_Be_Filled_Correct()
        {
            var sut = new MultiChoiceValueValidator();
            var bad = new MultiChoiceElementModel(new FormModel()) { Required = true, Options = { "teszt" }, MaxSelected = 1 };
            var fd = new MultiChoiceElementFillData(bad);
            fd.Selected.Add("teszt");
            var res = sut.TestValidate(fd);
            res.ShouldNotHaveValidationErrorFor(x => x.Selected);
        }

        [Fact]
        void Selected_Value_Must_Be_In_Element_Valid_Options_Correct()
        {
            var sut = new MultiChoiceValueValidator();
            var bad = new MultiChoiceElementModel(new FormModel()) { Options = { "teszt" }, MaxSelected = 1 };
            var fd = new MultiChoiceElementFillData(bad);
            fd.Selected.Add("teszt");
            var res = sut.TestValidate(fd);
            res.ShouldNotHaveValidationErrorFor(x => x.Selected);
        }

        [Fact]
        void Selected_Value_Must_Be_In_Element_Valid_Options_Incorrect()
        {
            var sut = new MultiChoiceValueValidator();
            var bad = new MultiChoiceElementModel(new FormModel()) { Options = { "teszt2" }, MaxSelected = 1 };
            var fd = new MultiChoiceElementFillData(bad);
            fd.Selected.Add("teszt");
            var res = sut.TestValidate(fd);
            res.ShouldHaveValidationErrorFor(x => x.Selected);
        }

        [Fact]
        void Maximum_Maxselected_Can_Be_Selected_Correct()
        {
            var sut = new MultiChoiceValueValidator();
            var bad = new MultiChoiceElementModel(new FormModel()) { Options = { "teszt" }, MaxSelected = 1 };
            var fd = new MultiChoiceElementFillData(bad);
            fd.Selected.Add("teszt");
            var res = sut.TestValidate(fd);
            res.ShouldNotHaveValidationErrorFor(x => x.Selected);
        }

        [Fact]
        void Maximum_Maxselected_Can_Be_Selected_Incorrect()
        {
            var sut = new MultiChoiceValueValidator();
            var bad = new MultiChoiceElementModel(new FormModel()) { Options = { "teszt", "teszt2" }, MaxSelected = 1 };
            var fd = new MultiChoiceElementFillData(bad);
            fd.Selected.Add("teszt");
            fd.Selected.Add("teszt2");
            var res = sut.TestValidate(fd);
            res.ShouldHaveValidationErrorFor(x => x.Selected);
        }
    }
}
