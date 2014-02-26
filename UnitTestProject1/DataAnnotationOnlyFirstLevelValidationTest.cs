using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace UnitTestProject1
{

    [TestClass]
    public class DataAnnotationOnlyFirstLevelValidationTest
    {

        [TestMethod]
        public void ValidateOnlyDataAnnotationFirstLevel()
        {
            var s = new Annot1();
            var results = s.Validate();
            Assert.AreEqual(0, results.Count()); //0 because it does not go deeper with Data Annotation
        }
    }

    public class Annot1:BaseClass2
    {
        public Annot1()
        {
            Annot1Property3 = new Annot2();
        }

        public string Annot1Property1 { get; set; }
        public string Annot1Property2 { get; set; }
        public Annot2 Annot1Property3 { get; set; }
  
    }

    public class Annot2 : BaseClass2
    {
        [Required]
        public string Annot2Property1 { get; set; }
        [Required]
        public string Annot2Property2 { get; set; }
 
    }

    public abstract class BaseClass2
    {
        public IEnumerable<ValidationResult> Validate()
        {
            var results = new List<ValidationResult>();
            var validationContext = new ValidationContext(this, null, null);
            Validator.TryValidateObject(this, validationContext, results, true);
            return results;
        }
    }
}
