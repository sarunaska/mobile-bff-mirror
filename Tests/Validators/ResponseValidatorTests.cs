using MobileBff.Services.ResponseValidation;
using Tests.Validators.Models;

namespace Tests.Validators
{
    public class ResponseValidatorTests
    {
        [Fact]
        public void ValidateAndUpdate_WhenRequiredStringPropertyIsNull_ShouldReturnFalse()
        {
            var model = new TestModelForResponseValidator { RequiredStringProperty = null };

            var responseValidator = new ResponseValidator();
            var isValid = responseValidator.ValidateAndUpdate(model);

            Assert.False(isValid);
        }

        [Fact]
        public void ValidateAndUpdate_WhenNonRequiredStringPropertyIsNull_ShouldReturnTrue()
        {
            var model = new TestModelForResponseValidator { NonRequiredStringProperty = null };

            var responseValidator = new ResponseValidator();
            var isValid = responseValidator.ValidateAndUpdate(model);

            Assert.True(isValid);
        }

        [Fact]
        public void ValidateAndUpdate_WhenRequiredListPropertyIsNull_ShouldReturnFalse()
        {
            var model = new TestModelForResponseValidator { RequiredStringsListProperty = null };

            var responseValidator = new ResponseValidator();
            var isValid = responseValidator.ValidateAndUpdate(model);

            Assert.False(isValid);
        }

        [Fact]
        public void ValidateAndUpdate_WhenNonRequiredListPropertyIsNull_ShouldReturnTrue()
        {
            var model = new TestModelForResponseValidator { NonRequiredStringsListProperty = null };

            var responseValidator = new ResponseValidator();
            var isValid = responseValidator.ValidateAndUpdate(model);

            Assert.True(isValid);
        }

        [Fact]
        public void ValidateAndUpdate_WhenRequiredListPropertyIsEmpty_ShouldReturnFalse()
        {
            var model = new TestModelForResponseValidator { RequiredStringsListProperty = new List<string>() };

            var responseValidator = new ResponseValidator();
            var isValid = responseValidator.ValidateAndUpdate(model);

            Assert.False(isValid);
        }

        [Fact]
        public void ValidateAndUpdate_WhenNonRequiredListPropertyIsEmpty_ShouldReturnTrue()
        {
            var model = new TestModelForResponseValidator { NonRequiredStringsListProperty = new List<string>() };

            var responseValidator = new ResponseValidator();
            var isValid = responseValidator.ValidateAndUpdate(model);

            Assert.True(isValid);
        }

        [Fact]
        public void ValidateAndUpdate_WhenRequiredObjectPropertyIsNull_ShouldReturnFalse()
        {
            var model = new TestModelForResponseValidator { RequiredObjectProperty = null };

            var responseValidator = new ResponseValidator();
            var isValid = responseValidator.ValidateAndUpdate(model);

            Assert.False(isValid);
        }

        [Fact]
        public void ValidateAndUpdate_WhenNonRequiredObjectPropertyIsNull_ShouldReturnTrue()
        {
            var model = new TestModelForResponseValidator { NonRequiredObjectProperty = null };

            var responseValidator = new ResponseValidator();
            var isValid = responseValidator.ValidateAndUpdate(model);

            Assert.True(isValid);
        }

        [Fact]
        public void ValidateAndUpdate_WhenRequiredPropertyOfInnerObjectIsNull_ShouldReturnFalse()
        {
            var model = new TestModelForResponseValidator();
            model.RequiredObjectProperty!.RequiredStringProperty = null;

            var responseValidator = new ResponseValidator();
            var isValid = responseValidator.ValidateAndUpdate(model);

            Assert.False(isValid);
        }

        [Fact]
        public void ValidateAndUpdate_WhenNonRequiredPropertyOfInnerObjectIsNull_ShouldReturnTrue()
        {
            var model = new TestModelForResponseValidator();
            model.RequiredObjectProperty!.NonRequiredStringProperty = null;

            var responseValidator = new ResponseValidator();
            var isValid = responseValidator.ValidateAndUpdate(model);

            Assert.True(isValid);
        }

        [Fact]
        public void ValidateAndUpdate_WhenPropertyIsPartialResponseReturnsTrue_ShouldReturnFalse()
        {
            var model = new TestModelForResponseValidator { IsPartialResponse = true };

            var responseValidator = new ResponseValidator();
            var isValid = responseValidator.ValidateAndUpdate(model);

            Assert.False(isValid);
        }

        [Fact]
        public void ValidateAndUpdate_WhenPropertyIsPartialResponseReturnsFalse_ShouldReturnTrue()
        {
            var model = new TestModelForResponseValidator { IsPartialResponse = false };

            var responseValidator = new ResponseValidator();
            var isValid = responseValidator.ValidateAndUpdate(model);

            Assert.True(isValid);
        }

        [Fact]
        public void ValidateAndUpdate_WhenObjectIsInListAndRequiredPropertyIsNull_ShouldRemoveObjectFromListAndReturnFalse()
        {
            var model = new TestModelForResponseValidator();
            model.ObjectsListProperty![0].RequiredStringProperty = null;

            var responseValidator = new ResponseValidator();
            var isValid = responseValidator.ValidateAndUpdate(model);

            Assert.Empty(model.ObjectsListProperty);
            Assert.False(isValid);
        }

        [Fact]
        public void ValidateAndUpdate_WhenObjectIsInListAndNonRequiredPropertyIsNull_ShouldNotRemoveObjectFromListAndReturnTrue()
        {
            var model = new TestModelForResponseValidator();
            model.ObjectsListProperty![0].NonRequiredStringProperty = null;

            var responseValidator = new ResponseValidator();
            var isValid = responseValidator.ValidateAndUpdate(model);

            Assert.Single(model.ObjectsListProperty);
            Assert.True(isValid);
        }

        [Fact]
        public void ValidateAndUpdate_WhenModelHasArrayProperty_ShouldNotThrowException()
        {
            var model = new TestModelWithArrayForResponseValidator();
            model.ObjectsArrayProperty[0].RequiredStringProperty = null;

            var responseValidator = new ResponseValidator();

            responseValidator.ValidateAndUpdate(model);
        }

        [Fact]
        public void ValidateAndUpdate_WhenObjectIsInInnerListAndRequiredPropertyIsNull_ShouldRemoveObjectFromListAndReturnFalse()
        {
            var model = new TestModelForResponseValidator();
            model.ObjectsListWithInnerListProperty![0].ObjectsListProperty[0].RequiredStringProperty = null;

            var responseValidator = new ResponseValidator();
            var isValid = responseValidator.ValidateAndUpdate(model);

            Assert.Empty(model.ObjectsListWithInnerListProperty![0].ObjectsListProperty);
            Assert.False(isValid);
        }
    }
}
