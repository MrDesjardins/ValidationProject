using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace UnitTestProject2
{

    public abstract class BaseClass
    {
        public IEnumerable<ValidationResult> Validate()
        {
            var results = new List<ValidationResult>();
            var validationContext = new ValidationContext(this, null, null);
            Validator.TryValidateObject(this, validationContext, results, true);
            var r = new List<ValidationResult>(results);

            if (this is IValidatableObject)
            {
                IEnumerable<ValidationResult> errors = (this as IValidatableObject).Validate(new ValidationContext(this));
                r.AddRange(errors);
            }
            var childrenResults = ValidateChildren();
            r.AddRange(childrenResults);
            return r;
        }

        public abstract IEnumerable<ValidationResult> ValidateChildren();

    }

    public class Class1 : BaseClass, IValidatableObject
    {
        public Class1()
        {
            Property1 = new Class2();
        }
        public Class2 Property1 { get; set; }
        [Required]
        public string AString1 { get; set; }
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return new[] { new ValidationResult("Error from class1") };
        }

        public override IEnumerable<ValidationResult> ValidateChildren()
        {
            var r = new List<ValidationResult>();
            var childrenResult = Property1.Validate();
            r.AddRange(childrenResult);
            return r;
        }
    }

    public class Class2 : BaseClass, IValidatableObject
    {
        public Class2()
        {
            Property2 = new Class3();
        }
        public Class3 Property2 { get; set; }
        [Required]
        public string AString2 { get; set; }
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            yield return new ValidationResult("Error from class2");
        }

        public override IEnumerable<ValidationResult> ValidateChildren()
        {
            var r = new List<ValidationResult>();
            var childrenResult = Property2.Validate();
            r.AddRange(childrenResult);
            return r;
        }
    }

    public class Class3 : BaseClass
    {
        [Required]
        public string AString3 { get; set; }

        public override IEnumerable<ValidationResult> ValidateChildren()
        {
            return new List<ValidationResult>();
        }
    }
    [TestClass]
    public class DataAnnotationAllLevelValidationTest
    {
        [TestMethod]
        public void DataAnnotationAllLevel()
        {
            var s = new Class1();
            var results = s.Validate();
            Assert.AreEqual(5, results.Count());
        }

    }

}
